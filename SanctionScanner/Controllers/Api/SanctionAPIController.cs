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
        [HttpGet("{id}/{fullName}")]
        public async Task<ActionResult<Sanction>> GetSanctionByName(int id, string fullName)
        {
            try
            {
                var sanctions = _context.Sanctions.Where(c => c.SourceSaction_Id == id && c.LegalName.Contains(fullName)).ToList();
                if (sanctions == null || sanctions.Count() == 0)
                    return NotFound($"Not Result Match For {fullName}");
                return Ok(sanctions);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("{sourceId}/{Citizenship}")]
        public async Task<ActionResult<Sanction>> GetSanctionsByCitizenship(int sourceId, string citizenship)
        {
            var errorMessage = "Please Enter CountryId";
            if (sourceId == 0)
                return NotFound(errorMessage);
            var sanctions = _context.Sanctions.Where(c => c.SourceSaction_Id == sourceId && c.Citizenship.Contains(citizenship)).ToList();
            if (sanctions == null)
                return NotFound();
            return Ok(sanctions);
        }
        //[HttpGet("{id}/{}")]
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
