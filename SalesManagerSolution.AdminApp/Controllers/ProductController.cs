using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesManagerSolution.Core.Constants;
using SalesManagerSolution.Core.Interfaces.Services.Products;
using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Products;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Products;
using SalesManagerSolution.HttpClient;

namespace SalesManagerSolution.AdminApp.Controllers
{
    [Route("[controller]")]
    public class ProductController : Controller
	{
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;
        private readonly ICategoryApiClient _categoryApiClient;

        public ProductController(IProductApiClient productApiClient,
            IConfiguration configuration,
           ICategoryApiClient categoryApiClient)
        {
            _configuration = configuration;
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 10)
        {
            keyword = string.Empty;
            var request = new ProductPagingViewModel()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                CategoryId = categoryId
            };
            var data = await _productApiClient.GetPagings(request);
            ViewBag.Keyword = keyword;

            var requestBase = new PagingRequestBase()
            {
                PageIndex = 1,
                PageSize = 10
            };

            var categories = await _categoryApiClient.GetAll(requestBase);

            ViewBag.Categories = categories.Items.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });

            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost("Create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateViewModel request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _productApiClient.CreateProduct(request);
            if (result)
            {
                TempData["result"] = "Thêm mới sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm sản phẩm thất bại");
            return View(request);
        }

        [HttpGet("GetById/{productId}")]
        public async Task<IActionResult> GetById(int productId)
        {
            var product = await _productApiClient.GetById(productId);
            if (product == null)
                return BadRequest("Cannot find product");
            return Ok(product);
        }

        [HttpGet("CategoryAssign")]
        public async Task<IActionResult> CategoryAssign(int id)
        {
            var roleAssignRequest = await GetCategoryAssignRequest(id);
            return View(roleAssignRequest);
        }

        [HttpPost("CategoryAssign")]
        public async Task<IActionResult> CategoryAssign(CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _productApiClient.CategoryAssign(request.Id, request);

            if (result.IsSuccessed)
            {
                TempData["result"] = "Cập nhật danh mục thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            var roleAssignRequest = await GetCategoryAssignRequest(request.Id);

            return View(roleAssignRequest);
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(int id)
        {

            var product = await _productApiClient.GetById(id);

            if(product is null)
            {
                return BadRequest();
            }

            var editVm = new ProductCreateViewModel()
            {
                Id = product.Id,
                Description = product.Description,
                Name = product.Name,
                Stock = product.Stock,
                Price = product.Price,
                OriginalPrice = product.OriginalPrice,
                IsFeatured = product.IsFeatured
            };
            return View(editVm);
        }

        [HttpPost("Edit")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] ProductCreateViewModel request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _productApiClient.UpdateProduct(request);
            if (result)
            {
                TempData["result"] = "Cập nhật sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Cập nhật sản phẩm thất bại");
            return View(request);
        }

        private async Task<CategoryAssignRequest> GetCategoryAssignRequest(int id)
        {
            var requestBase = new PagingRequestBase()
            {
                PageIndex = 1,
                PageSize = 10
            };

            var productObj = await _productApiClient.GetById(id);

            var categories = await _categoryApiClient.GetAll(requestBase);

            var categoryAssignRequest = new CategoryAssignRequest();

            foreach (var role in categories.Items)
            {
                categoryAssignRequest.Categories.Add(new SelectItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = productObj.Categories.Contains(role.Name)
                });
            }
            return categoryAssignRequest;
        }

        [HttpGet("Delete")]
        public IActionResult Delete(int id)
        {
            return View(new ProductDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(ProductDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _productApiClient.DeleteProduct(request.Id);
            if (result)
            {
                TempData["result"] = "Xóa sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Xóa không thành công");
            return View(request);
        }
    }
}
