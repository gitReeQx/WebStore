using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService cartService;

        public CartController(ICartService CartService) => cartService = CartService;

        public IActionResult Index() => View(cartService.TransformFromCart());

        public IActionResult AddToCart(int id)
        {
            cartService.AddToCart(id);

            var referer = Request.Headers["Referer"];
            if (Url.IsLocalUrl(referer))
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
    }
}
