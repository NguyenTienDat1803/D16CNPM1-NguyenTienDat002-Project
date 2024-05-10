using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SalesManagerSolution.Core.Constants;
using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Products;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Products;
using SalesManagerSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.HttpClient
{
    public class ProductApiClient : BaseApiClient,IProductApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ProductApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<bool> CreateProduct(ProductCreateViewModel request)
        {
           
            var requestContent = new MultipartFormDataContent();

            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumbnailImage", request.ThumbnailImage.FileName);
            }

            requestContent.Add(new StringContent(request.Price.ToString()), "price");
            requestContent.Add(new StringContent(request.OriginalPrice.ToString()), "originalPrice");
            requestContent.Add(new StringContent(request.Stock.ToString()), "stock");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Name) ? "" : request.Name.ToString()), "name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Description) ? "" : request.Description.ToString()), "description");

            var response = await AddAsync($"/api/products/", requestContent);
            return response;
        }

        public async Task<bool> UpdateProduct(ProductCreateViewModel request)
        
        
        
        
        
        {
            var requestContent = new MultipartFormDataContent();

            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumbnailImage", request.ThumbnailImage.FileName);
            }


            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Name) ? "" : request.Name.ToString()), "name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Description) ? "" : request.Description.ToString()), "description");
			requestContent.Add(new StringContent(request.Price.ToString()), "price");
			requestContent.Add(new StringContent(request.OriginalPrice.ToString()), "originalprice");
			requestContent.Add(new StringContent(request.Stock.ToString()), "stock");

			var response = await UpdateAsync($"/api/products/" + request.Id, requestContent);
            return response;
        }

        public async Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor
                .HttpContext.Request.Cookies[SystemConstants.AppSettings.Token];

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/products/{id}/categories", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ProductViewModel> GetById(int id)
        {
            var data = await GetAsync<ProductViewModel>($"/api/products/GetById/{id}");

            return data;
        }

        public async Task<List<ProductViewModel>> GetFeaturedProducts(int take)
        {
            var data = await GetListAsync<ProductViewModel>($"/api/products/featured/{take}");
            return data;
        }

        public async Task<List<ProductViewModel>> GetLatestProducts(int take)
        {
            var data = await GetListAsync<ProductViewModel>($"/api/products/latest/{take}");
            return data;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            return await Delete($"/api/products/" + id);
        }

        public async Task<PagedResult<ProductViewModel>> GetPagings(ProductPagingViewModel request)
        {
            string url = $"/api/products/paging?PageIndex={request.PageIndex}" +
                $"&PageSize={request.PageSize}";

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                url += $"&Keyword={request.Keyword}";
            }

            if(request.CategoryId != null)
            {
                url += $"&CategoryId ={request.CategoryId}";
            }

            var data = await GetAsync<PagedResult<ProductViewModel>>(url);

            return data;
        }

		public async Task<string> SearchProduct(string productName)
		{
            return await GetAsync<string>($"/api/products/SearchProduct/{productName}");
		}
	}
}
