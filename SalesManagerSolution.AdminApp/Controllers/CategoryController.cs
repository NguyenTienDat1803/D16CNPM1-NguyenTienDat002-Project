using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesManagerSolution.Core.Interfaces.Services.Products;
using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Categories;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Products;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Categories;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Products;
using SalesManagerSolution.Domain.Entities;
using SalesManagerSolution.HttpClient;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace SalesManagerSolution.AdminApp.Controllers
{
    [Route("[controller]")]
    public class CategoryController : Controller
	{
		private readonly IConfiguration _configuration;
		private readonly ICategoryApiClient _categoryApiClient;

		public CategoryController(
			IConfiguration configuration,
		   ICategoryApiClient categoryApiClient)
		{
			_configuration = configuration;
			_categoryApiClient = categoryApiClient;
		}

		// GET: CategoryController
		[HttpGet("Category/Index")]
		public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10)
		{
            var request = new PagingRequestBase()
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var data = await _categoryApiClient.GetAll(request);

			if (TempData["result"] != null)
			{
				ViewBag.SuccessMsg = TempData["result"];
			}
			return View(data);
		}

		//// GET: CategoryController/Details/5
		//public ActionResult Details(int id)
		//{
		//	return View();
		//}

		[HttpGet("Create")]
		public IActionResult Create()
		{
			return View();
		}


		[HttpPost("Create")]
        [Consumes("multipart/form-data")]
		public async Task<IActionResult> Create([FromForm] CategoryRequest request)
		{
			if (!ModelState.IsValid)
				return View(request);

			var result = await _categoryApiClient.CreateCategory(request);
			if (result)
			{
				TempData["result"] = "Thêm mới danh mục thành công";
				return RedirectToAction("Index");
			}

			ModelState.AddModelError("", "Thêm danh mục thất bại");
			return View(request);
		}


		[HttpGet("{categoryId}")]
		public async Task<IActionResult> GetById(int categoryId)
		{
			var category = await _categoryApiClient.GetById(categoryId);
			if (category == null)
				return BadRequest("Cannot find category");
			return Ok(category);
		}


		[HttpGet("Edit")]
		public async Task<IActionResult> Edit(int id)
		{

			var category = await _categoryApiClient.GetById(id);
			var editVm = new CategoryViewModel()
			{
				Id = category.Id,
				Description = category.Description,
				Name = category.Name
			};
			return View(editVm);
		}

		[HttpPost("Edit")]
		[Consumes("multipart/form-data")]
		public async Task<IActionResult> Edit([FromForm] CategoryRequest request)
		{
			if (!ModelState.IsValid)
				return View(request);

			var result = await _categoryApiClient.UpdateCategory(request);
			if (result)
			{
				TempData["result"] = "Cập nhật danh mục thành công";
				return RedirectToAction("Index");
			}

			ModelState.AddModelError("", "Cập nhật danh mục thất bại");
			return View(request);
		}

		[HttpGet("Delete")]
		public IActionResult Delete(int id)
		{
			return View(new CategoryDeleteRequest()
			{
				Id = id
			});
		}

		[HttpPost("Delete")]
		public async Task<IActionResult> Delete(CategoryDeleteRequest request)
		{
			if (!ModelState.IsValid)
				return View();

			var result = await _categoryApiClient.DeleteCategory(request.Id);
			if (result)
			{
				TempData["result"] = "Xóa danh mục thành công";
				return RedirectToAction("Index");
			}

			ModelState.AddModelError("", "Xóa không thành công");
			return View(request);
		}
	}
}
