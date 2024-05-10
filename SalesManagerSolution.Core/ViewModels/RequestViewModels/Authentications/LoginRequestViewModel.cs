using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.ViewModels.RequestViewModels.Authentications
{
	public class LoginRequestViewModel
	{
		[Required]
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
	}
}
