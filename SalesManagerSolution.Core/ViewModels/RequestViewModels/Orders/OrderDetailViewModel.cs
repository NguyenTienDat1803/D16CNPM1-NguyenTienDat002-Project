using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.ViewModels.RequestViewModels.Orders
{
    public class OrderDetailRequestViewModel
    {
        public int ProductId { set; get; }
        public int Quantity { set; get; }
        public decimal Price { set; get; }
    }
}
