//George Michael 991652543
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.TutorEntities
{
    public class Tutor
    {
        public Guid TutorId { get; set; }
        public string TutorName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Course { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public List<Session>? Sessions { get; set; }
        public List<Review>? Reviews { get; set; }
    }
}