using System.Text;
using BBComponents.Abstract;
using HubCloud.BlazorSheet.Core.Consts;
using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Events;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.Editors;
using HubCloud.BlazorSheet.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace HubCloud.BlazorSheet.Components;

public partial class SheetViewComponent : ComponentBase
{
    private bool _isHiddenCellsVisible;

    private bool _cellHasChanged;
    private bool _tryToFindFirstCell;

    private CellEditInfo _cellEditInfo;

    private SheetColumn _currentColumn;
    private SheetRow _currentRow;
    private SheetCell _currentCell;
    private CellStyleBuilder _cellStyleBuilder;
    private List<SheetCell> _selectedCells = new List<SheetCell>();
    private List<SheetRow> _selectedRowByNumberList = new List<SheetRow>();
    private List<SheetColumn> _selectedColumnByNumberList = new List<SheetColumn>();

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
    
    private JsCallService _jsCallService;

    [Parameter] public Sheet Sheet { get; set; }

    [Parameter] public SheetRegimes Regime { get; set; }

    [Parameter] public bool IsDisabled { get; set; }
    [Parameter] public bool IsVirtualizationEnabled { get; set; } = true;
    [Parameter] public string MaxHeight { get; set; }
    [Parameter] public string MaxWidth { get; set; }


    [Parameter] public EventCallback Changed { get; set; }

    [Parameter] public EventCallback<SheetCell> CellSelected { get; set; }
    [Parameter] public EventCallback<List<SheetCell>> CellsSelected { get; set; }
    [Parameter] public EventCallback<SheetRow> RowSelected { get; set; }
    [Parameter] public EventCallback<List<SheetRow>> RowsByNumberSelected { get; set; }
    [Parameter] public EventCallback<List<SheetColumn>> ColumnsByNumberSelected { get; set; }
    [Parameter] public EventCallback<SheetColumn> ColumnSelected { get; set; }
    [Parameter] public EventCallback<SheetCell> CellValueChanged { get; set; }

    [Parameter] public EventCallback<RowAddedEventArgs> RowAdded { get; set; }
    [Parameter] public EventCallback<RowRemovedEventArgs> RowRemoved { get; set; }
    [Parameter] public EventCallback<ColumnAddedEventArgs> ColumnAdded { get; set; }
    [Parameter] public EventCallback<ColumnRemovedEventArgs> ColumnRemoved { get; set; }

    [Inject] public IJSRuntime JsRuntime { get; set; }

    public string TableId => $"table_{Sheet.Uid}";


