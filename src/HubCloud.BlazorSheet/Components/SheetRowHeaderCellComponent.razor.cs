using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubCloud.BlazorSheet.Components
{
    public partial class SheetRowHeaderCellComponent
    {
        [Parameter] public int LeftSideCellWidth { get; set; }
        [Parameter] public bool IsHiddenCellsVisible { get; set; }
        [Parameter] public string CellHiddenBackground { get; set; }
        [Parameter] public Sheet Sheet { get; set; }
        [Parameter] public SheetRow Row { get; set; }
        [Parameter] public CellStyleBuilder CellStyleBuilder { get; set; }
        [Parameter] public EventCallback Click { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> RowContextMenu { get; set; }
        

        private string LeftSideCellStyle()
        {
            var sb = new StringBuilder();

            sb.Append("left:");
            sb.Append(0);
            sb.Append(";");

            sb.Append("position:");
            sb.Append("sticky");
            sb.Append(";");

            sb.Append("width:");
            sb.Append($"{LeftSideCellWidth}px");
            sb.Append(";");

            sb.Append("max-width:");
            sb.Append($"{LeftSideCellWidth}px");
            sb.Append(";");

            sb.Append("min-width:");
            sb.Append($"{LeftSideCellWidth}px");
            sb.Append(";");

            sb.Append("height:");
            sb.Append(Row.Height);
            sb.Append(";");

            sb.Append("max-height:");
            sb.Append(Row.Height);
            sb.Append(";");

            sb.Append("min-height:");
            sb.Append(Row.Height);
            sb.Append(";");

            CellStyleBuilder.AddFreezedStyle(sb, Sheet, Row, IsHiddenCellsVisible);

            if (Row.IsHidden && IsHiddenCellsVisible)
            {
                sb.Append("background:");
                sb.Append(CellHiddenBackground);
                sb.Append(";");
            }

            return sb.ToString();
        }

        private void RowGroupOpenCloseClick()
        {
            Row.IsOpen = !Row.IsOpen;
            Sheet.ChangeChildrenVisibility(Row, Row.IsOpen);
        }
    }
}
