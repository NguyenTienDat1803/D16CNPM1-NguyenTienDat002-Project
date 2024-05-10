using SalesManagerSolution.Domain.Enums;

namespace SalesManagerSolution.Domain.Entities
{
    public class Slide : BaseEntity
    {
        public string Name { set; get; } = default!;
        public string Description { set; get; } = default!;
		public string Url { set; get; } = default!;
        public string Image { get; set; } = default!;
        public int SortOrder { get; set; }
        public Status Status { set; get; }
    }
}