using HubCloud.BlazorSheet.Core.Consts;
using HubCloud.BlazorSheet.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubCloud.BlazorSheet.Infrastructure
{
    public class CellStyleBuilder
    {
        private const int MinCellHeight = 26;
        private const string CellHiddenBackground = "#cccccc";

        public int LeftSideCellWidth { get; set; } = 30;
        public int ChevronPlusCellWidth { get; set; } = 30;
        public int TopSideCellHeight { get; set; } = 30;

        public string GetCellStyle(Sheet sheet, SheetRow row, SheetColumn column, SheetCell cell, bool isHiddenCellsVisible)
        {
            var cellStyle = sheet.GetStyle(cell);

            var sb = new StringBuilder();

            sb.Append("overflow: hidden; white-space: nowrap;");

            var columnNumber = sheet.ColumnNumber(column);
            if (column.IsAutoFitColumn && columnNumber > sheet.FreezedColumns)
                sb.Append("max-width:fit-content;");
            else
            {
                sb.Append("width:");
                sb.Append(column.Width);
                sb.Append(";");

                sb.Append("max-width:");
                sb.Append(column.Width);
                sb.Append(";");
            }
            
            sb.Append("min-width:");
            sb.Append(column.Width);
            sb.Append(";");
            
            sb.Append("height:");
            sb.Append(row.Height);
            sb.Append(";");
            
            sb.Append("max-height:");
            sb.Append(row.Height);
            sb.Append(";");
            
            sb.Append("min-height:");
            sb.Append(row.Height);
            sb.Append(";");

            if ((column.IsHidden || row.IsHidden) && isHiddenCellsVisible)
            {
                sb.Append("background-color:");
                sb.Append(CellHiddenBackground);
                sb.Append(";");
            }
            else
            {
                if (!string.IsNullOrEmpty(cellStyle.BackgroundColor))
                {
                    sb.Append("background-color:");
                    sb.Append(cellStyle.BackgroundColor);
                    sb.Append(";");
                }
            }

            if (!string.IsNullOrEmpty(cellStyle.Color))
            {
                sb.Append("color:");
                sb.Append(cellStyle.Color);
                sb.Append(";");
            }

            if (!string.IsNullOrEmpty(cellStyle.FontSize))
            {
                sb.Append("font-size:");
                sb.Append(cellStyle.FontSize);
                sb.Append(";");
            }

            if (!string.IsNullOrEmpty(cellStyle.TextAlign))
            {
                sb.Append("text-align:");
                sb.Append(cellStyle.TextAlign);
                sb.Append(";");
            }

            if (!string.IsNullOrEmpty(cellStyle.FontWeight))
            {
                sb.Append("font-weight:");
                sb.Append(cellStyle.FontWeight);
                sb.Append(";");
            }

            if (!string.IsNullOrEmpty(cellStyle.FontStyle))
            {
                sb.Append("font-style:");
                sb.Append(cellStyle.FontStyle);
                sb.Append(";");
            }

            if (!string.IsNullOrEmpty(cellStyle.BorderTop))
            {
                sb.Append("border-top:");
                sb.Append(cellStyle.BorderTop);
                sb.Append(";");
            }

            if (!string.IsNullOrEmpty(cellStyle.BorderLeft))
            {
                sb.Append("border-left:");
                sb.Append(cellStyle.BorderLeft);
                sb.Append(";");
            }

            if (!string.IsNullOrEmpty(cellStyle.BorderBottom))
            {
                sb.Append("border-bottom:");
                sb.Append(cellStyle.BorderBottom);
                sb.Append(";");
            }

            if (!string.IsNullOrEmpty(cellStyle.BorderRight))
            {
                sb.Append("border-right:");
                sb.Append(cellStyle.BorderRight);
                sb.Append(";");
            }

            AddFreezedStyle(sb, sheet, row, column, isHiddenCellsVisible, cell.Colspan, cell.Rowspan);

            return sb.ToString();
        }

        public void AddFreezedStyle(StringBuilder sb, Sheet sheet, SheetRow row, bool isHiddenCellsVisible, bool needBorderBottom)
        {
            if (sheet.FreezedRows == 0)
                return;

            var rowNumber = sheet.RowNumber(row);
            if (rowNumber <= sheet.FreezedRows)
            {
                sb.Append("z-index:");
                sb.Append(10);
                sb.Append(";");

                var topPosition = TopPosition(sheet, rowNumber, isHiddenCellsVisible);
                if (!string.IsNullOrEmpty(topPosition))
                {
                    sb.Append("top: ");
                    sb.Append(topPosition);
                    sb.Append(";");
                }
            }

            if (needBorderBottom)
                if (rowNumber == sheet.FreezedRows || NeedSetBorderBottom(sheet, rowNumber, isHiddenCellsVisible))
                    sb.Append("border-bottom: 2px solid navy;");
        }

        public void AddFreezedStyle(StringBuilder sb, Sheet sheet, SheetColumn column, bool isHiddenCellsVisible, bool needBorderRight)
        {
            if (sheet.FreezedColumns == 0)
                return;

            var columnNumber = sheet.ColumnNumber(column);
            if (columnNumber <= sheet.FreezedColumns)
            {
                sb.Append("z-index: ");
                sb.Append(10);
                sb.Append(";");

                var leftPosition = LeftPosition(sheet, columnNumber, isHiddenCellsVisible);
                if (!string.IsNullOrEmpty(leftPosition))
                {
                    sb.Append("left: ");
                    sb.Append(leftPosition);
                    sb.Append(";");
                }
            }

            if (needBorderRight)
                if (columnNumber == sheet.FreezedColumns || NeedSetBorderRight(sheet, columnNumber, isHiddenCellsVisible))
                    sb.Append("border-right: 2px solid navy;");
        }

        private void AddFreezedStyle(StringBuilder sb, Sheet sheet, SheetRow row, SheetColumn column, bool isHiddenCellsVisible, int colspan, int rowspan)
        {
            if (sheet.FreezedColumns == 0 && sheet.FreezedRows == 0)
                return;

            var rowNumber = sheet.RowNumber(row);
            var columnNumber = sheet.ColumnNumber(column);

            var htmlPosition = (rowNumber <= sheet.FreezedRows || columnNumber <= sheet.FreezedColumns) ? "sticky" : "";

            if (!string.IsNullOrEmpty(htmlPosition))
            {
                sb.Append("position: ");
                sb.Append(htmlPosition);
                sb.Append(";");

                var leftPosition = LeftPosition(sheet, columnNumber, isHiddenCellsVisible);
                var topPosition = TopPosition(sheet, rowNumber, isHiddenCellsVisible);

                if (!string.IsNullOrEmpty(topPosition))
                {
                    sb.Append("top: ");
                    sb.Append(topPosition);
                    sb.Append(";");
                }
                if (!string.IsNullOrEmpty(leftPosition))
                {
                    sb.Append("left: ");
                    sb.Append(leftPosition);
                    sb.Append(";");
                }

                if (!string.IsNullOrEmpty(leftPosition) && !string.IsNullOrEmpty(topPosition))
                {
                    sb.Append("z-index: 10;");
                }
                else
                {
                    if (!string.IsNullOrEmpty(leftPosition) || !string.IsNullOrEmpty(topPosition))
                    {
                        sb.Append("z-index: 1;");
                    }
                }
            }

            if (rowNumber == sheet.FreezedRows ||
                NeedSetBorderBottom(sheet, rowNumber, rowspan) ||
                NeedSetBorderBottom(sheet, rowNumber, isHiddenCellsVisible))
            {
                sb.Append("border-bottom: 2px solid navy;");
            }

            if (columnNumber == sheet.FreezedColumns ||
                NeedSetBorderRight(sheet, columnNumber, colspan) ||
                NeedSetBorderRight(sheet, columnNumber, isHiddenCellsVisible))
            {
                sb.Append("border-right: 2px solid navy;");
            }
        }

        private string LeftPosition(Sheet sheet, int columnNumber, bool isHiddenCellsVisible)
        {
            double left = 0;

            if (columnNumber > 0)
            {
                var isChevronPlusAreaRows = sheet.Rows.Any(x => x.IsGroup || x.IsAddRemoveAllowed);
                if (isChevronPlusAreaRows)
                    left = SheetConsts.LeftSideCellWidth + SheetConsts.ChevronPlusCellWidth;
                else
                    left = SheetConsts.LeftSideCellWidth;
            }

            for (int i = 1; i < columnNumber; i++)
            {
                var column = sheet.GetColumn(i);

                if (column.IsHidden && !isHiddenCellsVisible)
                    continue;

                left += column.WidthValue;
            }

            return columnNumber <= sheet.FreezedColumns ? $"{(int)left}px" : "";
        }

        private string TopPosition(Sheet sheet, int rowNumber, bool isHiddenCellsVisible)
        {
            double top = 0;

            if (rowNumber > 0)
            {
                var isChevronPlusAreaColumns = sheet.Columns.Any(x => x.IsGroup || x.IsAddRemoveAllowed);
                if (isChevronPlusAreaColumns)
                    top = SheetConsts.TopSideCellHeight + SheetConsts.ChevronPlusCellHeight;
                else
                    top = SheetConsts.TopSideCellHeight;
            }

            for (int i = 1; i < rowNumber; i++)
            {
                var row = sheet.GetRow(i);
                if (row.IsHidden && !isHiddenCellsVisible)
                    continue;

                if (row.HeightValue < MinCellHeight)
                    top += MinCellHeight;
                else
                    top += row.HeightValue;
            }

            return rowNumber <= sheet.FreezedRows ? $"{(int)top}px" : "";
        }

        private bool NeedSetBorderBottom(Sheet sheet, int rowNumber, bool isHiddenCellsVisible)
        {
            if (sheet.Rows.Any(x => x.IsHidden) && !isHiddenCellsVisible)
            {
                var nextRowNumber = rowNumber + 1;

                if (nextRowNumber <= sheet.Rows.Count)
                {
                    var nextRow = sheet.GetRow(nextRowNumber);

                    if (nextRow.IsHidden)
                    {
                        if (nextRowNumber == sheet.FreezedRows)
                        {
                            return true;
                        }
                        else
                        {
                            return NeedSetBorderBottom(sheet, nextRowNumber, isHiddenCellsVisible);
                        }
                    }
                }
            }

            return false;
        }

        private bool NeedSetBorderBottom(Sheet sheet, int rowNumber, int rowspan)
        {
            if (rowNumber > sheet.FreezedRows)
                return false;

            if (rowspan == sheet.FreezedRows)
                return true;

            return false;
        }

        private bool NeedSetBorderRight(Sheet sheet, int columnNumber, bool isHiddenCellsVisible)
        {
            if (sheet.Columns.Any(x => x.IsHidden) && !isHiddenCellsVisible)
            {
                var nextColumnNumber = columnNumber + 1;

                if (nextColumnNumber <= sheet.Columns.Count)
                {
                    var nextColumn = sheet.GetColumn(nextColumnNumber);

                    if (nextColumn.IsHidden)
                    {
                        if (nextColumnNumber == sheet.FreezedColumns)
                        {
                            return true;
                        }
                        else
                        {
                            return NeedSetBorderRight(sheet, nextColumnNumber, isHiddenCellsVisible);
                        }
                    }
                }
            }

            return false;
        }

        private bool NeedSetBorderRight(Sheet sheet, int columnNumber, int colspan)
        {
            if (columnNumber > sheet.FreezedColumns)
                return false;

            if (colspan == sheet.FreezedColumns)
                return true;

            return false;
        }
    }
}
