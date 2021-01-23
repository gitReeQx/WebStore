using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;

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
            var order = await OrderService.CreateOrder(User.Identity.Name, cartService.TransformFromCart(), OrderModel);

            cartService.Clear();

            return RedirectToAction("OrderConfirmed", new { order.Id });
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}
