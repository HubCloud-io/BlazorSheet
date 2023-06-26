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
            sheetSettings.RowsCount = 60;
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

            var startRow = 1;
            var currentRow = startRow;

            sheet.GetCell(currentRow, 2).Value = "Arithmetic operations";
            sheet.GetCell(currentRow, 2).StyleUid = _titleStyle.Uid;
            currentRow++;

            WriteAddExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteAddConstantExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteAddIntConstantExample(sheet, currentRow, 2);
            currentRow++;
            

            sheet.GetCell(currentRow, 2).Value = "String operations";
            sheet.GetCell(currentRow, 2).StyleUid = _titleStyle.Uid;
            currentRow++;

            WriteSubstringExample_1(sheet, currentRow, 2);
            currentRow++;

            WriteSubstringExample_2(sheet, currentRow, 2);
            currentRow++;

            WriteToUpperExample(sheet, currentRow, 2);
            currentRow++;

            WriteToLowerExample(sheet, currentRow, 2);
            currentRow++;

            WriteIndexOfExample(sheet, currentRow, 2);
            currentRow++;

            WriteReplaceExample(sheet, currentRow, 2);
            currentRow++;

            sheet.GetCell(currentRow, 2).Value = "DateTime operations";
            sheet.GetCell(currentRow, 2).StyleUid = _titleStyle.Uid;
            currentRow++;

            WriteAddDaysExample(sheet, currentRow, 2);
            currentRow++;

            WriteAddHoursExample(sheet, currentRow, 2);
            currentRow++;

            WriteAddMinutesExample(sheet, currentRow, 2);
            currentRow++;

            WriteAddMonthsExample(sheet, currentRow, 2);
            currentRow++;

            WriteAddSecondsExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteAddYearsExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteAddQuartersExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteSetSecondExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteSetMinuteExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteSetHourExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteSetDayExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteSetQuarterExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteSetMonthExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteSetYearExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteEndYearExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteBeginYearExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteBeginDayExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteEndDayExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteBeginMonthExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteEndMonthExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteBeginQuarterExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteEndQuarterExample(sheet, currentRow, 2);
            currentRow++;

            sheet.GetCell(currentRow, 2).Value = "DateTime properties";
            sheet.GetCell(currentRow, 2).StyleUid = _titleStyle.Uid;
            currentRow++;

            WriteDayExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteHourExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteMonthExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteMinuteExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteSecondExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteYearExample(sheet, currentRow, 2);
            currentRow++;

            sheet.GetCell(currentRow, 2).Value = "ExpressoFunctions";
            sheet.GetCell(currentRow, 2).StyleUid = _titleStyle.Uid;
            currentRow++;

            WriteIsEmptyExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteIsNotEmptyExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteDateDiffExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteIifExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteIfsExample(sheet, currentRow, 2);
            currentRow++;

            sheet.GetCell(currentRow, 2).Value = "Сomparison operators";
            sheet.GetCell(currentRow, 2).StyleUid = _titleStyle.Uid;
            currentRow++;

            WriteMoreThanExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteLessThanExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteMoreThanOrEqualExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteLessThanOrEqualExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteEqualExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteNotEqualExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteEqualsExample(sheet, currentRow, 2);
            currentRow++;

            sheet.GetCell(currentRow, 2).Value = "Logic operators";
            sheet.GetCell(currentRow, 2).StyleUid = _titleStyle.Uid;
            currentRow++;

            WriteAndExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteOrExample(sheet, currentRow, 2);
            currentRow++;
            
            WriteAndExample2(sheet, currentRow, 2);
            currentRow++;
            
            WriteOrExample2(sheet, currentRow, 2);
            currentRow++;

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
        
        private void WriteAddConstantExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\")+2.5";
            sheet.GetCell(row, column).Value = formula;
            
            sheet.GetCell(row, column + 1).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = 1.5m;
            
            sheet.GetCell(row, column + 3).Formula = formula;
        }
        
        private void WriteAddIntConstantExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\")+2";
            sheet.GetCell(row, column).Value = formula;
            
            sheet.GetCell(row, column + 1).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = 1.5m;
            
            sheet.GetCell(row, column + 3).Formula = formula;
        }
        
        private void WriteMultiplyIntConstantExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\")*2";
            sheet.GetCell(row, column).Value = formula;
            
            sheet.GetCell(row, column + 1).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = 1.5m;
            
            sheet.GetCell(row, column + 3).Formula = formula;
        }
        
        private void WriteMultiplyConstantExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\")*2.5";
            sheet.GetCell(row, column).Value = formula;
            
            sheet.GetCell(row, column + 1).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = 1.5m;
            
            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteSubstringExample_1(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").Substring(1)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _stringInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = "012345";

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteSubstringExample_2(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").Substring(0, 2)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _stringInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = "012345";

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteToUpperExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").ToUpper()";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _stringInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = "qwerty";

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteToLowerExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").ToLower()";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _stringInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = "QWERTY";

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteIndexOfExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").IndexOf(\"w\")";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _stringInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = "qwerty";

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteReplaceExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").Replace(\"wert\", \"____\")";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _stringInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = "qwerty";

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteAddDaysExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").AddDays(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteAddHoursExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").AddHours(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteAddMinutesExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").AddMinutes(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteAddMonthsExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").AddMonths(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteAddSecondsExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").AddSeconds(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteAddYearsExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").AddYears(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteAddQuartersExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").AddQuarters(1)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteSetSecondExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").SetSecond(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteSetMinuteExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").SetMinute(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteSetHourExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").SetHour(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteSetDayExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").SetDay(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteSetQuarterExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").SetQuarter(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteSetMonthExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").SetMonth(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteSetYearExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").SetYear(10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteEndYearExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").EndYear()";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteBeginYearExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").BeginYear()";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteBeginDayExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").BeginDay()";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteEndDayExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").EndDay()";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteBeginMonthExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").BeginMonth()";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteEndMonthExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").EndMonth()";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteBeginQuarterExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").BeginQuarter()";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteEndQuarterExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").EndQuarter()";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }


        private void WriteDayExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").Day";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteHourExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").Hour";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteMonthExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").Month";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteMinuteExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").Minute";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteSecondExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").Second";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteYearExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").Year";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _dateTimeInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = DateTime.Now;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteIsEmptyExample(Sheet sheet, int row, int column)
        {
            var formula = $"IsEmpty(VAL(\"R{row}C3\"))";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _stringInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = "qwerty";

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteIsNotEmptyExample(Sheet sheet, int row, int column)
        {
            var formula = $"IsNotEmpty(VAL(\"R{row}C3\"))";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _stringInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = "qwerty";

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteDateDiffExample(Sheet sheet, int row, int column)
        {
            var formula = $"DateDiff(\"day\", VAL(\"R{row}C3\"), VAL(\"R{row}C4\"))";
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

        private void WriteMoreThanExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\") > VAL(\"R{row}C4\")";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = 10m;

            sheet.GetCell(row, column + 2).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 2).Value = 5m;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteLessThanExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\") < VAL(\"R{row}C4\")";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = 10m;

            sheet.GetCell(row, column + 2).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 2).Value = 5m;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteMoreThanOrEqualExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\") >= VAL(\"R{row}C4\")";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = 10m;

            sheet.GetCell(row, column + 2).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 2).Value = 5m;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteLessThanOrEqualExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\") <= VAL(\"R{row}C4\")";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = 10m;

            sheet.GetCell(row, column + 2).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 2).Value = 5m;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteEqualExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\") == VAL(\"R{row}C4\")";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = 10m;

            sheet.GetCell(row, column + 2).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 2).Value = 5m;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteNotEqualExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\") != VAL(\"R{row}C4\")";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = 10m;

            sheet.GetCell(row, column + 2).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 2).Value = 5m;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteEqualsExample(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\").Equals(VAL(\"R{row}C4\"))";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = 10m;

            sheet.GetCell(row, column + 2).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 2).Value = 5m;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteAndExample(Sheet sheet, int row, int column)
        {
            var formula = $"(VAL(\"R{row}C3\") < 5) && (VAL(\"R{row}C4\") > 10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = 10m;

            sheet.GetCell(row, column + 2).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 2).Value = 5m;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteOrExample(Sheet sheet, int row, int column)
        {
            var formula = $"(VAL(\"R{row}C3\") > 5) || (VAL(\"R{row}C4\") < 10)";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = 10m;

            sheet.GetCell(row, column + 2).EditSettingsUid = _numberInputSettings.Uid;
            sheet.GetCell(row, column + 2).Value = 5m;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteAndExample2(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\") && VAL(\"R{row}C4\")";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _stringInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = true;

            sheet.GetCell(row, column + 2).EditSettingsUid = _stringInputSettings.Uid;
            sheet.GetCell(row, column + 2).Value = true;

            sheet.GetCell(row, column + 3).Formula = formula;
        }

        private void WriteOrExample2(Sheet sheet, int row, int column)
        {
            var formula = $"VAL(\"R{row}C3\") || VAL(\"R{row}C4\")";
            sheet.GetCell(row, column).Value = formula;

            sheet.GetCell(row, column + 1).EditSettingsUid = _stringInputSettings.Uid;
            sheet.GetCell(row, column + 1).Value = true;

            sheet.GetCell(row, column + 2).EditSettingsUid = _stringInputSettings.Uid;
            sheet.GetCell(row, column + 2).Value = true;

            sheet.GetCell(row, column + 3).Formula = formula;
        }
    }
}