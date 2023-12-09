using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Services;
using API.Models.TutorEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TutorsController : ControllerBase
    {
        private GakuDb _db = new GakuDb();


        //POST api/tutor
        [HttpPost]
        public async Task<IActionResult> CreateTutor (Tutor tutor)
        {

            //todo: validation -> either [required] in model or ErrorResponse Method

            await _db.AddAsync(tutor);
            await _db.SaveChangesAsync();
            return Ok( new {
                UserName = tutor.TutorName,
                UserId = tutor.TutorId
            });

        }


        //Get api/tutor/byCourse?course=prog
        [HttpGet("available/byCourse")]
        public async Task<IActionResult> GetAvailableTutorByCourse ([FromBody] string course)
        {
            if(string.IsNullOrWhiteSpace(course))
                return BadRequest("Course is required");

            var availableTutorsByCourse = await _db.Tutors.Where( x=> x.IsAvailable==true && x.Course.ToLower().Contains(course.ToLower())).ToListAsync();

            return Ok(availableTutorsByCourse);
        }

        //GET api/tutor/{tutorId}
        public async Task<IActionResult> GetTutorById (Guid tutorId)
        {
            var tutor = await _db.Tutors.FirstOrDefaultAsync(x=>x.TutorId==tutorId);
            if(tutor == null)
                return NotFound("Tutor with this id is not found");

            return Ok(tutor);
        }


        //GET api/tutor
        public async Task<IActionResult> GetAllTutors()
        {
            var tutors = await _db.Tutors.ToListAsync();
            if( tutors == null)
                return NotFound("There are no tutors");
            return Ok(tutors);
        }


        // PUT: api/tutor/{tutorId}
        [HttpPut("{tutorId}")]
        public async Task<IActionResult> UpdateTutorProfile(Guid tutorId, [FromBody] Tutor updatedTutor)
        {
            var existingTutor = await _db.Tutors.FirstOrDefaultAsync(t => t.TutorId == tutorId);
            if (existingTutor == null)
                return NotFound($"Tutor with ID {tutorId} not found.");

            // Update only the provided fields
            if (!string.IsNullOrWhiteSpace(updatedTutor.TutorName))
                existingTutor.TutorName = updatedTutor.TutorName;
            
            if (!string.IsNullOrWhiteSpace(updatedTutor.Email))
                existingTutor.Email = updatedTutor.Email;
            
            if (!string.IsNullOrWhiteSpace(updatedTutor.PhoneNumber))
                existingTutor.PhoneNumber = updatedTutor.PhoneNumber;
            
            if (!string.IsNullOrWhiteSpace(updatedTutor.Course))
                existingTutor.Course = updatedTutor.Course;

            if (!string.IsNullOrWhiteSpace(updatedTutor.Description))
                existingTutor.Description = updatedTutor.Description;
            
            if (updatedTutor.IsAvailable != existingTutor.IsAvailable)
                existingTutor.IsAvailable = updatedTutor.IsAvailable;


            _db.SaveChanges();
            return Ok(existingTutor);
        }
     }
}