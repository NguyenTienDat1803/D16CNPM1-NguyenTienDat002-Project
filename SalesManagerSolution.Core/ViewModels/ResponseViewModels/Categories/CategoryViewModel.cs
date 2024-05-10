using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.ViewModels.ResponseViewModels.Categories
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;
		public string Description { get; set; } = default!;

		public int? ParentId { get; set; }
    }
}
