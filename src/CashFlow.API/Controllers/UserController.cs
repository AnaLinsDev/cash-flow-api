using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.API.Controllers;
[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase userCase,
        [FromBody] RequestRegisterUserJson request
        )
    {
        var response = await userCase.Execute(request);

        return Created(string.Empty, response);
    }
}
