using HubCloud.BlazorSheet.Core.Consts;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.Infrastructure;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubCloud.BlazorSheet.Components
{
    public partial class SheetColumnChevronPlusCellComponent
    {
        [Parameter] public bool IsHiddenCellsVisible { get; set; }
        [Parameter] public Sheet Sheet { get; set; }
        [Parameter] public SheetColumn Column { get; set; }
        [Parameter] public CellStyleBuilder CellStyleBuilder { get; set; }
        [Parameter] public EventCallback GroupOpenCloseClick { get; set; }

        private string TopSideCellStyle()
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
            sb.Append(0);
            sb.Append(";");

            CellStyleBuilder.AddFreezedStyle(sb, Sheet, Column, IsHiddenCellsVisible);

            sb.Append("background:");
            if (Column.IsHidden && IsHiddenCellsVisible)
                sb.Append(SheetConsts.CellHiddenBackground);
            else
                sb.Append(SheetConsts.WhiteBackground);
            sb.Append(";");

            return sb.ToString();
        }
    }
}
