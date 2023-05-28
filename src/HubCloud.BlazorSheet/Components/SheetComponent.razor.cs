using System.Text;
using BBComponents.Abstract;
using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace HubCloud.BlazorSheet.Components;

public partial class SheetComponent: ComponentBase
{
      private bool _multipleSelection;
    
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
    


    [Parameter]
    public Sheet Sheet { get; set; }
    
    [Parameter]
    public SheetRegimes Regime { get; set; }
    
    [Parameter]
    public string MaxHeight { get; set; }
    
    [Parameter]
    public EventCallback Changed { get; set; }
    
    [Parameter]
    public EventCallback<SheetCell> CellSelected { get; set; }
    [Parameter]
    public EventCallback<List<SheetCell>> CellsSelected { get; set; }
    [Parameter]
    public EventCallback<SheetRow> RowSelected { get; set; }
    [Parameter]
    public EventCallback<SheetColumn> ColumnSelected { get; set; }

    [Inject] public IJSRuntime JsRuntime { get; set; }
    

    protected override async Task OnInitializedAsync()
    {
        _rowMenuItems = ContextMenuBuilder.BuildColumnContextMenu("row");
        _columnMenuItems = ContextMenuBuilder.BuildColumnContextMenu("column");

        
        try
        {
            await JsRuntime.InvokeVoidAsync("addInputEvent");
        }
        catch(Exception e)
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

    private async Task OnCellClick(SheetRow row, SheetColumn column, SheetCell cell)
    {
        _currentCell = cell;

        if (!_multipleSelection)
        {
            _selectedCells.Clear();
            _selectedIdentifiers.Clear();
        }
        _selectedCells.Add(_currentCell);
        _selectedIdentifiers.Add(_currentCell.Uid);
        
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
    
    public  string CellClass(SheetCell cell)
    {
        var result = "hc-sheet-cell";

        if (_currentCell == null)
        {
            return result;
        }

        if (_selectedIdentifiers.Contains(cell.Uid))
        {
            result += " hc-sheet-cell__active";
        }
        
        return result;
    }

    public static string CellStyle(Sheet sheet, SheetRow row, SheetColumn column, SheetCell cell)
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
        
        sb.Append("height:");
        sb.Append(row.Height);
        sb.Append(";");
        
        if (!string.IsNullOrEmpty(cellStyle.BackgroundColor))
        {
            sb.Append("background-color:");
            sb.Append(cellStyle.BackgroundColor);
            sb.Append(";");
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


        return sb.ToString();
    }
}