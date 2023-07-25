using System.Collections.Generic;
using System.Linq;
using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Models;

namespace HubCloud.BlazorSheet.ExamplesShared.WorkbookBuilders
{
    public class WorkbookWithGroupsBuilder
    {
        private SheetCellStyle _headerCellStyle;
        private SheetCellStyle _groupCellStyle;
        private SheetCellEditSettings _numberInputSettings;
        public Workbook Build(int groupsCount = 2, int rowsInGroupsCount = 2)
        {
            var sheetSettings = new SheetSettings();
            sheetSettings.RowsCount = groupsCount * rowsInGroupsCount + groupsCount + 3; // 3 -> header, total, padding for total
            sheetSettings.ColumnsCount = 6;
            
            DeclareStyles(sheetSettings);
            DeclareInputStyles(sheetSettings);

            var sheet = new Sheet(sheetSettings);
            sheet.Name = "main";
            
            FillSheet(sheet, groupsCount, rowsInGroupsCount);
            
            var workbook = new Workbook();
            workbook.AddSheet(sheet);

            return workbook;
        }

        private void DeclareInputStyles(SheetSettings sheetSettings)
        {
            _numberInputSettings = new SheetCellEditSettings()
            {
                ControlKind = CellControlKinds.NumberInput,
                NumberDigits = 2
            };
            
            sheetSettings.EditSettings.Add(_numberInputSettings);
        }

        private void DeclareStyles(SheetSettings sheetSettings)
        {
            _headerCellStyle = new SheetCellStyle();
            _headerCellStyle.BackgroundColor = @"#D9D9D9";
            _headerCellStyle.FontWeight = "bold";

            _groupCellStyle = new SheetCellStyle();
            _groupCellStyle.BackgroundColor = @"#D9D9D9";
            
            sheetSettings.Styles.Add(_headerCellStyle);
            sheetSettings.Styles.Add(_groupCellStyle);
        }

        private void FillSheet(Sheet sheet, int groupsCount, int rowsInGroupsCount)
        {
            sheet.GetCell(1, 1).Text = "Groups";
            sheet.GetCell(1, 1).StyleUid = _headerCellStyle.Uid;
            
            sheet.GetCell(1, 2).StyleUid = _headerCellStyle.Uid;
            
            sheet.GetCell(1, 3).Text = "Month 1";
            sheet.GetCell(1, 3).StyleUid = _headerCellStyle.Uid;
            sheet.GetCell(1, 4).Text = "Month 2";
            sheet.GetCell(1, 4).StyleUid = _headerCellStyle.Uid;

            var row = 2;
            var groupTotalRows = new List<int>();
            for (var gr = 0; gr < groupsCount; gr++)
            {
                groupTotalRows.Add(row);
                sheet.GetCell(row, 1).Text = $"Group #{gr + 1}";
                sheet.GetCell(row, 1).StyleUid = _groupCellStyle.Uid;
                
                sheet.GetCell(row, 2).StyleUid = _groupCellStyle.Uid;

                sheet.GetCell(row, 3).Formula = $@"SUM(""R{row + 1}C3:R{row + rowsInGroupsCount}C3"")";
                sheet.GetCell(row, 3).StyleUid = _headerCellStyle.Uid;
                sheet.GetCell(row, 4).Formula = $@"SUM(""R{row + 1}C4:R{row + rowsInGroupsCount}C4"")";
                sheet.GetCell(row, 4).StyleUid = _headerCellStyle.Uid;

                row++;
                for (var i = 0; i < rowsInGroupsCount; i++)
                {
                    sheet.GetCell(row, 1).Text = $"Group item #{i + 1}";
                    sheet.GetCell(row, 3).EditSettingsUid = _numberInputSettings.Uid;
                    sheet.GetCell(row, 4).EditSettingsUid = _numberInputSettings.Uid;

                    row++;
                }
            }

            row++;
            sheet.GetCell(row, 1).Text = "Total:";
            sheet.GetCell(row, 1).StyleUid = _headerCellStyle.Uid;
            
            sheet.GetCell(row, 2).StyleUid = _headerCellStyle.Uid;

            sheet.GetCell(row, 3).Formula = GetTotalFormula(3, groupTotalRows);
            sheet.GetCell(row, 3).StyleUid = _headerCellStyle.Uid;
            sheet.GetCell(row, 4).Formula = GetTotalFormula(4, groupTotalRows);
            sheet.GetCell(row, 4).StyleUid = _headerCellStyle.Uid;
        }

        private string GetTotalFormula(int column, List<int> groupTotalRows)
        {
            var addresses = groupTotalRows
                .Select(row => $"R{row}C{column}")
                .Aggregate((x,y)=> $@"""{x}"",""{y}""");

            var formula = $"SUM({addresses})";
            return formula;
        }
    }
}