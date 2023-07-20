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
        _evaluator.EvalWorkbook(cellAddress);
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
        
        sheet.GetCell(2, 1).Text = "Type value:";
        sheet.GetCell(2, 2).EditSettingsUid = numberInputSettings.Uid;
        sheet.GetCell(2, 2).Value = 0;
        sheet.GetCell(2, 2).Text = "0";

        // SetDependentFormulas(sheet);
        SetDependentFormulas_CalcOrder(sheet);
        SetIndependentFormulas(sheet);
        //SetCycleDependency(sheet);

        var workbook = new Workbook();
        workbook.AddSheet(sheet);

        return workbook;
    }

    private void SetSumCases(Sheet sheet)
    {
        sheet.GetCell(2, 1).Value = 10;
        sheet.GetCell(2, 1).Text = "10";
        sheet.GetCell(2, 2).Value = 5;
        sheet.GetCell(2, 2).Text = "5";
        sheet.GetCell(2, 3).Value = 1;
        sheet.GetCell(2, 3).Text = "1";
        sheet.GetCell(2, 4).Value = 8;
        sheet.GetCell(2, 4).Text = "8";
        
        sheet.GetCell(4, 1).Text = @"SUM(""R2C2:R2C3""):";
        sheet.GetCell(4, 2).Formula = @"SUM(""R2C2:R2C3"")";
        
        sheet.GetCell(5, 1).Text = @"SUM(""R2C4""):";
        sheet.GetCell(5, 2).Formula = @"SUM(""R2C4"")";
        
        sheet.GetCell(6, 1).Text = @"SUM(""R2C1"", ""R2C2"", ""R2C3:R2C4""):";
        sheet.GetCell(6, 2).Formula = @"SUM(""R2C1"", ""R2C2"", ""R2C3:R2C4"")";
    }

    private void SetCycleDependency(Sheet sheet)
    {
        sheet.GetCell(3, 1).Text = @"VAL(""R2C2"") + VAL(""R4C2""): ";
        sheet.GetCell(3, 2).Formula = @"VAL(""R2C2"") + VAL(""R4C2"")";
        
        sheet.GetCell(4, 1).Text = @"VAL(""R2C2"") + VAL(""R3C2""): ";
        sheet.GetCell(4, 2).Formula = @"VAL(""R2C2"") + VAL(""R3C2"")";
    }
    
    private void SetDependentFormulas(Sheet sheet)
    {
        sheet.GetCell(1, 2).Text = "100";
        sheet.GetCell(1, 2).Value = 100;
            
        sheet.GetCell(3, 1).Text = @"VAL(""R2C2"") + 10: ";
        sheet.GetCell(3, 2).Formula = @"VAL(""R2C2"") + 10";
        
        sheet.GetCell(4, 1).Text = @"SUM(""R1C2:R2C2"") + VAL(""R1C2""): ";
        sheet.GetCell(4, 2).Formula = @"SUM(""R1C2:R2C2"") + VAL(""R1C2"")";

        sheet.GetCell(2, 3).Formula = @"VAL(""R2C-1"")";
    }
    
    private void SetDependentFormulas_CalcOrder(Sheet sheet)
    {
        sheet.GetCell(3, 1).Text = @"VAL(""R2C2"")+VAL(""R4C2""):";
        sheet.GetCell(3, 2).Formula = @"VAL(""R2C2"")+VAL(""R4C2"")";
        
        sheet.GetCell(4, 1).Text = @"VAL(""R5C2"")+VAL(""R2C2""):";
        sheet.GetCell(4, 2).Formula = @"VAL(""R5C2"")+VAL(""R2C2"")";
        
        sheet.GetCell(5, 1).Text = @"VAL(""R2C2"") + 40:";
        sheet.GetCell(5, 2).Formula = @"VAL(""R2C2"") + 40";
        
        sheet.GetCell(6, 1).Text = @"SUM(""R2C2:R2C2""):";
        sheet.GetCell(6, 2).Formula = @"SUM(""R2C2:R2C2"")";
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