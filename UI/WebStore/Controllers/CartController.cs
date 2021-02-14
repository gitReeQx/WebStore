using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.DTO.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService cartService;

        public CartController(ICartService CartService) => cartService = CartService;

        public IActionResult Index() => View(new CartOrderViewModel
        {
            Cart = cartService.TransformFromCart()
        });
           
        public IActionResult AddToCart(int id)
        {
            cartService.AddToCart(id);

            var referer = Request.Headers["Referer"].ToString();
            if (Url.IsLocalUrl(referer)) //насколько мне удалось узнать, то такая проблема только на локалке возникает...
                return Redirect(referer);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            cartService.RemoveFromCart(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DecrementeFromCart(int id)
        {
            cartService.DecrementFromCart(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Clear()
        {
            cartService.Clear();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Checkout(OrderViewModel OrderModel, [FromServices] IOrderService OrderService)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index), new CartOrderViewModel
                {
                    Cart = cartService.TransformFromCart(),
                    Order = OrderModel
                });

            var create_order_model = new CreateOrderModel(
                OrderModel,
                cartService.TransformFromCart().Items
                .Select(item => new OrderItemDTO(
                    item.Product.Id, 
                    item.Product.Price, 
                    item.Quantity))
                .ToList());

            var order = await OrderService.CreateOrder(User.Identity.Name, create_order_model);

            //var order = await OrderService.CreateOrder(User.Identity.Name, cartService.TransformFromCart(), OrderModel);

            cartService.Clear();

            return RedirectToAction("OrderConfirmed", new { order.Id });
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }

        #region WebAPI

        public IActionResult GetCartView() => ViewComponent("Cart");

        public IActionResult AddToCartAPI(int id)
        {
            cartService.AddToCart(id);
            return Json(new { id, message = $"Товар с id:{id} был добавлен в корзину" });
        }

        public IActionResult RemoveFromCartAPI(int id)
        {
            cartService.RemoveFromCart(id);
            return Ok();
        }

        public IActionResult DecrementFromCartAPI(int id)
        {
            cartService.DecrementFromCart(id);
            return Ok(new { id, message = $"Количество товара с id:{id} уменьшено на 1" });
        }

        public IActionResult ClearAPI()
        {
            cartService.Clear();
            return Ok();
        }

        #endregion
    }
}
