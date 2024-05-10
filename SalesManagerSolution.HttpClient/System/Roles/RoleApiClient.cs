using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Authentications;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.HttpClient.System.Roles
{
	public class RoleApiClient : BaseApiClient, IRoleApiClient
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _configuration;

		public RoleApiClient(IHttpClientFactory httpClientFactory,
				   IHttpContextAccessor httpContextAccessor,
					IConfiguration configuration)
		: base(httpClientFactory, httpContextAccessor, configuration)
		{
			_httpContextAccessor = httpContextAccessor;
			_configuration = configuration;
			_httpClientFactory = httpClientFactory;
		}

		public async Task<ApiResult<PagedResult<RoleVm>>> GetAll()
		{
			var data = await GetAsync<ApiResult<PagedResult<RoleVm>>>($"/api/roles");

			return data;
		}
	}
}
