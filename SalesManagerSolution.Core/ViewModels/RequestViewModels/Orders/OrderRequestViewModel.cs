using SalesManagerSolution.Domain.Entities;
using SalesManagerSolution.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.ViewModels.RequestViewModels.Orders
{
    public class OrderRequestViewModel
    {
        public DateTime OrderDate { set; get; }
        public int UserId { set; get; }
        public string ShipName { set; get; } = default!;
        public string ShipAddress { set; get; } = default!;
        public string ShipEmail { set; get; } = default!;
        public string ShipPhoneNumber { set; get; } = default!;
        public OrderStatus Status { set; get; }
        public List<OrderDetailRequestViewModel> OrderDetails { get; set; } = default!;
    }
}
