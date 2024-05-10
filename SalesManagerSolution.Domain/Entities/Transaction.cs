using SalesManagerSolution.Domain.Enums;

namespace SalesManagerSolution.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public DateTime TransactionDate { set; get; }
        public string ExternalTransactionId { set; get; } = default!;
        public decimal Amount { set; get; }
        public decimal Fee { set; get; }
        public string Result { set; get; } = default!;
		public string Message { set; get; } = default!;
        public TransactionStatus Status { set; get; }
        public string Provider { set; get; } = default!;
        public int UserId { get; set; }
        public AppUser AppUser { get; set; } = default!;

    }
}
