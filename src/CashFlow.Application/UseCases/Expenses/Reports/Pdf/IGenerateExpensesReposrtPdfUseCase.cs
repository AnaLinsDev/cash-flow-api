namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf;
public interface IGenerateExpensesReposrtPdfUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
