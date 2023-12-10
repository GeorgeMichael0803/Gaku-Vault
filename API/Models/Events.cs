//Mohammad Fahad Khan
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Events
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description {get; set;}
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsReminderSet { get; set; }
        public bool IsRecurring { get; set; }
    }
}