namespace HubCloud.BlazorSheet.EvalEngine.Engine
{
    public class FormulaConverter
    {
        public static string PrepareFormula(string formulaIn)
        {
            var formula = formulaIn;

           
            formula = formula
                .Replace("=","==")
                .Replace("====","==")
                .Replace("<>","!=")
                .Replace("!==","!=")
                .Replace(">==",">=")
                .Replace("<==","<=")
                .Replace("==>","=>")
                .Replace(" and "," && ")
                .Replace(" AND "," && ")
                .Replace(" or "," || ")
                .Replace(" OR "," || ");
            
            return formula;
        }
    }
}