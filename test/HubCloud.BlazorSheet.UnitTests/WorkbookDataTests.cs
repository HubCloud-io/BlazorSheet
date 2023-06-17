using System.Security.Cryptography;
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
    public void GetValue_OneSheet_ReturnValue()
    {
        var workbookData = BuildWorkbookData();

        var result = workbookData.GetValue("R2C3");
        
        Assert.AreEqual(6, result.Value);
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