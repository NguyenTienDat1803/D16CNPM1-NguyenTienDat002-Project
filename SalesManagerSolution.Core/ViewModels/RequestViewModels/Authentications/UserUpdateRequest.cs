using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SalesManagerSolution.Core.ViewModels.RequestViewModels.Authentications
{
	public class UserUpdateRequest
	{
		public int Id { get; set; }

		[Display(Name = "Tên")]
		public string FirstName { get; set; }

		[Display(Name = "Họ")]
		public string LastName { get; set; }

		[Display(Name = "Ngày sinh")]
		[DataType(DataType.Date)]
		public DateTime Dob { get; set; }

		[Display(Name = "Hòm thư")]
		public string Email { get; set; }

		[Display(Name = "Số điện thoại")]
		public string PhoneNumber { get; set; }
	}
}
