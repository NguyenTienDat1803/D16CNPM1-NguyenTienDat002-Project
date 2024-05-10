using System;
using System.Collections.Generic;
using System.Text;

namespace SalesManagerSolution.Core.ViewModels.Common
{
    public class PagedResult<T> : PagedResultBase
    {
        public List<T> Items { set; get; }
    }
}