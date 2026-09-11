using CashFlow.Communication.Requests;

namespace CashFlow.Application.UseCases.Expenses.Update;
public interface IUpdateExpenseByIdUseCase
{
    Task Execute(long id, RequestExpenseJson request);
}
