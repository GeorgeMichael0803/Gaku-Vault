//Mohammad Fahad Khan 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var result = await _db.Calendar.ToListAsync();
            await _db.SaveChangesAsync();

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(Guid id, Events events)
        {
            var result = await _db.Calendar.FindAsync(id);

            if (result == null)
                return NotFound("Event not Found");
            
            result.Title = events.Title;
            result.Description = events.Description;
            result.StartTime = events.StartTime;
            result.EndTime = events.EndTime;
            result.IsReminderSet = events.IsReminderSet;
            result.IsRecurring = events.IsRecurring;

            await _db.SaveChangesAsync();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var result = await _db.Calendar.FindAsync(id);

            if (result == null)
                return NotFound("Event not Found");

            _db.Calendar.Remove(result);
            await _db.SaveChangesAsync();

            return Ok(result);
        }
        
    }
}