using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Categories;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.ViewModels.ResponseViewModels
{
	public class HomeViewModel
	{
		public List<ProductViewModel> ProductViewModels { get; set; } = default!;

        public List<CategoryViewModel> CategoryViewModels { get; set; } = default!;
	}
}
