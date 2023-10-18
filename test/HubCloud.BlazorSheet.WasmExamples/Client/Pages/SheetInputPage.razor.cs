using HubCloud.BlazorSheet.Core.Events;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine;
using HubCloud.BlazorSheet.EvalEngine.Engine.CellShiftFormulaHelper;
using HubCloud.BlazorSheet.ExamplesShared.WorkbookBuilders;
using HubCloud.BlazorSheet.Infrastructure;
using HubCloud.BlazorSheet.WasmExamples.Client.DataProviders;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.WasmExamples.Client.Pages;

public partial class SheetInputPage: ComponentBase
{
     private Workbook _workbook;
    private WorkbookEvaluator _evaluator;
    private IComboBoxDataProviderFactory _dataProviderFactory;
    private bool _isDisabled;

    protected override void OnInitialized()
    {
        _dataProviderFactory = new ComboBoxDataProviderFactory();
        
        var builder = new WorkbookSmallBudgetBuilder();
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

    private void OnRowAdded(RowAddedEventArgs args)
    {
        var shiftHelper = new CellShiftFormulaHelper(_workbook.FirstSheet);
        shiftHelper.OnRowAdd(args.RowNumber);
        InitEvaluator();
       // _evaluator.EvalWorkbook();
    }

    private void OnRowRemoved(RowRemovedEventArgs args)
    {
        var shiftHelper = new CellShiftFormulaHelper(_workbook.FirstSheet);
        shiftHelper.OnRowDelete(args.RowNumber);
        InitEvaluator();
        _evaluator.EvalWorkbook();
    }

    private void OnColumnAdded(ColumnAddedEventArgs args)
    {
        var shiftHelper = new CellShiftFormulaHelper(_workbook.FirstSheet);
        shiftHelper.OnColumnAdd(args.ColumnNumber);
        InitEvaluator();
        //_evaluator.EvalWorkbook();
    }

    private void OnColumnRemoved(ColumnRemovedEventArgs args)
    {
        var shiftHelper = new CellShiftFormulaHelper(_workbook.FirstSheet);
        shiftHelper.OnColumnDelete(args.ColumnNumber);
        InitEvaluator();
        _evaluator.EvalWorkbook();
    }

    private void OnClearLogClick()
    {
        _evaluator.ClearLog();
    }

    private void InitEvaluator()
    {
        _evaluator = new WorkbookEvaluator(_workbook);
        _evaluator.SetLogLevel(LogLevel.Trace);
    }
}