using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public EmployeesApiController(IEmployeesData EmployeesData) => employeesData = EmployeesData;

        [HttpGet]
        public IEnumerable<Employee> Get() => employeesData.Get();
        
        [HttpGet("{id}")]
        public Employee Get(int id) => employeesData.Get(id);

        [HttpDelete("{id}")]
        public bool Delete(int id) => employeesData.Delete(id);

        [HttpPost]
        public int Add(Employee employee) => employeesData.Add(employee);

        [HttpPut]
        public void Update(Employee employee) => employeesData.Update(employee);
    }
}
