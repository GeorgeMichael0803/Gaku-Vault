//George Michael 991652543
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


        //POST api/tutors   //check
        [HttpPost]
        public async Task<IActionResult> CreateTutor (Tutor tutor)
        {

            //todo: validation -> either [required] in model or ErrorResponse Method

            await _db.AddAsync(tutor);
            await _db.SaveChangesAsync();

            return Ok( tutor);

        }


        //Get api/tutor/available/byCourse?course=prog   //check  
        [HttpGet("available/byCourse")]
        public async Task<IActionResult> GetAvailableTutorByCourse ([FromQuery] string course)
        {
            if(string.IsNullOrWhiteSpace(course))
                return BadRequest("Course is required");

            var availableTutorsByCourse = await _db.Tutors.Where( x=> x.IsAvailable==true && x.Course.ToLower().Contains(course.ToLower())).ToListAsync();

            return Ok(availableTutorsByCourse);
        }

        //GET api/tutor/{tutorId}   //check
        [HttpGet("{tutorId}")]
        public async Task<IActionResult> GetTutorById (Guid tutorId)
        {
            var tutor = await _db.Tutors.FirstOrDefaultAsync(x=>x.TutorId==tutorId);
            if(tutor == null)
                return NotFound("Tutor with this id is not found");

            return Ok(tutor);
        }


        //GET api/tutor   ///check
        [HttpGet]
        public async Task<IActionResult> GetAllTutors()
        {
            var tutors = await _db.Tutors.ToListAsync();
            if( tutors == null)
                return NotFound("There are no tutors");
            return Ok(tutors);
        }


        //PATCH api/tutors/{tutorId}    //check
        [HttpPatch("{tutorId}")]
        public async Task<IActionResult> UpdateTutorProfile(Guid tutorId,Tutor updatedTutor)
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




                // {
                // "Email":"tester@gmail.com",
                // "Course": "",
                // "TutorName":"",
                // "Description": "",
                // "PhoneNumber":"",
                //"isAvailable":true
                // }
        }


        //POST api/tutors/{tutorId}/schedule           //check
        [HttpPost("{tutorId}/schedule")]
        public async Task<IActionResult> ScheduleSession(Guid tutorId, Session session)
        {
            var tutor = await _db.Tutors.FindAsync(tutorId);

            if (tutor == null)
                return NotFound($"Tutor with ID {tutorId} not found.");

            if (!tutor.IsAvailable)
                return BadRequest("This tutor is currently not available.");

            session.TutorId = tutorId;
            session.SessionStatus = SessionStatus.Scheduled;
            //tutor.IsAvailable = false;

            var curentTime= TimeOnly.FromDateTime(DateTime.Now);
            if(session.Time==curentTime)
                session.SessionStatus = SessionStatus.Ongoing;

            _db.Sessions.Add(session);
            await _db.SaveChangesAsync();


            return Ok(session);
        }


        //GET api/tutors/sessions/{tutorId}    //check
        [HttpGet("sessions/{tutorId}")]
        public async Task<IActionResult> GetAllSessionsForTutor(Guid tutorId)
        {
            var tutor = await _db.Tutors.Include(x => x.Sessions)
                                        .FirstOrDefaultAsync(x => x.TutorId == tutorId);
            if (tutor == null)
                return NotFound($"Tutor with ID {tutorId} not found.");
    

            return Ok(tutor.Sessions);
        }


        //POST api/tutor/{tutorId}/review     //check
        [HttpPost("{tutorId}/review")]
        public async Task<IActionResult> AddReview(Guid tutorId, Review review)
        {
            if (review == null)
                return BadRequest("Review data is required.");

            var tutor = await _db.Tutors.Include(x => x.Reviews)
                                        .FirstOrDefaultAsync(x => x.TutorId == tutorId);
            if (tutor == null)
                return NotFound($"Tutor with ID {tutorId} not found.");

            tutor.Reviews.Add(review); //keep this in mind that the null might need to be chnaged here

            await _db.SaveChangesAsync(); 

            return Ok(review); 
        }


        //PUT api/tutor/session/{sessionId}     //check
        [HttpPatch("session/{sessionId}")]
        public async Task<IActionResult> CancelSession(Guid sessionId)
        {
            var session = await _db.Sessions.FindAsync(sessionId);

            if (session == null)
                return NotFound("Session with ID not found.");

            if (session.SessionStatus == SessionStatus.Canceled || session.SessionStatus == SessionStatus.Completed)
                return BadRequest("Session cannot be canceled as it is already completed or canceled.");

            session.SessionStatus = SessionStatus.Canceled;

            await _db.SaveChangesAsync();

            return Ok("Session has been canceled.");
        }


        [HttpDelete("{tutorId}")]
        public async Task<IActionResult> DeleteTutor(Guid tutorId)
        {
            var tutor = await _db.Tutors.FindAsync(tutorId);

            if (tutor == null)
                return NotFound($"Tutor with ID {tutorId} not found.");

             _db.Tutors.Remove(tutor);
            await _db.SaveChangesAsync();

            return Ok("Tutor profile has been deleted");
        }

        
     }
}