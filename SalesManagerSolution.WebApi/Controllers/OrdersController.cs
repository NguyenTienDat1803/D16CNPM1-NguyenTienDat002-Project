using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesManagerSolution.Core.Interfaces.Orders;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Orders;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Products;

namespace SalesManagerSolution.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

		[HttpGet("paging")]
		public async Task<IActionResult> GetAllPaging([FromQuery] OrderPagingViewModel request)
		{
			var products = await _orderService.GetAllPagings(request);
			return Ok(products);
		}


	}
}
