using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Components
{
    public class BrandsViewComponent: ViewComponent
    {
        private readonly IProductData productData;

        public BrandsViewComponent(IProductData _productData) => productData = _productData;

        public IViewComponentResult Invoke() => View(GetBrands());

        private IEnumerable<BrandViewModel> GetBrands() =>
            productData.GetBrands()
            .OrderBy(brand => brand.Order)
            .Select(brand => new BrandViewModel
            {
                Id = brand.Id,
                Name = brand.Name,
                ProductsCount = brand.ProductCount
            });
    }
}
