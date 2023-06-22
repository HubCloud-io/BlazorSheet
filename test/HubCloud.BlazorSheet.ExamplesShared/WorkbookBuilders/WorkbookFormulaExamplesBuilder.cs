using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Models;
using System;

namespace HubCloud.BlazorSheet.ExamplesShared.WorkbookBuilders
{

    public class WorkbookFormulaExamplesBuilder
    {
        private SheetCellEditSettings _numberInputSettings;
        private SheetCellEditSettings _stringInputSettings;
        private SheetCellEditSettings _dateTimeInputSettings;
        private SheetCellStyle _titleStyle;

        public Workbook Build()
        {
            var sheetSettings = new SheetSettings();
            sheetSettings.RowsCount = 50;
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
            _dateTimeInputSettings = new SheetCellEditSettings
            {
                ControlKind = CellControlKinds.DateTimeInput
            };

            sheetSettings.EditSettings.Add(_numberInputSettings);
            sheetSettings.EditSettings.Add(_stringInputSettings);
            sheetSettings.EditSettings.Add(_dateTimeInputSettings);

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

            sheet.GetCell(10, 2).Value = "DateTime operations";
            sheet.GetCell(10, 2).StyleUid = _titleStyle.Uid;

            WriteAddDaysExample(sheet, 11, 2);
            WriteAddHoursExample(sheet, 12, 2);
            WriteAddMinutesExample(sheet, 13, 2);
            WriteAddMonthsExample(sheet, 14, 2);
            WriteAddSecondsExample(sheet, 15, 2);
            WriteAddYearsExample(sheet, 16, 2);
            WriteAddQuartersExample(sheet, 17, 2);
            WriteSetSecondExample(sheet, 18, 2);
            WriteSetMinuteExample(sheet, 19, 2);
            WriteSetHourExample(sheet, 20, 2);
            WriteSetDayExample(sheet, 21, 2);
            WriteSetQuarterExample(sheet, 22, 2);
            WriteSetMonthExample(sheet, 23, 2);
            WriteSetYearExample(sheet, 24, 2);
            WriteEndYearExample(sheet, 25, 2);
            WriteBeginYearExample(sheet, 26, 2);
            WriteBeginDayExample(sheet, 27, 2);
            WriteEndDayExample(sheet, 28, 2);
            WriteBeginMonthExample(sheet, 29, 2);
            WriteEndMonthExample(sheet, 30, 2);
            WriteBeginQuarterExample(sheet, 31, 2);
            WriteEndQuarterExample(sheet, 32, 2);

            sheet.GetCell(33, 2).Value = "DateTime properties";
            sheet.GetCell(33, 2).StyleUid = _titleStyle.Uid;

            WriteDayExample(sheet, 34, 2);
            WriteHourExample(sheet, 35, 2);
            WriteMonthExample(sheet, 36, 2);
            WriteMinuteExample(sheet, 37, 2);
            WriteSecondExample(sheet, 38, 2);
            WriteYearExample(sheet, 39, 2);

            sheet.GetCell(40, 2).Value = "ExpressoFunctions";
            sheet.GetCell(40, 2).StyleUid = _titleStyle.Uid;

            WriteIsEmptyExample(sheet, 41, 2);
            WriteIsNotEmptyExample(sheet, 42, 2);
            WriteDateDiffExample(sheet, 43, 2);
            WriteIifExample(sheet, 44, 2);
            WriteIfsExample(sheet, 45, 2);

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

        private void WriteAddDaysExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R11C3\").AddDays(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteAddHoursExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R12C3\").AddHours(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteAddMinutesExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R13C3\").AddMinutes(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteAddMonthsExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R14C3\").AddMonths(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteAddSecondsExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R15C3\").AddSeconds(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteAddYearsExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R16C3\").AddYears(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteAddQuartersExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R17C3\").AddQuarters(1)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteSetSecondExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R18C3\").SetSecond(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteSetMinuteExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R19C3\").SetMinute(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteSetHourExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R20C3\").SetHour(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteSetDayExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R21C3\").SetDay(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteSetQuarterExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R22C3\").SetQuarter(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteSetMonthExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R23C3\").SetMonth(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteSetYearExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R24C3\").SetYear(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteEndYearExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R25C3\").EndYear()";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteBeginYearExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R26C3\").BeginYear()";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteBeginDayExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R27C3\").BeginDay()";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteEndDayExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R28C3\").EndDay()";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteBeginMonthExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R29C3\").BeginMonth()";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteEndMonthExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R30C3\").EndMonth()";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteBeginQuarterExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R31C3\").BeginQuarter()";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteEndQuarterExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R32C3\").EndQuarter()";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }


        private void WriteDayExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R34C3\").Day";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteHourExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R35C3\").Hour";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteMonthExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R36C3\").Month";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteMinuteExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R37C3\").Minute";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteSecondExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R38C3\").Second";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteYearExample(Sheet sheet, int row, int column)
        {
            var formula = "VAL(\"R39C3\").Year";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteIsEmptyExample(Sheet sheet, int row, int column)
        {
            var formula = "IsEmpty(VAL(\"R41C3\"))";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _stringInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = "qwerty";

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteIsNotEmptyExample(Sheet sheet, int row, int column)
        {
            var formula = "IsNotEmpty(VAL(\"R42C3\"))";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _stringInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = "qwerty";

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteDateDiffExample(Sheet sheet, int row, int column)
        {
            var formula = "DateDiff(\"day\", VAL(\"R43C3\"), VAL(\"R43C4\"))";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 2).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 2).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteIifExample(Sheet sheet, int row, int column)
        {
            //var formula = "iif( VAL(\"R44C3\") > VAL(\"R43C4\"), True, False)";
            var formula = "iif( 10 > 5, True, False)";
            sheet.GetCell(row, column).Value = formula;

            //sheet.GetCell(row, column + 1).EditSettingsUid = _numberInputSettings.Uid;
            //sheet.GetCell(row, column + 1).Value = 10m;

            //sheet.GetCell(row, column + 2).EditSettingsUid = _numberInputSettings.Uid;
            //sheet.GetCell(row, column + 2).Value = 5m;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteIfsExample(Sheet sheet, int row, int column)
        {
            var formula = "ifs( 5 > 10, True, 10 > 5, True, False)";
            sheet.GetCell(row, column).Value = formula;

            //sheet.GetCell(row, column + 1).EditSettingsUid = _numberInputSettings.Uid;
            //sheet.GetCell(row, column + 1).Value = 10m;

            //sheet.GetCell(row, column + 2).EditSettingsUid = _numberInputSettings.Uid;
            //sheet.GetCell(row, column + 2).Value = 5m;

            sheet.GetCell(row, column + 3).Formula = formula;
        }
    }
}