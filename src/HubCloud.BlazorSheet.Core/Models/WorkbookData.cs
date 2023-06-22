using HubCloud.BlazorSheet.Core.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class WorkbookData
    {
        private List<SheetData> _sheets = new List<SheetData>();
        
        public SheetData FirstSheet => _sheets.FirstOrDefault();
        
        public int CurrentRow { get; set; }
        public int CurrentColumn { get; set; }

        public WorkbookData(Workbook workbook)
        {
            foreach (var sheet in workbook.Sheets)
            {
                var sheetData = new SheetData(sheet);
                _sheets.Add(sheetData);
            }
        }

       

        public UniversalValue GetValue(string sheetName, int row, int column)
        {
            var val = new UniversalValue(null);

            var sheet = GetSheetByName(sheetName);

            if (sheet == null)
            {
                return val;
            }

            sheet.CurrentRow = CurrentRow;
            sheet.CurrentColumn = CurrentColumn;
            val = sheet.GetValue(row, column);
            
            return val;
        }
        
        public UniversalValue GetValue(string address)
        {
            var val = new UniversalValue(null);

            var sheet = GetSheet(address);

            if (sheet == null)
            {
                return val;
            }

            sheet.CurrentRow = CurrentRow;
            sheet.CurrentColumn = CurrentColumn;
            val = sheet.GetValue(address);
            
            return val;
        }
        
        public UniversalValue Sum(string address)
        {
            var total = new UniversalValue(0M);

            var sheet = GetSheet(address);

            if (sheet == null)
            {
                return total;
            }

            sheet.CurrentRow = CurrentRow;
            sheet.CurrentColumn = CurrentColumn;
            total = sheet.Sum(address);

            return total;
        }

        public UniversalValue IsEmpty(UniversalValue universalValue)
        {
            if (universalValue.Value == null)
                return new UniversalValue(true);

            if (universalValue.Value is int intValue)
                return new UniversalValue(intValue == 0);

            if (universalValue.Value is decimal decimalValue)
                return new UniversalValue(decimalValue == 0M);

            if (universalValue.Value is bool boolValue)
                return new UniversalValue(boolValue == false);

            if (universalValue.Value is DateTime dateTimeValue)
                return new UniversalValue(dateTimeValue == DateTime.MinValue);

            if (universalValue.Value is long longValue)
                return new UniversalValue(longValue == 0);

            if (universalValue.Value is byte byteValue)
                return new UniversalValue(byteValue == 0);

            if (string.IsNullOrEmpty(universalValue.Value?.ToString()))
                return new UniversalValue(true);

            if (string.IsNullOrWhiteSpace(universalValue.Value?.ToString()))
                return new UniversalValue(true);

            return new UniversalValue(false);
        }

        public UniversalValue IsNotEmpty(UniversalValue universalValue)
        {
            if (IsEmpty(universalValue).Value is bool boolValue)
                return new UniversalValue(!boolValue);
            else
                return new UniversalValue(false);
        }

        public UniversalValue DateDiff(string datePartName, UniversalValue universalValueStart, UniversalValue universalValueEnd)
        {
            var dateStart = GetDateTime(universalValueStart);
            var dateFinish = GetDateTime(universalValueEnd);

            if (dateStart != DateTime.MinValue && dateFinish != DateTime.MinValue)
            {
                TimeSpan span = dateFinish - dateStart;
                Enum.TryParse<DateParts>(datePartName, true, out var datePart);

                switch (datePart)
                {
                    case DateParts.Year:
                        return new UniversalValue(dateFinish.Year - dateStart.Year);
                    case DateParts.Month:
                        return new UniversalValue((dateFinish.Year - dateStart.Year) * 12 + dateFinish.Month - dateStart.Month);
                    case DateParts.Day:
                        return new UniversalValue((decimal)span.TotalDays);
                    case DateParts.Hour:
                        return new UniversalValue((decimal)span.TotalHours);
                    case DateParts.Minute:
                        return new UniversalValue((decimal)span.TotalMinutes);
                    case DateParts.Second:
                        return new UniversalValue((decimal)span.TotalSeconds);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(datePart), datePart, null);
                }
            }

            return new UniversalValue();
        }

        public UniversalValue Iif(bool logicResult, object valueTrue, object valueFalse)
        {
            return new UniversalValue(logicResult ? valueTrue : valueFalse);
        }

        public UniversalValue Ifs(params object[] args)
        {
            for (var i = 0; i < args.Length; i += 2)
            {
                var expressionResult = args[i];

                if (expressionResult is bool boolResult)
                {
                    if (boolResult == true)
                    {
                        if (i + 1 < args.Length)
                        {
                            var currentValue = args[i + 1];

                            // It is neccessary to cast number to decimal to avoid errors in next operations.
                            if (currentValue is double doubleVal)
                            {
                                return new UniversalValue((decimal)doubleVal);
                            }
                            else if (currentValue is float floatVal)
                            {
                                return new UniversalValue((decimal)floatVal);
                            }
                            else
                            {
                                return new UniversalValue(currentValue);
                            }

                        }
                        else
                        {
                            return new UniversalValue();
                        }
                    }
                }
            }

            return new UniversalValue();
        }

        private SheetData GetSheet(string address)
        {
            var range = new SheetRange(address, CurrentRow, CurrentColumn);

            SheetData sheet;
            if (string.IsNullOrWhiteSpace(range.SheetName))
            {
                sheet = FirstSheet;
            }
            else
            {
                sheet = _sheets.FirstOrDefault(x => x.Name.Equals(range.SheetName));
            }

            return sheet;
        }

        private DateTime GetDateTime(UniversalValue universalValue)
        {
            if (universalValue.Value is DateTime dateTime)
            {
                return dateTime;
            }

            if (universalValue.Value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, out dateTime))
                    return dateTime;
            }

            return DateTime.MinValue;
        }
        
        public SheetData GetSheetByName(string sheetName)
        {
         
            SheetData sheet;
            if (string.IsNullOrWhiteSpace(sheetName))
            {
                sheet = FirstSheet;
            }
            else
            {
                sheet = _sheets.FirstOrDefault(x => x.Name.Equals(sheetName));
            }

            return sheet;
        }
        
    }
}