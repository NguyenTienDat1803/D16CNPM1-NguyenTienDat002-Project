using SalesManagerSolution.Core.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.ViewModels.RequestViewModels.Authentications
{
	public class RoleAssignRequest
	{
		public int Id { get; set; }
		public List<SelectItem> Roles { get; set; } = new List<SelectItem>();
	}
}
