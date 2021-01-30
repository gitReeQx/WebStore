using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Products)]
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
        public BrandDTO GetBrandById(int id)
        {
            return productData.GetBrandById(id);
        }

        [HttpGet("brands")]
        public IEnumerable<BrandDTO> GetBrands()
        {
            return productData.GetBrands();
        }

        [HttpGet("{id}")]
        public ProductDTO GetProductById(int id)
        {
            return productData.GetProductById(id);
        }

        [HttpPost]
        public IEnumerable<ProductDTO> GetProducts([FromBody]ProductFilter Filter = null)
        {
            return productData.GetProducts(Filter);
        }

        [HttpGet("sections/{id}")]
        public SectionDTO GetSectionById(int id)
        {
            return productData.GetSectionById(id);
        }

        [HttpGet("sections")]
        public IEnumerable<SectionDTO> GetSections()
        {
            return productData.GetSections();
        }
    }
}
