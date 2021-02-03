using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.DTO.Orders;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services.Products.InSQL
{
    public class SQLOrderService : IOrderService
    {
        private readonly WebStoreDB db;
        private readonly UserManager<User> userManager;

        public SQLOrderService(WebStoreDB _db, UserManager<User> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }

        public async Task<OrderDTO> CreateOrder(string UserName, CreateOrderModel OrderModel)
        {
            var user = await userManager.FindByNameAsync(UserName);
            if (user is null)
                throw new InvalidOperationException($"Пользователь {UserName} не найден в БД");

            await using var transaction = await db.Database.BeginTransactionAsync();

            var order = new Order
            {
                Name = OrderModel.Order.Name,
                Address = OrderModel.Order.Address,
                Phone = OrderModel.Order.Phone,
                Date = DateTime.Now,
                User = user
            };

            foreach (var (id, _, quantity) in OrderModel.Items)
            {
                var product = await db.Products.FindAsync(id);
                if (product is null) continue;

                var order_item = new OrderItem
                {
                    Order = order,
                    Price = product.Price,
                    Quantity = quantity,
                    Product = product
                };
                order.Items.Add(order_item);
            }

            await db.Orders.AddAsync(order);

            await db.SaveChangesAsync();

            await transaction.CommitAsync();

            return order.ToDTO();
        }

        public async Task<OrderDTO> GetOrderById(int id) => (await db.Orders
            .Include(order => order.User)
            .Include(order => order.Items)
            .FirstOrDefaultAsync(order => order.Id == id)).ToDTO();

        public async Task<IEnumerable<OrderDTO>> GetUserOrders(string UserName) => (await db.Orders
            .Include(order => order.User)
            .Include(order => order.Items)
            .Where(order => order.User.UserName == UserName)
            .ToArrayAsync())
            .Select(o => o.ToDTO());
    }
}
