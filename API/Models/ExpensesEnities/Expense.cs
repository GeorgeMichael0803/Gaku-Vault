//George Michael 991652543
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.ExpensesEnities
{
    public class Expense
    {
    public Guid ExpenseId { get; set; }
    public double Amount { get; set; }
    public string Category { get; set; }
    public string NameOrDescription { get; set; }
    public DateTime Date { get; set; }

    [ForeignKey("Finance")]
    public Guid UserId { get; set; }
    
    //public Finance Finance { get; set; } 
    }
}