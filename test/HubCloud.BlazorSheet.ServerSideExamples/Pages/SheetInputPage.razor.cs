using Company.WebApplication1.Helpers;
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
        _evaluator.SetLogLevel(LogLevel.Trace);
    }

    private void OnCellValueChanged(SheetCell cell)
    {
        var coordinates = _sheet.CellCoordinates(cell);
        _evaluator.SetValue(coordinates.Item1, coordinates.Item2, cell.Value);
        _evaluator.EvalSheet();
    }

    private void OnClearLogClick()
    {
        _evaluator.ClearLog();
    }
    
}