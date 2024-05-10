using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesManagerSolution.Core.Interfaces.Services.Categories;
using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Categories;

namespace SalesManagerSolution.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

		[HttpPost]
		[Consumes("multipart/form-data")]
		[Authorize]
		public async Task<IActionResult> Create([FromForm] CategoryRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var affectedResult = await _categoryService.Create(request);
			if (affectedResult == 0)
				return BadRequest();

			return Ok();
		}


		[HttpPut("{categoryId}")]
		[Consumes("multipart/form-data")]
		[Authorize]
		public async Task<IActionResult> Update([FromRoute] int categoryId, [FromForm] CategoryRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			request.Id = categoryId;
			var affectedResult = await _categoryService.Update(request);
			if (affectedResult == 0)
				return BadRequest();
			return Ok();
		}

		[HttpDelete("{categoryId}")]
		[Authorize]
		public async Task<IActionResult> Delete(int categoryId)
		{
			var model = new CategoryDeleteRequest() 
			                {
				               Id = categoryId 
			                };

			var affectedResult = await _categoryService.Delete(model);
			if (affectedResult == 0)
				return BadRequest();
			return Ok();
		}


		[HttpGet("paging")]
        public async Task<IActionResult> GetAll([FromQuery] PagingRequestBase request)
        {
            var categories = await _categoryService.GetAll(request);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetById(id);
            return Ok(category);
        }
    }
}
