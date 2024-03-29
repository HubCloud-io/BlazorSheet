﻿using HubCloud.BlazorSheet.Core.Models;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.ServerSideExamples.Pages;

public partial class SheetRenderPage: ComponentBase
{
    private Sheet _sheet;
    private SheetSettings _sheetSettings;

    protected override void OnInitialized()
    {
        _sheetSettings = BuildSheetSettingsWithCellNames(1000, 15);

        _sheet = new Sheet(_sheetSettings);
    }
    
    private SheetSettings BuildSheetSettingsWithCellNames(int rowsCount, int columnsCount)
    {
        var sheetSettings = new SheetSettings()
        {
            RowsCount = rowsCount,
            ColumnsCount = columnsCount,
            FreezedColumns = 3,
            FreezedRows = 3
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