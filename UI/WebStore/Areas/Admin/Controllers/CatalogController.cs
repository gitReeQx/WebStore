using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = Role.Administrator)]
    public class CatalogController : Controller
    {
        private readonly IProductData productData;

        public CatalogController(IProductData _productData)
        {
            productData = _productData;
        }

        public IActionResult Index() => View(productData.GetProducts());

        public IActionResult Edit(int id)
        {
            var product = productData.GetProductById(id);
            if (product is null) return NotFound();
            return View(product.FromDTO());
        }

        [HttpPost]
        public IActionResult Edit(Product _product)
        {
            if (!ModelState.IsValid) return View(_product);

            productData.AddProduct(_product);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var product = productData.GetProductById(id);
            if (product is null) return NotFound();
            return View(product.FromDTO());
        }

        [HttpPost]
        public IActionResult DeleteConfirm(int id)
        {
            productData.DeleteProduct(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
