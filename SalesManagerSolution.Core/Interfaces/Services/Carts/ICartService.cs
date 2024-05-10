using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Carts;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Categories;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Carts;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Categories;
using SalesManagerSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.Interfaces.Services.Carts
{
	public interface ICartService
	{
		Task<List<CartViewModel>> GetAll(int userId);

		Task<int> GetCartItem(int userId);

		Task<CartViewModel> GetById(int id, int productId);

		Task<Cart> GetCartById(int id);

		Task<int> Create(CartResquestViewModel request);

		Task<int> Update(CartResquestViewModel request);

		Task<int> Delete(DeleteCartRequest request);

        Task<int> UpdateStatusCart(int id);
    }
}
