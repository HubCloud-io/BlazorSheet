using HubCloud.BlazorSheet.Core.Models;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.ServerSideExamples.Pages;

public partial class SheetEditPage: ComponentBase
{
    private Sheet _sheet;
    private SheetCommandPanelStyleModel _selectedCellStyle { get; set; } = new SheetCommandPanelStyleModel();
    private SheetCell _selectedCell { get; set; }
    private List<SheetCell> _selectedCells { get; set; }
    private SheetRow _selectedSheetRow { get; set; }
    private SheetColumn _selectedSheetColumn { get; set; }
    
    protected override void OnInitialized()
    {
        _sheet = new Sheet(5, 5);
    }
    
    private void OnRowSelected(SheetRow row)
    {
        _selectedSheetRow = row;
    }

    private void OnColumnSelected(SheetColumn column)
    {
        _selectedSheetColumn = column;
    }

    private void OnCellSelected(SheetCell cell)
    {
        _selectedCell = cell;
        var style = _sheet.GetStyle(cell);
        _selectedCellStyle.CopyFrom(style);
    }

    private void OnCellsSelected(List<SheetCell> cells)
    {
       _selectedCells = cells;
    }

    private async Task OnStyleChanged()
    {
        if (_selectedCell == null)
        {
            return;
        }

        _sheet.SetStyle(_selectedCells, _selectedCellStyle);
       

       
    }
    
}