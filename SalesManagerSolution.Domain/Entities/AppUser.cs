using Microsoft.AspNetCore.Identity;

namespace SalesManagerSolution.Domain.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;

		public DateTime Dob { get; set; }

        public List<Cart> Carts { get; set; } = default!;

		public List<Order> Orders { get; set; } = default!;

		public List<Transaction> Transactions { get; set; } = default!;
	}
}
