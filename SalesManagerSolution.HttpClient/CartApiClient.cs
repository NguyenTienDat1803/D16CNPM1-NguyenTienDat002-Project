using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SalesManagerSolution.Core.Constants;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Categories;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Categories;
using SalesManagerSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Products;
using System.Security.Policy;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Carts;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Carts;

namespace SalesManagerSolution.HttpClient
{
    public class CartApiClient : BaseApiClient, ICartApiClient
    {
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _configuration;
		public CartApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
			_httpContextAccessor = httpContextAccessor;
			_configuration = configuration;
			_httpClientFactory = httpClientFactory;
		}

		public async Task<bool> CreateCart(CartResquestViewModel request)
		{
			var requestContent = new MultipartFormDataContent();

            requestContent.Add(new StringContent(request.UserId.ToString()), "userId");
            requestContent.Add(new StringContent(request.ProductId.ToString()), "productId");
            requestContent.Add(new StringContent(request.Quantity.ToString()), "quantity");
            requestContent.Add(new StringContent(request.Price.ToString()), "price");

            var response = await AddAsync($"/api/carts/", requestContent);
			return response;
		}

        public Task<bool> DeleteCart(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteCategory(int id)
		{
			return await Delete($"/api/carts/" + id);
        }

		public async Task<List<CartViewModel>> GetAll(int userId)
        {
            var data = await GetAsync<List<CartViewModel>>($"/api/carts/paging?userId={userId}");

			return data;
        }

		public async Task<int> GetCartItem(int userId)
		{
			return await GetAsync<int>($"/api/carts/GetCartItem/{userId}");		
		}
		public async Task<CartViewModel> GetById(int id)
        {
            return await GetAsync<CartViewModel>($"/api/carts/{id}");
        }

		public async Task<bool> UpdateCart(CartResquestViewModel request)
		{
			var requestContent = new MultipartFormDataContent();

			requestContent.Add(new StringContent(request.Quantity.ToString()), "quantity");
			requestContent.Add(new StringContent(request.Price.ToString()), "price");

			var response = await UpdateAsync($"/api/carts/" + request.Id, requestContent);
			return response;
		}

    }
}
