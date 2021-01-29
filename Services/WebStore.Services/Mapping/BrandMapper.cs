using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class BrandMapper
    {
        public static BrandDTO ToDTO(this Brand Brand) => Brand is null
            ? null
            : new BrandDTO(Brand.Id, Brand.Name, Brand.Order);

        public static Brand FromDTO(this BrandDTO BrandDTO) => BrandDTO is null
            ? null
            : new Brand
            {
                Id = BrandDTO.Id,
                Name = BrandDTO.Name,
                Order = BrandDTO.Order,
            };

        public static IEnumerable<BrandDTO> ToDTO(this IEnumerable<Brand> Brands) => Brands.Select(ToDTO);

        public static IEnumerable<Brand> FromDTO(this IEnumerable<BrandDTO> Brands) => Brands.Select(FromDTO);
    }
}
