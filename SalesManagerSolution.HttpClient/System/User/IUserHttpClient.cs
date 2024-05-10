using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Authentications;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Authentications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.HttpClient.System.User
{
	public interface IUserHttpClient
	{
		Task<ApiResult<string>> Authencate(LoginRequestViewModel request);

		Task<ApiResult<PagedResult<UserVm>>> GetUsersPagings(GetUserPagingRequest request);

		Task<ApiResult<bool>> RegisterUser(RegisterRequest registerRequest);

		Task<ApiResult<bool>> UpdateUser(int id, UserUpdateRequest request);

		Task<ApiResult<UserVm>> GetById(int id);

		Task<ApiResult<bool>> Delete(int id);

		Task<ApiResult<bool>> RoleAssign(int id, RoleAssignRequest request);
	}
}
