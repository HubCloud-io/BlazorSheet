using BBComponents.Services;
using HubCloud.BlazorSheet.Components;
using HubCloud.BlazorSheet.Core.Interfaces;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.ServerSideExamples.Infrastructure;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.ServerSideExamples.Pages;

public partial class SheetEditPage: ComponentBase
{
    private IItemsSourceDataProvider _itemSourceDataProvider = new ItemsSourceProvider();

    private SheetComponent _sheetComponent;
    private Sheet _sheet;
    private SheetCommandPanelModel _commandPanelModel { get; set; } = new SheetCommandPanelModel();
    private SheetCell _selectedCell { get; set; }
    private List<SheetCell> _selectedCells { get; set; }
    private List<SheetRow> _selectedRowByNumberList { get; set; }
    private SheetRow _selectedSheetRow { get; set; }
    private SheetColumn _selectedSheetColumn { get; set; }

    private bool _canCellsBeJoined;
    private bool _canRowsBeGrouped;
    private bool _canRowsBeUngrouped;

    public int SelectedCellsCount => _selectedCells == null ? 0 : _selectedCells.Count;

    [Inject]
    public IAlertService AlertService { get; set; }

    protected override void OnInitialized()
    {
        _sheet = new Sheet(25, 25);
    }
    
    private void OnRowSelected(SheetRow row)
    {
        _selectedSheetRow = row;
    }

    private void OnRowsByNumberSelected(List<SheetRow> rows)
    {
        _selectedRowByNumberList = rows;

        if (_selectedRowByNumberList != null && _selectedRowByNumberList.Count > 0)
        {
            _canRowsBeGrouped = _sheet.CanRowsBeGrouped(rows);
            _canRowsBeUngrouped = _sheet.CanRowsBeUngrouped(rows);
        }
        else
        {
            _canRowsBeGrouped = false;
            _canRowsBeUngrouped = false;
        }
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

        var cellAddress = _sheet.CellAddress(cell);
        _commandPanelModel.SelectedCellAddress = $"R{cellAddress.Row}C{cellAddress.Column}";
        _commandPanelModel.InputText = cell.Formula;

        var editSettings = _sheet.GetEditSettings(cell);
        _commandPanelModel.SetEditSettings(editSettings);
    }

    private void OnCellsSelected(List<SheetCell> cells)
    {
       _selectedCells = cells;

        if (_selectedCells != null && _selectedCells.Count > 0)
            _canCellsBeJoined = _sheet.CanCellsBeJoined(_selectedCells);
        else
            _canCellsBeJoined = false;
    }

    private void OnStyleChanged()
    {
        if (_selectedCell == null)
            return;

        var result = _sheet.CheckFreezedRowsAndColumns(_commandPanelModel);

        if (!result)
        {
            _commandPanelModel.FreezedColumns = _sheet.FreezedColumns;
            _commandPanelModel.FreezedRows = _sheet.FreezedRows;

            AlertService.Add("Rows or columns can't be freezed", BBComponents.Enums.BootstrapColors.Warning);
        }

        _sheet.SetSettingsFromCommandPanel(_selectedCells, _selectedCell, _commandPanelModel);
    }

    private void OnGroupRows()
    {
        _sheet.GroupRows(_selectedRowByNumberList);
    }

    private void OnUngroupRows()
    {
        _sheet.UngroupRows(_selectedRowByNumberList);
    }

    private void OnCollapseExpandAll(bool isExpand)
    {
        var headRows = _sheet.Rows
            .Where(x => x.IsGroup)
            .ToList();

        foreach (var headRow in headRows)
        {
            headRow.IsOpen = isExpand;

            var groupedRows = _sheet.Rows
                .Where(x => x.ParentUid == headRow.Uid)
                .ToList();

            groupedRows.ForEach(x => x.IsHidden = isExpand ? false : true);
        }
    }
}