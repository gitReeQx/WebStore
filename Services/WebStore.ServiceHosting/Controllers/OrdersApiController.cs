﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly ILogger<OrdersApiController> logger;

        public OrdersApiController(IOrderService OrderService, ILogger<OrdersApiController> Logger)
        {
            orderService = OrderService;
            logger = Logger;
        }

        [HttpPost("{UserName}")]
        public Task<OrderDTO> CreateOrder(string UserName, [FromBody] CreateOrderModel OrderModel)
        {
            logger.LogInformation("Формирование заказа для пользователя {0}", UserName);
            return orderService.CreateOrder(UserName, OrderModel);
        }

        [HttpGet("{id}")]
        public Task<OrderDTO> GetOrderById(int id)
        {
            return orderService.GetOrderById(id);
        }

        [HttpGet("user/{UserName}")]
        public Task<IEnumerable<OrderDTO>> GetUserOrders(string UserName)
        {
            return orderService.GetUserOrders(UserName);
        }
    }
}
