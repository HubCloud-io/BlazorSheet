using HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverter.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverters.Abstractions;
using HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverters.Models;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverters
{
    public class SheetFormulaConverter : ISheetToExcelConverter
    {
        public ConvertResult Convert(string excelFormula, CellAddressFormat cellAddressFormat = CellAddressFormat.DefaultExcelFormat)
        {
            var convertResult = new ConvertResult
            {
                Formula = excelFormula
            };
            
            return convertResult;
        }
    }
}