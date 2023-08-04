using System.Collections.Generic;
using HubCloud.BlazorSheet.EvalEngine.Engine.CellShiftFormulaHelper.Models;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.CellShiftFormulaHelper.Abstractions
{
    public interface IFormulaShifter
    {
        List<ShiftLogModel> OnRowAdd(int insertedRowNumber);
        List<ShiftLogModel> OnColumnAdd(int insertedColumnNumber);
        List<ShiftLogModel> OnRowDelete(int deletedRowNumber);
        List<ShiftLogModel> OnColumnDelete(int deletedColumnNumber);
    }
}