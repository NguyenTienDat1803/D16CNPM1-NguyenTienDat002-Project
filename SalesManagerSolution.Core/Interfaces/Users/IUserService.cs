using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Authentications;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Authentications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.Interfaces.Users
{
	public interface IUserService
	{
		Task<ApiResult<bool>> Register(RegisterRequest request);

		Task<ApiResult<bool>> Update(int id, UserUpdateRequest request);

		Task<ApiResult<PagedResult<UserVm>>> GetUsersPaging(GetUserPagingRequest request);

		Task<ApiResult<UserVm>> GetById(int id);

		Task<ApiResult<bool>> Delete(int id);

		Task<ApiResult<bool>> RoleAssign(int id, RoleAssignRequest request);
	}
}
