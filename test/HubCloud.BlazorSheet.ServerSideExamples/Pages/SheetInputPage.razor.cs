using Company.WebApplication1.Helpers;
using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.EvalEngine;
using HubCloud.BlazorSheet.Core.Models;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.ServerSideExamples.Pages;

public partial class SheetInputPage : ComponentBase
{
    private Sheet _sheet;
    private SheetEvaluator _evaluator;

    protected override void OnInitialized()
    {
        var builder = new SheetSmallBudgetBuilder();
        _sheet = builder.BuildSheet();
        _evaluator = new SheetEvaluator(_sheet);
    }

    private void OnCellValueChanged(SheetCell cell)
    {
        var coordinates = _sheet.CellCoordinates(cell);
        _evaluator.SetValue(coordinates.Item1, coordinates.Item2, cell.Value);
        _evaluator.EvalSheet();
    }


    private SheetSettings BuildSheetSettings()
    {
        var sheetSettings = new SheetSettings();
        sheetSettings.RowsCount = 10;
        sheetSettings.ColumnsCount = 10;

        for (var r = 1; r <= sheetSettings.RowsCount; r++)
        {
            var newRow = new SheetRow();
            for (var c = 1; c <= sheetSettings.ColumnsCount; c++)
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
                };
                sheetSettings.Cells.Add(newCell);
            }

            sheetSettings.Rows.Add(newRow);
        }


        return sheetSettings;
    }
}