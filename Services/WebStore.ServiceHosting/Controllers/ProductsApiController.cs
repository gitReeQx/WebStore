using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsApiController : ControllerBase, IProductData
    {
        private readonly IProductData productData;

        public ProductsApiController(IProductData ProductData) => productData = ProductData;

        public void AddProduct(Product product)
        {
            productData.AddProduct(product);
        }

        public bool DeleteProduct(int id)
        {
            return productData.DeleteProduct(id);
        }

        [HttpGet("brands/{id}")]
        public Brand GetBrandById(int id)
        {
            return productData.GetBrandById(id);
        }

        [HttpGet("brands")]
        public IEnumerable<Brand> GetBrands()
        {
            return productData.GetBrands();
        }

        [HttpGet("{id}")]
        public Product GetProductById(int id)
        {
            return productData.GetProductById(id);
        }

        [HttpPost]
        public IEnumerable<Product> GetProducts([FromBody]ProductFilter Filter = null)
        {
            return productData.GetProducts(Filter);
        }

        [HttpGet("sections/{id}")]
        public Section GetSectionById(int id)
        {
            return productData.GetSectionById(id);
        }

        [HttpGet("sections")]
        public IEnumerable<Section> GetSections()
        {
            return productData.GetSections();
        }
    }
}
