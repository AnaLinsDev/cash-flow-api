
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Colors;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf;
public class GenerateExpensesReposrtPdfUseCase : IGenerateExpensesReposrtPdfUseCase
{
    private const string CURRENCY_SYMBOL = "€";
    private const int HEIGHT_ROW_EXPENSE_TABLE = 25;
    private readonly IExpensesReadOnlyRepository _repository;
    public GenerateExpensesReposrtPdfUseCase(IExpensesReadOnlyRepository repository)
    {
        _repository = repository;

        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var expenses = await _repository.FilterByMonth(month);

        if (expenses.Count == 0)
        {
            return [];
        }

        var document = CreateDocument(month);
        var page = CreatePage(document);

        CreateHeaderWithLogoAndName(page);

        var totalExpenses = expenses.Sum(expenses => expenses.Amount);
        CreateTotalSpentSection(page, month, totalExpenses);

        foreach (var expense in expenses)
        {
            var table = CreateExpenseTable(page, expense);

            // First Row __________
            var row = table.AddRow();
            row.Height = HEIGHT_ROW_EXPENSE_TABLE;
            AddExpenseTitle(row.Cells[0], expense.Title);
            AddHeaderForAmount(row.Cells[3]);

            // Second Row __________
            row = table.AddRow();
            row.Height = HEIGHT_ROW_EXPENSE_TABLE;

            row.Cells[0].AddParagraph(expense.Date.ToString("D"));
            SetStyleBaseForExpenseInformation(row.Cells[0]);
            row.Cells[0].Format.LeftIndent = 20;

            row.Cells[1].AddParagraph(expense.Date.ToString("t"));
            SetStyleBaseForExpenseInformation(row.Cells[1]);

            row.Cells[2].AddParagraph(expense.PaymentType.PaymentTypeToString());
            SetStyleBaseForExpenseInformation(row.Cells[2]);

            AddAmountForExpense(row.Cells[3], expense.Amount);

            // Third Row __________
            if (!string.IsNullOrWhiteSpace(expense.Description))
            {
                var descriptionRow = table.AddRow();
                descriptionRow.Height = HEIGHT_ROW_EXPENSE_TABLE;

                AddExpenseDescription(descriptionRow.Cells[0], expense.Description);

                row.Cells[3].MergeDown = 1;
            }

            // White Space __________
            AddWhiteSpace(table);
        }

        return RenderDocument(document);
    }

    private Document CreateDocument(DateOnly month)
    {
        var doc = new Document();
        doc.Info.Title = $"{ResourceReportGenerationMessages.EXPENSES_FOR}: {month:Y}";
        doc.Info.Author = "Ana Julia Lins";

        var style = doc.Styles["Normal"];
        style!.Font.Name = FontHelper.DEFAULT_FONT;


        return doc;
    }

    private Section CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();

        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;

        return section;
    }

    private void CreateHeaderWithLogoAndName(Section page)
    {
        var table = page.AddTable();
        table.AddColumn(); // [0]
        table.AddColumn("300"); // [1]

        var greetings = String.Format(ResourceReportGenerationMessages.GREETINGS, "Ana Julia Lins");

        var assembly = Assembly.GetExecutingAssembly();
        var directoryName = Path.GetDirectoryName(assembly.Location);
        var pathFile = Path.Combine(directoryName!, "Logo", "logo.png");

        var row = table.AddRow();
        row.Cells[0].AddImage(pathFile);
        row.Cells[1].AddParagraph(greetings);
        row.Cells[1].Format.Font = new Font
        {
            Name = FontHelper.RALEWAY_BLACK,
            Size = 16
        };
        row.Cells[1].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
    }

    private void CreateTotalSpentSection(Section page, DateOnly month, decimal totalExpenses)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = "40";
        paragraph.Format.SpaceAfter = "40";

        var title = String.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, month.ToString("Y"));

        paragraph.AddFormattedText(
            title,
            new Font
            {
                Name = FontHelper.RALEWAY_REGULAR,
                Size = 15
            });

        paragraph.AddLineBreak();

        var totalExpensesFormatted = $"{totalExpenses} {CURRENCY_SYMBOL}";

        paragraph.AddFormattedText(
            totalExpensesFormatted,
            new Font
            {
                Name = FontHelper.WORKSANS_BLACK,
                Size = 50
            });

    }

    private Table CreateExpenseTable(Section page, Expense expense)
    {
        var table = page.AddTable();

        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

        return table;
    }

    private byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document,
        };

        renderer.RenderDocument();

        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);

        return file.ToArray();
    }

    private void AddExpenseTitle(Cell cell, string expenseTitle)
    {
        cell.AddParagraph(expenseTitle);
        cell.MergeRight = 2;
        cell.Shading.Color = ColorsHelper.RED_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.Format.LeftIndent = 20;
        cell.Format.Font = new Font
        {
            Name = FontHelper.RALEWAY_BLACK,
            Size = 14,
            Color = ColorsHelper.BLACK
        };
    }

    private void AddHeaderForAmount(Cell cell)
    {
        cell.AddParagraph(ResourceReportGenerationMessages.AMOUNT);
        cell.Shading.Color = ColorsHelper.RED_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.Format.Font = new Font
        {
            Name = FontHelper.RALEWAY_BLACK,
            Size = 14,
            Color = ColorsHelper.WHITE
        };
    }

    private void SetStyleBaseForExpenseInformation(Cell cell)
    {
        cell.Shading.Color = ColorsHelper.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.Format.Font = new Font
        {
            Name = FontHelper.WORKSANS_REGULAR,
            Size = 12,
            Color = ColorsHelper.BLACK
        };
    }

    private void AddAmountForExpense(Cell cell, decimal expenseAmount)
    {
        var fomattedAmount = $"- {expenseAmount} {CURRENCY_SYMBOL}";
        cell.AddParagraph(fomattedAmount);
        cell.Shading.Color = ColorsHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.Format.Font = new Font
        {
            Name = FontHelper.WORKSANS_REGULAR,
            Size = 14,
            Color = ColorsHelper.BLACK
        };
    }

    private void AddExpenseDescription(Cell cell, string expenseDescription)
    {
        cell.AddParagraph(expenseDescription);
        cell.MergeRight = 2;
        cell.Shading.Color = ColorsHelper.GREEN_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.Format.LeftIndent = 20;
        cell.Format.Font = new Font
        {
            Name = FontHelper.WORKSANS_REGULAR,
            Size = 10,
            Color = ColorsHelper.BLACK
        };
    }

    private void AddWhiteSpace(Table table) {
        var row = table.AddRow();
        row.Height = 30;
        row.Borders.Visible = false;
    }

}
