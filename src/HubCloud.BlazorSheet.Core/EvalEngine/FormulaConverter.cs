namespace HubCloud.BlazorSheet.Core.EvalEngine
{
    public class FormulaConverter
    {
        public static string PrepareFormula(string formulaIn)
        {
            var result = formulaIn;

            result = result.Replace("$c", "_cells");

            return result;
        }
    }
}