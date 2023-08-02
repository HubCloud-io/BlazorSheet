using System.Collections.Generic;
using HubCloud.BlazorSheet.EvalEngine.Engine.CellShiftFormulaHelper.Models;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.CellShiftFormulaHelper.Abstractions
{
    public interface IFormulaShifter
    {
        List<ShiftLogModel> OnRowAdd(int insertedRowIndex);
        List<ShiftLogModel> OnColumnAdd(int insertedColumnIndex);
    }
}