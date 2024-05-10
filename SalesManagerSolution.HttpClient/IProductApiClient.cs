using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Products;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Products;
using SalesManagerSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.HttpClient
{
    public interface IProductApiClient
    {
        Task<PagedResult<ProductViewModel>> GetPagings(ProductPagingViewModel request);

        Task<bool> CreateProduct(ProductCreateViewModel request);

        Task<bool> UpdateProduct(ProductCreateViewModel request);

        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);

        Task<string> SearchProduct(string productName);

        Task<ProductViewModel> GetById(int id);

        Task<List<ProductViewModel>> GetFeaturedProducts(int take);

        Task<List<ProductViewModel>> GetLatestProducts(int take);

        Task<bool> DeleteProduct(int id);

    }
}
