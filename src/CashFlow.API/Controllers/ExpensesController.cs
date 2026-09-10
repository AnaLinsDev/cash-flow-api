using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.API.Controllers;

[Route("api/expenses")]
[ApiController]
public class ExpensesController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Register([
        FromServices] IRegisterExpenseUseCase useCase, 
        [FromBody] RequestRegisterExpenseJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }
}
