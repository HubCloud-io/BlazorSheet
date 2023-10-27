using HubCloud.BlazorSheet.Core.Models;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.ServerSideExamples.Pages
{
    public partial class SheetAutoFitColumnsPage : ComponentBase
    {
        private Sheet _sheet;
        private SheetSettings _sheetSettings;

        protected override void OnInitialized()
        {
            _sheetSettings = BuildSheetSettingsWithCellNames(3, 20);
            _sheet = new Sheet(_sheetSettings);

            var cell = _sheet.GetCell(1, 1);
            cell.Value = "1111111111111111111111";

            cell = _sheet.GetCell(2, 1);
            cell.Value = "11111111111111111";

            cell = _sheet.GetCell(1, 2);
            cell.Value = "22222222222222222";

            cell = _sheet.GetCell(3, 2);
            cell.Value = "2222222222222222222222";

            cell = _sheet.GetCell(1, 3);
            cell.Value = "3333333333333333333333";

            _sheet.PrepareCellText();

            var column = _sheet.GetColumn(1);
            column.IsAutoFitColumn = true;

            column = _sheet.GetColumn(2);
            column.IsAutoFitColumn = true;

            column = _sheet.GetColumn(3);
            column.IsAutoFitColumn = true;
        }

        private SheetSettings BuildSheetSettingsWithCellNames(int rowsCount, int columnsCount)
        {
            var sheetSettings = new SheetSettings()
            {
                RowsCount = rowsCount,
                ColumnsCount = columnsCount
            };

            for (var r = 1; r <= rowsCount; r++)
            {
                var newRow = new SheetRow();
                for (var c = 1; c <= columnsCount; c++)
                {
                    SheetColumn column;
                    if (r == 1)
                    {
                        column = new SheetColumn();
                        sheetSettings.Columns.Add(column);
                    }
                    else
                    {
                        column = sheetSettings.Columns[c - 1];
                    }

                    var newCell = new SheetCell()
                    {
                        RowUid = newRow.Uid,
                        ColumnUid = column.Uid,
                        Value = $"R{r}C{c}"
                    };
                    sheetSettings.Cells.Add(newCell);
                }

                sheetSettings.Rows.Add(newRow);
            }

            return sheetSettings;
        }

        private void OnFreezedColumnsOnChanged(bool indicator)
        {
            if (indicator)
                _sheet.FreezedColumns = 2;
            else
                _sheet.FreezedColumns = 0;
        }
    }
}
