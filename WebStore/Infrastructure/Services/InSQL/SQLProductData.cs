using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Infrastructure.Services.InSQL
{
    public class SQLProductData : IProductData
    {
        private readonly WebStoreDB db;

        public SQLProductData(WebStoreDB _db)
        {
            db = _db;
        }

        public IEnumerable<Brand> GetBrands() => db.Brands;

        public IEnumerable<Section> GetSections() => db.Sections;

        public IEnumerable<Product> GetProducts(ProductFilter Filter = null)
        {
            IQueryable<Product> query = db.Products;

            if (Filter?.BrandId != null)
                query = query.Where(p => p.BrandId == Filter.BrandId);
            if (Filter?.SectionId != null)
                query = query.Where(p => p.SectionId == Filter.SectionId);

            return query;
        }
    }
}
