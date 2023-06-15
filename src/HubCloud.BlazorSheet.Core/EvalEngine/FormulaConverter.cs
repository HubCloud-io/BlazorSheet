namespace HubCloud.BlazorSheet.Core.EvalEngine
{
    public class FormulaConverter
    {
        public static string PrepareFormula(string formulaIn) 
            => formulaIn.Replace("$c", "_cells");
    }
}