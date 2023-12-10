using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Models.ExpensesEnities;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class GakuDb : DbContext
    {
        public DbSet<Events> Calendar { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Finance> Finances { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=gaku.db");
        }

    }
}