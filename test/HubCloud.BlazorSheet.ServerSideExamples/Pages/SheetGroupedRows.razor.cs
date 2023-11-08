using HubCloud.BlazorSheet.Core.Models;
using Newtonsoft.Json;

namespace HubCloud.BlazorSheet.ServerSideExamples.Pages
{
    public partial class SheetGroupedRows
    {
        private string _sheetSerialized;
        private Sheet _sheet;

        protected override void OnInitialized()
        {
            _sheet = BuildSheet();
        }

        private Sheet BuildSheet()
        {
            var sheetSettings = new SheetSettings()
            {
                RowsCount = 10,
                ColumnsCount = 10
            };

            for (var r = 1; r <= sheetSettings.RowsCount; r++)
            {
                var newRow = new SheetRow();
                for (var c = 1; c <= sheetSettings.ColumnsCount; c++)
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

            var sheet = new Sheet(sheetSettings);

            var rows = new List<SheetRow>
            {
                sheet.GetRow(2),
                sheet.GetRow(3),
                sheet.GetRow(4),
                sheet.GetRow(5),
                sheet.GetRow(6),
                sheet.GetRow(7)
            };

            if (sheet.CanRowsBeGrouped(rows))
                sheet.GroupRows(rows);

            rows = new List<SheetRow>
            {
                sheet.GetRow(4),
                sheet.GetRow(5),
                sheet.GetRow(6)
            };

            if (sheet.CanRowsBeGrouped(rows))
                sheet.GroupRows(rows);

            return sheet;
        }

        private void OnSerializeSheet()
        {
            _sheetSerialized = JsonConvert.SerializeObject(_sheet);
        }

        private void OnDeserializeSheet()
        {
            var sheet = JsonConvert.DeserializeObject<Sheet>(_sheetSerialized);
            sheet.InitLookUp();
            sheet.PrepareCellText();
            _sheet = sheet;
        }
    }
}
