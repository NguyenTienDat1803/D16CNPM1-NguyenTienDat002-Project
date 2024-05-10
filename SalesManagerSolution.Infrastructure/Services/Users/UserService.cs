﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SalesManagerSolution.Core.Interfaces.Users;
using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Authentications;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Authentications;
using SalesManagerSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Infrastructure.Services.Users
{
	public class UserService : IUserService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly RoleManager<AppRole> _roleManager;
		private readonly IConfiguration _config;

		public UserService(UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager,
			RoleManager<AppRole> roleManager,
			IConfiguration config)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_config = config;
		}

		public async Task<ApiResult<bool>> Delete(int id)
		{
			var user = await _userManager.FindByIdAsync(id.ToString());
			if (user == null)
			{
				return new ApiErrorResult<bool>("User không tồn tại");
			}
			var reult = await _userManager.DeleteAsync(user);
			if (reult.Succeeded)
				return new ApiSuccessResult<bool>();

			return new ApiErrorResult<bool>("Xóa không thành công");
		}

		public async Task<ApiResult<UserVm>> GetById(int id)
		{
			var user = await _userManager.FindByIdAsync(id.ToString());
			if (user == null)
			{
				return new ApiErrorResult<UserVm>("User không tồn tại");
			}
			var roles = await _userManager.GetRolesAsync(user);
			var userVm = new UserVm()
			{
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				FirstName = user.FirstName,
				Dob = user.Dob,
				Id = user.Id,
				LastName = user.LastName,
				UserName = user.UserName,
				Roles = roles
			};
			return new ApiSuccessResult<UserVm>(userVm);
		}

		public async Task<ApiResult<PagedResult<UserVm>>> GetUsersPaging(GetUserPagingRequest request)
		{
			var query = _userManager.Users;
			if (!string.IsNullOrEmpty(request.Keyword))
			{
				query = query.Where(x => x.UserName.Contains(request.Keyword)
				 || x.PhoneNumber.Contains(request.Keyword));
			}

			//3. Paging
			int totalRow = await query.CountAsync();

			var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
				.Take(request.PageSize)
				.Select(x => new UserVm()
				{
					Email = x.Email,
					PhoneNumber = x.PhoneNumber,
					UserName = x.UserName,
					FirstName = x.FirstName,
					Id = x.Id,
					LastName = x.LastName
				}).ToListAsync();

			//4. Select and projection
			var pagedResult = new PagedResult<UserVm>()
			{
				TotalRecords = totalRow,
				PageIndex = request.PageIndex,
				PageSize = request.PageSize,
				Items = data
			};
			return new ApiSuccessResult<PagedResult<UserVm>>(pagedResult);
		}

		public async Task<ApiResult<bool>> Register(RegisterRequest request)
		{
			var user = await _userManager.FindByNameAsync(request.UserName);
			if (user != null)
			{
				return new ApiErrorResult<bool>("Tài khoản đã tồn tại");
			}
			if (await _userManager.FindByEmailAsync(request.Email) != null)
			{
				return new ApiErrorResult<bool>("Emai đã tồn tại");
			}

			user = new AppUser()
			{
				Dob = request.Dob,
				Email = request.Email,
				FirstName = request.FirstName,
				LastName = request.LastName,
				UserName = request.UserName,
				PhoneNumber = request.PhoneNumber
			};
			var result = await _userManager.CreateAsync(user, request.Password);
			if (result.Succeeded)
			{
				return new ApiSuccessResult<bool>();
			}
			return new ApiErrorResult<bool>("Đăng ký không thành công");
		}

		public async Task<ApiResult<bool>> RoleAssign(int id, RoleAssignRequest request)
		{
			var user = await _userManager.FindByIdAsync(id.ToString());
			if (user == null)
			{
				return new ApiErrorResult<bool>("Tài khoản không tồn tại");
			}
			var removedRoles = request.Roles.Where(x => x.Selected == false).Select(x => x.Name).ToList();
			foreach (var roleName in removedRoles)
			{
				if (await _userManager.IsInRoleAsync(user, roleName) == true)
				{
					await _userManager.RemoveFromRoleAsync(user, roleName);
				}
			}
			await _userManager.RemoveFromRolesAsync(user, removedRoles);

			var addedRoles = request.Roles.Where(x => x.Selected).Select(x => x.Name).ToList();
			foreach (var roleName in addedRoles)
			{
				if (await _userManager.IsInRoleAsync(user, roleName) == false)
				{
					await _userManager.AddToRoleAsync(user, roleName);
				}
			}

			return new ApiSuccessResult<bool>();
		}

		public async Task<ApiResult<bool>> Update(int id, UserUpdateRequest request)
		{
			if (await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != id))
			{
				return new ApiErrorResult<bool>("Emai đã tồn tại");
			}
			var user = await _userManager.FindByIdAsync(id.ToString());
			user.Dob = request.Dob;
			user.Email = request.Email;
			user.FirstName = request.FirstName;
			user.LastName = request.LastName;
			user.PhoneNumber = request.PhoneNumber;

			var result = await _userManager.UpdateAsync(user);
			if (result.Succeeded)
			{
				return new ApiSuccessResult<bool>();
			}
			return new ApiErrorResult<bool>("Cập nhật không thành công");
		}
	}
}