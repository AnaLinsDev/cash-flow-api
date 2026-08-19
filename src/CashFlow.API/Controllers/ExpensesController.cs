using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{
    [HttpPost]
    public IActionResult Register([FromBody] RequestRegisterExpenseJson request)
    {
        try
        {
            var reponse = new RegisterExpenseUseCase().Execute(request);
            return Created(string.Empty, reponse);
        }
        catch (ArgumentException ex)
        {
            var errorResponse = new ResponseErrorJson
            {
                ErrorMessage = ex.Message
            };

            return BadRequest(errorResponse);
        }
        catch
        {
            var errorResponse = new ResponseErrorJson
            {
                ErrorMessage = "Unknown error"
            };

            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }
}
