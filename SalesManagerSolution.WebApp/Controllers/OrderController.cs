using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SalesManagerSolution.Core.Constants;
using SalesManagerSolution.Core.Interfaces.Orders;
using SalesManagerSolution.Core.Interfaces.Services.Carts;
using SalesManagerSolution.Core.Interfaces.Services.Products;
using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Carts;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Orders;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels;
using SalesManagerSolution.Database.Pages;
using SalesManagerSolution.Domain.Entities;
using SalesManagerSolution.HttpClient;
using SalesManagerSolution.Infrastructure.Services.Carts;
using SalesManagerSolution.Infrastructure.Services.Products;
using System.Drawing.Printing;
using System.Text.RegularExpressions;

namespace SalesManagerSolution.WebApp.Controllers
{
    [Route("[controller]")]
    [Authorize]
	public class OrderController : Controller
    {
		private readonly ILogger<OrderController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IOrderService _orderService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICartService _cartService;

        public OrderController(ILogger<OrderController> logger,
          IConfiguration configuration,
          IOrderService orderService,
          ICartService cartService,
          UserManager<AppUser> userManager)
		{
			_logger = logger;
			_configuration = configuration;
			_orderService = orderService;
			_userManager = userManager;
            _cartService = cartService;
		}

        public async Task<IActionResult> Index()
        {
            var userInfomation = this.ControllerContext.HttpContext.User.Identity;

            if (!userInfomation.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = Convert.ToInt32(this.ControllerContext.HttpContext.User.Claims.ToList()[0].Value);

            var resultOrders = await _orderService.GetOrderByUserId(userId);

            return View(resultOrders);
        }


        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create(ShippingViewModel request)
        {
            var userInfomation = this.ControllerContext.HttpContext.User.Identity;

            //  nếu chưa đăng nhập thì nó trả về badrequest
            if (!userInfomation.IsAuthenticated)
            {
                return BadRequest("Please login");
            }

            var userId = Convert.ToInt32(this.ControllerContext.HttpContext.User.Claims.ToList()[0].Value);

			var data = await _cartService.GetAll(userId);

            var orderDetails = new List<OrderDetailRequestViewModel>();

            foreach(var cart in data)
            {
                var orderDetail = new OrderDetailRequestViewModel()
                {
                    ProductId = cart.ProductId,
                    Price = cart.Price,
                    Quantity = cart.Quantity
                };

                orderDetails.Add(orderDetail);

               await _cartService.Delete(new DeleteCartRequest()
                {
                    Id = cart.Id,
                });

			}

			var model = new OrderRequestViewModel()
            {
                ShipPhoneNumber = request.ShipPhoneNumber,
                ShipAddress = request.ShipAddress,
                ShipEmail = request.ShipEmail,
                ShipName = request.ShipName,
                OrderDetails = orderDetails
            };

			model.UserId = userId;

            if (!ModelState.IsValid)
                return View(model);

            var result = await _orderService.CreateOrder(model);

            

            if (result > 0)
            {
                return RedirectToAction("Index","Home");
            }

            ModelState.AddModelError("", "Thêm danh mục thất bại");
            return View(model);
        }

    }
}
