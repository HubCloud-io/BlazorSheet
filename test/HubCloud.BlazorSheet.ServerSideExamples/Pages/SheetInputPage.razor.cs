using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Models;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.ServerSideExamples.Pages;

public partial class SheetInputPage : ComponentBase
{
    private Sheet _sheet;
 

    protected override void OnInitialized()
    {
        _sheet = BuildSheet();
    }

    private Sheet BuildSheet()
    {
        var sheetSettings = new SheetSettings();
        sheetSettings.RowsCount = 10;
        sheetSettings.ColumnsCount = 10;
        
        var editSettings = new SheetCellEditSettings()
        {
            ControlKind = CellControlKinds.NumberInput,
            NumberDigits = 2
        };

        sheetSettings.EditSettings.Add(editSettings);

        var editSettingsDate = new SheetCellEditSettings()
        {
            ControlKind = CellControlKinds.DateInput
        };
        sheetSettings.EditSettings.Add(editSettingsDate);

        var sheet = new Sheet(sheetSettings);

        sheet.GetCell(1, 2).Value = "Department";
        sheet.GetCell(1, 3).Value = "Start date";
        sheet.GetCell(2, 3).EditSettingsUid = editSettingsDate.Uid;

        sheet.GetCell(4, 2).Value = "Budget item / Period";
        
        sheet.GetCell(4, 3).Value = "January";
        sheet.GetCell(4, 4).Value = "February";
        sheet.GetCell(4, 5).Value = "March";
       
        sheet.GetCell(5, 2).Value = "Rent";
        sheet.GetCell(6, 2).Value = "Tax";
        sheet.GetCell(7, 2).Value = "Salary";
    

        
        for (var r = 5; r <= 7; r++)
        {
            for(var c = 3; c<=5; c++)
            {
                sheet.GetCell(r, c).EditSettingsUid = editSettings.Uid;
            }
            
        }
        
        sheet.PrepareCellText();

        var column = sheet.GetColumn(1);
        column.WidthValue = 40;

        column = sheet.GetColumn(2);
        column.WidthValue = 200;

        sheet.GetColumn(3).WidthValue = 150;
        sheet.GetColumn(4).WidthValue = 150;
        sheet.GetColumn(5).WidthValue = 150;

        return sheet;
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