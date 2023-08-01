using System.Data.Common;
using System.Text;
using BBComponents.Abstract;
using BBComponents.Enums;
using BBComponents.Models;
using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace HubCloud.BlazorSheet.Components;

public partial class SheetComponent : ComponentBase
{
    private const int LeftSideCellWidth = 40;
    private const int TopSideCellHeight = 30;
    private const string CellHiddenBackground = "#cccccc";

    private bool _multipleSelection;
    private bool _isHiddenCellsVisible;

    private string _currentCellText;
    private bool _cellHasChanged;

    private SheetColumn _currentColumn;
    private SheetRow _currentRow;
    private SheetCell _currentCell;
    private CellStyleBuilder _cellStyleBuilder;
    private List<SheetCell> _selectedCells = new List<SheetCell>();
    private List<SheetRow> _selectedRowByNumberList = new List<SheetRow>();
    private HashSet<Guid> _selectedIdentifiers = new HashSet<Guid>();

    private double _clientX;
    private double _clientY;

    private bool _isRowContextMenuOpen;
    private bool _isRowHeightModalOpen;
    private IEnumerable<IMenuItem> _rowMenuItems;

    private bool _isColumnContextMenuOpen;
    private bool _isColumnWidthModalOpen;
    private IEnumerable<IMenuItem> _columnMenuItems;

    private bool _isSheetSizeModalOpen;
    private bool _isCellLinkInputModalOpen;

    private IComboBoxDataProvider<int> _fakeComboBoxDataProvider = new FakeComboBoxDataProvider();

    [Parameter] public Sheet Sheet { get; set; }

    [Parameter] public SheetRegimes Regime { get; set; }
    
    [Parameter] public bool IsDisabled { get; set; }
    [Parameter] public string MaxHeight { get; set; }
    [Parameter] public string MaxWidth { get; set; }

    [Parameter]
    public IComboBoxDataProviderFactory ComboBoxDataProviderFactory { get; set; }

    [Parameter] public EventCallback Changed { get; set; }

    [Parameter] public EventCallback<SheetCell> CellSelected { get; set; }
    [Parameter] public EventCallback<List<SheetCell>> CellsSelected { get; set; }
    [Parameter] public EventCallback<SheetRow> RowSelected { get; set; }
    [Parameter] public EventCallback<List<SheetRow>> RowsByNumberSelected { get; set; }
    [Parameter] public EventCallback<SheetColumn> ColumnSelected { get; set; }
    [Parameter] public EventCallback<SheetCell> CellValueChanged { get; set; }

    [Inject] public IJSRuntime JsRuntime { get; set; }


    protected override async Task OnInitializedAsync()
    {
        _rowMenuItems = ContextMenuBuilder.BuildColumnContextMenu("row");
        _columnMenuItems = ContextMenuBuilder.BuildColumnContextMenu("column");

        _cellStyleBuilder = new CellStyleBuilder
        {
            LeftSideCellWidth = LeftSideCellWidth,
            TopSideCellHeight = TopSideCellHeight
        };

        try
        {
            await JsRuntime.InvokeVoidAsync("addInputEvent");
        }
        catch (Exception e)
        {
            Console.WriteLine($"addInputEvent error. Message: {e.Message}.");
        }
    }

    protected override void OnParametersSet()
    {
    }

    private async Task OnInput(ChangeEventArgs e, SheetCell cell)
    {
        _currentCellText = e.Value?.ToString();
        _cellHasChanged = true;

        await Changed.InvokeAsync(null);
    }

