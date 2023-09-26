using System;
using System.Collections.Generic;
using HubCloud.BlazorSheet.Core.Models;

namespace HubCloud.BlazorSheet.Core.Dto
{
    public class SheetDto
    {
        public Guid Uid { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public bool UseVirtualization { get; set; }
        public bool IsProtected { get; set; }
        
        public int RowsCount { get; set; }
        public int ColumnsCount { get; set; }

        public int FreezedRows { get; set; }
        public int FreezedColumns { get; set; }
        
        public List<SheetRow> Rows { get; set; } = new List<SheetRow>();
        public  List<SheetColumn> Columns { get; set; } = new List<SheetColumn>();
        public  List<SheetCellDto> Cells { get; set; } = new List<SheetCellDto>();
      
        public  List<SheetCellStyle> Styles { get; set; } = new List<SheetCellStyle>();
        public  List<SheetCellEditSettings> EditSettings { get; set; } = new List<SheetCellEditSettings>();

        public SheetDto()
        {
            
        }

        public SheetDto(Sheet sheet)
        {
            Uid = sheet.Uid;
            Name = sheet.Name;
            UseVirtualization = sheet.UseVirtualization;
            IsProtected = sheet.IsProtected;

            RowsCount = sheet.RowsCount;
            ColumnsCount = sheet.ColumnsCount;

            FreezedRows = sheet.FreezedRows;
            FreezedColumns = sheet.FreezedColumns;
            
            Rows.AddRange(sheet.Rows);
            Columns.AddRange(sheet.Columns);
            
            Styles.AddRange(sheet.Styles);
            EditSettings.AddRange(sheet.EditSettings);

            foreach (var cell in sheet.Cells)
            {
                var cellDto = new SheetCellDto(cell);
                Cells.Add(cellDto);
            }
        }

        
    }
}