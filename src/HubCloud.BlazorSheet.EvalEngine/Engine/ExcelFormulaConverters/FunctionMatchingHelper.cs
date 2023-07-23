using System.Collections.Generic;
using System.Linq;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverters
{
    public class FunctionMatchingHelper
    {
        private static Dictionary<string, string> _excelToSheetDict;
        private static Dictionary<string, string> _sheetToExcelDict;
            
        private static readonly List<KeyValuePair<string, string>> ExcelToSheetFunctions =
            new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("SUM", "SUM"),
            new KeyValuePair<string, string>("IF", "IIF")
        };

        public static Dictionary<string, string> ExcelToSheetDict
        {
            get
            {
                if (_excelToSheetDict is null)
                    _excelToSheetDict = ExcelToSheetFunctions.ToDictionary(k => k.Key, v => v.Value);
                
                return _excelToSheetDict;
            }
        }
        
        public static Dictionary<string, string> SheetToExcelDict
        {
            get
            {
                if (_sheetToExcelDict is null)
                    _sheetToExcelDict = ExcelToSheetFunctions.ToDictionary(k => k.Value, v => v.Key);
                
                return _sheetToExcelDict;
            }
        }
    }
}