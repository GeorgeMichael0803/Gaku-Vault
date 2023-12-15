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


        //POST expenses/finance    //check
        [HttpPost("finance")]
        public async Task<IActionResult> CreateFinance(Finance finance)   
        {
            //todo: validation -> either [required] in model or ErrorResponse Method

            await _db.AddAsync(finance);
            await _db.SaveChangesAsync();
            return Ok(finance);
            // new {
            //     UserName = finance.UserName,
            //     UserId = finance.UserId
            // }
        }

        //POST api/expenses
        [HttpPost]
        public async Task<IActionResult> CreateExpense(Expense expense)   //ISO 8601 format
        {
            // Check if the Finance record exists
            var financeRecord = await _db.Finances.FindAsync(expense.UserId);
            if (financeRecord == null)
                return NotFound($"Finance record with UserId not found.");

            // Link the Expense to the Finance record
            expense.UserId = financeRecord.UserId;

            //todo: validation -> either [required] in model or ErrorResponse Method
            await _db.AddAsync(expense);
            await _db.SaveChangesAsync();

            return Ok( expense);

            // new {
            //     ExpenseId = expense.ExpenseId,
            //     NameOrDescription = expense.NameOrDescription
            // }
        }


        // Get api/expenses/summary/{userId}   //This does not work
        [HttpGet("summary/{userId}")]
        public async Task<IActionResult> GetSummary(Guid userId)
        {

            var user = await _db.Finances.Include(x=>x.Expenses).FirstOrDefaultAsync(x=>x.UserId == userId);

            if(user == null)
                return NotFound("User not found");
            var userExpenses =  _db.Expenses.Where(x => x.UserId==userId).Sum(x=> x.Amount);
            //var totalExpenses = user.Expenses.Sum(x=> x.Amount);
            var totalIncome = user.BiWeeklySalary1 + user.BiWeeklySalary2;
            var remainingBudget = user.MonthlyBudget - userExpenses;

            var result =new{
                TotalExpenses = userExpenses,
                TotalIncome = totalIncome,
                RemainingBudget = remainingBudget
            };

            return Ok( result);
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


        // DELETE /api/expenses/{userId}/deleteById/{expenseId}      //no string or special characters
        [HttpDelete("{userId}/deleteById/{expenseId}")]
        public async Task<IActionResult> DeleteExpenseById(Guid userId, Guid expenseId)
        {
            // Find the expense with the given expenseId and userId
            var expense = await _db.Expenses
                                .Where(x => x.ExpenseId == expenseId && x.UserId == userId)
                                .FirstOrDefaultAsync();

            if(expense == null)
                return NotFound("Expense does not exist or does not belong to the given user");

            _db.Expenses.Remove(expense);
            await _db.SaveChangesAsync();

            return Ok("Expense has been deleted");
        }


        // DELETE /api/expense/{userId}/deleteByName/{nameOrDescription}
        [HttpDelete("{userId}/deleteByName/{nameOrDescription}")]
        public async Task<IActionResult> DeleteExpenseByNameOrDescription(Guid userId, string nameOrDescription)
        {
            var expense = await _db.Expenses
                                .Where(x => x.UserId == userId && x.NameOrDescription.ToLower() == nameOrDescription.ToLower())
                                .FirstOrDefaultAsync();

            if(expense == null)
            {
                return NotFound("Expense does not exist or does not belong to the user.");
            }

            _db.Expenses.Remove(expense);
            await _db.SaveChangesAsync();

            return Ok("Expense has been deleted.");
        }



        //DELETE api/expenses/finance/{userId}
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



        //DELETE api/expenses/finance/byName/{userName}
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



        //         //DELETE /api/expense/deleteById{expenseId}     //edit the code so that it delets the expsnes based on the user specific 
        // [HttpDelete("deleteById{expenseId}")]    
        // public async Task<IActionResult> DeleteExpenseById(Guid expenseId)
        // {

        //     var expense = await _db.Expenses.FindAsync(expenseId);

        //     if(expense==null)
        //         return NotFound("Expense does not exist");

        //     _db.Expenses.Remove(expense);
        //     await _db.SaveChangesAsync();

        //     return Ok("Expense has been deleted");
        // }






        
        // //DELETE /api/expenses/deleteByName{nameOrDescription}
        // [HttpDelete("deleteByName{nameOrDescription}")]
        // public async Task<IActionResult> DeleteExpenseByNameOrDescription(string nameOrDescription)
        // {

        //     var expense = await _db.Expenses.FirstOrDefaultAsync(x=> x.NameOrDescription == nameOrDescription);

        //     if(expense==null)
        //         return NotFound("Expense does not exist");

        //     _db.Expenses.Remove(expense);
        //     await _db.SaveChangesAsync();

        //     return Ok("Expense has been deleted");
        // }