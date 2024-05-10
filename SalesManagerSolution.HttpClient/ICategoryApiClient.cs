using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Categories;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Categories;

namespace SalesManagerSolution.HttpClient
{
    public interface ICategoryApiClient
    {
        Task<PagedResult<CategoryViewModel>> GetAll(PagingRequestBase request);

        Task<CategoryViewModel> GetById(int id);

		Task<bool> CreateCategory(CategoryRequest request);

		Task<bool> UpdateCategory(CategoryRequest request);

        Task<bool> DeleteCategory(int id);
	}
}
