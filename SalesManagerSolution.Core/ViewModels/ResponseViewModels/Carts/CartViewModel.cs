using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.ViewModels.ResponseViewModels.Carts
{
    public class CartViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
		public decimal Price { get; set; }
        public decimal SubTotal { get; set; }
        public string ThumnailImage { get; set; }
    }
}
