using PdfSharp.Fonts;
using System.Reflection;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
public class ExpensesReportFontResolver : IFontResolver
{
    public byte[]? GetFont(string faceName)
    {
        var stream = ReadFontFile(faceName);

        stream ??= ReadFontFile(FontHelper.DEFAULT_FONT);

        var length = (int)stream!.Length;

        var data = new byte[length];

        stream.ReadExactly(buffer: data, offset: 0, count: length);

        return data;
    }

    public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic)
    {
        return new FontResolverInfo(familyName);
    }
    /// <summary>
    /// Loads an embedded font file using the specified font face name.
    /// </summary>
    /// <param name="faceName">The name of the embedded font file without the extension.</param>
    /// <returns>A stream containing the font file, or <c>null</c> if the resource is not found.</>
    private Stream? ReadFontFile(string faceName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        return assembly.GetManifestResourceStream($"CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts.{faceName}.ttf");
    }
}
