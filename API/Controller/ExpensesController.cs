//George Michael 991652543
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Models.ExpensesEnities;
using API.Services;
using Microsoft.EntityFrameworkCore;
using API.Responses;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ExpensesController : ControllerBase
    {
        private GakuDb _db = new GakuDb();


        //POST expenses/finance
        [HttpPost("finance")]
        public async Task<IActionResult> CreateFinance(Finance finance)
        {
            //todo: validation -> either [required] in model or ErrorResponse Method

            await _db.AddAsync(finance);
            await _db.SaveChangesAsync();
            return Ok( new {
                UserName = finance.UserName,
                UserId = finance.UserId
            });
        }

        //POST expenses
        [HttpPost]
        public async Task<IActionResult> CreateExpense(Expense expense)
        {
            //todo: validation -> either [required] in model or ErrorResponse Method
            await _db.AddAsync(expense);
            await _db.SaveChangesAsync();

            return Ok( new {
                ExpenseId = expense.ExpenseId,
                NameOrDescription = expense.NameOrDescription
            });
        }


        // Get api/expenses/summary{userId}
        [HttpGet("summary/{userId}")]
        public async Task<IActionResult> GetSummary(Guid userId)
        {

            var user = await _db.Finances.Include(x=>x.Expenses).FirstOrDefaultAsync(x=>x.UserId == userId);

            if(user == null)
                return NotFound("User not found");

            var totalExpenses = user.Expenses.Sum(x=> x.Amount);
            var totalIncome = user.BiWeeklySalary1 + user.BiWeeklySalary2;
            var remainingBudget = user.MonthlyBudget - totalExpenses;

            return Ok( new{
                TotalExpenses = totalExpenses,
                TotalIncome = totalIncome,
                RemainingBudget = remainingBudget
            });
        }


        //GET api/expenses/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllExpenses(Guid userId, int pageNumber=1, int pageSize =10)
        {
            var user = await _db.Finances.Include(x=>x.Expenses).FirstOrDefaultAsync(x=>x.UserId == userId);
            
            if(user == null)
                return NotFound("User not found");

            var userExpenses = await _db.Expenses.Where(x => x.UserId==userId)
                                             .OrderBy(x=>x.Date)
                                             .Skip((pageNumber-1) *pageSize)
                                             .Take(pageSize)
                                             .ToListAsync();

            var totalExpenses = await _db.Expenses.Where(x=> x.UserId == userId).CountAsync();
            var totalPages =(int)Math.Ceiling(totalExpenses / (double)pageSize);

            var response = new PagedResponse<Expense>(userExpenses);

            response.Meta.Add("TotalPages" , totalPages);
            response.Meta.Add("TotalRecords", totalExpenses);

            string baseURL = $"/api/expenses/{userId}";

            response.Links.Add("First", $"{baseURL}?pageNumber=1&pageSize={pageSize}");
            response.Links.Add("Last", $"{baseURL}?pageNumber={totalPages}&pageSize={pageSize}");

            if(pageNumber > 1)
            {
                response.Links.Add("Prev" , $"{baseURL}?pageNumber={pageNumber - 1}&pageSize={pageSize}");
            }

            if(pageNumber <totalPages)
            {
                response.Links.Add("Next" , $"{baseURL}?pageNumber={pageNumber +1}&pagesize={pageSize}");
            }
            return Ok(response);
        }


        //DELETE /api/expense/deleteById{expenseId}
        [HttpDelete("deleteById{expenseId}")]
        public async Task<IActionResult> DeleteExpenseById(Guid expenseId)
        {

            var expense = await _db.Expenses.FindAsync(expenseId);

            if(expense==null)
                return NotFound("Expense does not exist");

            _db.Expenses.Remove(expense);
            await _db.SaveChangesAsync();

            return Ok("Expense has been deleted");
        }

        //DELETE /api/expense/deleteByName{nameOrDescription}
        [HttpDelete("deleteByName{nameOrDescription}")]
        public async Task<IActionResult> DeleteExpenseByNameOrDescription(string nameOrDescription)
        {

            var expense = await _db.Expenses.FirstOrDefaultAsync(x=> x.NameOrDescription == nameOrDescription);

            if(expense==null)
                return NotFound("Expense does not exist");

            _db.Expenses.Remove(expense);
            await _db.SaveChangesAsync();

            return Ok("Expense has been deleted");
        }

        //DELETE api/expense/finance/{userId}
        [HttpDelete("finance/{userId}")]
        public async Task<IActionResult> DeleteUserById (Guid userId)
        {

            var user = await _db.Finances.Include(x=> x.Expenses).FirstOrDefaultAsync(x=> x.UserId == userId);

            if(user == null)
                return NotFound("User is not found");

            _db.Expenses.RemoveRange(user.Expenses);

            _db.Finances.Remove(user);
            await _db.SaveChangesAsync();

            return Ok("The user has been deleted");
        }



        //DELETE api/expense/finance/byName/{userName}
        [HttpDelete("finance/byName/{userName}")]
        public async Task<IActionResult> DeleteUserByNameOrDescription (string userName)
        {

            var user = await _db.Finances.Include(x=> x.Expenses).FirstOrDefaultAsync(x=> x.UserName.ToLower() == userName.ToLower());

            if(user == null)
                return NotFound("User is not found");

            _db.Expenses.RemoveRange(user.Expenses);

            _db.Finances.Remove(user);
            await _db.SaveChangesAsync();

            return Ok("The user has been deleted");
        }



                       
    }
}


        //Todo if time 

        // //PUT api/expense{expenseId}
        // [HttpPut("api/expense{expenseId}")]
        // public async Task<IActionResult> UpdateExpense(Guid expenseId, Expense expense)
        // {
        //     return Ok();
        // }

        // //PUT api/expense{expenseId}
        // [HttpPut("api/expense/finance/{UserId}")]
        // public async Task<IActionResult> UpdateUser(Guid expenseId, Finance Finace)
        // {
        //     return Ok();
        // }