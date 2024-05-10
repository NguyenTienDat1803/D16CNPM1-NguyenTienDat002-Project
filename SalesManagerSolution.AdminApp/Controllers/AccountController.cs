using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using SalesManagerSolution.Core.Constants;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Authentications;
using SalesManagerSolution.HttpClient.System.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SalesManagerSolution.AdminApp.Controllers
{
	[Authorize]
    public class AccountController : Controller
    {
		private readonly ILogger<AccountController> _logger;
		private readonly IUserHttpClient _userApiClient;
		private readonly IConfiguration _configuration;
		public AccountController(ILogger<AccountController> logger, IUserHttpClient userApiClient, IConfiguration configuration)
		{
			_logger = logger;
			_userApiClient = userApiClient;
			_configuration = configuration;
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login(LoginRequestViewModel request)
		{
			request.Password = "admin";
			if (!ModelState.IsValid)
				return View(ModelState);

			var result = await _userApiClient.Authencate(request);
			if (result.ResultObj == null)
			{
				ModelState.AddModelError("", result.Message);
				return View();
			}
			var userPrincipal = this.ValidateToken(result.ResultObj);
			var authProperties = new AuthenticationProperties
			{
				ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
				IsPersistent = false
			};

            Set(SystemConstants.AppSettings.Token, result.ResultObj, 10);

            await HttpContext.SignInAsync(
					   CookieAuthenticationDefaults.AuthenticationScheme,
					   userPrincipal,
					   authProperties);

			return RedirectToAction("Index", "Home");
		}

		private ClaimsPrincipal ValidateToken(string jwtToken)
		{
			IdentityModelEventSource.ShowPII = true;

			SecurityToken validatedToken;
			TokenValidationParameters validationParameters = new TokenValidationParameters();

			validationParameters.ValidateLifetime = true;

			validationParameters.ValidAudience = _configuration["JwtSettings:Issuer"];
			validationParameters.ValidIssuer = _configuration["JwtSettings:Issuer"];
			validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));

			ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

			return principal;
		}

        public void Set(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            Response.Cookies.Append(key, value, option);
        }
    }
}
