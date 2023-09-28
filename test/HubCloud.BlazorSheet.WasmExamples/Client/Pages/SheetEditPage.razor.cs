using BBComponents.Services;
using HubCloud.BlazorSheet.Components;
using HubCloud.BlazorSheet.Core.Interfaces;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.ExamplesShared.Infrastructure;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.WasmExamples.Client.Pages
{
    public partial class SheetEditPage : ComponentBase
    {
        private IItemsSourceDataProvider _itemSourceDataProvider = new ItemsSourceProvider();

        private SheetComponent _sheetComponent;
        private Sheet _sheet;
        private SheetCommandPanelModel _commandPanelModel { get; set; } = new SheetCommandPanelModel();
        private SheetCell _selectedCell { get; set; }
        private List<SheetCell> _selectedCells { get; set; }
        private SheetRow _selectedSheetRow { get; set; }
        private SheetColumn _selectedSheetColumn { get; set; }

        private bool _canCellsBeJoined;

        public int SelectedCellsCount => _selectedCells == null ? 0 : _selectedCells.Count;

        [Inject]
        public IAlertService AlertService { get; set; }

        protected override void OnInitialized()
        {
            _sheet = new Sheet(50, 15);
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
            _sheet.SetSettingsToCommandPanel(cell, _commandPanelModel);
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

            _sheet.SetSettingsFromCommandPanel(_selectedCells, _selectedCell, _commandPanelModel);
        }

        private void OnFreezedRowsChanged(int freezedRowsCount)
        {
            var result = _sheet.SetFreezedRows(freezedRowsCount);
            if (!result)
            {
                _commandPanelModel.FreezedRows = _sheet.FreezedRows;
                AlertService.Add("Rows can't be freezed", BBComponents.Enums.BootstrapColors.Warning);
            }
        }

        private void OnFreezedColumnsChanged(int freezedColumnsCount)
        {
            var result = _sheet.SetFreezedColumns(freezedColumnsCount);
            if (!result)
            {
                _commandPanelModel.FreezedColumns = _sheet.FreezedColumns;
                AlertService.Add("Columns can't be freezed", BBComponents.Enums.BootstrapColors.Warning);
            }
        }
    }
}
