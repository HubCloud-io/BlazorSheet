using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HubCloud.BlazorSheet.Core.Dto;
using HubCloud.BlazorSheet.Core.Enums;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class Sheet
    {
        private const int DefaultRowsCount = 10;
        private const int DefaultColumnsCount = 10;

        private int _rowsCount = DefaultRowsCount;
        private int _columnsCount = DefaultColumnsCount;
        private readonly List<SheetRow> _rows = new List<SheetRow>();
        private readonly Dictionary<Guid, SheetRow> _rowsDict = new Dictionary<Guid, SheetRow>();
        private readonly List<SheetColumn> _columns = new List<SheetColumn>();
        private readonly Dictionary<Guid, SheetColumn> _columnsDict = new Dictionary<Guid, SheetColumn>();
        private readonly List<SheetCell> _cells = new List<SheetCell>();
        private readonly SheetCellLookUp _cellsLookUp = new SheetCellLookUp();
        private readonly List<SheetCellStyle> _styles = new List<SheetCellStyle>();
        private readonly List<SheetCellEditSettings> _editSettings = new List<SheetCellEditSettings>();

        public Guid Uid { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public bool IsProtected { get; set; }

        public int RowsCount
        {
            get => _rowsCount;
            set
            {
                _rowsCount = value;
                if (_rowsCount <= 0)
                    _rowsCount = DefaultRowsCount;
            }
        }

        public int ColumnsCount
        {
            get => _columnsCount;
            set
            {
                _columnsCount = value;
                if (_columnsCount <= 0)
                {
                    _columnsCount = DefaultColumnsCount;
                }
            }
        }

        public int FreezedRows { get; set; }
        public int FreezedColumns { get; set; }

        public List<SheetRow> Rows => _rows;
        public List<SheetColumn> Columns => _columns;
        public List<SheetCell> Cells => _cells;
        public List<SheetCellStyle> Styles => _styles;
        public List<SheetCellEditSettings> EditSettings => _editSettings;

        public Sheet()
        {
        }

        public Sheet(SheetSettings settings)
        {
            Uid = settings.Uid;
            Name = settings.Name;
            RowsCount = settings.RowsCount;
            ColumnsCount = settings.ColumnsCount;
            FreezedColumns = settings.FreezedColumns;
            FreezedRows = settings.FreezedRows;
            IsProtected = settings.IsProtected;

            _rows.AddRange(settings.Rows);
            _rowsDict = _rows.ToDictionary(k => k.Uid, v => v);
            _columns.AddRange(settings.Columns);
            _columnsDict = _columns.ToDictionary(k => k.Uid, v => v);

            foreach (var cell in settings.Cells)
            {
                AddCell(cell);
            }

            _styles.AddRange(settings.Styles);
            _editSettings.AddRange(settings.EditSettings);

            if (!_cells.Any())
            {
                Init();
            }
            else
            {
                PrepareCellText();
            }
        }

        public Sheet(SheetDto dto)
        {
            Uid = dto.Uid;
            Name = dto.Name;
            RowsCount = dto.RowsCount;
            ColumnsCount = dto.ColumnsCount;
            FreezedColumns = dto.FreezedColumns;
            FreezedRows = dto.FreezedRows;
            IsProtected = dto.IsProtected;

            _rows.AddRange(dto.Rows);
            _rowsDict = _rows.ToDictionary(k => k.Uid, v => v);
            _columns.AddRange(dto.Columns);
            _columnsDict = _columns.ToDictionary(k => k.Uid, v => v);

            foreach (var cellDto in dto.Cells)
            {
                var cell = cellDto.BuildCell();
                AddCell(cell);
            }

            _styles.AddRange(dto.Styles);
            _editSettings.AddRange(dto.EditSettings);

            if (!_cells.Any())
            {
                Init();
            }
            else
            {
                PrepareCellText();
            }
        }

        public Sheet(int rowsCount, int columnsCount)
        {
            RowsCount = rowsCount;
            ColumnsCount = columnsCount;

            if (!_cells.Any())
            {
                Init();
            }
            else
            {
                PrepareCellText();
            }
        }

        public void Init()
        {
            Clear();

            for (var r = 1; r <= RowsCount; r++)
            {
                var row = new SheetRow();

                for (var c = 1; c <= ColumnsCount; c++)
                {
                    SheetColumn column;
                    if (r == 1)
                    {
                        column = new SheetColumn();
                        _columns.Add(column);
                        _columnsDict.Add(column.Uid, column);
                    }
                    else
                    {
                        column = _columns[c - 1];
                    }

                    var cell = new SheetCell();
                    cell.RowUid = row.Uid;
                    cell.ColumnUid = column.Uid;

                    AddCell(cell);
                }

                _rows.Add(row);
                _rowsDict.Add(row.Uid, row);
            }
        }

        public void InitLookUp()
        {
            _cellsLookUp.Init(_cells);
        }

        public void PrepareCellText()
        {
            foreach (var cell in Cells)
            {
                PrepareCellText(cell);
            }
        }

        public void PrepareCellText(SheetCell cell)
        {
            var editSettings = GetEditSettings(cell);
            cell.ApplyFormat(editSettings?.CellDataType ?? 0);
        }

        public List<SheetCell> GetRowCells(SheetRow row)
        {
            return Cells.Where(x => x.RowUid == row.Uid).ToList();
        }

        public List<SheetCell> GetColumnCells(SheetColumn column)
        {
            return Cells.Where(x => x.ColumnUid == column.Uid).ToList();
        }

        public SheetCell GetCell(Guid rowUid, Guid columnUid)
        {
            var cell = _cellsLookUp.Get(rowUid, columnUid);

            return cell;
        }
        
        public SheetCell GetCell(SheetRow row, SheetColumn column)
        {
           // var cell = Cells.FirstOrDefault(x => x.RowUid == row.Uid && x.ColumnUid == column.Uid);

           var cell = GetCell(row.Uid, column.Uid);

            return cell;
        }

        public SheetCell GetCell(SheetCellAddress cellAddress)
            => GetCell(cellAddress.Row, cellAddress.Column);

        public SheetCell GetCell(int rowNumber, int columnNumber)
        {
            if (rowNumber > _rowsCount || rowNumber <= 0)
                return null;

            if (columnNumber > _columnsCount || columnNumber <= 0)
                return null;

            var row = _rows[rowNumber - 1];
            var column = _columns[columnNumber - 1];

            if (row == null)
                return null;

            if (column == null)
                return null;

           // var cell = Cells.FirstOrDefault(x => x.RowUid == row.Uid && x.ColumnUid == column.Uid);

           var cell = GetCell(row.Uid, column.Uid);

            return cell;
        }

        public SheetCellStyle GetStyle(SheetCell cell)
        {
            SheetCellStyle style = null;

            if (cell.StyleUid.HasValue)
                style = Styles.FirstOrDefault(x => x.Uid == cell.StyleUid);

            if (style == null)
            {
                style = SheetCellStyle.DefaultCellStyle();
                cell.StyleUid = style.Uid;

                if (_styles.All(x => x.Uid != style.Uid))
                {
                    _styles.Add(style);
                }
                
            }

            return style;
        }

        public SheetCellEditSettings GetEditSettings(SheetCell cell)
        {
            SheetCellEditSettings editSettings;

            if (cell.EditSettingsUid.HasValue)
            {
                editSettings = EditSettings.FirstOrDefault(x => x.Uid == cell.EditSettingsUid);
            }
            else
            {
                editSettings = new SheetCellEditSettings();
            }
            
            return editSettings;
        }

        public int RowNumber(SheetRow row)
        {
            return _rows.IndexOf(row) + 1;
        }

        public int ColumnNumber(SheetColumn column)
        {
            return _columns.IndexOf(column) + 1;
        }

        public SheetCellAddress CellAddress(SheetCell cell)
        {
            var row = _rows.FirstOrDefault(x => x.Uid == cell.RowUid);
            var column = _columns.FirstOrDefault(x => x.Uid == cell.ColumnUid);

            var rowNumber = RowNumber(row);
            var columnNumber = ColumnNumber(column);

            return new SheetCellAddress(rowNumber, columnNumber);
        }
        
        public ValueAddress CellAddressSlim(SheetCell cell)
        {
            var row = _rowsDict[cell.RowUid];
            var column = _columnsDict[cell.ColumnUid];

            var rowNumber = RowNumber(row);
            var columnNumber = ColumnNumber(column);

            return new ValueAddress(rowNumber, columnNumber);
        }

        public SheetRow GetRow(int r)
        {
            return _rows[r - 1];
        }

        public SheetRow GetRow(Guid uid)
        {
            return _rows.FirstOrDefault(x => x.Uid == uid);
        }

        public SheetRow AddRow(SheetRow baseRow, int position, bool copySettings, Guid? rowUid = null)
        {
            var baseRowNumber = RowNumber(baseRow);
            var baseRowIndex = baseRowNumber - 1;

            var newRow = new SheetRow();
            if (rowUid.HasValue)
            {
                newRow.Uid = rowUid.Value;
            }

            if (copySettings)
            {
                newRow.IsAddRemoveAllowed = baseRow.IsAddRemoveAllowed;
                newRow.HeightValue = baseRow.HeightValue;
            }

            foreach (var column in _columns)
            {
                var newCell = new SheetCell()
                {
                    RowUid = newRow.Uid,
                    ColumnUid = column.Uid
                };

                if (copySettings)
                {
                    // Copy style and edit settings.
                    // var baseCell = _cells.FirstOrDefault(x => x.RowUid == baseRow.Uid
                    //                                           && x.ColumnUid == column.Uid);

                    var baseCell = _cellsLookUp.Get(baseRow.Uid, column.Uid);
                    CopyCellProperties(newCell, baseCell);
                }
                
                AddCell(newCell);
            }

            if (position == -1)
            {
                // Insert before.
                var i = baseRowNumber - 1;
                if (i < 0)
                    i = 0;

                _rows.Insert(i, newRow);
            }
            else
            {
                // Insert after.
                var i = baseRowIndex + 1;
                if (i > _rowsCount - 1)
                {
                    _rows.Add(newRow);
                }
                else
                {
                    _rows.Insert(i, newRow);
                }
            }

            _rowsCount++;

            _rowsDict.Add(newRow.Uid, newRow);
            return newRow;
        }

        public SheetColumn GetColumn(int c)
        {
            return _columns[c - 1];
        }

        public SheetColumn GetColumn(Guid uid)
        {
            return _columns.FirstOrDefault(x => x.Uid == uid);
        }

        public SheetColumn AddColumn(SheetColumn baseColumn, int position, bool copySettings, Guid? columnUid = null)
        {
            var baseColumnNumber = ColumnNumber(baseColumn);
            var baseColumnIndex = baseColumnNumber - 1;

            var newColumn = new SheetColumn();
            if (columnUid.HasValue)
            {
                newColumn.Uid = columnUid.Value;
            }

            if (copySettings)
            {
                newColumn.IsAddRemoveAllowed = baseColumn.IsAddRemoveAllowed;
                newColumn.WidthValue = baseColumn.WidthValue;
            }

            foreach (var row in _rows)
            {
                var newCell = new SheetCell()
                {
                    RowUid = row.Uid,
                    ColumnUid = newColumn.Uid
                };

                if (copySettings)
                {
                    // Copy style and edit settings.
                    // var baseCell = _cells.FirstOrDefault(x => x.RowUid == row.Uid
                    //                                           && x.ColumnUid == baseColumn.Uid);

                    var baseCell = _cellsLookUp.Get(row.Uid, baseColumn.Uid);
                    CopyCellProperties(newCell, baseCell);
                }

                AddCell(newCell);
            }

            if (position == -1)
            {
                // Insert before.
                var i = baseColumnNumber - 1;
                if (i < 0)
                    i = 0;

                _columns.Insert(i, newColumn);
            }
            else
            {
                // Insert after.
                var i = baseColumnIndex + 1;
                if (i > _columnsCount - 1)
                {
                    _columns.Add(newColumn);
                }
                else
                {
                    _columns.Insert(i, newColumn);
                }
            }

            _columnsCount++;

            _columnsDict.Add(newColumn.Uid, newColumn);
            return newColumn;
        }

        public void RemoveRow(SheetRow row)
        {
            if (row == null)
                return;

            var cellsToDelete = Cells.Where(x => x.RowUid == row.Uid).ToList();

            for (var i = cellsToDelete.Count() - 1; i >= 0; i--)
            {
                var cell = cellsToDelete[i];
                RemoveCell(cell);
            }

            if (_rowsDict.ContainsKey(row.Uid))
                _rowsDict.Remove(row.Uid);
            if (_rows.Remove(row))
                RowsCount -= 1;

            Rows.All(x => x.ShouldRender = true);
        }

        public void RemoveColumn(SheetColumn column)
        {
            if (column == null)
                return;

            var cellsToDelete = Cells.Where(x => x.ColumnUid == column.Uid).ToList();
            for (var i = cellsToDelete.Count() - 1; i >= 0; i--)
            {
                var cell = cellsToDelete[i];
                RemoveCell(cell);
            }

            if (_columnsDict.ContainsKey(column.Uid))
                _columnsDict.Remove(column.Uid);
            if (_columns.Remove(column))
                ColumnsCount -= 1;

            Rows.All(x => x.ShouldRender = true);
        }

        public void SetSettingsFromCommandPanel(List<SheetCell> cells, SheetCell cell,
            SheetCommandPanelModel commandPanelModel)
        {
            if (cells == null)
                return;

            if (commandPanelModel == null)
                return;

            var newStyle = new SheetCellStyle(commandPanelModel);
            SetStyle(cells, newStyle);
            
            IsProtected = commandPanelModel.SheetProtected;
        }

        public void SetFormat(List<SheetCell> cells, CellFormatTypes formatType, string customFormat)
        {
            if (cells == null)
                return;

            foreach (var item in cells)
            {
                item.SetFormat(formatType, customFormat);
                item.ShouldRender = true;
            }
        }

        public void SetFormula(List<SheetCell> cells, string formula)
        {
            if (cells == null)
                return;

            foreach (var item in cells)
                item.Formula = formula;
        }
        
        public void SetSettingsToCommandPanel(SheetCell cell, SheetCommandPanelModel commandPanelModel)
        {
            var style = GetStyle(cell);
            commandPanelModel.CopyFrom(style);

            var cellAddress = CellAddress(cell);
            commandPanelModel.SelectedCellAddress = $"R{cellAddress.Row}C{cellAddress.Column}";
            commandPanelModel.InputText = cell.Formula;
            commandPanelModel.SetFormatType(cell.Format);

            var editSettings = GetEditSettings(cell);
            commandPanelModel.SetEditSettings(editSettings);
        }

        public void SetStyle(SheetCell cell, SheetCellStyle newStyle)
        {
            if (cell == null)
            {
                return;
            }

            if (newStyle == null)
            {
                return;
            }

            var collection = new SheetCell[] {cell};
            SetStyle(collection, newStyle);
        }

        public void SetStyle(IEnumerable<SheetCell> cells, SheetCellStyle newStyle)
        {
            if (cells == null)
            {
                return;
            }

            if (newStyle == null)
            {
                return;
            }

            var existingStyle = FindExistingStyle(newStyle);

            Guid styleUid;

            if (existingStyle == null)
            {
                if (_styles.Any(x => x.Uid == newStyle.Uid))
                    newStyle.Uid = Guid.NewGuid();

                _styles.Add(newStyle);
                styleUid = newStyle.Uid;
            }
            else
            {
                styleUid = existingStyle.Uid;
            }

            foreach (var cell in cells)
            {
                cell.StyleUid = styleUid;
                cell.ShouldRender = true;
            }
        }

        public void SetEditSettings(SheetCell cell, SheetCellEditSettings newEditSettings)
        {
            if (cell == null)
            {
                return;
            }

            if (newEditSettings == null)
            {
                return;
            }

            var collection = new SheetCell[] {cell};
            SetEditSettings(collection, newEditSettings);
        }

        public void SetEditSettings(IEnumerable<SheetCell> cells, SheetCellEditSettings newEditSettings)
        {
            if (cells == null)
            {
                return;
            }

            if (newEditSettings == null)
            {
                return;
            }

            if (newEditSettings.ControlKind == CellControlKinds.Undefined)
            {
                // Drop edit settings.
                foreach (var cell in cells)
                {
                    cell.EditSettingsUid = null;
                }
                
                return;
            }

            // Set edit settings.
            var existingEditSettings = FindExistingEditSettings(newEditSettings);

            Guid settingsUid;
            if (existingEditSettings == null)
            {
                _editSettings.Add(newEditSettings);
                settingsUid = newEditSettings.Uid;
            }
            else
            {
                settingsUid = existingEditSettings.Uid;
            }

            foreach (var cell in cells)
            {
                cell.EditSettingsUid = settingsUid;
                cell.ShouldRender = true;
            }
        }

        public SheetCellStyle FindExistingStyle(SheetCellStyle newStyle)
        {
            foreach (var existingStyle in _styles)
            {
                if (existingStyle.IsStyleEqual(newStyle))
                    return existingStyle;
            }

            return null;
        }

        public SheetCellEditSettings FindExistingEditSettings(SheetCellEditSettings newEditSettings)
        {
            foreach (var item in _editSettings)
            {
                if (item.Equals(newEditSettings))
                    return item;
            }

            return null;
        }

        public void CopyRowStyle(Sheet sourceSheet, SheetRow sourceRow, SheetRow destinationRow)
        {
            var destinationRowNumber = this.RowNumber(destinationRow);
            var sourceCells = sourceSheet.Cells.Where(x => x.RowUid == sourceRow.Uid);

            foreach (var sourceCell in sourceCells)
            {
                var cellAddress = sourceSheet.CellAddress(sourceCell);
                var destinationCell = this.GetCell(destinationRowNumber, cellAddress.Column);

                if (destinationCell == null)
                {
                    continue;
                }

                destinationCell.StyleUid = sourceCell.StyleUid;
            }
        }

        public void CopyColumnStyle(Sheet sourceSheet, SheetColumn sourceColumn,
            SheetColumn destinationColumn)
        {
            var destinationColumnNumber = this.ColumnNumber(destinationColumn);
            var sourceCells = sourceSheet.Cells.Where(x => x.ColumnUid == sourceColumn.Uid);

            foreach (var sourceCell in sourceCells)
            {
                var cellAddress = sourceSheet.CellAddress(sourceCell);
                var destinationCell = this.GetCell(cellAddress.Row, destinationColumnNumber);

                if (destinationCell == null)
                {
                    continue;
                }

                destinationCell.StyleUid = sourceCell.StyleUid;
            }
        }

        public SheetSettings ToSettings()
        {
            var settings = new SheetSettings()
            {
                Uid = Uid,
                Name = Name,
                RowsCount = RowsCount,
                ColumnsCount = ColumnsCount,
                FreezedColumns = FreezedColumns,
                FreezedRows = FreezedRows,
                IsProtected = IsProtected
            };

            settings.Rows.AddRange(_rows);
            settings.Columns.AddRange(_columns);
            settings.Cells.AddRange(_cells);
            settings.SetStyles(_styles);
            settings.SetEditSettings(_editSettings);

            return settings;
        }

        public void ChangeSize(int newColumnsCount, int newRowsCount)
        {
            if (newColumnsCount <= 0 || newRowsCount <= 0)
                return;

            var columnsAddRemoveCount = Columns.Count - newColumnsCount;
            var rowsAddRemoveCount = Rows.Count - newRowsCount;

            if (columnsAddRemoveCount != 0)
            {
                Rows.All(x => x.ShouldRender = true);
                ChangeSize<SheetColumn>(columnsAddRemoveCount, Columns, RemoveColumn, AddColumn);
            }

            if (rowsAddRemoveCount != 0)
            {
                Rows.All(x => x.ShouldRender = true);
                ChangeSize<SheetRow>(rowsAddRemoveCount, Rows, RemoveRow, AddRow);
            }
        }

        private void ChangeSize<T>(int addRemoveCount, IReadOnlyCollection<T> collection, Action<T> removeAction,
            Func<T, int, bool, Guid?, T> addAction) where T : class
        {
            if (addRemoveCount > 0)
            {
                for (int i = 0; i < addRemoveCount; i++)
                {
                    var lastItem = collection.LastOrDefault();
                    if (lastItem != null)
                        removeAction(lastItem);
                }
            }
            else
            {
                for (int i = 0; i < addRemoveCount * -1; i++)
                {
                    var lastItem = collection.LastOrDefault();
                    if (lastItem != null)
                        addAction(lastItem, 1, false, null);
                }
            }
        }

        private void Clear()
        {
            _rows.Clear();
            _columns.Clear();
            _cells.Clear();
            _cellsLookUp.Clear();
            //_styles.Clear();
            //_editSettings.Clear();
        }

        public SheetCell JoinCells(List<SheetCell> selectedCells)
        {
            var cells = selectedCells.Distinct().ToList();

            if (cells.Count <= 1)
                return null;

            var cellWithAddressList = cells.Select(cell => new CellWithAddress
            {
                Cell = cell,
                Address = CellAddress(cell)
            });

            var grouppedByRow = cellWithAddressList.GroupBy(x => x.Address.Row).OrderBy(x => x.Key);
            var grouppedByColumn = cellWithAddressList.GroupBy(x => x.Address.Column).OrderBy(x => x.Key);

            var topLeftCell = grouppedByRow
                .First()
                .OrderBy(x => x.Address.Column)
                .First()
                .Cell;

            // join cells by horizontal and vertical
            if (grouppedByRow.Count() > 1 && grouppedByColumn.Count() > 1)
            {
                var colspanMax = grouppedByRow.Select(group => group.Sum(item => item.Cell.Colspan)).Max();
                var rowspanMax = grouppedByColumn.Select(group => group.Sum(item => item.Cell.Rowspan)).Max();

                topLeftCell.Colspan = colspanMax;
                topLeftCell.Rowspan = rowspanMax;

                foreach (var cell in cells)
                {
                    if (cell.Uid != topLeftCell.Uid)
                    {
                        cell.HiddenByJoin = true;
                        cell.Colspan = 1;
                        cell.Rowspan = 1;
                    }
                }
            }
            // join cells by horizontal
            else if (grouppedByRow.Count() == 1)
            {
                topLeftCell.Colspan = grouppedByRow
                    .First()
                    .Sum(x => x.Cell.Colspan);

                foreach (var cell in cells)
                {
                    if (cell.Uid != topLeftCell.Uid)
                    {
                        cell.HiddenByJoin = true;
                        cell.Colspan = 1;
                        cell.Rowspan = 1;
                    }
                }
            }
            // join cells by vertical
            else if (grouppedByColumn.Count() == 1)
            {
                topLeftCell.Rowspan = grouppedByColumn
                    .First()
                    .Sum(x => x.Cell.Rowspan);

                foreach (var cell in cells)
                {
                    if (cell.Uid != topLeftCell.Uid)
                    {
                        cell.HiddenByJoin = true;
                        cell.Colspan = 1;
                        cell.Rowspan = 1;
                    }
                }
            }

            Rows.All(x => x.ShouldRender = true);
            Cells.All(x => x.ShouldRender = true);

            return topLeftCell;
        }

        public void SplitCells(SheetCell currentCell)
        {
            var joinedCellRange = new SheetJoinedCellRange
            {
                Cell = currentCell,
                Address = CellAddress(currentCell)
            };

            var range = joinedCellRange.Range;

            foreach (var item in range)
            {
                var cell = GetCell(item);

                if (cell != null)
                {
                    cell.Colspan = 1;
                    cell.Rowspan = 1;
                    cell.HiddenByJoin = false;
                    cell.ShouldRender = true;
                }
            }
        }

        public bool CanCellsBeJoined(List<SheetCell> cells)
        {
            cells = cells.Distinct().ToList();

            if (cells.Count < 2)
                return false;

            var joinedCellRangeList = new List<SheetJoinedCellRange>();

            foreach (var cell in cells)
            {
                var joinedCellRange = new SheetJoinedCellRange
                {
                    Cell = cell,
                    Address = CellAddress(cell)
                };

                joinedCellRangeList.Add(joinedCellRange);
            }

            var addresses = new List<SheetCellAddress>();
            joinedCellRangeList.ForEach(cell => addresses.AddRange(cell.Range));

            var firstColumnNumber = addresses.Select(s => s.Column).Min();
            var lastColumnNumber = addresses.Select(s => s.Column).Max();
            var firstRowNumber = addresses.Select(s => s.Row).Min();
            var lastRowNumber = addresses.Select(s => s.Row).Max();

            for (int i = firstRowNumber; i <= lastRowNumber; i++)
            {
                for (int j = firstColumnNumber; j <= lastColumnNumber; j++)
                {
                    var cellAddress = addresses.FirstOrDefault(address => address.Row == i && address.Column == j);

                    if (cellAddress == null)
                        return false;
                }
            }

            return true;
        }

        public void GroupRows(List<SheetRow> rows)
        {
            var rowNumbers = rows
                    .Select(x => RowNumber(x))
                    .OrderBy(x => x)
                    .ToList();

            var firstRowNumber = rowNumbers.FirstOrDefault();
            if (firstRowNumber == 0)
                return;

            var firstRow = GetRow(firstRowNumber);
            if (firstRow.IsGroup)
                firstRow.IsGroup = false;

            var headRow = GetRow(firstRowNumber - 1);
            if (headRow.ParentUid == Guid.Empty)
            {
                headRow.IsGroup = true;
                headRow.IsOpen = true;
            }
            else
            {
                if (firstRow.ParentUid == Guid.Empty)
                    headRow = FindMainParentRow(headRow);
                else
                {
                    headRow.IsGroup = true;
                    headRow.IsOpen = true;
                }
            }

            foreach (var row in rows)
            {
                row.IsGroup = false;
                row.ParentUid = headRow.Uid;
                row.IsOpen = headRow.IsOpen;
                row.IsCollapsed = !headRow.IsOpen;

                ChangeChildrenParent(row, headRow.Uid);
                ChangeChildrenVisibility(row, row.IsOpen);
            }

            Rows.All(x => x.ShouldRender = true);
        }

        public void GroupColumns(List<SheetColumn> columns)
        {
            var columnNumbers = columns
                    .Select(x => ColumnNumber(x))
                    .OrderBy(x => x)
                    .ToList();

            var firstColumnNumber = columnNumbers.FirstOrDefault();
            if (firstColumnNumber == 0)
                return;

            var firstColumn = GetColumn(firstColumnNumber);
            if (firstColumn.IsGroup)
                firstColumn.IsGroup = false;

            var headColumn = GetColumn(firstColumnNumber - 1);
            if (headColumn.ParentUid == Guid.Empty)
            {
                headColumn.IsGroup = true;
                headColumn.IsOpen = true;
            }
            else
            {
                if (firstColumn.ParentUid == Guid.Empty)
                    headColumn = FindMainParentColumn(headColumn);
                else
                {
                    headColumn.IsGroup = true;
                    headColumn.IsOpen = true;
                }
            }

            foreach (var column in columns)
            {
                column.IsGroup = false;
                column.ParentUid = headColumn.Uid;
                column.IsOpen = headColumn.IsOpen;
                column.IsCollapsed = !headColumn.IsOpen;

                ChangeChildrenParent(column, headColumn.Uid);
                ChangeChildrenVisibility(column, column.IsOpen);
            }

            Rows.All(x => x.ShouldRender = true);
        }

        private void ChangeChildrenParent(SheetRow parentRow, Guid newParentUid)
        {
            var rows = Rows.Where(x => x.ParentUid == parentRow.Uid).ToList();
            rows.ForEach(x => x.ParentUid = newParentUid);
        }

        private void ChangeChildrenParent(SheetColumn parentColumn, Guid newParentUid)
        {
            var columns = Columns.Where(x => x.ParentUid == parentColumn.Uid).ToList();
            columns.ForEach(x => x.ParentUid = newParentUid);
        }

        public void ChangeChildrenVisibility(SheetRow parentRow, bool IsVisible)
        {
            var rows = Rows.Where(x => x.ParentUid == parentRow.Uid).ToList();

            foreach (var row in rows)
            {
                row.IsCollapsed = !IsVisible;

                if (row.IsGroup)
                {
                    row.IsOpen = IsVisible;
                    var childsVisible = IsVisible && row.IsOpen;
                    ChangeChildrenVisibility(row, childsVisible);
                }
            }
        }

        public void ChangeChildrenVisibility(SheetColumn parentColumn, bool IsVisible)
        {
            var columns = Columns.Where(x => x.ParentUid == parentColumn.Uid).ToList();

            foreach (var column in columns)
            {
                column.IsCollapsed = !IsVisible;

                if (column.IsGroup)
                {
                    column.IsOpen = IsVisible;
                    var childsVisible = IsVisible && column.IsOpen;
                    ChangeChildrenVisibility(column, childsVisible);
                }
            }
        }

        private SheetRow FindMainParentRow(SheetRow row)
        {
            if (row.ParentUid == Guid.Empty)
                return row;

            var parentRow = Rows.FirstOrDefault(x => x.Uid == row.ParentUid);
            if (parentRow == null)
                return null;

            return FindMainParentRow(parentRow);
        }

        private SheetColumn FindMainParentColumn(SheetColumn column)
        {
            if (column.ParentUid == Guid.Empty)
                return column;

            var parentColumn = Columns.FirstOrDefault(x => x.Uid == column.ParentUid);
            if (parentColumn == null)
                return null;

            return FindMainParentColumn(parentColumn);
        }

        public void UngroupRows(List<SheetRow> rows)
        {
            rows = rows
                    .OrderBy(x => RowNumber(x))
                    .ToList();

            var parentRowUids = rows.Select(x => x.ParentUid).Distinct();
            if (parentRowUids.Count() > 1)
                return;

            var parentRowUid = parentRowUids.FirstOrDefault();
            if (parentRowUid == null)
                return;

            var parentRow = Rows.FirstOrDefault(x => x.Uid == parentRowUid);
            if (parentRow == null)
                return;

            foreach (var row in rows)
                row.ParentUid = parentRow.ParentUid;

            var lastRow = rows.LastOrDefault();
            if (lastRow == null)
                return;

            var lastRowNumber = RowNumber(lastRow);

            var parentChildren = Rows.Where(x => x.ParentUid == parentRow.Uid && RowNumber(x) > lastRowNumber);
            if (parentChildren.Count() > 0)
            {
                lastRow.IsGroup = true;
                lastRow.IsOpen = true;

                foreach (var child in parentChildren)
                {
                    child.ParentUid = lastRow.Uid;
                    child.IsOpen = lastRow.IsOpen;
                    child.IsCollapsed = !lastRow.IsOpen;
                }
            }
            else
            {
                parentChildren = Rows.Where(x => x.ParentUid == parentRow.Uid);
                if (parentChildren.Except(rows).Count() == 0)
                    parentRow.IsGroup = false;
            }

            Rows.All(x => x.ShouldRender = true);
        }

        public void UngroupAllRows()
        {
            foreach (var row in Rows)
            {
                row.ParentUid = Guid.Empty;
                row.IsOpen = false;
                row.IsGroup = false;
                row.IsCollapsed = false;
                row.ShouldRender = true;
            }
        }

        public void UngroupColumns(List<SheetColumn> columns)
        {
            columns = columns
                .OrderBy(x => ColumnNumber(x))
                .ToList();

            var parentColumnUids = columns.Select(x => x.ParentUid).Distinct();
            if (parentColumnUids.Count() > 1)
                return;

            var parentColumnUid = parentColumnUids.FirstOrDefault();
            if (parentColumnUid == null)
                return;

            var parentColumn = Columns.FirstOrDefault(x => x.Uid == parentColumnUid);
            if (parentColumn == null)
                return;

            foreach (var column in columns)
                column.ParentUid = parentColumn.ParentUid;

            var lastColumn = columns.LastOrDefault();
            if (lastColumn == null)
                return;

            var lastColumnNumber = ColumnNumber(lastColumn);

            var parentChildren = Columns.Where(x => x.ParentUid == parentColumn.Uid && ColumnNumber(x) > lastColumnNumber);
            if (parentChildren.Count() > 0)
            {
                lastColumn.IsGroup = true;
                lastColumn.IsOpen = true;

                foreach (var child in parentChildren)
                {
                    child.ParentUid = lastColumn.Uid;
                    child.IsOpen = lastColumn.IsOpen;
                    child.IsHidden = !lastColumn.IsOpen;
                }
            }
            else
            {
                parentChildren = Columns.Where(x => x.ParentUid == parentColumn.Uid);
                if (parentChildren.Except(columns).Count() == 0)
                    parentColumn.IsGroup = false;
            }

            Rows.All(x => x.ShouldRender = true);
        }

        public void UngroupAllColumns()
        {
            foreach (var column in Columns)
            {
                column.ParentUid = Guid.Empty;
                column.IsOpen = false;
                column.IsGroup = false;
                column.IsCollapsed = false;
            }

            Rows.All(x => x.ShouldRender = true);
        }

        public bool CanRowsBeGrouped(List<SheetRow> rows)
        {
            var rowNumbers = rows
                .Select(x => RowNumber(x))
                .OrderBy(x => x)
                .ToList();

            if (rowNumbers.Contains(1))
                return false;

            var headRow = GetRow(rowNumbers.First() - 1);
            if (headRow.IsHidden)
                return false;

            if (rows.Contains(headRow))
                return false;

            if (rows.Any(x => x.ParentUid != Guid.Empty) && 
                rows.Any(x => x.ParentUid == Guid.Empty))
                return false;

            var current = rowNumbers.First();

            for (int i = 0; i < rowNumbers.Count; i++)
            {
                if (current != rowNumbers[i])
                    return false;

                current++;
            }

            return true;
        }

        public bool CanColumnsBeGrouped(List<SheetColumn> columns)
        {
            return true;
        }

        public bool CanRowsBeUngrouped(List<SheetRow> rows)
        {
            var rowNumbers = rows
                .Select(x => RowNumber(x))
                .OrderBy(x => x)
                .ToList();

            if (rowNumbers.Contains(1))
                return false;

            var headRow = GetRow(rowNumbers.First() - 1);
            if (headRow.IsHidden)
                return false;

            if (rows.Any(x => x.ParentUid != Guid.Empty) &&
                rows.Any(x => x.ParentUid == Guid.Empty))
                return false;

            var current = rowNumbers.First();

            for (int i = 0; i < rowNumbers.Count; i++)
            {
                if (current != rowNumbers[i])
                    return false;

                current++;
            }

            return true;
        }

        public bool CanColumnsBeUngrouped(List<SheetColumn> columns)
        {
            return true;
        }

        public bool SetFreezedRows(int freezedRows)
        {
            var result = CanFreezedRowsBeSet(freezedRows);
            if (!result)
                return false;

            if (freezedRows > RowsCount)
                return false;

            FreezedRows = freezedRows;

            Rows.All(x => x.ShouldRender = true);
            Cells.All(x => x.ShouldRender = true);

            return true;
        }

        private bool CanFreezedRowsBeSet(int freezedRows)
        {
            if (freezedRows > 0)
            {
                var cellWithAddressList = Cells
                    .Where(cell => cell.Rowspan > 1)
                    .Select(cell => new CellWithAddress
                    {
                        Cell = cell,
                        Address = CellAddress(cell)
                    })
                    .ToList();

                foreach (var cell in cellWithAddressList)
                {
                    var joinedCellsCount = cell.Address.Row + cell.Cell.Rowspan - 1;

                    if (cell.Address.Row <= freezedRows && joinedCellsCount > freezedRows)
                        return false;
                }
            }

            return true;
        }

        public bool SetFreezedColumns(int freezedColumns)
        {
            var result = CanFreezedColumnsBeSet(freezedColumns);
            if (!result)
                return false;

            if (freezedColumns > ColumnsCount)
                return false;

            FreezedColumns = freezedColumns;

            Rows.All(x => x.ShouldRender = true);
            Cells.All(x => x.ShouldRender = true);

            return true;
        }

        private bool CanFreezedColumnsBeSet(int freezedColumns)
        {
            if (freezedColumns > 0)
            {
                var cellWithAddressList = Cells
                    .Where(cell => cell.Colspan > 1)
                    .Select(cell => new CellWithAddress
                    {
                        Cell = cell,
                        Address = CellAddress(cell)
                    })
                    .ToList();

                foreach (var cell in cellWithAddressList)
                {
                    var joinedCellsCount = cell.Address.Column + cell.Cell.Colspan - 1;

                    if (cell.Address.Column <= freezedColumns && joinedCellsCount > freezedColumns)
                        return false;
                }
            }

            return true;
        }

        public Sheet Copy()
        {
            var output = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<Sheet>(output,
                new JsonSerializerSettings {FloatParseHandling = FloatParseHandling.Decimal});
        }

        public void AddCell(SheetCell cell)
        {
            _cells.Add(cell);
            _cellsLookUp.Add(cell);
        }

        public void RemoveCell(SheetCell cell)
        {
            _cellsLookUp.Remove(cell);
            _cells.Remove(cell);
            
        }

        private void CopyCellProperties(SheetCell destinationCell, SheetCell sourceCell)
        {
            if (sourceCell == null)
            {
                return;
            }

            destinationCell.StyleUid = sourceCell.StyleUid;
            destinationCell.EditSettingsUid = sourceCell.EditSettingsUid;
            destinationCell.Formula = sourceCell.Formula;
            destinationCell.Format = sourceCell.Format;
            destinationCell.Locked = sourceCell.Locked;
            destinationCell.Link = sourceCell.Link;

            if (!destinationCell.EditSettingsUid.HasValue 
                && string.IsNullOrEmpty(sourceCell.Formula))
            {
                destinationCell.Value = sourceCell.Value;
                destinationCell.Text = sourceCell.Text;
            }
        }

        public List<SheetCell> GetCellsByRange(int fromRow, int fromCol, int toRow, int toCol)
        {
            var cells = new List<SheetCell>();

            if (fromRow < 1 || fromCol < 1 || toRow < 1 || toCol < 1)
                return cells;

            if (fromRow > toRow || fromCol > toCol)
                return cells;

            for (int i = fromRow; i <= toRow; i++)
            {
                for (int j = fromCol; j <= toCol; j++)
                {
                    var cell = GetCell(i, j);

                    if (cell != null)
                        cells.Add(cell);
                }
            }

            return cells;
        }

        public int GetDimensionEndRowNumber()
        {
            var rowUids = Cells
                .Where(x => x.Value != null)
                .Select(x => x.RowUid)
                .Distinct();

            var rows = Rows.Where(x => rowUids.Contains(x.Uid));

            return rows
                .Select(x => RowNumber(x))
                .Max();
        }

        public void ApplyStyleParams(IEnumerable<SheetCell> cells, Dictionary<string, object> styleParams)
        {
            if (!cells.Any() || !styleParams.Any())
                return;

            var style = new SheetCellStyle();

            foreach (var item in styleParams)
            {
                var prop = style.GetType().GetProperty(item.Key);
                if (prop != null)
                    prop.SetValue(style, item.Value, null);
            }

            var styleUids = cells.Select(x => x.StyleUid).Distinct();
            foreach (var styleUid in styleUids)
            {
                var currentStyle = Styles.FirstOrDefault(x => x.Uid == styleUid);
                if (currentStyle == null)
                    currentStyle = style;
                else
                {
                    currentStyle = currentStyle.Copy();

                    foreach (var item in styleParams)
                    {
                        var prop = currentStyle.GetType().GetProperty(item.Key);
                        if (prop != null)
                            prop.SetValue(currentStyle, item.Value, null);
                    }
                }

                var currentCells = cells.Where(x => x.StyleUid == styleUid);
                SetStyle(currentCells, currentStyle);
            }

            var withoutStyleCells = cells.Where(x => !x.StyleUid.HasValue);
            if (withoutStyleCells.Any())
                SetStyle(withoutStyleCells, style);
        }

        public void CollapseExpandRows(bool isExpand)
        {
            var headRows = Rows.Where(x => x.IsGroup && x.ParentUid == Guid.Empty);

            foreach (var headRow in headRows)
            {
                headRow.IsOpen = isExpand;
                ChangeChildrenVisibility(headRow, headRow.IsOpen);
            }

            Rows.All(x => x.ShouldRender = true);
        }

        public void CollapseExpandColumns(bool isExpand)
        {
            var headColumns = Columns.Where(x => x.IsGroup && x.ParentUid == Guid.Empty);

            foreach (var headColumn in headColumns)
            {
                headColumn.IsOpen = isExpand;
                ChangeChildrenVisibility(headColumn, headColumn.IsOpen);
            }

            Rows.All(x => x.ShouldRender = true);
        }

        public IEnumerable<SheetColumn> GetVisibleColumns(bool isHiddenCellsVisible)
        {
            return Columns.Where(x => (!x.IsHidden || isHiddenCellsVisible) && !x.IsCollapsed);
        }

        public List<SheetRow> GetVisibleRows(bool isHiddenCellsVisible)
        {
            var rows = Rows
                .Skip(FreezedRows)
                .Where(x => (!x.IsHidden || isHiddenCellsVisible) && !x.IsCollapsed)
                .ToList();

            return rows;
        }

        public IEnumerable<SheetRow> GetFreezedVisibleRows(bool isHiddenCellsVisible)
        {
            var rows = Rows.Take(FreezedRows)
                .Where(x => (!x.IsHidden || isHiddenCellsVisible) && !x.IsCollapsed);

            return rows;
        }
    }
}