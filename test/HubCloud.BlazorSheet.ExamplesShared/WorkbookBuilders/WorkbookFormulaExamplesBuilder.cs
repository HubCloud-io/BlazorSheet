using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Models;

namespace HubCloud.BlazorSheet.ExamplesShared.WorkbookBuilders
{

    public class WorkbookFormulaExamplesBuilder
    {
        private SheetCellEditSettings _numberInputSettings;
        private SheetCellEditSettings _stringInputSettings;
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
            _stringInputSettings = new SheetCellEditSettings
            {
                ControlKind = CellControlKinds.TextInput
            };

            sheetSettings.EditSettings.Add(_numberInputSettings);
            sheetSettings.EditSettings.Add(_stringInputSettings);

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

            sheet.GetCell(3, 2).Value = "String operations";
            sheet.GetCell(3, 2).StyleUid = _titleStyle.Uid;

            WriteSubstringExample_1(sheet, 4, 2);
            WriteSubstringExample_2(sheet, 5, 2);
            WriteToUpperExample(sheet, 6, 2);
            WriteToLowerExample(sheet, 7, 2);
            WriteIndexOfExample(sheet, 8, 2);
            WriteReplaceExample(sheet, 9, 2);

            sheet.PrepareCellText();

            sheet.GetColumn(1).WidthValue = 30;
            sheet.GetColumn(2).WidthValue = 300;

            var workbook = new Workbook();
            workbook.AddSheet(sheet);

            return workbook;
        }

        private void WriteAddExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R2C3\")+VAL(\"R2C4\")";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = 1.5m;

            sheet.GetCell(row, column + 2).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 2).Value = 2.5m;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteSubstringExample_1(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R4C3\").Substring(1)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _stringInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = "012345";

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteSubstringExample_2(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R5C3\").Substring(0, 2)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _stringInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = "012345";

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteToUpperExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R6C3\").ToUpper()";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _stringInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = "qwerty";

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteToLowerExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R7C3\").ToLower()";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _stringInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = "QWERTY";

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteIndexOfExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R8C3\").IndexOf(\"w\")";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _stringInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = "qwerty";

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteReplaceExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R9C3\").Replace(\"wert\", \"____\")";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _stringInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = "qwerty";

            sheet.GetCell(row, column + 3).Formula = formula;
        }
    }
}