using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Models;

namespace HubCloud.BlazorSheet.ExamplesShared.WorkbookBuilders
{
    public class WorkbookFreezedAreaBuilder
    {
        public Workbook Build()
        {
            var sheetSettings = new SheetSettings();
            sheetSettings.RowsCount = 10;
            sheetSettings.ColumnsCount = 10;
            sheetSettings.FreezedColumns = 4;
            
            var sheet = new Sheet(sheetSettings);
            var column = sheet.GetColumn(1);
            column.WidthValue = 100;

            column = sheet.GetColumn(2);
            column.WidthValue = 200;

            var cell = sheet.GetCell(1, 1);
            cell.Value = "Department";

            var editSettings = new SheetCellEditSettings()
            {
                ControlKind = CellControlKinds.Select,
                CellDataType = 23
            };

            var editStyle = new SheetCellStyle()
            {
                BackgroundColor = "#ebffeb"
            };

            cell = sheet.GetCell(1, 2);
            sheet.SetEditSettings(cell, editSettings);
            sheet.SetStyle(cell, editStyle);

            cell = sheet.GetCell(2, 1);
            cell.Value = "Date start";

            var dateEditSettings = new SheetCellEditSettings()
            {
                ControlKind = CellControlKinds.DateInput,
            };
            cell = sheet.GetCell(2, 2);
            sheet.SetEditSettings(cell, dateEditSettings);
            sheet.SetStyle(cell, editStyle);
            
            sheet.PrepareCellText();
            
            var workbook = new Workbook();
            workbook.AddSheet(sheet);

            return workbook;
        }
    }
}