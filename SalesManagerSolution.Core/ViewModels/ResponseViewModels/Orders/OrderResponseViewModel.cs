using SalesManagerSolution.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.ViewModels.ResponseViewModels.Orders
{
    public class OrderResponseViewModel
    {
        public int Id { get; set; }
        public int UserId { set; get; }
        public string ShipName { set; get; } = default!;
        public string ShipAddress { set; get; } = default!;
        public string ShipEmail { set; get; } = default!;
        public string ShipPhoneNumber { set; get; } = default!;
        public OrderStatus Status { set; get; }
        public int TotalProduct { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
