using Microsoft.EntityFrameworkCore;
using SalesManagerSolution.Core.Interfaces.Orders;
using SalesManagerSolution.Core.ViewModels.Common;
using SalesManagerSolution.Core.ViewModels.RequestViewModels.Orders;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Orders;
using SalesManagerSolution.Core.ViewModels.ResponseViewModels.Products;
using SalesManagerSolution.Domain.Entities;
using SalesManagerSolution.Domain.Enums;
using SalesManagerSolution.Infrastructure.EntityFramework;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace SalesManagerSolution.Infrastructure.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateOrder(OrderRequestViewModel request)
        {
            var order = new Order()
            {
                UserId = request.UserId,
                ShipAddress = request.ShipAddress,
                ShipEmail = request.ShipEmail,
                ShipName = request.ShipName,
                ShipPhoneNumber = request.ShipPhoneNumber,
                Status = OrderStatus.InProgress,
                OrderDate = DateTime.UtcNow,
            };

            var orderDetails = new List<OrderDetail>();

            foreach (var item in request.OrderDetails)
            {
                var orderDetail = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    OrderId = order.Id,
                };

                _context.OrderDetails.Add(orderDetail);

                orderDetails.Add(orderDetail);
            }

            order.OrderDetails = orderDetails;

            _context.Orders.Add(order);

            return await _context.SaveChangesAsync();

        }

        public async Task<PagedResult<OrderResponseViewModel>> GetAllPagings(OrderPagingViewModel request)
        {
            var query = from o in _context.Orders
                        join od in _context.OrderDetails on o.Id equals od.OrderId into ood
                        where o.IsDeleted == false
                        select new { o, ood };
            //2. filter
            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.o.ShipName.Contains(request.Keyword) ||
                                         x.o.ShipEmail.Contains(request.Keyword) ||
                                         x.o.ShipPhoneNumber.Contains(request.Keyword) ||
                                         x.o.ShipName.Contains(request.Keyword));

            if (request.OrderId != null && request.OrderId != 0)
            {
                query = query.Where(p => p.o.Id == request.OrderId);
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new OrderResponseViewModel()
                {
                   Id = x.o.Id,
                   UserId = x.o.UserId,
                   ShipAddress = x.o.ShipAddress,
                   ShipName = x.o.ShipName, 
                   ShipPhoneNumber= x.o.ShipPhoneNumber,
                   ShipEmail = x.o.ShipEmail,
                   Status = x.o.Status,
                   TotalProduct = x.ood.Select(x=>x.ProductId).Count(),
                   TotalPrice = x.ood.Select(x => x.Price).Sum(),
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<OrderResponseViewModel>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<OrderResponseViewModel> GetOrderById(int id)
        {
              var data = _context.Orders
                                 .Join(_context.OrderDetails, o => o.Id, od => od.OrderId, (o, od) => new { o = o, od = od })
                                 .Where(x => x.o.Id == id)
                                 .Select(x => x)
                                 .AsQueryable();

            var totalProduct = await data.CountAsync();

            var totalPrice = await data.Select(x=>x.od.Price)
                                       .SumAsync();

            var shippingAddress = await data.Select(x => x.o.ShipAddress).FirstOrDefaultAsync() ?? string.Empty;
            var shippingName = await data.Select(x => x.o.ShipName).FirstOrDefaultAsync() ?? string.Empty;
            var shippingEmail = await data.Select(x => x.o.ShipEmail).FirstOrDefaultAsync() ?? string.Empty;
            var shippingPhoneNumber = await data.Select(x => x.o.ShipPhoneNumber).FirstOrDefaultAsync() ?? string.Empty;

            var result = new OrderResponseViewModel()
            {
               ShipAddress = shippingAddress,
               ShipPhoneNumber = shippingPhoneNumber,
               ShipName = shippingName,
               ShipEmail = shippingEmail,
               TotalPrice = totalPrice,
               TotalProduct = totalProduct
            };

            return result;
        }

        public async Task<List<OrderResponseViewModel>> GetOrderByUserId(int userId)
        {
            var query = _context.Orders
                               .Join(_context.OrderDetails, o => o.Id, od => od.OrderId, (o, od) => new { o = o, od = od })
                               .Where(x => x.o.UserId == userId)
                               .Select(x => x)
            .AsQueryable();

            var dataGroups = query.GroupBy(x => new { x.o })
                                 .Select(x => new
                                 {
                                     order = x.Key,
                                     orderDetails = x.ToList()
                                 });

            var data = await dataGroups
                        .Select(x => new OrderResponseViewModel()
                        {
                            ShipAddress = x.order.o.ShipAddress,
                            ShipName = x.order.o.ShipName,
                            ShipPhoneNumber = x.order.o.ShipPhoneNumber,
                            ShipEmail = x.order.o.ShipEmail,
                            Status = x.order.o.Status,
                            TotalProduct = x.orderDetails.Select(x => x.od.ProductId).Count(),
                            TotalPrice = x.orderDetails.Select(x => x.od.ProductId).Sum(),
                        }).ToListAsync();

            return data;

        }

        public Task<int> UpdateOrder(OrderRequestViewModel request)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateStatusOrder(int orderId, OrderStatus orderStatus)
        {
            var order = await _context.Orders
                                      .FirstOrDefaultAsync(x=>x.Id == orderId);

            if(order == null)
            {
                throw new Exception($"Order with Id : {orderId} not found!!!");
            }

            order.Status = orderStatus;

            _context.Orders.Update(order);

            return await _context.SaveChangesAsync();
        }
    }
}
