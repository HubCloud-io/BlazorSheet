using HubCloud.BlazorSheet.Core.Models;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.WasmExamples.Client.Pages;

public partial class SheetTestRenderPage: ComponentBase
{
    private int _rowsCount = 100;
    private int _columnsCount = 4;
    
    private Sheet _sheet;
    private SheetSettings _sheetSettings;
    
    protected override void OnAfterRender(bool firstRender)
    {
        Console.WriteLine($"Rendered: {DateTime.Now:yyyy-MM-dd:hh:mm:ss.fff}");
    }

    private void OnRenderClick()
    {
        Console.WriteLine($"Start sheet build: {DateTime.Now:yyyy-MM-dd:hh:mm:ss.fff}");
        
        _sheetSettings = BuildSheetSettingsWithCellNames(_rowsCount, _columnsCount);

        _sheet = new Sheet(_sheetSettings);
        
        Console.WriteLine($"End sheet build: {DateTime.Now:yyyy-MM-dd:hh:mm:ss.fff}");
    }
    
    private SheetSettings BuildSheetSettingsWithCellNames(int rowsCount, int columnsCount)
    {
        var sheetSettings = new SheetSettings()
        {
            RowsCount = rowsCount,
            ColumnsCount = columnsCount,
            // FreezedColumns = 3,
            // FreezedRows = 3
        };

        for (var r = 1; r <= rowsCount; r++)
        {
            var newRow = new SheetRow();
            for (var c = 1; c <= columnsCount; c++)
            {
                SheetColumn column;
                if (r == 1)
                {
                    column = new SheetColumn();
                    sheetSettings.Columns.Add(column);
                }
                else
                {
                    column = sheetSettings.Columns[c - 1];
                }

                var newCell = new SheetCell()
                {
                    RowUid = newRow.Uid,
                    ColumnUid = column.Uid,
                    Value = $"R{r}C{c}"
                };
                sheetSettings.Cells.Add(newCell);
            }

            sheetSettings.Rows.Add(newRow);
        }

        return sheetSettings;
    }
}