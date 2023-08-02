namespace HubCloud.BlazorSheet.EvalEngine.Engine.CellShiftFormulaHelper.Models
{
    public class ShiftLogModel
    {
        public string CellAddress { get; set; }
        public string PrevFormula { get; set; }
        public string CurrentFormula { get; set; }
        public string Error { get; set; }
    }
}