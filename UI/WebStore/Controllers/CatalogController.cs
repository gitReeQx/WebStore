using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using WebStore.Domain;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private const string __PageSize = "CatalogPageSize";

        private readonly IProductData productData;
        private readonly IConfiguration _Configuration;

        public CatalogController(IProductData ProductData, IConfiguration Configuration)
        {
            productData = ProductData;
            _Configuration = Configuration;
        }

        public IActionResult Shop(int? BrandId, int? SectionId, int Page = 1, int? PageSize = null)
        {
            var page_size = PageSize
                ?? (int.TryParse(_Configuration[__PageSize], out var value) ? value : null);

            var filter = new ProductFilter
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Page = Page,
                PageSize = page_size
            };

            var (products, total_count) = productData.GetProducts(filter);

            return View(new CatalogViewModel
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Products = products.OrderBy(p => p.Order).FromDTO().ToView(),
                PageViewModel = new PageViewModel
                {
                    Page = Page,
                    PageSize = page_size ?? 0,
                    TotalItems = total_count,
                },
            });
        }

        public IActionResult Details(int id) =>
            productData.GetProductById(id) is { } product
                ? View(product.FromDTO().ToView())
                : NotFound();

        #region WebAPI

        public IActionResult GetFeaturedItems(int? BrandId, int? SectionId, int Page = 1, int? PageSize = null)
            => PartialView("Partial/_FeaturesItems", GetProducts(BrandId, SectionId, Page, PageSize));

        private IEnumerable<ProductViewModel> GetProducts(int? BrandId, int? SectionId, int Page, int? PageSize) =>
            productData.GetProducts(
                    new ProductFilter
                    {
                        SectionId = SectionId,
                        BrandId = BrandId,
                        Page = Page,
                        PageSize = PageSize
                            ?? (int.TryParse(_Configuration[__PageSize], out var size) ? size : null)
                    })
               .Products.OrderBy(p => p.Order)
               .FromDTO()
               .ToView();


        #endregion
    }
}
