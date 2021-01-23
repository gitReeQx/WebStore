using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebStore.Domain.Entities;
using Newtonsoft.Json;
using WebStore.Domain;
using WebStore.Infrastructure.Mapping;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Infrastructure.Services.InCookies
{
    public class InCookiesCartService: ICartService
    {
        private readonly IProductData productData;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string _CartName;

        private Cart Cart
        {
            get
            {
                var context = httpContextAccessor.HttpContext;
                var cookies = context.Response.Cookies;
                var cart_cookie = context.Request.Cookies[_CartName];

                if(cart_cookie is null)
                {
                    var cart = new Cart();
                    cookies.Append(_CartName, JsonConvert.SerializeObject(cart));
                    return cart;
                }

                ReplaceCookies(cookies, cart_cookie);
                return JsonConvert.DeserializeObject<Cart>(cart_cookie);
            }
            set => ReplaceCookies(httpContextAccessor.HttpContext.Response.Cookies, JsonConvert.SerializeObject(value));
        }

        private void ReplaceCookies(IResponseCookies cookies, string cookie)
        {
            cookies.Delete(_CartName);
            cookies.Append(_CartName, cookie);
        }

        public void AddToCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null)
                cart.Items.Add(new CartItem { ProductId = id, Quantity = 1 });
            else
                item.Quantity++;

            Cart = cart;
        }

        public void DecrementFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null) return;

            if (item.Quantity > 0)
                item.Quantity--;

            if (item.Quantity == 0)
                cart.Items.Remove(item);

            Cart = cart;
        }

        public void RemoveFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null) return;

            cart.Items.Remove(item);

            Cart = cart;
        }

        public void Clear()
        {
            var cart = Cart;

            cart.Items.Clear();

            Cart = cart;
        }

        public CartViewModel TransformFromCart()
        {
            var products = productData.GetProducts(new ProductFilter
            {
                Ids = Cart.Items.Select(item => item.ProductId).ToArray()
            });

            var product_view_models = products.ToView().ToDictionary(p => p.Id);

            return new CartViewModel
            {
                Items = Cart.Items.Select(item => (product_view_models[item.ProductId], item.Quantity))
            };
        }

        public InCookiesCartService(IProductData ProductData, IHttpContextAccessor HttpContextAccessor)
        {
            productData = ProductData;
            httpContextAccessor = HttpContextAccessor;

            var user = HttpContextAccessor.HttpContext!.User;
            var user_name = user.Identity.IsAuthenticated ? $"{user.Identity.Name}" : null;

            _CartName = $"WebStore.Cart{user_name}";
        }
    }
}
