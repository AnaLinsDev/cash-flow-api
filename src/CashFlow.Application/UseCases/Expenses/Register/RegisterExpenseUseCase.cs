using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.Register;
public class RegisterExpenseUseCase
{
    public ResponseRegisterExpenseJson Execute(RequestRegisterExpenseJson request)
    {

        validate(request);

        return new ResponseRegisterExpenseJson
        {
            Title = request.Title,
        };
    }

    private void validate(RequestRegisterExpenseJson request)
    {
        var titleIsEmpty = string.IsNullOrWhiteSpace(request.Title);

        if (titleIsEmpty)
        {
            throw new ArgumentException("The title is required.");
        }

        if (request.Amount <= 0)
        {
            throw new ArgumentException("The amount must be greater than 0.");
        }

        var dateComparison = DateTime.Compare(request.Date, DateTime.UtcNow);

        if (dateComparison > 0)
        {
            throw new ArgumentException("Expenses cannot be for the future.");
        }

        var paymentTypeIsValid = Enum.IsDefined(typeof(PaymentType), request.PaymentType);

        if (!paymentTypeIsValid)
        {
            throw new ArgumentException("Payment Type is not valid.");
        }
    }
}

