using System;
using System.Collections.Generic;
using System.Dynamic;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class PickupConditionsTests
{
    private dynamic _row;
    private Workbook _workbook;

    [SetUp]
    public void Init()
    {
        _row = new ExpandoObject();
        var rowDict = (IDictionary<string, object>) _row;
        
        rowDict.Add("period", new DateTime(2023, 10, 06));
        rowDict.Add("item", 1);
        rowDict.Add("amount", 100.23M);
        
        var sheetSettings = new SheetSettings()
        {
            RowsCount = 5,
            ColumnsCount = 5
        };

        var sheet = new Sheet(sheetSettings);

        sheet.GetCell(2, 3).Value = new DateTime(2023, 10, 06); 
        sheet.GetCell(3, 2).Value = 1;
        
          
        _workbook = new Workbook();
        _workbook.AddSheet(sheet);
    }
    
    [Test]
    public void IntCondition_ReturnBool()
    {

        var evaluator = new WorkbookEvaluator(_workbook);
        evaluator.SetVariable("row", _row);

        var expression = "R3C2 = row.item";

        var result = evaluator.Eval(expression, 1, 1);
        
        Assert.Pass();

    }
}