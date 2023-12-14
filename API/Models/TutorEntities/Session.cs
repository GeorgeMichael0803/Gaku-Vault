//George Michael 991652543
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.TutorEntities
{
    public class Session
    {
        public Guid SessionId { get; set; }
        public string StudentName { get; set; }
        public int StudentId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public SessionStatus SessionStatus { get; set; }

        [ForeignKey("Tutor")]
        public Guid TutorId { get; set; }

        //public Tutor Tutor { get; set; }  // might have to remove this top fix the issue

    }



    public enum SessionStatus
    {
        Scheduled,
        Completed,
        Canceled,
        Ongoing,

    }
}