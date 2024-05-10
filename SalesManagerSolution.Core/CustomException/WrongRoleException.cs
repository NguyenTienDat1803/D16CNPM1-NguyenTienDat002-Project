using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.CustomException
{
	public class WrongRoleException : Exception
	{
		public WrongRoleException(string message) : base(message)
		{
		}
	}
}
