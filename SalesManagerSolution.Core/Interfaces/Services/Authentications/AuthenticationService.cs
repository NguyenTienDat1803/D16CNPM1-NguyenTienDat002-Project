using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SalesManagerSolution.Core.Constants;
using SalesManagerSolution.Core.CustomException;
using SalesManagerSolution.Core.Interfaces.Authentications;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Authentications;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Authentications;
using SalesManagerSolution.Domain.Entities;

namespace SalesManagerSolution.Core.Services.Authentications
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly IJwtTokenGenerator _jwtTokenGenerator;
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly RoleManager<AppRole> _roleManager;
		private readonly IConfiguration _config;
		public AuthenticationService
		(
			IJwtTokenGenerator jwtTokenGenerator,
			UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager,
			RoleManager<AppRole> roleManager,
			IConfiguration config
		)
		{
			_jwtTokenGenerator = jwtTokenGenerator;
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_config = config;
		}

		/// <summary>
		/// Method using login user in system
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public async Task<AuthenticationResponseViewModel> LoginAsync(LoginRequestViewModel model, bool adminWeb = false)
		{
			//Find user by Username
			var user = await _userManager.FindByEmailAsync(model.Email);

			// if user is null return user not exists
			if (user is null) throw new Exception("User not found.");

			// get role by user
			var roles = await _userManager.GetRolesAsync(user);


			// check if user has role
			if (!adminWeb) //user web
			{
				if (roles == null)
				{
					throw new WrongRoleException("Your account role is not allowed.");
				}

				//check login with user
				var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

				if (!result.Succeeded) throw new Exception("Login failed.");

				// return response
				return new AuthenticationResponseViewModel
				{
					Id = user.Id,
					UserName = model.Email,
					Token = string.Empty
				};
			}
			else  //admin web
			{
				if (roles == null || (!roles.Contains(AuthenticationConstant.RoleAdmin) && !roles.Contains(AuthenticationConstant.RoleManager)))
				{
					throw new WrongRoleException("Your account role is not allowed.");
				}

				//get token when user login
				var token = _jwtTokenGenerator.GenerateToken(user.Id, model.Email);

				// return response
				return new AuthenticationResponseViewModel
				{
					Id = user.Id,
					UserName = model.Email,
					Token = token
				}; ;
			}


		}


		/// <summary>
		/// Method using register new user
		/// </summary>
		/// <param name="model">Email,password</param>
		/// <returns>AuthenticationResponse(Token)</returns>
		/// <exception cref="NotImplementedException"></exception>
		public async Task<AuthenticationResponseViewModel> RegisterAsync(RegisterRequestViewModel model)
		{
			// check if user exists 
			if (await _userManager.FindByEmailAsync(model.Email) != null)
			{
				throw new RegistrationException(new IdentityError[] { new IdentityError() { Code = "0", Description = "Account existed" } });
			}

			// create new user
			var user = new AppUser()
			{
				Email = model.Email,
				UserName = model.Email,
				PhoneNumber = model.PhoneNumber != default ? model.PhoneNumber : "",
				FirstName = model.FirstName,
				LastName = model.LastName
			};

			// add user to database
			var result = await _userManager.CreateAsync(user, model.Password);

			// check if create user failed
			if (!result.Succeeded)
			{
				throw new RegistrationException(result.Errors);
			}

			await _userManager.AddToRoleAsync(user, AuthenticationConstant.RoleApplicant);

			// create jwt token
			var token = _jwtTokenGenerator.GenerateToken(user.Id, model.Email);

			// return result
			return new AuthenticationResponseViewModel() 
			{
				Id = user.Id,
			    UserName = model.Email,
			    Token = token
			};
		}

		public async Task<bool> Logout()
		{
			await _signInManager.SignOutAsync();
			return true;
		}
	}
}
