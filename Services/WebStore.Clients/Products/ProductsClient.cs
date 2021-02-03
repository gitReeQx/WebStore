using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(IConfiguration Configuration) : base(Configuration, WebAPI.Products)
        {
        }

        public void AddProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public bool DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        public BrandDTO GetBrandById(int id) => Get<BrandDTO>($"{Address}/brands/{id}");

        public IEnumerable<BrandDTO> GetBrands() => Get<IEnumerable<BrandDTO>>($"{Address}/brands");

        public ProductDTO GetProductById(int id) => Get<ProductDTO>($"{Address}/{id}");

        public IEnumerable<ProductDTO> GetProducts(ProductFilter Filter = null) =>
            Post(Address, Filter ?? new ProductFilter())
            .Content
            .ReadAsAsync<IEnumerable<ProductDTO>>()
            .Result;

        public SectionDTO GetSectionById(int id) => Get<SectionDTO>($"{Address}/sections/{id}");

        public IEnumerable<SectionDTO> GetSections() => Get<IEnumerable<SectionDTO>>($"{Address}/sections");
    }
}