    protected override void OnInitialized()
    {
        _cellStyleBuilder = new CellStyleBuilder
        {
            LeftSideCellWidth = SheetConsts.LeftSideCellWidth,
            TopSideCellHeight = SheetConsts.TopSideCellHeight
        };

        _jsCallService = new JsCallService(JsRuntime);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await _jsCallService.DisableArrowScroll("hc-sheet-container");
        }
    }

    private bool NeedToFindFirstEditingCell()
    {
        if (!_tryToFindFirstCell &&
            Regime == SheetRegimes.InputForm
            && _currentCell == null
            && Sheet != null
            && Sheet.EditSettings.Any())
            return true;

        return false;
    }

    protected override async Task OnParametersSetAsync()
    {
        if (NeedToFindFirstEditingCell())
        {
            _tryToFindFirstCell = true;
            var nextCell = Sheet.Cells.FirstOrDefault(x => x.EditSettingsUid.HasValue);
            if (nextCell != null)
            {
                await OnCellClicked(nextCell);
                await _jsCallService.FocusElementAsync(TableId);
            }
        }
    }

    private async Task OnCellClicked(SheetCell cell)
    {
        _currentCell = cell;
        _cellEditInfo = null;

        _selectedCells.Clear();
        _selectedRowByNumberList.Clear();
        _selectedColumnByNumberList.Clear();

        if (!_selectedCells.Contains(_currentCell))
            _selectedCells.Add(_currentCell);
        else
            _selectedCells.Remove(_currentCell);


        await CellSelected.InvokeAsync(cell);
        await CellsSelected.InvokeAsync(_selectedCells);
    }

    private void OnRowContextMenu(MouseEventArgs e, SheetRow row)
    {
        _clientX = e.ClientX;
        _clientY = e.ClientY;

        _currentRow = row;
        _rowMenuItems = ContextMenuBuilder.BuildColumnContextMenu("row", Regime, row.IsAddRemoveAllowed);


        _isColumnContextMenuOpen = false;
        _isRowContextMenuOpen = true;
    }

    private void OnColumnContextMenu(MouseEventArgs e, SheetColumn column)
    {
        _clientX = e.ClientX;
        _clientY = e.ClientY;

        _currentColumn = column;
        _columnMenuItems = ContextMenuBuilder.BuildColumnContextMenu("column", Regime, column.IsAddRemoveAllowed);

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

                var newColumnBefore = Sheet.AddColumn(_currentColumn, -1, true);
                newColumnBefore.ParentUid = _currentColumn.ParentUid;
                newColumnBefore.WidthValue = _currentColumn.WidthValue;

                await ColumnAdded.InvokeAsync(new ColumnAddedEventArgs()
                {
                    SourceUid = _currentColumn.Uid,
                    ColumnUid = newColumnBefore.Uid,
                    ColumnNumber = Sheet.ColumnNumber(newColumnBefore),
                    Position = -1
                });

                await Changed.InvokeAsync(null);

                break;

            case ContextMenuBuilder.AddAfterItemName:

                var newColumnAfter = Sheet.AddColumn(_currentColumn, 1, true);
                newColumnAfter.ParentUid = _currentColumn.ParentUid;
                newColumnAfter.WidthValue = _currentColumn.WidthValue;

                await ColumnAdded.InvokeAsync(new ColumnAddedEventArgs()
                {
                    SourceUid = _currentColumn.Uid,
                    ColumnUid = newColumnAfter.Uid,
                    ColumnNumber = Sheet.ColumnNumber(newColumnAfter),
                    Position = 1
                });

                await Changed.InvokeAsync(null);

                break;

            case ContextMenuBuilder.RemoveItemName:

                var columnRemovedArgs = new ColumnRemovedEventArgs()
                {
                    ColumnUid = _currentColumn.Uid,
                    ColumnNumber = Sheet.ColumnNumber(_currentColumn)
                };

                Sheet.RemoveColumn(_currentColumn);

                await ColumnRemoved.InvokeAsync(columnRemovedArgs);
                await Changed.InvokeAsync(null);
                break;

            case ContextMenuBuilder.AllowAddRemoveItemName:

                _currentColumn.IsAddRemoveAllowed = !_currentColumn.IsAddRemoveAllowed;
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

                var newRowBefore = Sheet.AddRow(_currentRow, -1, true);
                newRowBefore.ParentUid = _currentRow.ParentUid;
                newRowBefore.HeightValue = _currentRow.HeightValue;

                await RowAdded.InvokeAsync(new RowAddedEventArgs()
                {
                    SourceUid = _currentRow.Uid,
                    RowUid = newRowBefore.Uid,
                    RowNumber = Sheet.RowNumber(newRowBefore),
                    Position = -1
                });
                await Changed.InvokeAsync(null);

                break;

            case ContextMenuBuilder.AddAfterItemName:

                var newRowAfter = Sheet.AddRow(_currentRow, 1, true);
                newRowAfter.ParentUid = _currentRow.ParentUid;
                newRowAfter.HeightValue = _currentRow.HeightValue;

                await RowAdded.InvokeAsync(new RowAddedEventArgs()
                {
                    SourceUid = _currentRow.Uid,
                    RowUid = newRowAfter.Uid,
                    RowNumber = Sheet.RowNumber(newRowAfter),
                    Position = 1
                });
                await Changed.InvokeAsync(null);

                break;

            case ContextMenuBuilder.RemoveItemName:

                var removeArgs = new RowRemovedEventArgs()
                {
                    RowUid = _currentRow.Uid,
                    RowNumber = Sheet.RowNumber(_currentRow)
                };
                Sheet.RemoveRow(_currentRow);

                await RowRemoved.InvokeAsync(removeArgs);
                await Changed.InvokeAsync(null);

                break;

            case ContextMenuBuilder.AllowAddRemoveItemName:

                _currentRow.IsAddRemoveAllowed = !_currentRow.IsAddRemoveAllowed;
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

    private async Task OnEditorChanged(SheetCell cell)
    {
        cell.ValidationFailed = false;

        _cellEditInfo = null;
        await OnCellClicked(cell);

        await CellValueChanged.InvokeAsync(cell);

        SheetCell nextCell = null;
      


        if (nextCell != null)
        {
            await OnCellClicked(nextCell);
            await StartCellEditAsync(nextCell);
        }
        else
        {
            await OnCellClicked(cell);
            await _jsCallService.FocusElementAsync(TableId);
        }
    }

    private async Task OnCellEditCancelled(SheetCell cell)
    {
        _cellEditInfo = null;
        await OnCellClicked(cell);
        await _jsCallService.FocusElementAsync(TableId);
    }

    private async Task StartCellEditAsync(SheetCell cell)
    {
        if (Regime == SheetRegimes.Design)
        {
            if (cell.EditSettingsUid.HasValue)
            {
                return;
            }

            var domRect = await _jsCallService.GetElementCoordinates($"cell_{cell.Uid}");

            if (domRect == null)
            {
                return;
            }

            // Edit settings for text input.
            var textInputEditSettings = new SheetCellEditSettings()
            {
                ControlKind = CellControlKinds.TextInput,
                CellDataType = (int) CellDataTypes.String,
            };

            var cellEditInfo = new CellEditInfo()
            {
                DomRect = domRect,
                EditSettings = textInputEditSettings,
                Cell = cell,
            };

            _cellEditInfo = cellEditInfo;
        }
        else if (Regime == SheetRegimes.InputForm)
        {
            var editSettings = Sheet.GetEditSettings(cell);
            if (editSettings == null)
            {
                return;
            }

            if (editSettings.ControlKind == CellControlKinds.Undefined)
            {
                return;
            }

            var domRect = await _jsCallService.GetElementCoordinates($"cell_{cell.Uid}");

            if (domRect == null)
            {
                return;
            }

            var cellEditInfo = new CellEditInfo()
            {
                DomRect = domRect,
                EditSettings = editSettings,
                Cell = cell,
            };

            _cellEditInfo = cellEditInfo;
        }
    }

    private async Task OnColumnNumberCellClick(SheetColumn column)
    {
        if (Regime == SheetRegimes.InputForm)
        {
            return;
        }

        _selectedCells.Clear();
        _selectedColumnByNumberList.Clear();

        if (!_selectedColumnByNumberList.Contains(column))
            _selectedColumnByNumberList.Add(column);

        var cells = Sheet.Cells.Where(x => x.ColumnUid == column.Uid);

        if (!cells.Any())
            return;

        foreach (var cell in cells)
        {
            if (!_selectedCells.Contains(cell))
                _selectedCells.Add(cell);
        }

        var firstCell = cells.FirstOrDefault();
        if (firstCell != null)
        {
            await CellSelected.InvokeAsync(firstCell);
        }

        await CellsSelected.InvokeAsync(_selectedCells);
        await ColumnsByNumberSelected.InvokeAsync(_selectedColumnByNumberList);
        await ColumnSelected.InvokeAsync(column);
    }

    private async Task OnRowNumberCellClick(SheetRow row)
    {
        if (Regime == SheetRegimes.InputForm)
            return;

        _selectedCells.Clear();
        _selectedRowByNumberList.Clear();

        if (!_selectedRowByNumberList.Contains(row))
            _selectedRowByNumberList.Add(row);

        var cells = Sheet.Cells.Where(x => x.RowUid == row.Uid);

        if (!cells.Any())
            return;

        foreach (var cell in cells)
        {
            if (!_selectedCells.Contains(cell))
                _selectedCells.Add(cell);
        }

        var firstCell = cells.FirstOrDefault();
        if (firstCell != null)
            await CellSelected.InvokeAsync(firstCell);

        await CellsSelected.InvokeAsync(_selectedCells);
        await RowsByNumberSelected.InvokeAsync(_selectedRowByNumberList);
        await RowSelected.InvokeAsync(row);
    }

    public string TopLeftEmptyCellStyle()
    {
        var sb = new StringBuilder();

        sb.Append("width:");
        sb.Append($"{SheetConsts.LeftSideCellWidth}px");
        sb.Append(";");

        sb.Append("max-width:");
        sb.Append($"{SheetConsts.LeftSideCellWidth}px");
        sb.Append(";");

        sb.Append("min-width:");
        sb.Append($"{SheetConsts.LeftSideCellWidth}px");
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

    public string CellStyle(SheetRow row, SheetColumn column, SheetCell cell)
    {
        return _cellStyleBuilder.GetCellStyle(Sheet, row, column, cell, _isHiddenCellsVisible);
    }

    private bool ShouldCellBeDisplayed(SheetColumn column, SheetRow row, SheetCell cell)
    {
        return ((column.IsHidden || row.IsHidden) && !_isHiddenCellsVisible) || cell.HiddenByJoin || row.IsCollapsed ||
               column.IsCollapsed;
    }

    private string GetHtmlSpacing(int indent)
    {
        var spacing = string.Empty;

        for (int i = 0; i < indent; i++)
            spacing += "\u00A0";

        return spacing;
    }
}