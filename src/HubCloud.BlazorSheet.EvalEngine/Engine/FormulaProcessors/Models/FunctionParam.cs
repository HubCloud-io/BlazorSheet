using System.Collections.Generic;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.FormulaProcessors.Models
{
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