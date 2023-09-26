using HubCloud.BlazorSheet.Core.Models;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Globalization;

namespace HubCloud.BlazorSheet.WasmExamples.Client.Pages
{
    public partial class SheetExpandCollapseRowsPage : ComponentBase
    {
        private Sheet _sheet;

        protected override void OnInitialized()
        {
            //var sheetSettings = BuildSheetSettingsWithCellNames(2000, 20);
            //_sheet = new Sheet(sheetSettings);

            //var row = _sheet.Rows.FirstOrDefault();
            //if (row != null)
            //{
            //    row.IsGroup = true;
            //    row.IsOpen = true;

            //    var rows = _sheet.Rows.Skip(1).Take(999);
            //    rows.ToList().ForEach(x => x.ParentUid = row.Uid);

            //    // Collapse row
            //    row.IsOpen = !row.IsOpen;
            //    Console.WriteLine("Collapsing");
            //    Console.WriteLine($"start ChangeChildrenVisibility: {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");
            //    Stopwatch stopwatch = Stopwatch.StartNew();
            //    _sheet.ChangeChildrenVisibility(row, row.IsOpen);
            //    Console.WriteLine($"end ChangeChildrenVisibility: {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");
            //    stopwatch.Stop();
            //    Console.WriteLine(stopwatch.Elapsed);

            //    // Expand row
            //    Console.WriteLine("Expanding");
            //    Console.WriteLine($"start ChangeChildrenVisibility: {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");
            //    stopwatch = Stopwatch.StartNew();
            //    _sheet.ChangeChildrenVisibility(row, row.IsOpen);
            //    Console.WriteLine($"end ChangeChildrenVisibility: {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");
            //    stopwatch.Stop();
            //    Console.WriteLine(stopwatch.Elapsed);
            //}
        }

        private void OnRenderClick()
        {
            var sheetSettings = BuildSheetSettingsWithCellNames(2000, 20);
            _sheet = new Sheet(sheetSettings);

            var row = _sheet.Rows.FirstOrDefault();
            if (row != null)
            {
                row.IsGroup = true;
                row.IsOpen = true;

                var rows = _sheet.Rows.Skip(1).Take(999);
                rows.ToList().ForEach(x => x.ParentUid = row.Uid);
            }
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
    }
}
