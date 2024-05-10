using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Categories;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Categories;
using SalesManagerSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Core.Interfaces.Services.Categories
{
    public interface ICategoryService
    {
        Task<PagedResult<CategoryViewModel>> GetAll(PagingRequestBase request);

        Task<CategoryViewModel> GetById(int id);

        Task<int> Create(CategoryRequest request);

		Task<int> Update(CategoryRequest request);

		Task<int> Delete(CategoryDeleteRequest request);

	}
}
