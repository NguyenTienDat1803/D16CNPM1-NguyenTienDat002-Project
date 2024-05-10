using System;
using System.Collections.Generic;
using System.Text;

namespace SalesManagerSolution.Domain.Entities
{
    public class OrderDetail : BaseEntity
    {
        public int OrderId { set; get; }
        public int ProductId { set; get; }
        public int Quantity { set; get; }
        public decimal Price { set; get; }
        public Order Order { get; set; } = default!;
        public Product Product { get; set; } = default!;

	}
}
