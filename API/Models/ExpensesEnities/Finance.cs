//George Michael 991652543
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.ExpensesEnities
{
    public class Finance
    {

    [Key]
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public double MonthlyBudget { get; set; }
    public double BiWeeklySalary1 { get; set; }
    public double BiWeeklySalary2 { get; set; }
    public List<Expense>? Expenses { get; set; }      
    }
}