using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Categories;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.HttpClient
{
	public interface IOrderApiClient
	{
		Task<PagedResult<OrderResponseViewModel>> GetAll(OrderPagingViewModel request);
	}
}
