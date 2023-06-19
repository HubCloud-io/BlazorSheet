namespace HubCloud.BlazorSheet.EvalEngine.Engine
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