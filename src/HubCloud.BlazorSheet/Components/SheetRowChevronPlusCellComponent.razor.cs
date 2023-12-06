using HubCloud.BlazorSheet.Core.Consts;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.Infrastructure;
using Microsoft.AspNetCore.Components;
using System.Text;

namespace HubCloud.BlazorSheet.Components
{
    public partial class SheetRowChevronPlusCellComponent
    {
        [Parameter] public bool IsHiddenCellsVisible { get; set; }
        [Parameter] public Sheet Sheet { get; set; }
        [Parameter] public SheetRow Row { get; set; }
        [Parameter] public CellStyleBuilder CellStyleBuilder { get; set; }
        [Parameter] public EventCallback AddRowAfter { get; set; }
        [Parameter] public EventCallback GroupOpenCloseClick { get; set; }

        private string GetCellStyle()
        {
            var sb = new StringBuilder();

            sb.Append("left:");
            sb.Append(0);
            sb.Append(";");

            sb.Append("position:");
            sb.Append("sticky");
            sb.Append(";");

            sb.Append("width:");
            sb.Append($"{SheetConsts.ChevronPlusCellWidth}px");
            sb.Append(";");

            sb.Append("max-width:");
            sb.Append($"{SheetConsts.ChevronPlusCellWidth}px");
            sb.Append(";");

            sb.Append("min-width:");
            sb.Append($"{SheetConsts.ChevronPlusCellWidth}px");
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

            sb.Append("background:");
            if (Row.IsHidden && IsHiddenCellsVisible)
                sb.Append(SheetConsts.CellHiddenBackground);
            else
                sb.Append(SheetConsts.WhiteBackground);
            sb.Append(";");

            return sb.ToString();
        }
    }
}
