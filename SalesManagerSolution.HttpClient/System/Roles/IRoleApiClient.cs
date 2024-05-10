using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Authentications;

namespace SalesManagerSolution.HttpClient.System.Roles
{
	public interface IRoleApiClient
	{
		Task<ApiResult<PagedResult<RoleVm>>> GetAll();
	}
}
