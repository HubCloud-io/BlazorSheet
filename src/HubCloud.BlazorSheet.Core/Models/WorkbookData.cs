using HubCloud.BlazorSheet.Core.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public UniversalValue Sum(params string[] addresses)
        {
            var sum = 0m;
            foreach (var address in addresses)
            {
                var result = Sum(address);
                sum += (decimal)result.Value;
            }
            
            var total = new UniversalValue(sum);
            return total;
        }

        public UniversalValue Row()
        {
            return new UniversalValue(CurrentRow);
        }

        public UniversalValue Column()
        {
            return new UniversalValue(CurrentColumn);
        }

        public UniversalValue IsEmpty(UniversalValue universalValue)
        {

            var result = ExpressoFunctions.FunctionLibrary.IsEmptyFunction.Eval(universalValue.Value);
            return new UniversalValue(result);
        }

        public UniversalValue IsNotEmpty(UniversalValue universalValue)
        {
            var result = ExpressoFunctions.FunctionLibrary.IsNotEmptyFunction.Eval(universalValue.Value);
            return new UniversalValue(result);
        }

        public UniversalValue Now()
        {
            return new UniversalValue(DateTime.Now);
        }
        
        public UniversalValue DateDiff(string datePartName, UniversalValue universalValueStart, UniversalValue universalValueEnd)
        {
            var dateStart = universalValueStart.ToDate();
            var dateFinish = universalValueEnd.ToDate();
            
            var result = ExpressoFunctions.FunctionLibrary.DateDiffFunction.Eval(datePartName, dateStart, dateFinish);
            return new UniversalValue(result);
        }

        public UniversalValue Iif(bool logicResult, object valueTrue, object valueFalse)
        {
            var result = ExpressoFunctions.FunctionLibrary.IifFunction.Eval(logicResult, valueTrue, valueFalse);
            return new UniversalValue(result);
        }

        public UniversalValue Ifs(params object[] args)
        {
            var result = ExpressoFunctions.FunctionLibrary.IfsFunction.Eval(args);
            return new UniversalValue(result);
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