using SalesManagerSolution.Core.ViewModels.RequestViewModels.Authentications;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Authentications;

namespace SalesManagerSolution.Core.Interfaces.Authentications
{
    public interface IAuthenticationService
	{
		Task<AuthenticationResponseViewModel> LoginAsync(LoginRequestViewModel model, bool adminWeb = false);
		Task<AuthenticationResponseViewModel> RegisterAsync(RegisterRequestViewModel model);
		Task<bool> Logout();
	}
}
