using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.Interfaces.Services.Products;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Products;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Products;
using SalesManagerSolution.Infrastructure.EntityFramework;
using SalesManagerSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using SalesManagerSolution.Core.Interfaces.Common;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.ProductImages;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.ProductImages;

namespace SalesManagerSolution.Infrastructure.Services.Products
{
	public class ProductService : IProductService
	{
		private readonly ApplicationDbContext _context;
        private readonly IStorageService _storageService;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        public ProductService(ApplicationDbContext context, IStorageService storageService)
		{
			_context = context;
			_storageService = storageService;
		}

		public async Task AddViewcount(int productId)
		{
            var product = await _context.Products.FindAsync(productId);

            if (product is null)
                return;

            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

		public async Task<int> Create(ProductCreateViewModel request)
		{
			var product = new Product()
			{
				Name = request.Name,
				Description = request.Description,
				Price = request.Price,
				OriginalPrice = request.OriginalPrice,
				IsFeatured = request.IsFeatured,
				IsDeleted = false,
				Stock = request.Stock,
				ViewCount = 0
			};

            //Save image
            if (request.ThumbnailImage != null)
            {

                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbnail image",
                        DateCreated = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        IsDefault = true,
                        SortOrder = 1
                    }
                };
               
            }

            _context.Products.Add(product);

			await _context.SaveChangesAsync();

			return product.Id;	
		}

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;

            if(string.IsNullOrEmpty(originalFileName) )
            {
                return string.Empty;
            }

            originalFileName = originalFileName.Trim();

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        public async Task<int> Delete(int productId)
		{
			var product = await _context.Products.FindAsync(productId);
			if (product == null) throw new Exception($"Cannot find a product: {productId}");

			var images = _context.ProductImages.Where(i => i.ProductId == productId);
			foreach (var image in images)
			{
				await _storageService.DeleteFileAsync(image.ImagePath);
			}

			_context.Products.Remove(product);

			return await _context.SaveChangesAsync();
		}

		public async Task<PagedResult<ProductViewModel>> GetAllPaging(ProductPagingViewModel request)
		{
            var query = from p in _context.Products
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        where pi.IsDefault == true
                        select new { p, pic, pi,c };
            //2. filter
            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.p.Name.Contains(request.Keyword));

