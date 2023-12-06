using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private GakuDb _db = new GakuDb();

        [HttpPost]
        public async Task<IActionResult> CreateEvent(Events events)
        {
            await _db.Calendar.AddAsync(events);
            await _db.SaveChangesAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventbyID(Guid id)
        {
            var result = await _db.Calendar.FindAsync(id);
            await _db.SaveChangesAsync();
            
            if (result == null)
                return NotFound();
            
            return Ok(result);
        }
        
    }
}