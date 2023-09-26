using System;
using HubCloud.BlazorSheet.Core.Models;
using Newtonsoft.Json;

namespace HubCloud.BlazorSheet.Core.Dto
{
    public class SheetCellDto
    {
        // Uid
        public Guid Uid { get; set; } = Guid.NewGuid();
        // RowUid
        public Guid RUid { get; set; }
        // ColumnUid
        public Guid CUid { get; set; }
        
        // StyleUid
        public Guid? SUid { get; set; }
        // EditSettingsUid
        public Guid? EUid { get; set; }
        // Name
        public string N { get; set; }
        // Link
        public string Lk { get; set; }
        
        // Colspan
        public int Csp { get; set; } = 1;
        // Rowspan
        public int Rsp { get; set; } = 1;
        // HiddenByJoin
        public bool H { get; set; }
        // Locked
        public bool Ld { get; set; } = true;
        // Indent
        public int I { get; set; }
        
        // Value
        public object V { get; set; }
        // Formula
        public string F { get; set; }
        // Format
        public string Ft { get; set; } = string.Empty;

        public SheetCellDto()
        {
            
        }

        public SheetCellDto(SheetCell cell)
        {
            Uid = cell.Uid;
            RUid = cell.RowUid;
            CUid = cell.ColumnUid;

            SUid = cell.StyleUid;
            EUid = cell.EditSettingsUid;

            N = cell.Name;
            Lk = cell.Link;

            Csp = cell.Colspan;
            Rsp = cell.Rowspan;
            H = cell.HiddenByJoin;
            Ld = cell.Locked;
            I = cell.Indent;
            
            V = cell.Value;
            F = cell.Formula;
            Ft = cell.Format;
        }

        public SheetCell BuildCell()
        {
            var cell = new SheetCell()
            {
                Uid = this.Uid,
                RowUid = this.RUid,
                ColumnUid = this.CUid,
                
                StyleUid = this.SUid,
                EditSettingsUid = this.EUid,
                
                Name = this.N,
                Link = this.Lk,
                
                Colspan = this.Csp,
                Rowspan = this.Rsp,
                HiddenByJoin = this.H,
                
                Locked = this.Ld,
                Indent = this.I,
                
                Value = this.V,
                Formula = this.F,
                Format = this.Ft

            };

            return cell;
        }
     
       
    }
}