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
    public class CourseFeedbackController : ControllerBase
    {
        private GakuDb _db = new GakuDb();

        [HttpPost]
        public async Task<IActionResult> GiveFeedback(CourseFeedback feedback)
        {
            var result = await _db.Feedback.AddAsync(feedback);

            if (result == null)
                return BadRequest();
            
            await _db.SaveChangesAsync();

            return Created("", result);
        }
    }
}