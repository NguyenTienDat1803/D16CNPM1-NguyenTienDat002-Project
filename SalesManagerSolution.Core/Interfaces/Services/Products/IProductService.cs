using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.ProductImages;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Products;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.ProductImages;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Products;
using SalesManagerSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.Interfaces.Services.Products
{
	public interface IProductService
	{
		Task<int> Create(ProductCreateViewModel request);

		Task<int> Update(ProductCreateViewModel request);

		Task<int> Delete(int productId);

		Task<ProductViewModel> GetById(int productId);

		Task<bool> UpdatePrice(int productId, decimal newPrice);

		Task<bool> UpdateStock(int productId, int addedQuantity);

		Task AddViewcount(int productId);

		Task<PagedResult<ProductViewModel>> GetAllPaging(ProductPagingViewModel request);


        Task<int> AddImage(int productId, ProductImageCreateRequest request);

        Task<int> RemoveImage(int imageId);

        Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request);

        Task<ProductImageViewModel> GetImageById(int imageId);

        Task<List<ProductImageViewModel>> GetListImages(int productId);

        Task<PagedResult<ProductViewModel>> GetAllByCategoryId(PublicProductPagingRequest request);

		Task<string> SearchProduct(string productName);

        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);

        Task<List<ProductViewModel>> GetFeaturedProducts(int take);

        Task<List<ProductViewModel>> GetLatestProducts(int take);
    }
}
