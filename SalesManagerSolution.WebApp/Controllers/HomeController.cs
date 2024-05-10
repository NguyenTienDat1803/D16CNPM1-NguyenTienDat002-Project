using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesManagerSolution.Core.Constants;
using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Carts;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels;
using SalesManagerSolution.Database.Pages;
using SalesManagerSolution.HttpClient;
using System.Drawing.Printing;
using System.Text.RegularExpressions;

namespace SalesManagerSolution.WebApp.Controllers
{
	public class HomeController : Controller
    {
		private readonly ILogger<HomeController> _logger;
		private readonly IProductApiClient _productApiClient;
		private readonly ICategoryApiClient _categoryApiClient;
		private readonly ICartApiClient _cartApiClient;

		public HomeController(ILogger<HomeController> logger,
		  IProductApiClient productApiClient,
		  ICategoryApiClient categoryApiClient,
		  ICartApiClient cartApiClient)
		{
			_logger = logger;
			_productApiClient = productApiClient;
			_categoryApiClient = categoryApiClient;
			_cartApiClient = cartApiClient;
		}

		public async Task<IActionResult> Index()
        {
			if (!this.ControllerContext.HttpContext.User.Identity.IsAuthenticated)
		    {
				return RedirectToAction("Login", "Account");
			}

			var resultProducts = await _productApiClient.GetFeaturedProducts(SystemConstants.ProductSettings.NumberOfFeaturedProducts);

			var userId = Convert.ToInt32(this.ControllerContext.HttpContext.User.Claims.ToList()[0].Value);

			var countProduct = await _cartApiClient.GetCartItem(userId);

			HttpContext.Session.SetInt32("CountItem", countProduct);

			var request1 = new PagingRequestBase()
            {
                PageIndex = 1,
                PageSize = 10
            };

            var resultCategories = await _categoryApiClient.GetAll(request1);

			var viewModel = new HomeViewModel
			{
				ProductViewModels = resultProducts,
				CategoryViewModels = resultCategories.Items
			};
			return View(viewModel);
        }

		public IActionResult Contact()
		{
			return View();
		}

		public IActionResult About()
		{
			return View();
		}

		public async Task<IActionResult> GetById(int productId)
		{
			var result = await _productApiClient.GetById(productId);

			return Json(result);
		}


	}
}
