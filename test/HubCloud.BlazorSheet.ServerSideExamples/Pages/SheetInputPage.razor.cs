using Company.WebApplication1.Helpers;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine;
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
        var cellAddress = _sheet.CellAddress(cell);
        _evaluator.SetValue(cellAddress.Row, cellAddress.Column, cell.Value);
        _evaluator.EvalSheet();
    }

    private void OnClearLogClick()
    {
        _evaluator.ClearLog();
    }
    
}