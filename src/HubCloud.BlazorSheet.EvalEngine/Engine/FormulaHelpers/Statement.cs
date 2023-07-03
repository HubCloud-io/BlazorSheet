using System.Collections.Generic;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.FormulaHelpers
{
    public class Statement
    {
        public ElementType Type { get; set; }
        public string OriginStatement { get; set; }
        public string ProcessedStatement { get; set; }
        public string FunctionName { get; set; }
        public List<FunctionParam> FunctionParams { get; set; }
    }
    
    public class FunctionParam
    {
        public string Origin { get; set; }
        public List<Statement> InnerStatements { get; set; }
        public Statement ParentStatement { get; set; }

        public FunctionParam(Statement parentStatement)
        {
            ParentStatement = parentStatement;
            InnerStatements = new List<Statement>();
        }
    }
}