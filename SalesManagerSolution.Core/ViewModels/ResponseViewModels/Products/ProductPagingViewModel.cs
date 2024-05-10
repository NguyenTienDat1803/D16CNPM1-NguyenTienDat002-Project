using SalesManagerSolution.Core.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.ViewModels.ResponseViewModels.Products
{
	public class ProductPagingViewModel : PagingRequestBase
	{
		public string? Keyword { get; set; }

		public int? CategoryId { get; set; }
	}
}
