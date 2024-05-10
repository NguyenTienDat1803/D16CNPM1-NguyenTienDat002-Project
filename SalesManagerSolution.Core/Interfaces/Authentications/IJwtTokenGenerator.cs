using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.Interfaces.Authentications
{
	public interface IJwtTokenGenerator
	{
		string GenerateToken(int id, string userName);
	}
}
