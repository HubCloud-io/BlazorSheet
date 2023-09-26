using HubCloud.BlazorSheet.Core.Models;

namespace HubCloud.BlazorSheet.WasmExamples.Shared.Helpers;

public class SheetWithCellNamesBuilder
{
    public SheetSettings BuildSettings(int rowsCount, int columnsCount)
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