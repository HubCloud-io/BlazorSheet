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
    private const int LeftSideCellWidth = 30;
    private const int TopSideCellWidth = 30;
    private const string CellHiddenBackground = "#cccccc";

    private bool _multipleSelection;
    private bool _showHiddenCells;

    private string _currentCellText;
    private bool _cellHasChanged;

    private SheetColumn _currentColumn;
    private SheetRow _currentRow;
    private SheetCell _currentCell;
    private List<SheetCell> _selectedCells = new List<SheetCell>();
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
    [Parameter] public EventCallback<SheetColumn> ColumnSelected { get; set; }
    [Parameter] public EventCallback<SheetCell> CellValueChanged { get; set; }

    [Inject] public IJSRuntime JsRuntime { get; set; }


    protected override async Task OnInitializedAsync()
    {
        _rowMenuItems = ContextMenuBuilder.BuildColumnContextMenu("row");
        _columnMenuItems = ContextMenuBuilder.BuildColumnContextMenu("column");


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
                _showHiddenCells = !_showHiddenCells;
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
                _showHiddenCells = !_showHiddenCells;
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
        if (!_multipleSelection)
        {
            _selectedCells.Clear();
            _selectedIdentifiers.Clear();
        }

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
        {
            await CellSelected.InvokeAsync(firstCell);
        }

        await CellsSelected.InvokeAsync(_selectedCells);
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
        sb.Append($"{TopSideCellWidth}px");
        sb.Append(";");

        sb.Append("max-height:");
        sb.Append($"{TopSideCellWidth}px");
        sb.Append(";");

        sb.Append("min-height:");
        sb.Append($"{TopSideCellWidth}px");
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

        if (sheet.FreezedRows > 0)
        {
            var rowIndex = sheet.Rows.ToList().IndexOf(row);
            var rowNumber = rowIndex + 1;

            if (rowNumber <= sheet.FreezedRows)
            {
                sb.Append("z-index:");
                sb.Append(10);
                sb.Append(";");

                var topPosition = TopPosition(sheet, rowNumber, rowIndex);

                if (!string.IsNullOrEmpty(topPosition))
                {
                    sb.Append("top: ");
                    sb.Append(topPosition);
                    sb.Append(";");
                }
            }

            if (rowNumber == sheet.FreezedRows || NeedSetBorderBottom(sheet, rowIndex))
            {
                sb.Append("border-bottom: 2px solid navy;");
            }
        }

        if (row.IsHidden && _showHiddenCells)
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
        sb.Append($"{TopSideCellWidth}px");
        sb.Append(";");

        sb.Append("max-height:");
        sb.Append($"{TopSideCellWidth}px");
        sb.Append(";");

        sb.Append("min-height:");
        sb.Append($"{TopSideCellWidth}px");
        sb.Append(";");

        sb.Append("position: ");
        sb.Append("sticky");
        sb.Append(";");

        sb.Append("top: ");
        sb.Append(0);
        sb.Append(";");

        if (sheet.FreezedColumns > 0)
        {
            var columnIndex = sheet.Columns.ToList().IndexOf(column);
            var columnNumber = columnIndex + 1;

            if (columnNumber <= sheet.FreezedColumns)
            {
                sb.Append("z-index: ");
                sb.Append(10);
                sb.Append(";");

                var leftPosition = LeftPosition(sheet, columnNumber, columnIndex);

                if (!string.IsNullOrEmpty(leftPosition))
                {
                    sb.Append("left: ");
                    sb.Append(leftPosition);
                    sb.Append(";");
                }
            }

            if (columnNumber == sheet.FreezedColumns || NeedSetBorderRight(sheet, columnIndex))
            {
                sb.Append("border-right: 2px solid navy;");
            }
        }

        if (column.IsHidden && _showHiddenCells)
        {
            sb.Append("background:");
            sb.Append(CellHiddenBackground);
            sb.Append(";");
        }

        return sb.ToString();
    }

    private bool NeedSetBorderRight(Sheet sheet, int columnIndex)
    {
        if (sheet.Columns.Any(x => x.IsHidden) && !_showHiddenCells)
        {
            var nextColumnIndex = ++columnIndex;
            var nextColumnNumber = nextColumnIndex + 1;

            if (nextColumnIndex < sheet.Columns.Count)
            {
                var nextColumn = sheet.Columns.ToArray()[nextColumnIndex];

                if (nextColumn.IsHidden)
                {
                    if (nextColumnNumber == sheet.FreezedColumns)
                    {
                        return true;
                    }
                    else
                    {
                        return NeedSetBorderRight(sheet, nextColumnIndex);
                    }
                }
            }
        }

        return false;
    }

    private bool NeedSetBorderBottom(Sheet sheet, int rowIndex)
    {
        if (sheet.Rows.Any(x => x.IsHidden) && !_showHiddenCells)
        {
            var nextRowIndex = ++rowIndex;
            var nextRowNumber = nextRowIndex + 1;

            if (nextRowIndex < sheet.Rows.Count)
            {
                var nextRow = sheet.Rows.ToArray()[nextRowIndex];

                if (nextRow.IsHidden)
                {
                    if (nextRowNumber == sheet.FreezedRows)
                    {
                        return true;
                    }
                    else
                    {
                        return NeedSetBorderBottom(sheet, nextRowIndex);
                    }
                }
            }
        }

        return false;
    }

    public string CellStyle(Sheet sheet, SheetRow row, SheetColumn column, SheetCell cell)
    {
        var cellStyle = sheet.GetStyle(cell);

        var sb = new StringBuilder();

        sb.Append("overflow: hidden; white-space: nowrap;");

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
        sb.Append(row.Height);
        sb.Append(";");

        sb.Append("max-height:");
        sb.Append(row.Height);
        sb.Append(";");

        sb.Append("min-height:");
        sb.Append(row.Height);
        sb.Append(";");

        if ((column.IsHidden || row.IsHidden) && _showHiddenCells)
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

        AddFreezedStyle(sb, sheet, row, column);

        return sb.ToString();
    }

    private void AddFreezedStyle(StringBuilder sb, Sheet sheet, SheetRow row, SheetColumn column)
    {
        if (sheet.FreezedColumns == 0 && sheet.FreezedRows == 0)
            return;

        var rowIndex = sheet.Rows.ToList().IndexOf(row);
        var rowNumber = rowIndex + 1;

        var columnIndex = sheet.Columns.ToList().IndexOf(column);
        var columnNumber = columnIndex + 1;

        var htmlPosition = HtmlPosition(sheet, rowNumber, columnNumber);

        if (!string.IsNullOrEmpty(htmlPosition))
        {
            sb.Append("position: ");
            sb.Append(htmlPosition);
            sb.Append(";");

            var leftPosition = LeftPosition(sheet, columnNumber, columnIndex);
            var topPosition = TopPosition(sheet, rowNumber, rowIndex);

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

        if (rowNumber == sheet.FreezedRows || NeedSetBorderBottom(sheet, rowIndex))
        {
            sb.Append("border-bottom: 2px solid navy;");
        }

        if (columnNumber == sheet.FreezedColumns || NeedSetBorderRight(sheet, columnIndex))
        {
            sb.Append("border-right: 2px solid navy;");
        }
    }

    private static string HtmlPosition(Sheet sheet, int rowNumber, int columnNumber)
    {
        return (rowNumber <= sheet.FreezedRows || columnNumber <= sheet.FreezedColumns) ? "sticky" : "";
    }

    private string LeftPosition(Sheet sheet, int columnNumber, int columnIndex)
    {
        double left = 0;

        if (columnNumber > 0)
            left = LeftSideCellWidth;

        for (int i = 0; i < columnIndex; i++)
        {
            var column = sheet.Columns.ToArray()[i];

            if (column.IsHidden && !_showHiddenCells)
                continue;

            left += column.WidthValue;
        }

        return columnNumber <= sheet.FreezedColumns ? $"{(int)left}px" : "";
    }

    private string TopPosition(Sheet sheet, int rowNumber, int rowIndex)
    {
        double top = 0;

        if (rowNumber > 0)
            top = TopSideCellWidth;

        for (int i = 0; i < rowIndex; i++)
        {
            var row = sheet.Rows.ToArray()[i];

            if (row.IsHidden && !_showHiddenCells) 
                continue;

            if (row.HeightValue < 26)
                top += 26;
            else
                top += row.HeightValue;
        }

        return rowNumber <= sheet.FreezedRows ? $"{(int)top}px" : "";
    }

    private bool CellHidden(SheetColumn column, SheetRow row, SheetCell cell)
    {
        return ((column.IsHidden || row.IsHidden) && !_showHiddenCells) || cell.HiddenByJoin;
    }

    private bool CellHidden(SheetColumn column, SheetRow row)
    {
        return (column.IsHidden || row.IsHidden) && !_showHiddenCells;
    }

    private bool CellHidden(SheetColumn column)
    {
        return column.IsHidden && !_showHiddenCells;
    }

    private bool CellHidden(SheetRow row)
    {
        return row.IsHidden && !_showHiddenCells;
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
}