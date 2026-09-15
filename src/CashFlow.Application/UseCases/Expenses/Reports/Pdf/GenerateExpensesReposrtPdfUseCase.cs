
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Fonts;

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

        var paragraph = page.AddParagraph();
        var title = String.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, month.ToString("Y"));

        paragraph.AddFormattedText(
            title,
            new Font
            {
                Name = FontHelper.RALEWAY_REGULAR,
                Size = 15
            });

        paragraph.AddLineBreak();

        var totalExpenses = expenses.Sum(expenses => expenses.Amount);
        var totalExpensesFormatted = $"{totalExpenses} {CURRENCY_SYMBOL}";

        paragraph.AddFormattedText(
            totalExpensesFormatted,
            new Font
            {
                Name = FontHelper.WORKSANS_BLACK,
                Size = 50
            });

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
