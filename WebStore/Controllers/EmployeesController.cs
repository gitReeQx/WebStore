using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        private List<Employee> employees;

        public EmployeesController() => employees = TestData._employees;

        public IActionResult Index() => View(TestData._employees);

        public IActionResult Details(int id)
        {
            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee is not null)
                return View(employee);

            return NotFound();
        }

        public IActionResult Edit(int id)
        {
            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee is not null)
                return View(employee);

            return NotFound();
        }

        public IActionResult SaveChanges(Employee _employee)
        {
            int index = employees.IndexOf(employees.Where(n => n.Id == _employee.Id).FirstOrDefault());
            employees[index] = _employee;
            return RedirectToAction("Details", new { id = _employee.Id });
        }

        public IActionResult Remove(int id)
        {
            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee is not null)
            {
                employees.Remove(employee);
                return RedirectToAction("Index");
            }

            return NotFound();
        }
    }
}
