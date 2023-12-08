﻿using HubCloud.BlazorSheet.Core.Consts;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text;

namespace HubCloud.BlazorSheet.Components
{
    public partial class SheetColumnHeaderCellComponent
    {
        [Parameter] public bool IsHiddenCellsVisible { get; set; }
        [Parameter] public Sheet Sheet { get; set; }
        [Parameter] public SheetColumn Column { get; set; }
        [Parameter] public CellStyleBuilder CellStyleBuilder { get; set; }
        [Parameter] public EventCallback Click { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> ColumnContextMenu { get; set; }

        private string GetCellStyle()
        {
            var sb = new StringBuilder();

            var columnNumber = Sheet.ColumnNumber(Column);
            if (Column.IsAutoFitColumn && columnNumber > Sheet.FreezedColumns)
                sb.Append("max-width:fit-content;");
            else
            {
                sb.Append("width:");
                sb.Append(Column.Width);
                sb.Append(";");

                sb.Append("max-width:");
                sb.Append(Column.Width);
                sb.Append(";");
            }

            sb.Append("min-width:");
            sb.Append(Column.Width);
            sb.Append(";");

            sb.Append("height:");
            sb.Append($"{SheetConsts.TopSideCellHeight}px");
            sb.Append(";");

            sb.Append("max-height:");
            sb.Append($"{SheetConsts.TopSideCellHeight}px");
            sb.Append(";");

            sb.Append("min-height:");
            sb.Append($"{SheetConsts.TopSideCellHeight}px");
            sb.Append(";");

            sb.Append("position:");
            sb.Append("sticky");
            sb.Append(";");

            sb.Append("top:");
            var isChevronPlusAreaColumns = Sheet.Columns.Any(x => x.IsGroup || x.IsAddRemoveAllowed); ;
            if (isChevronPlusAreaColumns)
                sb.Append($"{SheetConsts.TopSideCellHeight}px");
            else
                sb.Append(0);
            sb.Append(";");

            CellStyleBuilder.AddFreezedStyle(sb, Sheet, Column, IsHiddenCellsVisible, true);

            if (Column.IsHidden && IsHiddenCellsVisible)
            {
                sb.Append("background:");
                sb.Append(SheetConsts.CellHiddenBackground);
                sb.Append(";");
            }

            return sb.ToString();
        }
    }
}
