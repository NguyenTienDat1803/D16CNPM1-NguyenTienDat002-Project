using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Infrastructure.Services.Authentications
{
	public class JwtSettings
	{
		public const string SectionsName = "JwtSettings";

		public string Secret { get; init; } = default!;

		public int ExpiryMinutes { get; init; } = default!;

		public string Audience { get; init; } = default!;

		public string Issuer { get; init; } = default!;
	}
}
