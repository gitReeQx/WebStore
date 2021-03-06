﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;

namespace WebStore.Services.Mapping
{
    public static class ProductMapper
    {
        public static ProductViewModel ToView(this Product p) => new ProductViewModel()
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            ImageUrl = p.ImageUrl,
            Brand = p.Brand?.Name
        };

        public static IEnumerable<ProductViewModel> ToView(this IEnumerable<Product> p) => p.Select(ToView);

        public static ProductDTO ToDTO(this Product Product) => Product is null
            ? null
            : new ProductDTO(
                Product.Id, 
                Product.Name, 
                Product.Order, 
                Product.Price, 
                Product.ImageUrl, 
                Product.Brand.ToDTO(), 
                Product.Section.ToDTO());

        public static Product FromDTO(this ProductDTO ProductDTO) => ProductDTO is null
            ? null
            : new Product
            {
                Id = ProductDTO.Id,
                Name = ProductDTO.Name,
                Order = ProductDTO.Order,
                Price = ProductDTO.Price,
                ImageUrl = ProductDTO.ImageUrl,
                BrandId = ProductDTO.brand?.Id,
                Brand = ProductDTO.brand.FromDTO(),
                SectionId = ProductDTO.section.Id,
                Section = ProductDTO.section.FromDTO(),
            };

        public static IEnumerable<ProductDTO> ToDTO(this IEnumerable<Product> Products) => Products.Select(ToDTO);

        public static IEnumerable<Product> FromDTO(this IEnumerable<ProductDTO> Products) => Products.Select(FromDTO);
    }
}
