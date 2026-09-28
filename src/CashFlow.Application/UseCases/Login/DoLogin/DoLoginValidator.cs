using CashFlow.Application.UseCases.Users;
using CashFlow.Communication.Requests;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCases.Login.DoLogin;
public class DoLoginValidator : AbstractValidator<RequestLoginJson>
{
    public DoLoginValidator()
    {
        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress()
            .WithMessage(ResourceErrorMessages.EMAIL_INVALID);

        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestLoginJson>());
    }
}

