﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Authentications;
using SalesManagerSolution.HttpClient.System.Roles;
using SalesManagerSolution.HttpClient.System.User;

namespace SalesManagerSolution.AdminApp.Controllers
{
	public class UserController : Controller
	{
		private readonly IUserHttpClient _userApiClient;
		private readonly IConfiguration _configuration;
		private readonly IRoleApiClient _roleApiClient;

		public UserController(IUserHttpClient userApiClient,
			IRoleApiClient roleApiClient,
			IConfiguration configuration)
		{
			_userApiClient = userApiClient;
			_configuration = configuration;
			_roleApiClient = roleApiClient;
		}

		public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
		{
			var request = new GetUserPagingRequest()
			{
				Keyword = keyword,
				PageIndex = pageIndex,
				PageSize = pageSize
			};
			var data = await _userApiClient.GetUsersPagings(request);
			ViewBag.Keyword = keyword;
			if (TempData["result"] != null)
			{
				ViewBag.SuccessMsg = TempData["result"];
			}
			return View(data.ResultObj);
		}

		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			var result = await _userApiClient.GetById(id);
			return View(result.ResultObj);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(RegisterRequest request)
		{
			if (!ModelState.IsValid)
				return View();

			var result = await _userApiClient.RegisterUser(request);
			if (result.IsSuccessed)
			{
				TempData["result"] = "Thêm mới người dùng thành công";
				return RedirectToAction("Index");
			}

			ModelState.AddModelError("", result.Message);
			return View(request);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var result = await _userApiClient.GetById(id);
			if (result.IsSuccessed)
			{
				var user = result.ResultObj;
				var updateRequest = new UserUpdateRequest()
				{
					Dob = user.Dob,
					Email = user.Email,
					FirstName = user.FirstName,
					LastName = user.LastName,
					PhoneNumber = user.PhoneNumber,
					Id = id
				};
				return View(updateRequest);
			}
			return RedirectToAction("Error", "Home");
		}

		[HttpPost]
		public async Task<IActionResult> Edit(UserUpdateRequest request)
		{
			if (!ModelState.IsValid)
				return View();

			var result = await _userApiClient.UpdateUser(request.Id, request);
			if (result.IsSuccessed)
			{
				TempData["result"] = "Cập nhật người dùng thành công";
				return RedirectToAction("Index");
			}

			ModelState.AddModelError("", result.Message);
			return View(request);
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			HttpContext.Session.Remove("Token");
			return RedirectToAction("Index", "Login");
		}

		[HttpGet]
		public IActionResult Delete(int id)
		{
			return View(new UserDeleteRequest()
			{
				Id = id
			});
		}

		[HttpPost]
		public async Task<IActionResult> Delete(UserDeleteRequest request)
		{
			if (!ModelState.IsValid)
				return View();

			var result = await _userApiClient.Delete(request.Id);
			if (result.IsSuccessed)
			{
				TempData["result"] = "Xóa người dùng thành công";
				return RedirectToAction("Index");
			}

			ModelState.AddModelError("", result.Message);
			return View(request);
		}

		[HttpGet]
		public async Task<IActionResult> RoleAssign(int id)
		{
			var roleAssignRequest = await GetRoleAssignRequest(id);
			return View(roleAssignRequest);
		}

		[HttpPost]
		public async Task<IActionResult> RoleAssign(RoleAssignRequest request)
		{
			if (!ModelState.IsValid)
				return View();

			var result = await _userApiClient.RoleAssign(request.Id, request);

			if (result.IsSuccessed)
			{
				TempData["result"] = "Cập nhật quyền thành công";
				return RedirectToAction("Index");
			}

			ModelState.AddModelError("", result.Message);
			var roleAssignRequest = await GetRoleAssignRequest(request.Id);

			return View(roleAssignRequest);
		}

		private async Task<RoleAssignRequest> GetRoleAssignRequest(int id)
		{
			var userObj = await _userApiClient.GetById(id);
			var roleObj = await _roleApiClient.GetAll();
			var roleAssignRequest = new RoleAssignRequest();
			foreach (var role in roleObj.ResultObj.Items)
			{
				roleAssignRequest.Roles.Add(new SelectItem()
				{
					Id = role.Id.ToString(),
					Name = role.Name,
					Selected = userObj.ResultObj.Roles.Contains(role.Name)
				});
			}
			return roleAssignRequest;
		}
	}
}
