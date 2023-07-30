using System.Collections.Generic;
using System.Text;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.CellShiftFormulaHelper.Models
{
    public class FormulaModel
    {
        public FormulaModel()
        {
            AddressList = new List<ShiftAddressModel>();
        }
        
        public List<ShiftAddressModel> AddressList { get; set; }
        public StringBuilder ReplacedFormula { get; set; }
    }
}