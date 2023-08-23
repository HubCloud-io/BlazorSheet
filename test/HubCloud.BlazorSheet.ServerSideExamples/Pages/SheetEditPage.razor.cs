﻿using BBComponents.Services;
using HubCloud.BlazorSheet.Components;
using HubCloud.BlazorSheet.Core.Interfaces;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.ServerSideExamples.Infrastructure;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.ServerSideExamples.Pages;

public partial class SheetEditPage: ComponentBase
{
    private IDataTypeDataProvider _dataTypeDataProvider = new DataTypeDataProvider();

    private SheetComponent _sheetComponent;
    private Sheet _sheet;
    private SheetCommandPanelModel _commandPanelModel { get; set; } = new SheetCommandPanelModel();
    private SheetCell _selectedCell { get; set; }
    private List<SheetCell> _selectedCells { get; set; }
    private List<SheetRow> _selectedRowByNumberList { get; set; }
    private List<SheetColumn> _selectedColumnByNumberList { get; set; }
    private SheetRow _selectedSheetRow { get; set; }
    private SheetColumn _selectedSheetColumn { get; set; }

    private bool _canCellsBeJoined;
    private bool _canRowsBeGrouped;
    private bool _canColumnsBeGrouped;
    private bool _canRowsBeUngrouped;
    private bool _canColumnsBeUngrouped;

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

    private void OnColumnsByNumberSelected(List<SheetColumn> columns)
    {
        _selectedColumnByNumberList = columns;

        if (_selectedColumnByNumberList != null && _selectedColumnByNumberList.Count > 0)
        {
            _canColumnsBeGrouped = _sheet.CanColumnsBeGrouped(columns);
            _canColumnsBeUngrouped = _sheet.CanColumnsBeUngrouped(columns);
        }
        else
        {
            _canColumnsBeGrouped = false;
            _canColumnsBeUngrouped = false;
        }
    }

    private void OnColumnSelected(SheetColumn column)
    {
        _selectedSheetColumn = column;
    }

    private void OnCellSelected(SheetCell cell)
    {
        _selectedCell = cell;
        _sheet.SetSettingsToCommandPanel(cell, _commandPanelModel);
        _canRowsBeGrouped = false;
        _canRowsBeUngrouped = false;
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

    private void OnGroupColumns()
    {
        _sheet.GroupColumns(_selectedColumnByNumberList);
    }

    private void OnUngroupRows()
    {
        _sheet.UngroupRows(_selectedRowByNumberList);
    }

    private void OnUngroupColumns()
    {
        _sheet.UngroupColumns(_selectedColumnByNumberList);
    }

    private void OnCollapseExpandAllRows(bool isExpand)
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

    private void OnCollapseExpandAllColumns(bool isExpand)
    {
        var headColumns = _sheet.Columns
            .Where(x => x.IsGroup)
            .ToList();

        foreach (var headColumn in headColumns)
        {
            headColumn.IsOpen = isExpand;

            var groupedColumns = _sheet.Columns
                .Where(x => x.ParentUid == headColumn.Uid)
                .ToList();

            groupedColumns.ForEach(x => x.IsHidden = isExpand ? false : true);
        }
    }
}