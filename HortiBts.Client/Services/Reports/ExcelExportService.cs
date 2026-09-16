using ClosedXML.Excel;
using Microsoft.JSInterop;

namespace HortiBts.Client.Services.Reports;

public class ExcelColumn<T>
{
    public string HeaderEn { get; set; } = "";
    public string HeaderHi { get; set; } = "";
    public Func<T, object?> ValueSelector { get; set; } = _ => "";
    public Func<IEnumerable<T>, object?>? TotalSelector { get; set; }
}

public class ExcelExportService
{
    private readonly IJSRuntime _js;
    public ExcelExportService(IJSRuntime js) => _js = js;

    public async Task ExportAsync<T>(
        List<T> data,
        List<ExcelColumn<T>> columns,
        string sheetName,
        Func<string, string, string> lang,
        string fileName)
    {
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add(sheetName.Length > 31 ? sheetName[..31] : sheetName);

        // Header row
        ws.Cell(1, 1).Value = lang("S.No.", "क्रमाँक");
        ws.Cell(1, 1).Style.Font.Bold = true;
        for (int c = 0; c < columns.Count; c++)
        {
            ws.Cell(1, c + 2).Value = lang(columns[c].HeaderEn, columns[c].HeaderHi);
            ws.Cell(1, c + 2).Style.Font.Bold = true;
        }

        // Data rows
        for (int row = 0; row < data.Count; row++)
        {
            ws.Cell(row + 2, 1).Value = row + 1;
            for (int c = 0; c < columns.Count; c++)
            {
                var value = columns[c].ValueSelector(data[row]);
                SetCellValue(ws.Cell(row + 2, c + 2), value);
            }
        }

        // Totals row
        if (columns.Any(c => c.TotalSelector != null))
        {
            int totalRow = data.Count + 2;
            ws.Cell(totalRow, 1).Value = lang("Total", "योग");
            ws.Row(totalRow).Style.Font.Bold = true;
            for (int c = 0; c < columns.Count; c++)
            {
                if (columns[c].TotalSelector != null)
                    SetCellValue(ws.Cell(totalRow, c + 2), columns[c].TotalSelector!(data));
            }
        }

        ws.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        using var streamRef = new DotNetStreamReference(stream);
        await _js.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
    }

    private static void SetCellValue(IXLCell cell, object? value)
    {
        switch (value)
        {
            case null: cell.Value = ""; break;
            case int i: cell.Value = i; break;
            case double d: cell.Value = d; break;
            case decimal m: cell.Value = m; break;
            default: cell.Value = value.ToString(); break;
        }
    }
}