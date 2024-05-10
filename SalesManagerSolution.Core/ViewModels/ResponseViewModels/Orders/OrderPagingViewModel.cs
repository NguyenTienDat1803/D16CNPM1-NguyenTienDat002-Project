using SalesManagerSolution.Core.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.ViewModels.ResponseViewModels.Orders
{
    public class OrderPagingViewModel : PagingRequestBase
    {
        public string? Keyword { get; set; }

        public int? OrderId { get; set; }
    }
}
