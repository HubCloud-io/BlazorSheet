using System;
using System.Collections.Generic;
using System.Linq;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class Sheet
    {
        private const int DefaultRowsCount = 10;
        private const int DefaultColumnsCount = 10;

        private int _rowsCount = DefaultRowsCount;
        private int _columnsCount = DefaultColumnsCount;
        private readonly List<SheetRow> _rows = new List<SheetRow>();
        private readonly List<SheetColumn> _columns = new List<SheetColumn>();
        private readonly List<SheetCell> _cells = new List<SheetCell>();
        private readonly List<SheetCellStyle> _styles = new List<SheetCellStyle>();
        private readonly List<SheetCellEditSettings> _editSettings = new List<SheetCellEditSettings>();

        public Guid Uid { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public bool UseVirtualization { get; set; }
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

        public IReadOnlyCollection<SheetRow> Rows => _rows;
        public IReadOnlyCollection<SheetColumn> Columns => _columns;
        public IReadOnlyCollection<SheetCell> Cells => _cells;
        public IReadOnlyCollection<SheetCellStyle> Styles => _styles;
        public IReadOnlyCollection<SheetCellEditSettings> EditSettings => _editSettings;

        public Sheet()
        {
        }

        public Sheet(SheetSettings settings)
        {
            Uid = settings.Uid;
            Name = settings.Name;
            UseVirtualization = settings.UseVirtualization;
            RowsCount = settings.RowsCount;
            ColumnsCount = settings.ColumnsCount;
            FreezedColumns = settings.FreezedColumns;
            FreezedRows = settings.FreezedRows;

            _rows.AddRange(settings.Rows);
            _columns.AddRange(settings.Columns);
            _cells.AddRange(settings.Cells);
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
                    }
                    else
                    {
                        column = _columns[c - 1];
                    }

                    var cell = new SheetCell();
                    cell.RowUid = row.Uid;
                    cell.ColumnUid = column.Uid;

                    _cells.Add(cell);
                }

                _rows.Add(row);
            }
        }

        public void PrepareCellText()
        {
            foreach (var cell in Cells)
            {
                cell.Text = cell.Value?.ToString();
            }
        }

        public List<SheetCell> GetRowCells(SheetRow row)
        {
            var cells = Cells.Where(x => x.RowUid == row.Uid).ToList();

            return cells;
        }

        public SheetCell GetCell(SheetRow row, SheetColumn column)
        {
            var cell = Cells.FirstOrDefault(x => x.RowUid == row.Uid && x.ColumnUid == column.Uid);

            return cell;
        }

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

            var cell = Cells.FirstOrDefault(x => x.RowUid == row.Uid && x.ColumnUid == column.Uid);

            return cell;
        }

        public SheetCellStyle GetStyle(SheetCell cell)
        {
            SheetCellStyle style = null;

            if (cell.StyleUid.HasValue)
                style = Styles.FirstOrDefault(x => x.Uid == cell.StyleUid);

            if (style == null)
            {
                style = new SheetCellStyle();
                cell.StyleUid = style.Uid;
                _styles.Add(style);
            }

            return style;
        }

        public SheetCellEditSettings GetEditSettings(SheetCell cell)
        {
            SheetCellEditSettings editSettings = null;

            if (cell.EditSettingsUid.HasValue)
                editSettings = EditSettings.FirstOrDefault(x => x.Uid == cell.EditSettingsUid);

            if (editSettings == null)
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

        public SheetRow GetRow(int r)
        {
            return _rows[r - 1];
        }

        public void AddRow(SheetRow baseRow, int position)
        {
            var baseRowNumber = RowNumber(baseRow);
            var baseRowIndex = baseRowNumber - 1;

            var newRow = new SheetRow();

            foreach (var column in _columns)
            {
                var newCell = new SheetCell()
                {
                    RowUid = newRow.Uid,
                    ColumnUid = column.Uid
                };

                _cells.Add(newCell);
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
        }

        public SheetColumn GetColumn(int c)
        {
            return _columns[c-1];
        }

        public void AddColumn(SheetColumn baseColumn, int position)
        {
            var baseColumnNumber = ColumnNumber(baseColumn);
            var baseColumnIndex = baseColumnNumber - 1;

            var newColumn = new SheetColumn();

            foreach (var row in _rows)
            {
                var newCell = new SheetCell()
                {
                    RowUid = row.Uid,
                    ColumnUid = newColumn.Uid
                };

                _cells.Add(newCell);
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
        }

        public void RemoveRow(SheetRow row)
        {
            if (row == null)
            {
                return;
            }

            var cellsToDelete = Cells.Where(x => x.RowUid == row.Uid).ToList();

            for (var i = cellsToDelete.Count() - 1; i >= 0; i--)
            {
                var cell = cellsToDelete[i];
                _cells.Remove(cell);
            }

            if (_rows.Remove(row))
            {
                RowsCount -= 1;
            }
        }

        public void RemoveColumn(SheetColumn column)
        {
            if (column == null)
            {
                return;
            }

            var cellsToDelete = Cells.Where(x => x.ColumnUid == column.Uid).ToList();
            for (var i = cellsToDelete.Count() - 1; i >= 0; i--)
            {
                var cell = cellsToDelete[i];
                _cells.Remove(cell);
            }

            if (_columns.Remove(column))
            {
                ColumnsCount -= 1;
            }
        }

        public void SetSettingsFromCommandPanel(List<SheetCell> cells, SheetCell cell, SheetCommandPanelModel commandPanelModel)
        {
            if (cells == null)
            {
                return;
            }

            if (commandPanelModel == null)
            {
                return;
            }

            if (cell != null)
            {
                cell.Formula = commandPanelModel.InputText;
            }

            var newStyle = new SheetCellStyle(commandPanelModel);
            SetStyle(cells, newStyle);
            
            var newEditSettings = new SheetCellEditSettings(commandPanelModel);
            if (!newEditSettings.IsStandard())
            {
                SetEditSettings(cells, newEditSettings);
            }

            FreezedRows = commandPanelModel.FreezedRows;
            FreezedColumns = commandPanelModel.FreezedColumns;
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
                UseVirtualization = UseVirtualization,
                RowsCount = RowsCount,
                ColumnsCount = ColumnsCount,
                FreezedColumns = FreezedColumns,
                FreezedRows = FreezedRows
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
                ChangeSize<SheetColumn>(columnsAddRemoveCount, Columns, RemoveColumn, AddColumn);

            if (rowsAddRemoveCount != 0)
                ChangeSize<SheetRow>(rowsAddRemoveCount, Rows, RemoveRow, AddRow);
        }

        private void ChangeSize<T>(int addRemoveCount, IReadOnlyCollection<T> collection, Action<T> removeAction, Action<T, int> addAction) where T : class
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
                        addAction(lastItem, 1);
                }
            }
        }

        private void Clear()
        {
            _rows.Clear();
            _columns.Clear();
            _cells.Clear();
            //_styles.Clear();
            //_editSettings.Clear();
        }
    }
}