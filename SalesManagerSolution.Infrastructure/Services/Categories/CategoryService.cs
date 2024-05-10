using Microsoft.EntityFrameworkCore;
using SalesManagerSolution.Core.Interfaces.Services.Categories;
using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Categories;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Categories;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Products;
using SalesManagerSolution.Domain.Entities;
using SalesManagerSolution.Infrastructure.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerSolution.Infrastructure.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

		public async Task<int> Create(CategoryRequest request)
		{
            var cateogry = new Category()
            {
                Name = request.Name,
                Description = request.Description ?? string.Empty,
                IsShowOnHome = true
            };

            _context.Categories.Add(cateogry);

           return  await _context.SaveChangesAsync();
		}

		public async Task<int> Delete(CategoryDeleteRequest request)
		{
			var category = await _context.Categories
										  .Where(x => x.Id == request.Id)
										  .FirstOrDefaultAsync();

			if (category is null)
			{
				throw new Exception($"Cant find category with Id : {request.Id}");
			}

            _context.Categories.Remove(category);

            return await _context.SaveChangesAsync();
		}

		public async Task<PagedResult<CategoryViewModel>> GetAll(PagingRequestBase request)
        {
            var query = from c in _context.Categories
                        select new { c};

            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CategoryViewModel()
                {
                    Id = x.c.Id,
                    Name = x.c.Name,
                    Description = x.c.Description
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<CategoryViewModel>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };

            return pagedResult;
        }


        public async Task<CategoryViewModel> GetById(int id)
        {
            var query = from c in _context.Categories
                        where  c.Id == id
                        select new { c};
            var result = await query.Select(x => new CategoryViewModel()
            {
                Id = x.c.Id,
                Name = x.c.Name,
                ParentId = x.c.ParentId
            }).FirstOrDefaultAsync();

            return result ?? new CategoryViewModel();
        }

		public async Task<int> Update(CategoryRequest request)
		{
			var category = await _context.Categories
										  .Where(x => x.Id == request.Id)
                                          .FirstOrDefaultAsync();

            if(category is null)
            {
                throw new Exception($"Cant find category with Id : {request.Id}");
            }

            category.Name  = request.Name;
            category.Description = request.Description ?? string.Empty;
            category.UpdatedAt = DateTime.Now;

            return await _context.SaveChangesAsync();
		}
	}
}
