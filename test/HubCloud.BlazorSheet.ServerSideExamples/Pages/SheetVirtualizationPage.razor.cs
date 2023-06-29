using HubCloud.BlazorSheet.Core.Models;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.ServerSideExamples.Pages
{
    public partial class SheetVirtualizationPage : ComponentBase
    {
        private Sheet _sheet;
        private SheetSettings _sheetSettings;

        protected override void OnInitialized()
        {

            _sheetSettings = BuildSheetSettingsWithCellNames(125, 25, 4);

            _sheet = new Sheet(_sheetSettings);
        }

        private SheetSettings BuildSheetSettingsWithCellNames(int rowsCount, int columnsCount, int freezedRows)
        {
            var sheetSettings = new SheetSettings()
            {
                RowsCount = rowsCount,
                ColumnsCount = columnsCount,
                FreezedRows = freezedRows
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

        private void CheckboxClicked(ChangeEventArgs e)
        {
            _sheet.UseVirtualization = (bool)e.Value;
        }
    }
}
