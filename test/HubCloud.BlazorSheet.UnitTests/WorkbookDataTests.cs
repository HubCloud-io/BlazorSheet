using System;
using System.Security.Cryptography;
using DynamicExpresso;
using HubCloud.BlazorSheet.Core.Models;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class WorkbookDataTests
{
    [Test]
    public void Sum_OneSheet_ReturnValue()
    {
        var workbookData = BuildWorkbookData();

        var result = workbookData.Sum("R1C1:R1C3");
        
        Assert.AreEqual(6, result.Value);
    }
    
    [Test]
    public void Sum_Params_OneSheet_ReturnValue()
    {
        var workbookData = BuildWorkbookData();

        var result = workbookData.Sum("R1C1","R1C1:R1C3");
        
        Assert.AreEqual(7, result.Value);
    }
    
    [Test]
    public void GetValue_OneSheet_ReturnValue()
    {
        var workbookData = BuildWorkbookData();

        var result = workbookData.GetValue("R2C3");
        
        Assert.AreEqual(6, result.Value);
    }

    [TestCase("day", "2021-10-17", "2021-10-18", 1)]
    [TestCase("Day", "2021-10-17", "2021-10-15", -2)]
    [TestCase("month", "2021-08-17", "2021-10-18", 2)]
    public void DateDiff_OneSheet_ReturnValue(string datePart, string dateStartStr, string dateFinishStr, decimal expected)
    {
        var workbookData = BuildWorkbookData();

        var uvStart = new UniversalValue(dateStartStr);
        var uvEnd = new UniversalValue(dateFinishStr);

        var result = workbookData.DateDiff(datePart, uvStart, uvEnd);

        Assert.AreEqual(expected, result.Value);
    }

    [Test]
    public void Ifs_SimpleIf_ReturnTrueValue()
    {
        var workbookData = BuildWorkbookData();

        var taxId = 1;
        var result = workbookData.Ifs(taxId == 1, 10, true, 0);

        Assert.AreEqual(10, result.Value);
    }

    [Test]
    public void Ifs_SimpleIf_ReturnFalseValue()
    {
        var workbookData = BuildWorkbookData();

        var taxId = 2;
        var result = workbookData.Ifs(taxId == 1, 10, true, 0);

        Assert.AreEqual(0, result.Value);
    }

    [Test]
    public void Ifs_MultipleIf_ReturnSecondValue()
    {
        var workbookData = BuildWorkbookData();

        var taxId = 2;
        var result = workbookData.Ifs(taxId == 1, 10, taxId == 2, 20, true, 0);

        Assert.AreEqual(20, result.Value);
    }

    [Test]
    public void Ifs_MultipleIf_ReturnDefaultValue()
    {
        var workbookData = BuildWorkbookData();

        var taxId = 3;
        var result = workbookData.Ifs(taxId == 1, 10, taxId == 2, 20, true, 0);

        Assert.AreEqual(0, result.Value);
    }

    [Test]
    public void Ifs_WrongParametersNumber_ReturnNull()
    {
        var workbookData = BuildWorkbookData();

        var taxId = 2;
        var result = workbookData.Ifs(taxId == 1, 10, 20);

        Assert.Null(result.Value);
    }

    [Test]
    public void Ifs_WrongFirstConditionType_ReturnDefaultValue()
    {
        var workbookData = BuildWorkbookData();

        var result = workbookData.Ifs(10, 10, true, 0);

        Assert.AreEqual(0, result.Value);
    }

    [Test]
    public void Ifs_WrongConditionType_ReturnDefaultValue()
    {
        var workbookData = BuildWorkbookData();

        var result = workbookData.Ifs(10, 10);

        Assert.Null(result.Value);
    }

    [Test]
    public void Ifs_InterpreterTest()
    {
        var workbookData = BuildWorkbookData();

        var interpreter = new Interpreter(InterpreterOptions.DefaultCaseInsensitive);

        Func<object[], UniversalValue> ifsFunction = workbookData.Ifs;
        interpreter.SetFunction("ifs", ifsFunction);

        interpreter.SetVariable("taxId", 2);

        var formula = "ifs( taxId == 2, 0.2, true, 0)";

        UniversalValue result = (UniversalValue)interpreter.Eval(formula);

        Assert.AreEqual(0.2, result.Value);
    }

    private WorkbookData BuildWorkbookData()
    {
        var sheet = new Sheet(3, 3);
        
        var workbook = new Workbook();
        workbook.AddSheet(sheet);

        var workbookData = new WorkbookData(workbook);

        var sheetData = workbookData.FirstSheet;

        sheetData[1, 1] = 1;
        sheetData[1, 2] = 2;
        sheetData[1, 3] = 3;

        sheetData[2, 1] = 4;
        sheetData[2, 2] = 5;
        sheetData[2, 3] = 6;

        return workbookData;
    }
}