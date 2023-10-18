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
        private bool _shouldRender;

        [Parameter] public bool IsHiddenCellsVisible { get; set; }
        [Parameter] public Sheet Sheet { get; set; }
        [Parameter] public SheetRow Row { get; set; }
        [Parameter] public CellStyleBuilder StyleBuilder { get; set; }
        [Parameter] public SheetRegimes Regime { get; set; }
        [Parameter] public EventCallback RowHeaderCellClick { get; set; }
        [Parameter] public EventCallback<SheetCell> CellClicked { get; set; }
        [Parameter] public EventCallback<SheetCell> CellStartEdit { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> RowContextMenu { get; set; }

        private bool ShouldCellBeDisplayed(SheetColumn column, SheetCell cell)
        {
            return ((column.IsHidden || Row.IsHidden) && !IsHiddenCellsVisible) || cell.HiddenByJoin ||
                   Row.IsCollapsed ||
                   column.IsCollapsed;
        }

        protected override void OnParametersSet()
        {
            _shouldRender = false;
            if (Regime == SheetRegimes.InputForm)
            {
                CheckRowShouldRender();
            }
            else
            {
                _shouldRender = true;
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
#if (DEBUG)
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd:hh:mm:ss.fff} - Row {Sheet.RowNumber(Row)} Rendered.");
#endif
        }

        protected override bool ShouldRender() => _shouldRender;

        private void CheckRowShouldRender()
        {
            _shouldRender = Row.ShouldRender;

            if (!_shouldRender)
            {
                foreach (var column in Sheet.Columns)
                {
                    var cell = Sheet.GetCell(Row, column);
                    if (cell == null)
                        continue;

                    if (cell.ShouldRender)
                    {
                        _shouldRender = true;
                        break;
                    }
                }
            }

            if (_shouldRender)
                Row.ShouldRender = false;
        }
    }
}