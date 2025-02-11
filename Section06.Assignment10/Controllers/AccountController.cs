using Microsoft.AspNetCore.Mvc;

namespace Section6.Assignment10.Controllers;

public class AccountController : Controller
{
    private AccountInfo accountInfo = new(AccountNumber: 1001, AccountHolderName: "Example Name", CurrentBalance: 5000);

    [Route("/account-details")]
    public IActionResult AccountDetails()
    {
        return Json(accountInfo);
    }

    [Route("/account-statement")]
    public IActionResult AccountStatement()
    {
        return File("statement.pdf", "application/pdf", "statement.pdf");
    }

    [Route("/get-current-balance/{accountNumber:int?}")]
    public IActionResult GetCurrentBalance()
    {
        if (!Request.RouteValues.ContainsKey("accountNumber"))
            return NotFound("Account Number should be supplied");

        int accountNumber = int.Parse(Request.RouteValues["accountNumber"]?.ToString() ?? "0");

        if (accountNumber != accountInfo.AccountNumber)
            return BadRequest($"Account Number should be {accountInfo.AccountNumber}");

        return Content(accountInfo.CurrentBalance.ToString(), "plain/text");
    }
}

public record AccountInfo(int AccountNumber, string AccountHolderName, decimal CurrentBalance);
