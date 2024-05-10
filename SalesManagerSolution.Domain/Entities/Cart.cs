using SalesManagerSolution.Domain.Enums;

namespace SalesManagerSolution.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public int ProductId { set; get; }
        public int Quantity { set; get; }
        public decimal Price { set; get; }
        public int UserId { get; set; }
        public int TestId { get; set; }
        public Product Product { get; set; } = default!;
        public CartStatus CartStatus { get; set; }
        public DateTime DateCreated { get; set; }
        public AppUser AppUser { get; set; } = default!;
	}
}
