using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Orders;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.HttpClient
{
	public class OrderApiClient : BaseApiClient, IOrderApiClient
	{
		public OrderApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : 
			base(httpClientFactory, httpContextAccessor, configuration)
		{
		}

		public async Task<PagedResult<OrderResponseViewModel>> GetAll(OrderPagingViewModel request)
		{
			string url = $"/api/orders/paging?PageIndex={request.PageIndex}" +
			   $"&PageSize={request.PageSize}";

			if (!string.IsNullOrEmpty(request.Keyword))
			{
				url += $"&Keyword={request.Keyword}";
			}

			if (request.OrderId != null)
			{
				url += $"&OrderId ={request.OrderId}";
			}

			var data = await GetAsync<PagedResult<OrderResponseViewModel>>(url);

			return data;
		}
	}
}
