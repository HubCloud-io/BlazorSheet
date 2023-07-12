namespace HubCloud.BlazorSheet.EvalEngine.Engine.FormulaProcessors.Models
{
    public enum ElementType
    {
        NumericOrOther,
        Operator,
        Function,
        Address,
        ValFunction,
        ExceptionFunction,
        AddressRange,
        Property
    }
}