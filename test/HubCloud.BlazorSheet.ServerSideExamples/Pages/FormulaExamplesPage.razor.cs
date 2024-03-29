﻿using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine;
using HubCloud.BlazorSheet.ExamplesShared.WorkbookBuilders;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.ServerSideExamples.Pages;

public partial class FormulaExamplesPage:ComponentBase
{
    private Workbook _workbook;
    private WorkbookEvaluator _evaluator;

    protected override void OnInitialized()
    {
        var builder = new WorkbookFormulaExamplesBuilder();
        _workbook = builder.Build();
        
        _evaluator = new WorkbookEvaluator(_workbook);
        _evaluator.SetLogLevel(LogLevel.Trace);
    }

    private void OnCellValueChanged(SheetCell cell)
    {
        var cellAddress = _workbook.FirstSheet.CellAddress(cell);
        _evaluator.SetValue(cellAddress.Row, cellAddress.Column, cell.Value);
        _evaluator.EvalWorkbook();
    }

    private void OnClearLogClick()
    {
        _evaluator.ClearLog();
    }

    private void OnCalcClick()
    {
        _evaluator.EvalWorkbook();
    }
}