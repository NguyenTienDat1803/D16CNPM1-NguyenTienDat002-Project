using Microsoft.AspNetCore.Identity;

namespace SalesManagerSolution.Core.CustomException
{
	public class RegistrationException : Exception
	{
		public RegistrationException(IEnumerable<IdentityError> errors)
		{
			Errors = errors;
		}

		public IEnumerable<IdentityError>? Errors { get; set; } = default;
	}
}
