using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Authentications;

namespace SalesManagerSolution.Core.Interfaces.Roles
{
	public interface IRoleService
	{
        Task<ApiResult<PagedResult<RoleVm>>> GetAll();
	}
}
