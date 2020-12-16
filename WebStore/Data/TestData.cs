using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Models;

namespace WebStore.Data
{
    public static class TestData
    {
        public static List<Employee> Employees { get; } = new()
        {
            new Employee { Id = 1, FirstName = "Евгений", LastName = "Кузнецов", Patronymic = "Александрович", Age = 32 },
            new Employee { Id = 2, FirstName = "Евгений", LastName = "Беляев", Patronymic = "Юрьевич", Age = 35 },
            new Employee { Id = 3, FirstName = "Сергей", LastName = "Чурствин", Patronymic = "Юрьевич", Age = 33 }
        };

        public static IEnumerable<Section> Sections { get; } = new[]
        {
            new Section{Id = 1, Name = "Спорт", Order = 1},
            new Section{Id = 2, Name = "Обувь", Order = 2, ParentId = 1},
            new Section{Id = 3, Name = "Аксессуары", Order = 3},
            new Section{Id = 4, Name = "Часы", Order = 4, ParentId = 3},
            new Section{Id = 5, Name = "Браслеты", Order = 5, ParentId = 3},
        };

        public static IEnumerable<Brand> Brands { get; } = new[]
        {
            new Brand{Id = 1, Name = "Adidas", Order = 1},
            new Brand{Id = 2, Name = "Nike", Order = 2},
        };

        public static IEnumerable<Product> Products { get; } = new[]
        {
            new Product{Id = 1, Name = "Кроссовки", Order = 1, ImageUrl = "image9.jpg", BrandId = 1, SectionId = 2, Price = 9000},
            new Product{Id = 2, Name = "Часы", Order = 2, ImageUrl = "image10.jpg", SectionId = 4, Price = 12000},
        };
    }
}
