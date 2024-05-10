using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.Constants
{
	public class AuthenticationConstant
	{
		//has all rights
		public const string RoleAdmin = "Administrator";

		//only can update his job application details after login
		public const string RoleApplicant = "User";

		//fulltime staff
		public const string RoleStaff = " Staff";

		//higher rank than staff, but lower than admin
		public const string RoleManager = "Manager";

		//part time staff
		public const string RoleTutor = "Tutor";

	}
}
