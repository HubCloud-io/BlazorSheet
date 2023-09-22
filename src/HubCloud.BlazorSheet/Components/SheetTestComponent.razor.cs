using System.Text;
using BBComponents.Abstract;
using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Events;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.Editors;
using HubCloud.BlazorSheet.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace HubCloud.BlazorSheet.Components;

public partial class SheetTestComponent: ComponentBase
{

    private const int LeftSideCellWidth = 40;
    private const int TopSideCellHeight = 30;
    private const string CellHiddenBackground = "#cccccc";

    private bool _multipleSelection;
    private bool _isHiddenCellsVisible;

    private bool _cellHasChanged;

    private CellEditInfo _cellEditInfo;

    private SheetColumn _currentColumn;
    private SheetRow _currentRow;
    private SheetCell _currentCell;
    private CellStyleBuilder _cellStyleBuilder;
    private List<SheetCell> _selectedCells = new List<SheetCell>();
    private List<SheetRow> _selectedRowByNumberList = new List<SheetRow>();
    private List<SheetColumn> _selectedColumnByNumberList = new List<SheetColumn>();
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
    private JsCallService _jsCallService;
    
    [Parameter] public Sheet Sheet { get; set; }

    [Parameter] public SheetRegimes Regime { get; set; }

    [Parameter] public bool IsDisabled { get; set; }
    [Parameter] public string MaxHeight { get; set; }
    [Parameter] public string MaxWidth { get; set; }

    [Parameter] public IComboBoxDataProviderFactory ComboBoxDataProviderFactory { get; set; }

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

    private void OnScroll()
    {
        _cellEditInfo = null;
    }
    
    private async Task OnCellClicked(SheetCell cell)
    {
        _currentCell = cell;
        _cellEditInfo = null;

        if (!_multipleSelection)
        {
            _selectedCells.Clear();
            _selectedIdentifiers.Clear();
            _selectedRowByNumberList.Clear();
            _selectedColumnByNumberList.Clear();
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
        //await RowSelected.InvokeAsync(row);
        //await ColumnSelected.InvokeAsync(column);
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
    
    private bool ShouldRowBeDisplayed(SheetRow row)
    {
        return (!row.IsHidden || _isHiddenCellsVisible) && !row.IsCollapsed;
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
    
    private bool ShouldColumnBeDisplayed(SheetColumn column)
    {
        return (!column.IsHidden || _isHiddenCellsVisible) && !column.IsCollapsed;
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
            _selectedColumnByNumberList.Clear();
        }

        if (!_selectedColumnByNumberList.Contains(column))
            _selectedColumnByNumberList.Add(column);

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
        if (firstCell != null)
        {
            await CellSelected.InvokeAsync(firstCell);
        }

        await CellsSelected.InvokeAsync(_selectedCells);
        await ColumnsByNumberSelected.InvokeAsync(_selectedColumnByNumberList);
        await ColumnSelected.InvokeAsync(column);
    }
    
    private void ColumnGroupOpenCloseClick(SheetColumn column)
    {
        column.IsOpen = !column.IsOpen;
        Sheet.ChangeChildrenVisibility(column, column.IsOpen);
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
    
    private async Task OnTableKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Control")
        {
            _multipleSelection = true;
        }

        SheetCell nextCell = null;

        switch (e.Key.ToUpper())
        {
            case KeyboardKeys.Tab:
                nextCell = SheetArrowNavigationHelper.ArrowRight(Sheet, _currentCell);
                break;

            case KeyboardKeys.Escape:

                _cellEditInfo = null;
                break;

            case KeyboardKeys.ArrowUp:

                nextCell = SheetArrowNavigationHelper.ArrowUp(Sheet, _currentCell);
                break;

            case KeyboardKeys.ArrowDown:

                nextCell = SheetArrowNavigationHelper.ArrowDown(Sheet, _currentCell);
                break;

            case KeyboardKeys.ArrowLeft:

                nextCell = SheetArrowNavigationHelper.ArrowLeft(Sheet, _currentCell);
                break;

            case KeyboardKeys.ArrowRight:

                nextCell = SheetArrowNavigationHelper.ArrowRight(Sheet, _currentCell);
                break;

            default:

                if (IsLetterOrNumberOrEnter(e.Key))
                {
                    if (_currentCell != null)
                    {
                        await StartCellEditAsync(_currentCell);
                    }
                }

                break;
        }

        if (nextCell != null)
        {
            _currentCell = nextCell;
            await OnCellClicked(_currentCell);
        }
    }
    
    private bool IsLetterOrNumberOrEnter(string key)
    {
        return (key.Length == 1 && char.IsLetterOrDigit(key[0])) ||
               (key.Length > 1 && key.StartsWith("Digit")) ||
               key.Equals("Enter", StringComparison.OrdinalIgnoreCase);
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

            if (ComboBoxDataProviderFactory != null)
            {
                
                var itemsSource = cellEditInfo.EditSettings.ItemsSource;
                if (!string.IsNullOrEmpty(itemsSource))
                {
                    var currentCellAddress = Sheet.CellAddress(cell);
                    var helper = new ItemsSourceParametersHelper(Sheet, itemsSource, currentCellAddress.Row, currentCellAddress.Column);
                    itemsSource = helper.Execute();
                }
                
                cellEditInfo.ComboBoxDataProvider = ComboBoxDataProviderFactory.Create(editSettings.CellDataType, itemsSource);
            }

            _cellEditInfo = cellEditInfo;
        }
    }
     
     private void OnTableKeyUp(KeyboardEventArgs e)
     {
         if (e.Key == "Control")
         {
             _multipleSelection = false;
         }
     }
}