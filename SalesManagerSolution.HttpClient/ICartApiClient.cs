using SalesManagerSolution.Core.ViewModels.RequestViewModels.Carts;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Carts;

namespace SalesManagerSolution.HttpClient
{
    public interface ICartApiClient
    {
        Task<List<CartViewModel>> GetAll(int userId);

        Task<CartViewModel> GetById(int id);

		Task<bool> CreateCart(CartResquestViewModel request);

		Task<int> GetCartItem(int userId);

		Task<bool> UpdateCart(CartResquestViewModel request);

        Task<bool> DeleteCart(int id);
		
	}
}
