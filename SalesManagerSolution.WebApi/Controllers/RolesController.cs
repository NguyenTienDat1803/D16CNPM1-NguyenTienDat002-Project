using Microsoft.AspNetCore.Mvc;
using SalesManagerSolution.Core.Interfaces.Roles;

namespace SalesManagerSolution.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RolesController : ControllerBase
	{
		private readonly IRoleService _roleService;

		public RolesController(IRoleService roleService)
		{
			_roleService = roleService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var roles = await _roleService.GetAll();
			return Ok(roles);
		}
	}
}
