using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Models;

namespace HubCloud.BlazorSheet.ExamplesShared.WorkbookBuilders
{

    public class WorkbookSmallBudgetBuilder
    {
        public Workbook Build()
        {

            var sheetSettings = new SheetSettings();
            sheetSettings.RowsCount = 10;
            sheetSettings.ColumnsCount = 6;

            var editSettings = new SheetCellEditSettings()
            {
                ControlKind = CellControlKinds.NumberInput,
                NumberDigits = 2
            };

            sheetSettings.EditSettings.Add(editSettings);

            var editSettingsDate = new SheetCellEditSettings()
            {
                ControlKind = CellControlKinds.DateInput
            };
            sheetSettings.EditSettings.Add(editSettingsDate);

            var editSettingsDepartment = new SheetCellEditSettings()
            {
                ControlKind = CellControlKinds.ComboBox,
                ItemsSource = "department"
            };
            sheetSettings.EditSettings.Add(editSettingsDepartment);

            var totalStyle = new SheetCellStyle()
            {
                FontWeight = "bold",
                FontSize = "14px",
                TextAlign = "right"
            };
            sheetSettings.Styles.Add(totalStyle);

            var sheet = new Sheet(sheetSettings);
            sheet.Name = "budget";

            sheet.GetCell(1, 2).Value = "Department";
            sheet.GetCell(2, 2).EditSettingsUid = editSettingsDepartment.Uid;
            
            
            sheet.GetCell(1, 3).Value = "Start date";
            sheet.GetCell(2, 3).EditSettingsUid = editSettingsDate.Uid;

            sheet.GetCell(4, 2).Value = "Budget item / Period";

            sheet.GetCell(4, 3).Formula = $"Val(\"R2C3\").BeginYear()";
            sheet.GetCell(4, 4).Formula = $"Val(\"R2C3\").BeginYear().AddMonths(1)";
            sheet.GetCell(4, 5).Formula = $"Val(\"R2C3\").BeginYear().AddMonths(2)";

            sheet.GetCell(5, 2).Value = "Rent";
            sheet.GetCell(6, 2).Value = "Tax";
            sheet.GetCell(7, 2).Value = "Salary";


            for (var r = 5; r <= 7; r++)
            {
                for (var c = 3; c <= 5; c++)
                {
                    sheet.GetCell(r, c).EditSettingsUid = editSettings.Uid;
                }
            }

            sheet.GetCell(8, 2).StyleUid = totalStyle.Uid;
            sheet.GetCell(8, 2).Value = "Total";

            SetTotalColumnFormula(8, 3, totalStyle, sheet);
            SetTotalColumnFormula(8, 4, totalStyle, sheet);
            SetTotalColumnFormula(8, 5, totalStyle, sheet);

            SetTotalRowFormula(5, 6, totalStyle, sheet);
            SetTotalRowFormula(6, 6, totalStyle, sheet);
            SetTotalRowFormula(7, 6, totalStyle, sheet);

            sheet.GetCell(4, 6).Value = "Total";
            sheet.GetCell(4, 6).StyleUid = totalStyle.Uid;

            sheet.GetCell(8, 6).Formula = $"Sum(\"R5C6:R-1C6\")";
            sheet.GetCell(8, 6).StyleUid = totalStyle.Uid;

            sheet.GetCell(1, 6).Value = "About Blazor";
            sheet.GetCell(1, 6).Link = "https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor";

            sheet.PrepareCellText();

            var column = sheet.GetColumn(1);
            column.WidthValue = 40;

            column = sheet.GetColumn(2);
            column.WidthValue = 200;

            sheet.GetColumn(3).WidthValue = 120;
            sheet.GetColumn(4).WidthValue = 120;
            sheet.GetColumn(5).WidthValue = 120;

            var workbook = new Workbook();
            workbook.AddSheet(sheet);

            return workbook;
        }

        private void SetTotalColumnFormula(int row, int column, SheetCellStyle totalStyle, Sheet sheet)
        {

            sheet.GetCell(8, column).Formula = $"Val(\"R5C{column}\")+Val(\"R6C{column}\")+Val(\"R7C{column}\")";
            sheet.GetCell(8, column).StyleUid = totalStyle.Uid;

        }

        private void SetTotalRowFormula(int row, int column, SheetCellStyle totalStyle, Sheet sheet)
        {
            sheet.GetCell(row, column).Formula = $"Sum(\"R{row}C3:R{row}C5\")";
            sheet.GetCell(row, column).StyleUid = totalStyle.Uid;
        }
    }
}