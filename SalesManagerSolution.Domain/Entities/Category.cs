using SalesManagerSolution.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesManagerSolution.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int SortOrder { set; get; }
        public bool IsShowOnHome { set; get; }
        public int? ParentId { set; get; }
        public Status Status { set; get; }
        public List<ProductInCategory> ProductInCategories { get; set; } = default!;
    }
}
