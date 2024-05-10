using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SalesManagerSolution.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = default!;
		public string Description { get; set; } = default!;
		public decimal Price { set; get; }
        public decimal OriginalPrice { set; get; }
        public int Stock { set; get; }
        public int ViewCount { set; get; }
        public DateTime DateCreated { set; get; }
        public bool? IsFeatured { get; set; }
        public List<ProductInCategory> ProductInCategories { get; set; } = default!;

        public List<OrderDetail> OrderDetails { get; set; } = default!; 

        public List<Cart> Carts { get; set; } = default!;

        public List<ProductImage> ProductImages { get; set; } = default!;
	}
}