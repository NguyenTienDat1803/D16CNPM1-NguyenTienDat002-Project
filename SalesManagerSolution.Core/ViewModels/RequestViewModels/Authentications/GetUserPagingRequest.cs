using SalesManagerSolution.Core.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.ViewModels.RequestViewModels.Authentications
{
	public class GetUserPagingRequest : PagingRequestBase
	{
		public string? Keyword { get; set; }
	}
}
