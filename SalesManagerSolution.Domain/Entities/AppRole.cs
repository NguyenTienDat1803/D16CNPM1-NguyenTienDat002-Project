using Microsoft.AspNetCore.Identity;
namespace SalesManagerSolution.Domain.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public string Description { get; set; } = default!;
    }
}
