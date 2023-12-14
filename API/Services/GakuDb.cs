using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Models.ExpensesEnities;
using API.Models.TutorEntities;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class GakuDb : DbContext
    {
        public DbSet<CourseFeedback> Feedback {get; set;}
        public DbSet<Events> Calendar { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Finance> Finances { get; set; }
        public DbSet<Tutor> Tutors { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=gaku.db");
        }

    }
}