using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Infrastructure.Services.InSQL
{
    public class SQLProductData : IProductData
    {
        private readonly WebStoreDB db;

        public SQLProductData(WebStoreDB _db)
        {
            db = _db;
        }

        public IEnumerable<Brand> GetBrands() => db.Brands.Include(brand => brand.Products);

        public IEnumerable<Section> GetSections() => db.Sections.Include(section => section.Products);

        public IEnumerable<Product> GetProducts(ProductFilter Filter = null)
        {
            IQueryable<Product> query = db.Products
                .Include(p => p.Section)
                .Include(p => p.Brand);

            if (Filter?.Ids?.Length > 0)
            {
                query = query.Where(product => Filter.Ids.Contains(product.Id));
            }
            else
            {
                if (Filter?.BrandId != null)
                    query = query.Where(p => p.BrandId == Filter.BrandId);
                if (Filter?.SectionId != null)
                    query = query.Where(p => p.SectionId == Filter.SectionId);
            }

            return query;
        }

        public Section GetSectionById(int id) => db.Sections
            .Include(section => section.Products)
            .FirstOrDefault(s => s.Id == id);

        public Brand GetBrandById(int id) => db.Brands
            .Include(brands => brands.Products)
            .FirstOrDefault(s => s.Id == id);

        public Product GetProductById(int id) => db.Products
            .Include(p => p.Section)
            .Include(p => p.Brand)
            .FirstOrDefault(p => p.Id == id);

        public void AddProduct(Product product)
        {
            db.Products.Update(product);
            db.SaveChanges();
        }

        public bool DeleteProduct(int id)
        {
            Product product = db.Products.FirstOrDefault(p => p.Id == id);

            if (product is null) return false;

            db.Products.Remove(product);
            db.SaveChanges();
            return true;
        }
    }
}
