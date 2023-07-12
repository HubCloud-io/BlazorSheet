using HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverter.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverter.Abstractions
{
    public interface ISheetToExcelConverter
    {
        ConvertResult Convert(string excelFormula);
    }
}