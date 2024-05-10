using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.Constants
{
	public class SystemConstants
	{
		public const string MainConnectionString = "DefaultConnection";
		public class AppSettings
		{
			public const string Token = "Token";
			public const string TokenUser = "TokenUser";
			public const string BaseAddress = "BaseAddress";
		}

		public class ProductSettings
		{
			public const int NumberOfFeaturedProducts = 20;
		}
	}
}
