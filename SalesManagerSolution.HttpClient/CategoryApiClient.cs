using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Categories;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Categories;
using SalesManagerSolution.Core.ViewModels.Common;

namespace SalesManagerSolution.HttpClient
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _configuration;
		public CategoryApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
			_httpContextAccessor = httpContextAccessor;
			_configuration = configuration;
			_httpClientFactory = httpClientFactory;
		}

		public async Task<bool> CreateCategory(CategoryRequest request)
		{
			var requestContent = new MultipartFormDataContent();

			requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Name) ? "" : request.Name.ToString()), "name");
			requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Description) ? "" : request.Description.ToString()), "description");

			var response = await AddAsync($"/api/categories/", requestContent);
			return response;
		}

		public async Task<bool> DeleteCategory(int id)
		{
			return await Delete($"/api/categories/" + id);
        }

		public async Task<PagedResult<CategoryViewModel>> GetAll(PagingRequestBase request)
        {
            var data = await GetAsync<PagedResult<CategoryViewModel>>($"/api/categories/paging?PageIndex={request.PageIndex}" +
                $"&PageSize={request.PageSize}");

			return data;
        }

        public async Task<CategoryViewModel> GetById(int id)
        {
            return await GetAsync<CategoryViewModel>($"/api/categories/{id}");
        }

		public async Task<bool> UpdateCategory(CategoryRequest request)
		{
			var requestContent = new MultipartFormDataContent();

			requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Name) ? "" : request.Name.ToString()), "name");
			requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Description) ? "" : request.Description.ToString()), "description");

			var response = await UpdateAsync($"/api/categories/" + request.Id, requestContent);
			return response;
		}
	}
}
