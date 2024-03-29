﻿using HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverter.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverters.Models;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverters.Abstractions
{
    public interface ISheetToExcelConverter
    {
        ConvertResult Convert(string excelFormula,
            CellAddressFormat cellAddressFormat = CellAddressFormat.A1Format,
            int currentRow = 0,
            int currentCol = 0);
    }
}