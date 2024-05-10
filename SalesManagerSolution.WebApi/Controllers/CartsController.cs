using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesManagerSolution.Core.Interfaces.Services.Carts;
using SalesManagerSolution.Core.Interfaces.Services.Categories;
using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Carts;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Categories;

namespace SalesManagerSolution.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize("Role=Admin,User")]
        public async Task<IActionResult> Create([FromForm] CartResquestViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedResult = await _cartService.Create(request);
            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }


        [HttpPut("{cartId}")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int categoryId, [FromForm] CartResquestViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.Id = categoryId;
            var affectedResult = await _cartService.Update(request);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }

        [HttpDelete("{cartId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int cartId)
        {
            var model = new DeleteCartRequest()
            {
                Id = cartId
            };

            var affectedResult = await _cartService.Delete(model);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(int userId)
        {
            var categories = await _cartService.GetAll(userId);
            return Ok(categories);
        }

        [HttpGet("GetCartItem/{userId}")]
        public async Task<IActionResult> GetCartItem(int userId)
        {
            var cartItem = await _cartService.GetCartItem(userId);
            return Ok(cartItem);
        }
    }
}
