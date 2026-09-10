namespace CashFlow.Communication.Responses;
public class ResponseExpensesJson
{
    public IList<ResponseShortExpenseJson> Expenses { get; set; } = [];
}
