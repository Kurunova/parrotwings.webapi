using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ParrotWings.WebAPI.Core.Controllers
{
    //[Authorize]
    //[ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        //[HttpPost]
        //public IActionResult CreateTransaction(Transaction transaction)
        //{
        //    try
        //    {
        //        var userId = Int32.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
        //        transaction.SenderUserId = userId;
        //        _transactionService.Create(transaction);
        //    }
        //    catch (ConditionException exception)
        //    {
        //        ModelState.AddModelError("General", exception.Message);
        //    }
        //    catch (Exception)
        //    {
        //        ModelState.AddModelError("General", "An error occured");
        //    }

        //    return PartialView();
        //}

        //[HttpGet]
        //public IActionResult UserBalance()
        //{
        //    var balance = _transactionService.GetUserBalance(userId);
        //    return balance.ToString();
        //}

        //[HttpGet]
        //public IActionResult UserTransactions()
        //{
        //    var userTransactions = _transactionService.GetUserTransactions(userId, 15);
        //    return View(userTransactions);
        //}
    }
}