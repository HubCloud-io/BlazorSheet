namespace HubCloud.BlazorSheet.Core.EvalEngine
{
    public interface IFormulaConverter
    {
        string PrepareFormula(string formula, string contextName);
    }
}