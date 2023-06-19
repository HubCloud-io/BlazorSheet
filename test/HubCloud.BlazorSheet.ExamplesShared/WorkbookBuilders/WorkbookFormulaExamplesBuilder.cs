using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Models;

namespace HubCloud.BlazorSheet.ExamplesShared.WorkbookBuilders
{

    public class WorkbookFormulaExamplesBuilder
    {
        private SheetCellEditSettings _numberInputSettings;
        private SheetCellStyle _titleStyle;

        public Workbook Build()
        {
            var sheetSettings = new SheetSettings();
            sheetSettings.RowsCount = 10;
            sheetSettings.ColumnsCount = 6;

            _numberInputSettings = new SheetCellEditSettings()
            {
                ControlKind = CellControlKinds.NumberInput,
                NumberDigits = 2
            };

            sheetSettings.EditSettings.Add(_numberInputSettings);

            var editSettingsDate = new SheetCellEditSettings()
            {
                ControlKind = CellControlKinds.DateInput
            };
            sheetSettings.EditSettings.Add(editSettingsDate);

            _titleStyle = new SheetCellStyle()
            {
                FontWeight = "bold",
                FontSize = "14px",
                TextAlign = "right"
            };
            sheetSettings.Styles.Add(_titleStyle);

            var sheet = new Sheet(sheetSettings);
            sheet.Name = "main";

            sheet.GetCell(1, 2).Value = "Arithmetic operations";
            sheet.GetCell(1, 2).StyleUid = _titleStyle.Uid;

            WriteAddExample(sheet, 2, 2);

            sheet.PrepareCellText();

            sheet.GetColumn(1).WidthValue = 30;
            sheet.GetColumn(2).WidthValue = 300;

            var workbook = new Workbook();
            workbook.AddSheet(sheet);

            return workbook;
        }

        private void WriteAddExample(Sheet sheet, int row, int column)
        {
            sheet.GetCell(row, column).Value = "VAL(\"R2C3\")+VAL(\"R2C4\")";

            sheet.GetCell(row, column + 1).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = 1.5m;

            sheet.GetCell(row, column + 2).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 2).Value = 2.5m;

            sheet.GetCell(row, column + 3).Formula = "VAL(\"R2C3\")+VAL(\"R2C4\")";
        }
    }
}