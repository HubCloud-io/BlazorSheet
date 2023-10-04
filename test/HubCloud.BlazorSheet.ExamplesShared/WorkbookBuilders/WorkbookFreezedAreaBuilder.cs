using HubCloud.BlazorSheet.Core.Models;

namespace HubCloud.BlazorSheet.ExamplesShared.WorkbookBuilders
{
    public class WorkbookFreezedAreaBuilder
    {
        public Workbook Build()
        {
            var sheetSettings = new SheetSettings();
            sheetSettings.RowsCount = 10;
            sheetSettings.ColumnsCount = 30;
            sheetSettings.FreezedColumns = 4;
            
            var sheet = new Sheet(sheetSettings);
            
            var workbook = new Workbook();
            workbook.AddSheet(sheet);

            return workbook;
        }
    }
}