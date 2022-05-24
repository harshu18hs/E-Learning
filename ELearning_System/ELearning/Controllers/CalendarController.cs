using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearning.Controllers
{


    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private ApplicationDbContext _databaseContext;
        public CalendarController(ApplicationDbContext _databaseContext)
        {
            this._databaseContext = _databaseContext;

        }

        [HttpGet]
        public ActionResult CurrentDateTime()
        {
            try
            {
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostEvent([FromBody] Calendar @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _databaseContext.Events.Add(@event);
            await _databaseContext.SaveChangesAsync();

            return CreatedAtAction("GetEvents", new { id = @event.Id }, @event);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvents([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var @event = await _databaseContext.Events.Where(m => m.Id == id).ToListAsync();
            if (@event == null)
            {
                return NotFound();
            }
            return Ok(@event);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var @event = await _databaseContext.Events.SingleOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            _databaseContext.Events.Remove(@event);
            await _databaseContext.SaveChangesAsync();

            return Ok(@event);
        }
    }
}
