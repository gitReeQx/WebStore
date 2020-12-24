using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Infrastructure.Interfaces;
using WebStore.Infrastructure.Mapping;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData productData;

        public CatalogController(IProductData _productData)
        {
            productData = _productData;
        }

        public IActionResult Shop(int? BrandId, int? SectionId)
        {
            var filter = new ProductFilter
            {
                BrandId = BrandId,
                SectionId = SectionId,
            };

            var products = productData.GetProducts(filter);

            return View(new CatalogViewModel
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Products = products.OrderBy(p => p.Order).ToView()
            });
        }

        public IActionResult Details(int id)
        {
            var product = productData.GetProductById(id);

            if (product is null)
                return NotFound();

            return View(product.ToView());
        }
    }
}
