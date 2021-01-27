using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    //[Route("api/[controller]")] [controller] = EmployeesApi
    [Route("api/employees")]
    [ApiController]
    public class EmployeesApiController : ControllerBase, IEmployeesData
    {
        private readonly IEmployeesData employeesData;
        private readonly ILogger<EmployeesApiController> logger;

        public EmployeesApiController(IEmployeesData EmployeesData, ILogger<EmployeesApiController> Logger)
        {
            employeesData = EmployeesData;
            logger = Logger;
        }

        [HttpGet]
        public IEnumerable<Employee> Get() => employeesData.Get();
        
        [HttpGet("{id}")]
        public Employee Get(int id) => employeesData.Get(id);

        [HttpDelete("{id}")]
        public bool Delete(int id) => employeesData.Delete(id);

        [HttpPost]
        public int Add(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                logger.LogInformation("Ошибка модели данных при добавлении сотрудника {0} {1} {2}",
                employee.LastName, employee.FirstName, employee.Patronymic);
                return 0;
            }

            logger.LogInformation("Добавление сотрудника {0} {1} {2}",
                employee.LastName, employee.FirstName, employee.Patronymic);

            var id = employeesData.Add(employee);
            if (id > 0) logger.LogInformation("Сотрудник [id: {0}] {1} {2} {3} успешно добавлен",
                employee.Id, employee.LastName, employee.FirstName, employee.Patronymic);
            else
                logger.LogWarning("Ошибка при добавлении");
            return employeesData.Add(employee);
        }

        [HttpPut]
        public void Update(Employee employee) => employeesData.Update(employee);
    }
}
