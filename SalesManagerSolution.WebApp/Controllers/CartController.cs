using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SalesManagerSolution.Core.Interfaces.Services.Carts;
using SalesManagerSolution.Core.Interfaces.Services.Products;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Carts;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Authentications;
using SalesManagerSolution.Domain.Entities;
using SalesManagerSolution.HttpClient;

namespace SalesManagerSolution.WebApp.Controllers
{
		[Route("[controller]")]
	    public class CartController : Controller
		{
			private readonly IConfiguration _configuration;
			private readonly ICartService _cartService;
			private readonly IProductService _productService;
			private readonly UserManager<AppUser> _userManager;
		    private readonly ICartApiClient _cartApiClient;
		public CartController(
				IConfiguration configuration,
			   ICartService cartService,
			   IProductService productService,
			   ICartApiClient cartApiClient,
			   UserManager<AppUser> userManager)
			{
				_configuration = configuration;
				_cartService = cartService;
				_userManager = userManager;
			    _productService = productService;
			    _cartApiClient = cartApiClient;
			}

			// GET: CartController
			[HttpGet("Index")]
		    [AllowAnonymous]	
			public async Task<IActionResult> Index()
			{
			var userInfomation = this.ControllerContext.HttpContext.User.Identity;

			if (!userInfomation.IsAuthenticated)
			{
				return RedirectToAction("Login", "Account");
			}

			var userId = Convert.ToInt32(this.ControllerContext.HttpContext.User.Claims.ToList()[0].Value);

			var data = await _cartService.GetAll(userId);

			

				if (TempData["result"] != null)
				{
					ViewBag.SuccessMsg = TempData["result"];
				}
				return View(data);
			}

			//// GET: CartController/Details/5
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
			public async Task<IActionResult> Create(CartResquestViewModel request)
			{
				var userInfomation = this.ControllerContext.HttpContext.User.Identity;

				if (!userInfomation.IsAuthenticated)
				{
				   return BadRequest("Please login");
			    }

				var userId = Convert.ToInt32(this.ControllerContext.HttpContext.User.Claims.ToList()[0].Value);

				request.UserId = userId;

			    if (!ModelState.IsValid)
					return View(request);

				var result = await _cartService.Create(request);

			    await _productService.UpdateStock(request.ProductId, -request.Quantity);

				if (result > 0)
				{
					var countProduct = await _cartApiClient.GetCartItem(userId);

					HttpContext.Session.SetInt32("CountItem", countProduct);

					TempData["result"] = "Thêm mới danh mục thành công";
					return RedirectToAction("Index");
				}

				ModelState.AddModelError("", "Thêm danh mục thất bại");
					return View(request);
			}

		[HttpPost("UpdateQuantity")]
		public async Task<IActionResult> Update(int cartId,int productId, int quantity, bool addQuantity)
		{

			int quantityProduct = quantity;

			if (addQuantity)
			{
				quantityProduct = -quantity;
			}

			var request = await _cartService.GetById(cartId, productId);

			if(request == null)
			{
				ModelState.AddModelError("", "Giỏ hàng không tồn tại");
			}

			var userInfomation = this.ControllerContext.HttpContext.User.Identity;

			if (!userInfomation.IsAuthenticated)
			{
				return RedirectToAction("Login", "Account");
			}

			var userId = Convert.ToInt32(this.ControllerContext.HttpContext.User.Claims.ToList()[0].Value);


			var cartReques = new CartResquestViewModel() 
			{
				Id = cartId,
				Quantity = quantity,
				UserId = userId,
				Price = request.Price,
				ProductId = request.Id
			};

			var result = await _cartService.Update(cartReques);

			await _productService.UpdateStock(productId, quantityProduct);

			if (result > 0)
			{
				TempData["result"] = "Cập nhật giỏ hàng thành công";
				return RedirectToAction("Index");
			}

			ModelState.AddModelError("", "Cập nhật giỏ hàng thất bại");
			return View(request);
		}


		[HttpPost("Delete")]
		public async Task<IActionResult> Delete(int cartId)
		{
			if (!ModelState.IsValid)
				return View();

			var model = new DeleteCartRequest()
			{
				Id = cartId,
			};

			var cart = await _cartService.GetCartById(cartId);

			await _productService.UpdateStock(cart.ProductId, cart.Quantity);

			var result = await _cartService.Delete(model);
			if (result > 0)
			{
				var userId = Convert.ToInt32(this.ControllerContext.HttpContext.User.Claims.ToList()[0].Value);

				var countProduct = await _cartApiClient.GetCartItem(userId);

				HttpContext.Session.SetInt32("CountItem", countProduct);

				return RedirectToAction("Index");
			}

			ModelState.AddModelError("", "Xóa không thành công");
			return View();
		}
	}
	}
