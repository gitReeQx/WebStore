using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Infrastructure.Interfaces;
using WebStore.Models;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    //[Route("Users")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData employees;
        public EmployeesController(IEmployeesData _employees) => employees = _employees;
        //[Route("All")]
        public IActionResult Index()
        {
            var _employees = employees.Get();
            return View(_employees);
        }
        //[Route("Users/Details({id})")]
        public IActionResult Details(int id)
        {
            var employee = employees.Get(id);
            if (employee is not null)
                return View(employee);

            return NotFound();
        }

        #region Editing Employee
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }
            var employee = employees.Get(id);
            if (employee is null)
                return NotFound();

            return View(new EmployeesViewModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
            });
        }

        [HttpPost]
        public IActionResult Edit(EmployeesViewModel _model)
        {
            if (_model is null)
                throw new ArgumentNullException(nameof(_model));

            var employee = new Employee
            {
                Id = _model.Id,
                FirstName = _model.FirstName,
                LastName = _model.LastName,
                Patronymic = _model.Patronymic,
                Age = _model.Age,
            };

            employees.Update(employee);
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete Employee
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var employee = employees.Get(id);
            if (employee is null)
                return NotFound();

            return View(new EmployeesViewModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
            });
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            employees.Delete(id);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
