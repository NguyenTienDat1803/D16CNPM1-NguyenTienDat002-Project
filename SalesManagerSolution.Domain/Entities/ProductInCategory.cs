using System;
using System.Collections.Generic;
using System.Text;

namespace SalesManagerSolution.Domain.Entities
{
    public class ProductInCategory : BaseEntity
    {
        public int ProductId { get; set; }

        public Product Product { get; set; } = default!;

        public int CategoryId { get; set; }

        public Category Category { get; set; } = default!;
    }
}
