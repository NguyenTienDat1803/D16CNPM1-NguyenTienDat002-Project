using SalesManagerSolution.Core.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SalesManagerSolution.Core.Constants;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Authentications;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Authentications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Products;

namespace SalesManagerSolution.HttpClient.System.User
{
	public class UserHttpClient : BaseApiClient, IUserHttpClient
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _configuration;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UserHttpClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ApiResult<string>> Authencate(LoginRequestViewModel request)
		{
			var json = JsonConvert.SerializeObject(request);
			var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

			var client = _httpClientFactory.CreateClient();
			client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
			var response = await client.PostAsync("/api/Authentications/Login", httpContent);

			if (response.IsSuccessStatusCode)
			{
				var token =  JsonConvert.DeserializeObject<AuthenticationResponseViewModel>(await response.Content.ReadAsStringAsync());

				return new ApiSuccessResult<string>(token.Token);
			}

			return JsonConvert.DeserializeObject<ApiErrorResult<string>>(await response.Content.ReadAsStringAsync()) ?? new ApiErrorResult<string>(string.Empty);
		}

		public async Task<ApiResult<bool>> Delete(int id)
		{
            var sessions = _httpContextAccessor
                .HttpContext
                .Request
                .Cookies[SystemConstants.AppSettings.Token];
            var client = _httpClientFactory.CreateClient();
			client.BaseAddress = new Uri(_configuration["BaseAddress"]);
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
			var response = await client.DeleteAsync($"/api/users/{id}");
			var body = await response.Content.ReadAsStringAsync();
			if (response.IsSuccessStatusCode)
				return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(body);

			return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(body);
		}

		public async Task<ApiResult<UserVm>> GetById(int id)
		{
            var sessions = _httpContextAccessor
                .HttpContext
                .Request
                .Cookies[SystemConstants.AppSettings.Token];

            var client = _httpClientFactory.CreateClient();
			client.BaseAddress = new Uri(_configuration["BaseAddress"]);
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
			var response = await client.GetAsync($"/api/users/{id}");
			var body = await response.Content.ReadAsStringAsync();
			if (response.IsSuccessStatusCode)
				return JsonConvert.DeserializeObject<ApiSuccessResult<UserVm>>(body);

			return JsonConvert.DeserializeObject<ApiErrorResult<UserVm>>(body);
		}

		public async Task<ApiResult<PagedResult<UserVm>>> GetUsersPagings(GetUserPagingRequest request)
		{
            string url = $"/api/users/paging?PageIndex={request.PageIndex}" +
                $"&PageSize={request.PageSize}";

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                url += $"&Keyword={request.Keyword}";
            }

            var data = await GetAsync<ApiResult<PagedResult<UserVm>>>(url);

            return data;
        }

		public async Task<ApiResult<bool>> RegisterUser(RegisterRequest registerRequest)
		{
			var client = _httpClientFactory.CreateClient();
			client.BaseAddress = new Uri(_configuration["BaseAddress"]);

			var json = JsonConvert.SerializeObject(registerRequest);
			var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await client.PostAsync($"/api/users", httpContent);
			var result = await response.Content.ReadAsStringAsync();
			if (response.IsSuccessStatusCode)
				return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

			return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
		}

		public async Task<ApiResult<bool>> RoleAssign(int id, RoleAssignRequest request)
		{
			var client = _httpClientFactory.CreateClient();
			client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var sessions = _httpContextAccessor
                .HttpContext
                .Request
                .Cookies[SystemConstants.AppSettings.Token];

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

			var json = JsonConvert.SerializeObject(request);
			var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await client.PutAsync($"/api/users/{id}/roles", httpContent);
			var result = await response.Content.ReadAsStringAsync();
			if (response.IsSuccessStatusCode)
				return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

			return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
		}

		public async Task<ApiResult<bool>> UpdateUser(int id, UserUpdateRequest request)
		{
			var client = _httpClientFactory.CreateClient();
			client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var sessions = _httpContextAccessor
                .HttpContext
                .Request
                .Cookies[SystemConstants.AppSettings.Token];

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

			var json = JsonConvert.SerializeObject(request);
			var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await client.PutAsync($"/api/users/{id}", httpContent);
			var result = await response.Content.ReadAsStringAsync();
			if (response.IsSuccessStatusCode)
				return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

			return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
		}
	}
}
