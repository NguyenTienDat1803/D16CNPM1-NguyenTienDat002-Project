using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Authentications;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Orders;
using SalesManagerSolution.HttpClient;
using SalesManagerSolution.HttpClient.System.Roles;
using SalesManagerSolution.HttpClient.System.User;

namespace SalesManagerSolution.AdminApp.Controllers
{
	public class OrderController : Controller
	{
		private readonly IUserHttpClient _userApiClient;
		private readonly IConfiguration _configuration;
		private readonly IOrderApiClient _orderApiClient;

		public OrderController(IUserHttpClient userApiClient,
			IOrderApiClient orderApiClient,
			IConfiguration configuration)
		{
			_userApiClient = userApiClient;
			_configuration = configuration;
			_orderApiClient = orderApiClient;
		}

		public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
		{
			var request = new OrderPagingViewModel()
			{
				Keyword = keyword,
				PageIndex = pageIndex,
				PageSize = pageSize
			};
			var data = await _orderApiClient.GetAll(request);
			ViewBag.Keyword = keyword;
			if (TempData["result"] != null)
			{
				ViewBag.SuccessMsg = TempData["result"];
			}
			return View(data);
		}

		
	}
}
