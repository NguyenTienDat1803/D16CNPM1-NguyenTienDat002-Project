using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SalesManagerSolution.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.HttpClient
{
    public class BaseApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected BaseApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        protected async Task<TResponse> GetAsync<TResponse>(string url)
        {
            var sessions = _httpContextAccessor
                .HttpContext
                .Request
                .Cookies[SystemConstants.AppSettings.Token];

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                TResponse myDeserializedObjList = (TResponse)JsonConvert.DeserializeObject(body,
                    typeof(TResponse));

                return myDeserializedObjList;
            }
            return JsonConvert.DeserializeObject<TResponse>(body);
        }

        public async Task<List<T>> GetListAsync<T>(string url, bool requiredLogin = false)
        {
            var sessions = _httpContextAccessor
                 .HttpContext
                 .Request
                 .Cookies[SystemConstants.AppSettings.Token];
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var data = (List<T>)JsonConvert.DeserializeObject(body, typeof(List<T>));
                return data;
            }
            throw new Exception(body);
        }


		public async Task<bool> UpdateAsync(string url, MultipartFormDataContent requestContent, bool requiredLogin = false)
		{
			var sessions = _httpContextAccessor
				 .HttpContext
				 .Request
				 .Cookies[SystemConstants.AppSettings.Token];
			var client = _httpClientFactory.CreateClient();
			client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
			var response = await client.PutAsync(url, requestContent);
			return response.IsSuccessStatusCode;
		}

		public async Task<bool> AddAsync(string url, MultipartFormDataContent requestContent, bool requiredLogin = false)
		{
			var sessions = _httpContextAccessor
				 .HttpContext
				 .Request
				 .Cookies[SystemConstants.AppSettings.Token];
			var client = _httpClientFactory.CreateClient();
			client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
			var response = await client.PostAsync(url, requestContent);
			return response.IsSuccessStatusCode;
		}

		public async Task<bool> Delete(string url)
        {
            var sessions = _httpContextAccessor
                  .HttpContext
                  .Request
                  .Cookies[SystemConstants.AppSettings.Token];
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
