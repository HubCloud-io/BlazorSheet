using System.Text;
using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace HubCloud.BlazorSheet.Components;

public partial class SheetTestComponent: ComponentBase
{

    private const int LeftSideCellWidth = 40;
    private const int TopSideCellHeight = 30;
    private const string CellHiddenBackground = "#cccccc";
    
    private bool _isHiddenCellsVisible;
    private HashSet<Guid> _selectedIdentifiers = new HashSet<Guid>();
    private CellStyleBuilder _cellStyleBuilder;
    
    [Parameter]
    public Sheet Sheet { get; set; }
    [Parameter] public SheetRegimes Regime { get; set; }

    [Parameter] public string MaxHeight { get; set; }
    [Parameter] public string MaxWidth { get; set; }
    
    public string TableId => $"table_{Sheet.Uid}";

    protected override void OnInitialized()
    {
        _cellStyleBuilder = new CellStyleBuilder
        {
            LeftSideCellWidth = LeftSideCellWidth,
            TopSideCellHeight = TopSideCellHeight
        };
    }

    protected override void OnAfterRender(bool firstRender)
    {
        Console.WriteLine($"Sheet test rendered: {DateTime.Now: yyyy-MM-dd:hh:mm:ss.fff}");
    }
    
    private bool ShouldCellBeDisplayed(SheetColumn column, SheetRow row, SheetCell cell)
    {
        return ((column.IsHidden || row.IsHidden) && !_isHiddenCellsVisible) || cell.HiddenByJoin || row.IsCollapsed ||
               column.IsCollapsed;
    }

    private void OnCellClicked()
    {
        
    }

    private void OnCellStartEdit()
    {
        
    }
    
    public string LeftSideCellStyle(Sheet sheet, SheetRow row)
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
        sb.Append(row.Height);
        sb.Append(";");

        sb.Append("max-height:");
        sb.Append(row.Height);
        sb.Append(";");

        sb.Append("min-height:");
        sb.Append(row.Height);
        sb.Append(";");

        _cellStyleBuilder.AddFreezedStyle(sb, sheet, row, _isHiddenCellsVisible);

        if (row.IsHidden && _isHiddenCellsVisible)
        {
            sb.Append("background:");
            sb.Append(CellHiddenBackground);
            sb.Append(";");
        }

        return sb.ToString();
    }
    
    private void RowGroupOpenCloseClick(SheetRow row)
    {
        row.IsOpen = !row.IsOpen;
        Sheet.ChangeChildrenVisibility(row, row.IsOpen);
    }
    
    private async Task OnRowNumberCellClick(SheetRow row)
    {
        if (Regime == SheetRegimes.InputForm)
            return;

        // if (!_multipleSelection)
        // {
        //     _selectedCells.Clear();
        //     _selectedIdentifiers.Clear();
        //     _selectedRowByNumberList.Clear();
        // }
        //
        // if (!_selectedRowByNumberList.Contains(row))
        //     _selectedRowByNumberList.Add(row);
        //
        // var cells = Sheet.Cells.Where(x => x.RowUid == row.Uid);
        //
        // if (!cells.Any())
        //     return;
        //
        // foreach (var cell in cells)
        // {
        //     if (!_selectedCells.Contains(cell))
        //         _selectedCells.Add(cell);
        //
        //     if (!_selectedIdentifiers.Contains(cell.Uid))
        //         _selectedIdentifiers.Add(cell.Uid);
        // }
        //
        // var firstCell = cells.FirstOrDefault();
        // if (firstCell != null)
        //     await CellSelected.InvokeAsync(firstCell);
        //
        // await CellsSelected.InvokeAsync(_selectedCells);
        // await RowsByNumberSelected.InvokeAsync(_selectedRowByNumberList);
        // await RowSelected.InvokeAsync(row);
    }
    
    private void OnRowContextMenu(MouseEventArgs e, SheetRow row)
    {
        // _clientX = e.ClientX;
        // _clientY = e.ClientY;
        //
        // _currentRow = row;
        // _rowMenuItems = ContextMenuBuilder.BuildColumnContextMenu("row", Regime, row.IsAddRemoveAllowed);
        //
        //
        // _isColumnContextMenuOpen = false;
        // _isRowContextMenuOpen = true;
    }
}