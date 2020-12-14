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
        };

        public static IEnumerable<Brand> Brands { get; } = new[]
        {
            new Brand{Id = 1, Name = "Adidas", Order = 1},
        };
    }
}
