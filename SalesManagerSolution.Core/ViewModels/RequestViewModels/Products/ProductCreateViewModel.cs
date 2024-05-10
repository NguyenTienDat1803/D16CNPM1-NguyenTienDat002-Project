using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.ViewModels.RequestViewModels.Products
{
	public class ProductCreateViewModel
	{
        public int Id { get; set; }
        public decimal Price { set; get; }
		public decimal OriginalPrice { set; get; }
		public int Stock { set; get; }

		[Required(ErrorMessage = "Bạn phải nhập tên sản phẩm")]
		public string Name { set; get; } = default!;
		public string Description { set; get; } = default!;
		public bool? IsFeatured { get; set; }
		public IFormFile ThumbnailImage { get; set; } = default!;
	}
}
