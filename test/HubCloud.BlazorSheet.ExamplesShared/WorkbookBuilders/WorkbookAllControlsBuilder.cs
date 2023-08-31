using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Models;

namespace HubCloud.BlazorSheet.ExamplesShared.WorkbookBuilders
{
    public class WorkbookAllControlsBuilder
    {
        public Workbook Build()
        {
            var sheetSettings = new SheetSettings();
            sheetSettings.RowsCount = 10;
            sheetSettings.ColumnsCount = 3;
            
            var sheet = new Sheet(sheetSettings);

            var textColumn = 2;
            
            AddSelectControl(sheet, 1, textColumn);
            AddNumberInputControl(sheet, 2, textColumn);
            AddTextInputControl(sheet,3, textColumn);
            AddDateInputControl(sheet, 4, textColumn);
            AddCheckboxControl(sheet, 5, textColumn);

            
            sheet.PrepareCellText();

            sheet.GetColumn(1).WidthValue = 30;
            sheet.GetColumn(2).WidthValue = 150;
            sheet.GetColumn(3).WidthValue = 200;
            
            var workbook = new Workbook();
            workbook.AddSheet(sheet);

            return workbook;
        }

        private void AddSelectControl(Sheet sheet, int row, int column)
        {
            var cell = sheet.GetCell(row, column);
            cell.Value = "Select";
            
            var selectEditSettings = new SheetCellEditSettings()
            {
                ControlKind = CellControlKinds.Select,
                CellDataType = 26
            };

            var editCell = sheet.GetCell(row, column+1);
            sheet.SetEditSettings(editCell, selectEditSettings);
        }

        private void AddNumberInputControl(Sheet sheet, int row, int column)
        {
            var cell = sheet.GetCell(row, column);
            cell.Value = "Number input";
            
            var selectEditSettings = new SheetCellEditSettings()
            {
                ControlKind = CellControlKinds.NumberInput,
                CellDataType = (int)CellDataTypes.Number,
                NumberDigits = 2
            };

            var editCell = sheet.GetCell(row, column+1);
            sheet.SetEditSettings(editCell, selectEditSettings);
        }
        
        private void AddTextInputControl(Sheet sheet, int row, int column)
        {
            var cell = sheet.GetCell(row, column);
            cell.Value = "Text input";
            
            var selectEditSettings = new SheetCellEditSettings()
            {
                ControlKind = CellControlKinds.TextInput,
                CellDataType = (int)CellDataTypes.String,
              
            };

            var editCell = sheet.GetCell(row, column+1);
            sheet.SetEditSettings(editCell, selectEditSettings);
        }
        
        private void AddDateInputControl(Sheet sheet, int row, int column)
        {
            var cell = sheet.GetCell(row, column);
            cell.Value = "Date input";
            
            var selectEditSettings = new SheetCellEditSettings()
            {
                ControlKind = CellControlKinds.DateInput,
                CellDataType = (int)CellDataTypes.Date,
              
            };

            var editCell = sheet.GetCell(row, column+1);
            sheet.SetEditSettings(editCell, selectEditSettings);
        }
        
        private void AddCheckboxControl(Sheet sheet, int row, int column)
        {
            var cell = sheet.GetCell(row, column);
            cell.Value = "Flag";
            
            var selectEditSettings = new SheetCellEditSettings()
            {
                ControlKind = CellControlKinds.CheckBox,
                CellDataType = (int)CellDataTypes.Bool,
             
            };

            var editCell = sheet.GetCell(row, column+1);
            sheet.SetEditSettings(editCell, selectEditSettings);
        }
        
    }
}