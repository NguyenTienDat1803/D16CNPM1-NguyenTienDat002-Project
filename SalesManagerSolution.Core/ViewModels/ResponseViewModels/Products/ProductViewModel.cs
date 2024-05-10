using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.ViewModels.ResponseViewModels.Products
{
    public class ProductViewModel
    {
        public int Id { set; get; }
        public decimal Price { set; get; }
        public decimal OriginalPrice { set; get; }
        public int Stock { set; get; }
        public int ViewCount { set; get; }
        public DateTime DateCreated { set; get; }
        public string Name { set; get; } = default!;
        public string Description { set; get; } = default!;
        public string Details { set; get; } = default!;
        public string SeoDescription { set; get; } = default!;
        public string SeoTitle { set; get; } = default!;
        public string SeoAlias { get; set; } = default!;
        public bool? IsFeatured { get; set; }
        public string ThumbnailImage { get; set; } = default!;
        public List<string>? CategoryName { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
    }
}
