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
    public class CourseFeedbackController : ControllerBase
    {
        private GakuDb _db = new GakuDb();

        [HttpPost]
        public async Task<IActionResult> GiveFeedback(CourseFeedback feedback)
        {
            try
            {
                await _db.Feedback.AddAsync(feedback);
                await _db.SaveChangesAsync();
                return Created("",feedback);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseFeedback(Guid id)
        {
            var result = await _db.Feedback.FindAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("Course/{Id}")]
        public async Task<IActionResult> GetFeedbackByCourse(string id, int pageNumber = 1, int pageSize = 10)
        {
            var feedback = await _db.Feedback
                                    .Where(x => x.CourseId == id)
                                    .Skip((pageNumber -1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return Ok(feedback);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteFeedback(Guid id)
        {
            var result = await _db.Feedback.FindAsync(id);
            if (result == null)
                return NotFound();
            _db.Feedback.Remove(result);

            await _db.SaveChangesAsync();

            return Ok(result);
        }
    }
}