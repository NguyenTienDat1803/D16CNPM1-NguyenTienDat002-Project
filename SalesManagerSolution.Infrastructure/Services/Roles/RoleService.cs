using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalesManagerSolution.Core.Interfaces.Roles;
using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Authentications;
using SalesManagerSolution.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SalesManagerSolution.Infrastructure.Services.Roles
{
	public class RoleService : IRoleService
	{
		private readonly RoleManager<AppRole> _roleManager;

		public RoleService(RoleManager<AppRole> roleManager)
		{
			_roleManager = roleManager;
		}

		public async Task<ApiResult<PagedResult<RoleVm>>> GetAll()
        {
			var query = _roleManager.Roles.AsQueryable();

            int totalRow = await query.CountAsync();

            var data = await query
                            .Select(x => new RoleVm()
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Description = x.Description
                            }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<RoleVm>()
            {
                TotalRecords = totalRow,
                PageIndex = 1,
                PageSize = 10,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<RoleVm>>(pagedResult);
        }
	}
}