            if (request.CategoryId != null && request.CategoryId != 0)
            {
                query = query.Where(p => p.pic.CategoryId == request.CategoryId);
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.p.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.p.Description,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    ThumbnailImage = x.pi.ImagePath
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<ProductViewModel>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

		public async Task<ProductViewModel> GetById(int productId)
		{

			var query = from p in _context.Products
						join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
						from pi in ppi.DefaultIfEmpty()
						where p.Id == productId
						select new { p, pi };

			var product = await query
				                .Select(x => new ProductViewModel()
				                {
					                Id = x.p.Id,
					                Name = x.p.Name,
					                DateCreated = x.p.DateCreated,
					                Description = x.p.Description,
					                OriginalPrice = x.p.OriginalPrice,
					                Price = x.p.Price,
					                Stock = x.p.Stock,
					                ViewCount = x.p.ViewCount,
					                ThumbnailImage = x.pi.ImagePath
				                }).FirstOrDefaultAsync();

			return product == null ? new ProductViewModel() : product;
		}

		public async Task<int> Update(ProductCreateViewModel request)
		{
			var product = await _context.Products
                                        .FindAsync(request.Id);

			if (product == null) throw new Exception($"Cannot find a product with id: {request.Id}");

			product.Name = request.Name;
			product.OriginalPrice = request.OriginalPrice;
			product.Price = request.Price;
			product.Stock = request.Stock;
			product.Description = request.Description;

            _context.Products.Update(product);

			if (request.ThumbnailImage != null)
			{
				var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductId == request.Id);
				if (thumbnailImage != null)
				{
					thumbnailImage.FileSize = request.ThumbnailImage.Length;
					thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
					_context.ProductImages.Update(thumbnailImage);
				}
			}

			await _context.SaveChangesAsync();

			return product.Id;

		}

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new Exception($"Cannot find a product with id: {productId}");
            product.Price = newPrice;
            return await _context.SaveChangesAsync() > 0;
        }

		public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new Exception($"Cannot find a product with id: {productId}");
            product.Stock += addedQuantity;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption ?? string.Empty,
                DateCreated = DateTime.Now,
                IsDefault = request.IsDefault,
                ProductId = productId,
                SortOrder = request.SortOrder
            };

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.Id;
        }

        public async Task<int> RemoveImage(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new Exception($"Cannot find an image with id {imageId}");
            _context.ProductImages.Remove(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new Exception($"Cannot find an image with id {imageId}");

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null)
                throw new Exception($"Cannot find an image with id {imageId}");

            var viewModel = new ProductImageViewModel()
            {
                Caption = image.Caption,
                DateCreated = image.DateCreated,
                FileSize = image.FileSize,
                Id = image.Id,
                ImagePath = image.ImagePath,
                IsDefault = image.IsDefault,
                ProductId = image.ProductId,
                SortOrder = image.SortOrder
            };
            return viewModel;
        }

        public async Task<List<ProductImageViewModel>> GetListImages(int productId)
        {
            return await _context.ProductImages.Where(x => x.ProductId == productId)
                .Select(i => new ProductImageViewModel()
                {
                    Caption = i.Caption,
                    DateCreated = i.DateCreated,
                    FileSize = i.FileSize,
                    Id = i.Id,
                    ImagePath = i.ImagePath,
                    IsDefault = i.IsDefault,
                    ProductId = i.ProductId,
                    SortOrder = i.SortOrder
                }).ToListAsync();
        }

        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(PublicProductPagingRequest request)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        select new { p, pic };
            //2. filter
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p => p.pic.CategoryId == request.CategoryId);
            }
            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.p.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.p.Description,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<ProductViewModel>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request)
        {
            var user = await _context.Products.FindAsync(id);
            if (user == null)
            {
                return new ApiErrorResult<bool>($"Sản phẩm với id {id} không tồn tại");
            }
            foreach (var category in request.Categories)
            {
                var productInCategory = await _context.ProductInCategories
                    .FirstOrDefaultAsync(x => x.CategoryId == int.Parse(category.Id)
                    && x.ProductId == id);
                if (productInCategory != null && category.Selected == false)
                {
                    _context.ProductInCategories.Remove(productInCategory);
                }
                else if (productInCategory == null && category.Selected)
                {
                    await _context.ProductInCategories.AddAsync(new ProductInCategory()
                    {
                        CategoryId = int.Parse(category.Id),
                        ProductId = id
                    });
                }
            }
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<List<ProductViewModel>> GetFeaturedProducts(int take)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        select new { p,  pic, pi,c };

            var data = await query.OrderByDescending(x => x.p.DateCreated).Take(take)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.p.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.p.Description,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
					ThumbnailImage = x.pi.ImagePath
                }).ToListAsync();


              foreach(var product in data)
              {
                var categoryNames = await _context.ProductInCategories
                                      .Join(_context.Products, pc => pc.ProductId, p => p.Id, (pc, p) => new { pc = pc, p = p })
                                      .Join(_context.Categories, pc => pc.pc.CategoryId, c => c.Id, (ppc, c) => new { ppc = ppc, c = c })
                                      .Where(x=>x.ppc.p.Id == product.Id)
                                      .Select(x => x.c.Name)
                                      .ToListAsync();

                product.CategoryName = categoryNames == null ? new List<string>() { string.Empty} : categoryNames;

			  }

            return data;
        }

        public async Task<List<ProductViewModel>> GetLatestProducts(int take)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        where pi == null || pi.IsDefault == true
                        select new { p, pic, pi };

            var data = await query.OrderByDescending(x => x.p.DateCreated).Take(take)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.p.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.p.Description,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    ThumbnailImage = x.pi.ImagePath
                }).ToListAsync();

            return data;
        }

		public async Task<string> SearchProduct(string productName)
		{
            var searchProduct = await (from p in _context.Products
                                       where p.Name == productName
                                       select p)
                                 .ToListAsync();
            return searchProduct.ToString();
		}
	}
}
