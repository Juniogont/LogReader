using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogReaderApp.Data;
using LogReaderApp.Models;
using LogReaderApp.Services;
using LogReaderApp.Services.Dto;

namespace LogReaderApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogReaderService _service;
        public LogsController(ILogReaderService service)
        {
            _service = service;
        }

        // GET: api/Logs
        [HttpGet]
        public ActionResult<IEnumerable<Log>> GetAll()
        {
            return Ok(_service.GetAll());
        }

        // GET: api/Logs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Log>> GetLog(int id)
        {
            var log = await _service.Get(id);

            if (log == null)
            {
                return NotFound();
            }

            return log;
        }

        // GET: api/Logs/5
        [HttpGet("{ip}/{user}")]
        public async Task<ActionResult<IEnumerable<Log>>> GetLog(string ip, string user)
        {
            var filter = new FilterInput();
            if (ip != "0")
                filter.Ip = ip;
            filter.User = user;

            return Ok(await _service.GetFiltered(filter));

        }

        // PUT: api/Logs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLog(int id, Log log)
        {
            if (id != log.Id)
            {
                return BadRequest();
            }           
            try
            {
                var updated = await _service.Update(id, log);
                if (updated == false)
                    return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/Logs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult PostLog([FromBody] Log log)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var id = _service.Create(log);
            return CreatedAtAction(nameof(GetLog), new { id = id }, log);
        }

        // POST: api/Logs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost, Route("Lote")]
        public async Task<ActionResult> PostLote([FromBody] Lote lote)
        {
            if (lote == null || lote.Logs == null || lote.Logs.Count() == 0)
            {
                return BadRequest();
            }
            await _service.CreateBatch(lote);
            return Ok();
        }

        // DELETE: api/Logs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Log>> DeleteLog(int id)
        {
            var log = await _service.Get(id);
            if (log == null)
            {
                return NotFound();
            }
            await _service.Delete(id);

            return new Log();
        }


    }
}
