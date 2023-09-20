using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.Infrastructure;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class ItemsSourceParametersHelperTests
{
    [Test]
    public void Parse_OneParameter_ReturnParameterList()
    {
        var sheet = new Sheet();
        var expression = @"catalog.product | ProductGroupId( &R2C3 ) | Select()";
        var helper = new ItemsSourceParametersHelper(sheet, expression, 1, 1);
        var parameters = helper.Parse(expression);
        
        Assert.AreEqual(1, parameters.Count);
        Assert.AreEqual("R2C3", parameters[0]);
    }

    [Test]
    public void Execute_OneParameter_ReturnPreparedExpression()
    {
        var sheetSettings = new SheetSettings();
        sheetSettings.RowsCount = 3;
        sheetSettings.ColumnsCount = 3;
            
        var sheet = new Sheet(sheetSettings);
        var cell = sheet.GetCell(2, 3);
        cell.Value = 1;
        
        var expression = @"catalog.product | ProductGroupId( &R2C3 ) | Select()";
        var helper = new ItemsSourceParametersHelper(sheet, expression, 1, 1);

        var resultExpression = helper.Execute();

        var check = "catalog.product | ProductGroupId( 1 ) | Select()";
        Assert.AreEqual(check, resultExpression);
    }
    
    [Test]
    public void Execute_RelativeAddress_ReturnPreparedExpression()
    {
        var sheetSettings = new SheetSettings();
        sheetSettings.RowsCount = 3;
        sheetSettings.ColumnsCount = 3;
            
        var sheet = new Sheet(sheetSettings);
        var cell = sheet.GetCell(2, 2);
        cell.Value = 1;
        
        var expression = @"catalog.product | ProductGroupId( &RC2 ) | Select()";
        var helper = new ItemsSourceParametersHelper(sheet, expression, 2, 3);

        var resultExpression = helper.Execute();

        var check = "catalog.product | ProductGroupId( 1 ) | Select()";
        Assert.AreEqual(check, resultExpression);
    }
}