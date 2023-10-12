using HubCloud.BlazorSheet.Core.Enums;
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
    public partial class SheetRowComponent
    {
        [Parameter] public bool IsHiddenCellsVisible { get; set; }
        [Parameter] public Sheet Sheet { get; set; }
        [Parameter] public SheetRow Row { get; set; }
        [Parameter] public CellStyleBuilder StyleBuilder { get; set; }
        [Parameter] public HashSet<Guid> SelectedIdentifiers { get; set; }
        [Parameter] public SheetRegimes Regime { get; set; }
        [Parameter] public EventCallback RowHeaderCellClick { get; set; }
        [Parameter] public EventCallback<SheetCell> CellClicked { get; set; }
        [Parameter] public EventCallback<SheetCell> CellStartEdit { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> RowContextMenu { get; set; }

        private bool ShouldCellBeDisplayed(SheetColumn column, SheetCell cell)
        {
            return ((column.IsHidden || Row.IsHidden) && !IsHiddenCellsVisible) || cell.HiddenByJoin || Row.IsCollapsed ||
                   column.IsCollapsed;
        }
    }
}
