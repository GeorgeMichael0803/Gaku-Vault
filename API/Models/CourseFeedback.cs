//Mohammad Fahad Khan
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class CourseFeedback
    {
        public Guid Id { get; set; }
        public string? CourseId { get; set; }
        public string? StudentId { get; set; }
        public int Rating { get; set; }
        public string? Feedback { get; set; }
        public DateTime DateSubmitted { get; set; } = DateTime.UtcNow;
    }
}