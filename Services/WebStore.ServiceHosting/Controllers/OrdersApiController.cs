using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain.DTO.Orders;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Orders)]
    [ApiController]
    public class OrdersApiController : ControllerBase, IOrderService
    {
        private readonly IOrderService orderService;
        private readonly Logger<OrdersApiController> logger;

        public OrdersApiController(IOrderService OrderService, Logger<OrdersApiController> Logger)
        {
            orderService = OrderService;
            logger = Logger;
        }

        public Task<OrderDTO> CreateOrder(string UserName, [FromBody] CreateOrderModel OrderModel)
        {
            logger.LogInformation("Формирование заказа для пользователя {0}", UserName);
            return orderService.CreateOrder(UserName, OrderModel);
        }

        public Task<OrderDTO> GetOrderById(int id)
        {
            return orderService.GetOrderById(id);
        }

        public Task<IEnumerable<OrderDTO>> GetUserOrders(string UserName)
        {
            return orderService.GetUserOrders(UserName);
        }
    }
}