    private async Task OnCellClick(MouseEventArgs e, SheetRow row, SheetColumn column, SheetCell cell)
    {
        _currentCell = cell;

        _clientX = e.ClientX;
        _clientY = e.ClientY;

        if (!_multipleSelection)
        {
            _selectedCells.Clear();
            _selectedIdentifiers.Clear();
            _selectedRowByNumberList.Clear();
        }

        if (!_selectedCells.Contains(_currentCell))
            _selectedCells.Add(_currentCell);
        else
            _selectedCells.Remove(_currentCell);

        if (!_selectedIdentifiers.Contains(_currentCell.Uid))
            _selectedIdentifiers.Add(_currentCell.Uid);
        else
            _selectedIdentifiers.Remove(_currentCell.Uid);

        await CellSelected.InvokeAsync(cell);
        await CellsSelected.InvokeAsync(_selectedCells);
        await RowSelected.InvokeAsync(row);
        await ColumnSelected.InvokeAsync(column);
    }

    private void OnCellFocusOut(FocusEventArgs e, SheetCell cell)
    {
        if (_cellHasChanged)
        {
            cell.Value = _currentCellText;
            _cellHasChanged = false;
            _currentCellText = "";
        }
    }

    private void OnTableKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Control")
        {
            _multipleSelection = true;
        }
    }

    private void OnTableKeyUp(KeyboardEventArgs e)
    {
        if (e.Key == "Control")
        {
            _multipleSelection = false;
        }
    }

    private void OnRowContextMenu(MouseEventArgs e, SheetRow row)
    {
        _clientX = e.ClientX;
        _clientY = e.ClientY;

        _currentRow = row;

        _isColumnContextMenuOpen = false;
        _isRowContextMenuOpen = true;
    }

    private void OnColumnContextMenu(MouseEventArgs e, SheetColumn column)
    {
        _clientX = e.ClientX;
        _clientY = e.ClientY;

        _currentColumn = column;

        _isColumnContextMenuOpen = true;
        _isRowContextMenuOpen = false;
    }

    private async Task OnColumnMenuClosed(IMenuItem menuItem)
    {
        _isColumnContextMenuOpen = false;
        _isRowContextMenuOpen = false;

        switch (menuItem.Name)
        {
            case ContextMenuBuilder.AddBeforeItemName:

                Sheet.AddColumn(_currentColumn, -1);
                await Changed.InvokeAsync(null);

                break;

            case ContextMenuBuilder.AddAfterItemName:

                Sheet.AddColumn(_currentColumn, 1);
                await Changed.InvokeAsync(null);

                break;

            case ContextMenuBuilder.RemoveItemName:

                Sheet.RemoveColumn(_currentColumn);
                await Changed.InvokeAsync(null);
                break;

            case ContextMenuBuilder.WidthItemName:
                _isColumnWidthModalOpen = true;
                break;

            case ContextMenuBuilder.CloseItemName:
                break;

            case ContextMenuBuilder.SheetSizeItemName:
                _isSheetSizeModalOpen = true;
                break;

            case ContextMenuBuilder.ShowHideItemName:
                _currentColumn.IsHidden = !_currentColumn.IsHidden;
                break;

            case ContextMenuBuilder.ShowHiddenHideHiddenItemName:
                _isHiddenCellsVisible = !_isHiddenCellsVisible;
                break;
        }
    }

    private async Task OnRowMenuClosed(IMenuItem menuItem)
    {
        _isColumnContextMenuOpen = false;
        _isRowContextMenuOpen = false;

        switch (menuItem.Name)
        {
            case ContextMenuBuilder.AddBeforeItemName:

                Sheet.AddRow(_currentRow, -1);
                await Changed.InvokeAsync(null);

                break;

            case ContextMenuBuilder.AddAfterItemName:

                Sheet.AddRow(_currentRow, 1);
                await Changed.InvokeAsync(null);

                break;

            case ContextMenuBuilder.RemoveItemName:

                Sheet.RemoveRow(_currentRow);
                await Changed.InvokeAsync(null);

                break;

            case ContextMenuBuilder.HeightItemName:
                _isRowHeightModalOpen = true;
                break;

            case ContextMenuBuilder.CloseItemName:
                break;

            case ContextMenuBuilder.SheetSizeItemName:
                _isSheetSizeModalOpen = true;
                break;

            case ContextMenuBuilder.ShowHideItemName:
                _currentRow.IsHidden = !_currentRow.IsHidden;
                break;

            case ContextMenuBuilder.ShowHiddenHideHiddenItemName:
                _isHiddenCellsVisible = !_isHiddenCellsVisible;
                break;
        }
    }

    private async Task OnColumnWidthValueModalClosed(object args)
    {
        _isColumnWidthModalOpen = false;

        if (_currentColumn == null)
        {
            return;
        }

        if (args is double widthValue)
        {
            _currentColumn.WidthValue = widthValue;
            await Changed.InvokeAsync(null);
        }
    }

    private async Task OnRowHeightValueModalClosed(object args)
    {
        _isRowHeightModalOpen = false;

        if (_currentRow == null)
        {
            return;
        }

        if (args is double heightValue)
        {
            _currentRow.HeightValue = heightValue;
            await Changed.InvokeAsync(null);
        }
    }

    private async Task OnSheetSizeModalClosed(object args)
    {
        _isSheetSizeModalOpen = false;

        if (args is SheetSize sheetSize)
        {
            Sheet.ChangeSize(sheetSize.Columns, sheetSize.Rows);

            await Changed.InvokeAsync(null);
        }
    }

    private async void OnCellLinkInputModalClosed(CellLink cellLink)
    {
        _isCellLinkInputModalOpen = false;

        if (cellLink == null)
            return;

        _currentCell.Link = cellLink.Link;
        _currentCell.Value = cellLink.Text;

        await Changed.InvokeAsync(null);
    }

    private async Task OnCellValueChanged(SheetCell cell)
    {
        await CellValueChanged.InvokeAsync(cell);
    }

    private async Task OnColumnNumberCellClick(SheetColumn column)
    {
        if (Regime == SheetRegimes.InputForm)
        {
            return;
        }
        
        if (!_multipleSelection)
        {
            _selectedCells.Clear();
            _selectedIdentifiers.Clear();
        }

        var cells = Sheet.Cells.Where(x => x.ColumnUid == column.Uid);

        if (!cells.Any())
            return;
        
        foreach (var cell in cells)
        {
            if (!_selectedCells.Contains(cell))
                _selectedCells.Add(cell);

            if (!_selectedIdentifiers.Contains(cell.Uid))
                _selectedIdentifiers.Add(cell.Uid);
        }

        var firstCell = cells.FirstOrDefault();
        if(firstCell != null)
        {
            await CellSelected.InvokeAsync(firstCell);
        }

        await CellsSelected.InvokeAsync(_selectedCells);
        await ColumnSelected.InvokeAsync(column);
    }

    private async Task OnRowNumberCellClick(SheetRow row)
    {
        if (Regime == SheetRegimes.InputForm)
            return;

        if (!_multipleSelection)
        {
            _selectedCells.Clear();
            _selectedIdentifiers.Clear();
            _selectedRowByNumberList.Clear();
        }

        if (!_selectedRowByNumberList.Contains(row))
            _selectedRowByNumberList.Add(row);

        var cells = Sheet.Cells.Where(x => x.RowUid == row.Uid);

        if (!cells.Any())
            return;

        foreach (var cell in cells)
        {
            if (!_selectedCells.Contains(cell))
                _selectedCells.Add(cell);

            if (!_selectedIdentifiers.Contains(cell.Uid))
                _selectedIdentifiers.Add(cell.Uid);
        }

        var firstCell = cells.FirstOrDefault();
        if (firstCell != null)
            await CellSelected.InvokeAsync(firstCell);

        await CellsSelected.InvokeAsync(_selectedCells);
        await RowsByNumberSelected.InvokeAsync(_selectedRowByNumberList);
        await RowSelected.InvokeAsync(row);
    }

    public string CellClass(SheetCell cell)
    {
        var result = "hc-sheet-cell";

        if (_selectedIdentifiers.Contains(cell.Uid))
            return result += " hc-sheet-cell__active";

        return result;
    }

    public string TopLeftEmptyCellStyle()
    {
        var sb = new StringBuilder();

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
        sb.Append($"{TopSideCellHeight}px");
        sb.Append(";");

        sb.Append("max-height:");
        sb.Append($"{TopSideCellHeight}px");
        sb.Append(";");

        sb.Append("min-height:");
        sb.Append($"{TopSideCellHeight}px");
        sb.Append(";");

        sb.Append("top:");
        sb.Append(0);
        sb.Append(";");

        sb.Append("left:");
        sb.Append(0);
        sb.Append(";");

        sb.Append("position:");
        sb.Append("sticky");
        sb.Append(";");

        sb.Append("z-index:");
        sb.Append(20);
        sb.Append(";");

        return sb.ToString();
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

    public string TopSideCellStyle(Sheet sheet, SheetColumn column)
    {
        var sb = new StringBuilder();

        sb.Append("width:");
        sb.Append(column.Width);
        sb.Append(";");

        sb.Append("max-width:");
        sb.Append(column.Width);
        sb.Append(";");

        sb.Append("min-width:");
        sb.Append(column.Width);
        sb.Append(";");

        sb.Append("height:");
        sb.Append($"{TopSideCellHeight}px");
        sb.Append(";");

        sb.Append("max-height:");
        sb.Append($"{TopSideCellHeight}px");
        sb.Append(";");

        sb.Append("min-height:");
        sb.Append($"{TopSideCellHeight}px");
        sb.Append(";");

        sb.Append("position: ");
        sb.Append("sticky");
        sb.Append(";");

        sb.Append("top: ");
        sb.Append(0);
        sb.Append(";");

        _cellStyleBuilder.AddFreezedStyle(sb, sheet, column, _isHiddenCellsVisible);

        if (column.IsHidden && _isHiddenCellsVisible)
        {
            sb.Append("background:");
            sb.Append(CellHiddenBackground);
            sb.Append(";");
        }

        return sb.ToString();
    }

    public string CellStyle(SheetRow row, SheetColumn column, SheetCell cell)
    {
        return _cellStyleBuilder.GetCellStyle(Sheet, row, column, cell, _isHiddenCellsVisible);
    }

    private bool CellHidden(SheetColumn column, SheetRow row, SheetCell cell)
    {
        return ((column.IsHidden || row.IsHidden) && !_isHiddenCellsVisible) || cell.HiddenByJoin;
    }


    private bool CellHidden(SheetColumn column, SheetRow row)
    {
        return (column.IsHidden || row.IsHidden) && !_isHiddenCellsVisible;
    }


    private bool CellHidden(SheetColumn column)
    {
        return column.IsHidden && !_isHiddenCellsVisible;
    }

    private bool CellHidden(SheetRow row)
    {
        return row.IsHidden && !_isHiddenCellsVisible;
    }

    public void OpenCellLinkModal()
    {
        if (_clientX != 0 && _clientY != 0 && _currentCell != null)
            _isCellLinkInputModalOpen = true;
        else
            _isCellLinkInputModalOpen = false;
    }

    public void SplitJoinCells()
    {
        if (_selectedCells.Count > 1)
        {
            if (Sheet.CanCellsBeJoined(_selectedCells))
            {
                var topLeftCell = Sheet.JoinCells(_selectedCells);
                if (topLeftCell != null)
                {
                    _currentCell = topLeftCell;
                    _selectedCells.Clear();
                    _selectedCells.Add(topLeftCell);
                }
            }
        }
        else
        {
            Sheet.SplitCells(_currentCell);
        }
    }

    private void RowGroupOpenCloseClick(SheetRow row)
    {
        row.IsOpen = !row.IsOpen;
        ChangeChildsVisibility(row, !row.IsOpen);
    }

    private void ChangeChildsVisibility(SheetRow parentRow, bool IsVisible)
    {
        var rows = Sheet.Rows.Where(x => x.ParentUid == parentRow.Uid).ToList();

        foreach (var row in rows)
            row.IsHidden = IsVisible;
    }
}