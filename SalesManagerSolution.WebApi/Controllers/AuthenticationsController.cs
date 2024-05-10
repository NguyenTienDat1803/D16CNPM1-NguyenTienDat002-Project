using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesManagerSolution.Core.Interfaces.Authentications;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Authentications;

namespace SalesManagerSolution.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticationsController : ControllerBase
	{
		private readonly IAuthenticationService _authenticationService;

		public AuthenticationsController(IAuthenticationService authenticationService)
		{
			_authenticationService = authenticationService;
		}

		/// <summary>
		/// Login Method
		/// </summary>
		/// <param name="loginRequestViewModel"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost("Login")]
		public async Task<ActionResult> Login(LoginRequestViewModel loginRequestViewModel)
		{
			// if model not valid return bad request
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			// get token when user login in systerm
			var token = await _authenticationService.LoginAsync(loginRequestViewModel, true);

			// return result
			return Ok(token);
		}
	}
}