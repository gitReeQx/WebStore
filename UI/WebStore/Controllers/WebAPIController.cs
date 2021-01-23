using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Interfaces.TestAPI;

namespace WebStore.Controllers
{
    public class WebAPIController : Controller
    {
        private readonly IValuesServices valueServices;

        public WebAPIController(IValuesServices ValueServices)
        {
            valueServices = ValueServices;
        }

        public IActionResult Index()
        {
            var values = valueServices.Get();

            return View(values);
        }
    }
}
