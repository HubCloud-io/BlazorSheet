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