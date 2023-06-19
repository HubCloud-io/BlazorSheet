using System.Security.Cryptography;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine;
using HubCloud.BlazorSheet.ExamplesShared.WorkbookBuilders;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class WorkbookEvaluatorTests
{
    private Workbook _workbookFormulas;
    
    [SetUp]
    public void Setup()
    {
        var builder = new WorkbookFormulaExamplesBuilder();
        _workbookFormulas = builder.Build();
    }

    // Write tests here.
    
    [Test]
    public void Add_ReturnResult()
    {
        var evaluator = new WorkbookEvaluator(_workbookFormulas);
        evaluator.EvalWorkbook();

        var result = _workbookFormulas.FirstSheet.GetCell(2, 5).Value;
        
        Assert.AreEqual(4m,result);
    }
}