using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Models;
using WebStore.Services.Data;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration) => _configuration = configuration;

        public IActionResult Index() => View();

        public IActionResult Employees()
        {
            return View(TestData.Employees);
        }

        public IActionResult Employee(int id)
        {
            Employee employee = TestData.Employees.Find(Employee => Employee.Id == id);
            return View(employee);
        }

        public IActionResult Error404() => View();
        public IActionResult Blogs() => View();
        public IActionResult BlogSingle() => View();
        public IActionResult Cart() => View();
        public IActionResult Checkout() => View();
        public IActionResult ContactUs() => View();
        public IActionResult Login() => View();
        public IActionResult ProductDetails() => View();
        public IActionResult Shop() => View();
    }
}
