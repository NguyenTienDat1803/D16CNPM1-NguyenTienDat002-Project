using SalesManagerSolution.Core.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.ViewModels.ResponseViewModels.Products
{
    public class PublicProductPagingRequest : PagingRequestBase
    {
        public int? CategoryId { get; set; }
    }
}
