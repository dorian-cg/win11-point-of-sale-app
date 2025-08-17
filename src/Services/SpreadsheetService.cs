using ClosedXML.Excel;

namespace MiniPos.Services;

public class SpreadsheetService
{
    #region static factory
    private static SpreadsheetService? instance;

    public static SpreadsheetService Instance
    {
        get
        {
            instance ??= new SpreadsheetService();
            return instance;
        }
    }

    private SpreadsheetService()
    {
    }
    #endregion

    public string[,] ReadSpreadsheet(string filepath, int sheetIndex = 1)
    {        
        using var document = new XLWorkbook(filepath);

        var sheet = document.Worksheet(sheetIndex);
        var range = sheet.RangeUsed();

        if (range == null) 
        { 
            return new string[0, 0];
        }

        var rows = range.RowCount();
        var columns = range.ColumnCount();

        var dataset = new string[rows, columns];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // spreadsheets use 1-based indexing, so we adjust by adding 1
                dataset[row, col] = sheet.Cell(row + 1, col + 1).GetValue<string>();
            }
        }

        return dataset;
    }

    public void WriteSpreadsheet(string filepath, string[,] dataset, string sheetName = "Sheet1")
    {
        using var document = new XLWorkbook();

        var sheet = document.Worksheets.Add(sheetName);

        var rows = dataset.GetLength(0);
        var columns = dataset.GetLength(1);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // spreadsheets use 1-based indexing, so we adjust by adding 1
                var cell = sheet.Cell(row + 1, col + 1);
                cell.Value = dataset[row, col];
            }
        }

        document.SaveAs(filepath);
    }
}