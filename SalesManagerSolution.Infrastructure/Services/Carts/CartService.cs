using Microsoft.EntityFrameworkCore;
using SalesManagerSolution.Core.Interfaces.Services.Carts;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Carts;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Carts;
using SalesManagerSolution.Domain.Entities;
using SalesManagerSolution.Domain.Enums;
using SalesManagerSolution.Infrastructure.EntityFramework;

namespace SalesManagerSolution.Infrastructure.Services.Carts
{
	public class CartService : ICartService
	{
		private readonly ApplicationDbContext _context;

		public CartService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<int> Create(CartResquestViewModel request)
		{
			var cartByUser = await _context.Carts.FirstOrDefaultAsync(x=>x.UserId == request.UserId && x.ProductId == request.ProductId);
			

			if(cartByUser == null)
			{ 

				var cart = new Cart()
				{
					Quantity = request.Quantity,
					ProductId = request.ProductId,
					Price = request.Price,
					UserId = request.UserId,
					CartStatus = CartStatus.NotDone,
					IsDeleted = false
				};

				_context.Carts.Add(cart);

				return await _context.SaveChangesAsync();
		    }

			cartByUser.Quantity += request.Quantity;

			_context.Carts.Update(cartByUser);

			return await _context.SaveChangesAsync();
		}

		public async Task<int> Delete(DeleteCartRequest request)
		{
			var cart = await _context.Carts
									 .Where(x => x.Id == request.Id)
									 .FirstOrDefaultAsync();

			if (cart is null)
			{
				throw new Exception($"Cant find cart with Id : {request.Id}");
			}

			_context.Carts.Remove(cart);

			return await _context.SaveChangesAsync();
		}

		public async Task<List<CartViewModel>> GetAll(int userId)
		{
			var query = from c in _context.Carts
						join p in _context.Products on c.ProductId equals p.Id
						join pi in _context.ProductImages on p.Id equals pi.ProductId
						where c.UserId == userId && !c.IsDeleted && c.CartStatus == CartStatus.NotDone
						select new { c,p,pi };

			int totalRow = await query.CountAsync();

			var data = await query.Select(x => new CartViewModel()
									{
										Id = x.c.Id,
										Price = x.p.Price,
										ProductName = x.p.Name,
										Quantity = x.c.Quantity,
										ProductId = x.c.ProductId,
										ThumnailImage = x.pi.ImagePath,
										SubTotal = x.p.Price * x.c.Quantity
									}).ToListAsync();
			return data;
		}


		public async Task<CartViewModel> GetById(int id, int productId)
		{
            var query = from c in _context.Carts
                        join p in _context.Products on c.ProductId equals p.Id
						where c.Id == id && !c.IsDeleted && p.Id == productId
						select new { c, p };

            var result = await query.Select(x => new CartViewModel()
            {
                Id = x.c.Id,
                Price = x.c.Price,
                ProductName = x.p.Name,
                Quantity = x.c.Quantity,
				ProductId = x.p.Id
            }).FirstOrDefaultAsync();

            return result ?? new CartViewModel();
		}

		public async Task<Cart> GetCartById(int id)
		{
			var cart = await _context.Carts
									 .Where(x => x.Id == id)
									 .FirstOrDefaultAsync();

			return cart ?? new Cart();
		}

		public async Task<int> Update(CartResquestViewModel request)
		{
			var cart = await _context.Carts
									 .Where(x => x.Id == request.Id)
									 .FirstOrDefaultAsync();

			if (cart is null)
			{
				throw new Exception($"Cant find cart with Id : {request.Id}");
			}

            cart.Price = request.Price;
            cart.Quantity = request.Quantity;
            cart.UpdatedAt = DateTime.Now;

			_context.Carts.Update(cart);

			return await _context.SaveChangesAsync();
		}

        public async Task<int> UpdateStatusCart(int id)
        {
            var cart = await GetCartById(id);

			cart.CartStatus = CartStatus.Done;

			_context.Carts.Update(cart);

			return await _context.SaveChangesAsync();
        }

		public async Task<int> GetCartItem(int userId) => await _context.Carts.CountAsync(x => x.UserId == userId);

	}
}
