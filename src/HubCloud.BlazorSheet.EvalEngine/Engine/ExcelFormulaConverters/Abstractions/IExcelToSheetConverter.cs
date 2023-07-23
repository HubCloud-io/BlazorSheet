using HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverter.Models;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverters.Abstractions
{
    public interface IExcelToSheetConverter
    {
        ConvertResult Convert(string excelFormula);
    }
}
