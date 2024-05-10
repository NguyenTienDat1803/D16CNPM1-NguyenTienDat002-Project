using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SalesManagerSolution.Core.Interfaces.Authentications;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Authentications;
using SalesManagerSolution.WebApp.Models;
using System.Diagnostics;

namespace SalesManagerSolution.WebApp.Controllers
{
	public class AccountController : Controller
	{
		private readonly ILogger<AccountController> _logger;
		private readonly IConfiguration _configuration;
		private readonly IAuthenticationService _authenticationService;

		public AccountController(ILogger<AccountController> logger,
								  IConfiguration configuration,
								  IAuthenticationService authenticationService
			)
		{
			_logger = logger;
			_configuration = configuration;
			_authenticationService = authenticationService;
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult> Login(LoginRequestViewModel request)
		{
			if (!ModelState.IsValid)
				return View(request);

			// get token when user login in system
			try
			{
				var result = await _authenticationService.LoginAsync(request);
				return RedirectToAction("Index", "Home");
			}
			catch
			{
				return BadRequest("Login failed");
			}
		}


		[HttpGet]
		[AllowAnonymous]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult> Register(RegisterRequestViewModel request)
		{
			if (!ModelState.IsValid)
				return View(request);

			// get token when user login in system
			try
			{
				var result = await _authenticationService.RegisterAsync(request);
				return RedirectToAction("Login", "Account");
			}
			catch
			{
				return BadRequest("Register failed");
			}
		}
	}
}