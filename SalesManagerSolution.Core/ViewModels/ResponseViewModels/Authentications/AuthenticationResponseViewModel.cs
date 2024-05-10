using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.ViewModels.ResponseViewModels.Authentications
{
	public class AuthenticationResponseViewModel
	{
		public int Id { get; set; }
		public string UserName { get; set; } = default!;
		public string Token { get; set; } = default!;
	}
}
