using Microsoft.AspNetCore.Mvc;
using SalesManagerSolution.Core.Interfaces.Services.Products;

namespace SalesManagerSolution.WebApp.Controllers
{
	[Route("[controller]")]
	public class ProductController : Controller
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}


		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			var result = await _productService.GetById(id);
			return View(result);
		}
	}
}
