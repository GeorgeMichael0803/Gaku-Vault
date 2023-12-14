//George Michael 991652543
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.TutorEntities
{
    public class Review
    {
        public Guid ReviewId { get; set; }
        public string ReviewDescription { get; set; }
        public double Rating { get; set; }

        [ForeignKey("Tutor")]
        public Guid TutorId { get; set; }
        
       // public Tutor Tutor { get; set; } //same thing as session
    }
}