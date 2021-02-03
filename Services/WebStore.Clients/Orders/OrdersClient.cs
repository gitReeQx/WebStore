using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebStore.Clients.Base;
using WebStore.Domain.DTO.Orders;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Orders
{
    public class OrdersClient : BaseClient, IOrderService
    {
        private readonly ILogger<OrdersClient> logger;

        public OrdersClient(IConfiguration Configuration, ILogger<OrdersClient> Logger) 
            : base(Configuration, WebAPI.Orders) => 
            logger = Logger;

        public async Task<OrderDTO> CreateOrder(string UserName, CreateOrderModel OrderModel)
        {
            var response = await PostAsync($"{Address}/{UserName}", OrderModel);
            return await response.Content.ReadAsAsync<OrderDTO>();
        }

        public async Task<OrderDTO> GetOrderById(int id) =>
            await GetAsync<OrderDTO>($"{Address}/{id}");

        public async Task<IEnumerable<OrderDTO>> GetUserOrders(string UserName) =>
            await GetAsync<IEnumerable<OrderDTO>>($"{Address}/user/{UserName}");
    }
}
