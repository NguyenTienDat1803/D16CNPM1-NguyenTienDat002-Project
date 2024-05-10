using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Orders;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Orders;
using SalesManagerSolution.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.Interfaces.Orders
{
    public interface IOrderService
    {
        Task<PagedResult<OrderResponseViewModel>> GetAllPagings(OrderPagingViewModel request);

        Task<int> CreateOrder(OrderRequestViewModel request);

        Task<int> UpdateOrder(OrderRequestViewModel request);

        Task<OrderResponseViewModel> GetOrderById(int id);

        Task<List<OrderResponseViewModel>> GetOrderByUserId(int userId);

        Task<int> UpdateStatusOrder(int orderId, OrderStatus orderStatus);
    }
}
