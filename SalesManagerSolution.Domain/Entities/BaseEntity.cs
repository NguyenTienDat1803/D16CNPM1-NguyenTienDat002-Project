using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Domain.Entities
{
	public class BaseEntity
	{
		public int Id { get; set; }

		public bool IsDeleted { get; set; }

		public DateTime DeletedAt { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

		public void UpdateTimeStamps()
		{
			if (!IsDeleted && Id > 0)
			{
				UpdatedAt = DateTime.UtcNow;
			}
			else if (!IsDeleted && Id == 0)
			{
				CreatedAt = DateTime.UtcNow;
			}
			else if (IsDeleted && Id > 0)
			{
				DeletedAt = DateTime.UtcNow;
			}
			else if (IsDeleted && Id == 0)
			{
				throw new NotSupportedException("Cannot delete a new entity");
			}
		}
	}
}
