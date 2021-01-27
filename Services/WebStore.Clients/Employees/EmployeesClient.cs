using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WebStore.Clients.Base;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesData
    {
        private readonly ILogger<EmployeesClient> logger;

        public EmployeesClient(IConfiguration Configuration, ILogger<EmployeesClient> Logger) 
            : base(Configuration, "api/employees")
        {
            logger = Logger;
        }

        public int Add(Employee employee)
        {
            return Post(Address, employee).Content.ReadAsAsync<int>().Result;
        }

        public bool Delete(int id)
        {
            return Delete($"{Address}/{id}").IsSuccessStatusCode;
        }

        public IEnumerable<Employee> Get()
        {
            return Get<IEnumerable<Employee>>(Address);
        }

        public Employee Get(int id)
        {
            return Get<Employee>($"{Address}/{id}");
        }

        public void Update(Employee employee)
        {
            Put(Address, employee);
        }
    }
}
