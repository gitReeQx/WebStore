using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private static readonly List<Employee> _employees = new()
        {
            new Employee { Id = 1, FirstName = "Евгений", LastName = "Кузнецов", Patronymic = "Александрович", Age = 32 },
            new Employee { Id = 2, FirstName = "Евгений", LastName = "Беляев", Patronymic = "Юрьевич", Age = 35 },
            new Employee { Id = 3, FirstName = "Сергей", LastName = "Чурствин", Patronymic = "Юрьевич", Age = 33 }
        };

        public HomeController(IConfiguration configuration) => _configuration = configuration;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Employees()
        {
            return View(_employees);
        }
    }
}
