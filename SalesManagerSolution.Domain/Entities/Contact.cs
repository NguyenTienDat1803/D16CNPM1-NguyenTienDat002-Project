using SalesManagerSolution.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesManagerSolution.Domain.Entities
{
    public class Contact : BaseEntity
    {
        public string Name { set; get; } = default!;
        public string Email { set; get; } = default!;
        public string PhoneNumber { set; get; } = default!;
		public string Message { set; get; } = default!;
        public Status Status { set; get; }

    }
}
