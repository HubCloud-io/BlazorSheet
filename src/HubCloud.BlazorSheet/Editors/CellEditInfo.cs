using HubCloud.BlazorSheet.Core.Models;

namespace HubCloud.BlazorSheet.Editors;

public class CellEditInfo
{
    public DomRect DomRect { get; set; }
    public SheetCellEditSettings EditSettings { get; set; }
    public SheetCell Cell { get; set; }
    public SheetRow Row { get; set; }
    public SheetColumn Column { get; set; }
}