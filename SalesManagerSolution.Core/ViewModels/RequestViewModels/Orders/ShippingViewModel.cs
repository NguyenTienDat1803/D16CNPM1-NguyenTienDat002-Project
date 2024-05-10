using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.ViewModels.RequestViewModels.Orders
{
	public class ShippingViewModel
	{
		public string ShipName { set; get; } = default!;
		public string ShipAddress { set; get; } = default!;
		public string ShipEmail { set; get; } = default!;
		public string ShipPhoneNumber { set; get; } = default!;
	}
}
