using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine;
using HubCloud.BlazorSheet.ExamplesShared.WorkbookBuilders;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.ServerSideExamples.Pages;

public partial class DependencyCalcPage : ComponentBase
{
    private Workbook _workbook;
    private WorkbookEvaluator _evaluator;

    protected override void OnInitialized()
    {
        _workbook = BuildTestWorkbook();
        
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

    private Workbook BuildTestWorkbook()
    {
        var sheetSettings = new SheetSettings();
        sheetSettings.RowsCount = 100;
        sheetSettings.ColumnsCount = 6;
        
        var numberInputSettings = new SheetCellEditSettings()
        {
            ControlKind = CellControlKinds.NumberInput,
            NumberDigits = 2
        };
        
        sheetSettings.EditSettings.Add(numberInputSettings);
        
        var sheet = new Sheet(sheetSettings);
        sheet.Name = "main";
        
        // Set test value
        sheet.GetCell(2, 1).Text = "Type value:";
        sheet.GetCell(2, 2).EditSettingsUid = numberInputSettings.Uid;
        
        SetDependentFormulas(sheet);
        SetIndependentFormulas(sheet);

        var workbook = new Workbook();
        workbook.AddSheet(sheet);

        return workbook;
    }

    private void SetDependentFormulas(Sheet sheet)
    {
        sheet.GetCell(3, 1).Text = "VAL(\"R2C2\") + 10: ";
        sheet.GetCell(3, 2).Formula = "VAL(\"R2C2\") + 10";
        
        sheet.GetCell(4, 1).Text = "VAL(\"R2C2\") + 40: ";
        sheet.GetCell(4, 2).Formula = "VAL(\"R2C2\") + 40";
    }
    
    private void SetDependentFormulas_CalcOrder(Sheet sheet)
    {
        sheet.GetCell(3, 1).Text = "VAL(\"R2C2\")+VAL(\"R4C2\"): ";
        sheet.GetCell(3, 2).Formula = "VAL(\"R2C2\")+VAL(\"R4C2\")";
        
        sheet.GetCell(4, 1).Text = "VAL(\"R5C2\")+VAL(\"R2C2\"): ";
        sheet.GetCell(4, 2).Formula = "VAL(\"R5C2\")+VAL(\"R2C2\")";
        
        sheet.GetCell(5, 1).Text = "VAL(\"R2C2\") + 40: ";
        sheet.GetCell(5, 2).Formula = "VAL(\"R2C2\") + 40";
    }

    private void SetIndependentFormulas(Sheet sheet)
    {
        sheet.GetCell(2, 4).Text = "Other value:";
        sheet.GetCell(2, 5).Text = "100";
        sheet.GetCell(2, 5).Value = 100m;
        
        sheet.GetCell(3, 4).Text = "VAL(\"R2C5\")+VAL(\"R2C5\"): ";
        sheet.GetCell(3, 5).Formula = "VAL(\"R2C5\")+VAL(\"R2C5\")";
        
        sheet.GetCell(4, 4).Text = "VAL(\"R2C5\") - 50: ";
        sheet.GetCell(4, 5).Formula = "VAL(\"R2C5\")-50";
    }
}