
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf;
public class GenerateExpensesReposrtPdfUseCase : IGenerateExpensesReposrtPdfUseCase
{
    private const string CURRENCY_SYMBOL = "€";
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
}
