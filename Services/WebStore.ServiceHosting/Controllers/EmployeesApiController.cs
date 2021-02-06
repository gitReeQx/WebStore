using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    /// <summary>
    /// API управления сотрудниками
    /// </summary>
    [Route(WebAPI.Employees)]
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

        /// <summary>Получение всех сотрудников</summary>
        /// <returns>Список сотрудников</returns>
        [HttpGet]
        public IEnumerable<Employee> Get() => employeesData.Get();

        /// <summary>Получение сотрудника по идентификатору</summary>
        /// <param name="id">Идентификатор сотрудника</param>
        [HttpGet("{id}")]
        public Employee Get(int id) => employeesData.Get(id);

        /// <summary>Добавление нового сотрудника</summary>
        /// <param name="employee">Добавляемый сотрудник</param>
        /// <returns>Идентификатор нового сотрудника</returns>
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

        /// <summary>Редактирование сотрудника</summary>
        /// <param name="employee">Информация для изменения данных сотрудника</param>
        [HttpPut]
        public void Update(Employee employee) => employeesData.Update(employee);

        /// <summary>Удаление сотрудника по его id</summary>
        /// <param name="id">Идентификатор удаляемого сотрудника</param>
        /// <returns>Истина, если сотрудник был удалён</returns>
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            var result = employeesData.Delete(id);
            if (result)
                logger.LogInformation("Сотрудник с id:{0} успешно удалён", id);
            else
                logger.LogWarning("ошибка при попытке удаления сотрудника с id:{0}", id);

            return result;
        }
    }
}
