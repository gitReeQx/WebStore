using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebStore.ServiceHosting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static readonly List<string> _Values = Enumerable
            .Range(1, 10)
            .Select(i => $"Value{i:00}")
            .ToList();

        [HttpGet]
        public IEnumerable<string> Get() => _Values;

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            if (id < 0) return BadRequest();
            if (id >= _Values.Count) return NotFound();

            return _Values[id];
        }

        [HttpPost]
        [HttpPost("add")] // http://localhost:5001/api/values/add
        public ActionResult Post([FromBody] string value)
        {
            _Values.Add(value);
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            if (id < 0) return BadRequest();
            if (id >= _Values.Count) return NotFound();

            _Values[id] = value;
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (id < 0) return BadRequest();
            if (id >= _Values.Count) return NotFound();

            _Values.RemoveAt(id);

            return Ok();
        }
    }
}
