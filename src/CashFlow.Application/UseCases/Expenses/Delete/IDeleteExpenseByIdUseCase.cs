namespace CashFlow.Application.UseCases.Expenses.Delete;
public interface IDeleteExpenseByIdUseCase
{
    Task Execute(long id);
}
