using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SanctionScanner.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SanctionScanner.Controllers.Api
{
    [Route("api/[controller]")]
    public class SanctionAPIController : Controller
    {
        private readonly SanctionScannerDbContext _context;
        public SanctionAPIController(SanctionScannerDbContext context)
        {
            _context = context;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<SourceSanction>> GetSourceSanctionScanner(int id)
        {
            var sourceSanction = await _context.SourceSanctions.FindAsync(id);
            if (sourceSanction == null)
                return NotFound();
            sourceSanction.Sanctions = _context.Sanctions.Where(c => c.SourceSaction_Id == id).Skip(10).ToList();
            return sourceSanction;
        }
        [HttpGet("{sourceId}/{Citizenship}")]
        public async Task<ActionResult<Sanction>> GetSanctions(int sourceId, string citizenship)
        {
            var sanctions = _context.Sanctions.Where(c => c.SourceSaction_Id == sourceId && c.Citizenship.Contains(citizenship)).ToList();
            if (sanctions == null)
                return NotFound();
            return Ok(sanctions);
        }

        //// GET api/<controller>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

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
