using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.Infrastructure.Services
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private readonly List<Employee> employees = TestData.Employees;

        public int Add(Employee employee)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            if (employees.Contains(employee))
            {
                return employee.Id;
            }

            employee.Id = employees
                .Select(item => item.Id)
                .DefaultIfEmpty()
                .Max() + 1;

            employees.Add(employee);
            return employee.Id;
        }

        public bool Delete(int id)
        {
            var item = Get(id);

            if (item is null)
                return false;

            return employees.Remove(item);
        }

        public IEnumerable<Employee> Get() => employees;

        public Employee Get(int id) => employees.FirstOrDefault(item => item.Id == id);

        public void Update(Employee employee)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            if (employees.Contains(employee))
            {
                return;
            }

            var db_item = Get(employee.Id);
            
            if (db_item is null) return;

            db_item.FirstName = employee.FirstName;
            db_item.LastName = employee.LastName;
            db_item.Patronymic = employee.Patronymic;
            db_item.Age = employee.Age;

        }
    }
}
