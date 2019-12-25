using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComptageTirAPI.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ComptageTirAPI.Controllers
{
    [Route("api/[controller]")]
    public class ArcheryController : Controller
    {
        private readonly ArcheryService _archeryService;

        public ArcheryController(ArcheryService feminaService)
        {
            _archeryService = feminaService;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("Connect/{username}/{password}")]
        public string Connect(string username, string password)
        {
            _archeryService.
        }

        [HttpGet("testadd")]
        public string TestAdd()
        {
            _archeryService.AddAccount("thomas" , "test");

            return "ok";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
