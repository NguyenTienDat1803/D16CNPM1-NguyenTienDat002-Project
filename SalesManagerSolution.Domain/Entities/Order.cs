using SalesManagerSolution.Domain.Enums;

namespace SalesManagerSolution.Domain.Entities
{
   public class Order : BaseEntity
    {
        public DateTime OrderDate { set; get; }
        public int UserId { set; get; }
        public string ShipName { set; get; } = default!;
        public string ShipAddress { set; get; } = default!;
		public string ShipEmail { set; get; } = default!;
		public string ShipPhoneNumber { set; get; } = default!;
		public OrderStatus Status { set; get; }
        public List<OrderDetail> OrderDetails { get; set; } = default!;
		public AppUser AppUser { get; set; } = default!;


	}
}
