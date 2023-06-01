using HubCloud.BlazorSheet.Core.Models;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.ServerSideExamples.Pages;

public partial class SheetEditPage: ComponentBase
{
    private Sheet _sheet;
    private SheetCommandPanelModel _commandPanelModel { get; set; } = new SheetCommandPanelModel();
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
        _commandPanelModel.CopyFrom(style);

        var cellCoordinates = _sheet.CellCoordinates(cell);
        _commandPanelModel.SelectedCellAddress = $"R{cellCoordinates.Item1}C{cellCoordinates.Item2}";
        _commandPanelModel.InputText = cell.Formula;

        var editSettings = _sheet.GetEditSettings(cell);
        _commandPanelModel.SetEditSettings(editSettings);
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
        
        _sheet.SetSettingsFromCommandPanel(_selectedCells, _selectedCell, _commandPanelModel);
    }
    
}