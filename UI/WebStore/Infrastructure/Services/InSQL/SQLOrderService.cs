using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Infrastructure.Services.InSQL
{
    public class SQLOrderService: IOrderService
    {
        private readonly WebStoreDB db;
        private readonly UserManager<User> userManager;

        public SQLOrderService(WebStoreDB _db, UserManager<User> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }

        public async Task<Order> CreateOrder(string UserName, CartViewModel Cart, OrderViewModel OrderModel)
        {
            var user = await userManager.FindByNameAsync(UserName);
            if (user is null)
                throw new InvalidOperationException($"Пользователь {UserName} не найден в БД");

            await using var transaction = await db.Database.BeginTransactionAsync();

            var order = new Order
            {
                Name = OrderModel.Name,
                Address = OrderModel.Address,
                Phone = OrderModel.Phone,
                Date = DateTime.Now,
                User = user
            };

            foreach(var (product_model, quantity) in Cart.Items)
            {
                var product = await db.Products.FindAsync(product_model.Id);
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

            return order;
        }

        public async Task<Order> GetOrderById(int id) => await db.Orders
            .Include(order => order.User)
            .Include(order => order.Items)
            .FirstOrDefaultAsync(order => order.Id == id);

        public async Task<IEnumerable<Order>> GetUserOrders(string UserName) => await db.Orders
            .Include(order => order.User)
            .Include(order => order.Items)
            .Where(order => order.User.UserName == UserName)
            .ToArrayAsync();
    }
}
