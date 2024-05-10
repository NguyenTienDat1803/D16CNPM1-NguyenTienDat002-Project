using System;
using System.Collections.Generic;
using System.Text;

namespace SalesManagerSolution.Domain.Entities
{
    public class ProductImage : BaseEntity
    {

        public int ProductId { get; set; }

        public string ImagePath { get; set; } = default!;   

		public string Caption { get; set; } = default!;

        public bool IsDefault { get; set; }

        public DateTime DateCreated { get; set; }

        public int SortOrder { get; set; }

        public long FileSize { get; set; }

        public Product Product { get; set; } = default!;
    }
}
