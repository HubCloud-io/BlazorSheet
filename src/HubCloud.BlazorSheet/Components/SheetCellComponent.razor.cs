using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.Editors;
using HubCloud.BlazorSheet.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace HubCloud.BlazorSheet.Components;

public partial class SheetCellComponent: ComponentBase
{
    [Parameter]
    public Sheet Sheet { get; set; }
    
    [Parameter]
    public SheetRow Row { get; set; }
    
    [Parameter]
    public SheetColumn Column { get; set; }
    
    [Parameter]
    public SheetCell Cell { get; set; }
    
    [Parameter]
    public bool IsHiddenCellsVisible { get; set; }
    
    [Parameter]
    public CellStyleBuilder StyleBuilder { get; set; }
    
    [Parameter]
    public EventCallback<CellEditInfo> StartEdit { get; set; }
    
    private async Task OnCellDblClick(MouseEventArgs e, SheetCell cell)
    {
        
        if (!cell.EditSettingsUid.HasValue)
        {
            return;
        }
        
        var row = Sheet.GetRow(cell.RowUid);
        var column = Sheet.GetColumn(cell.ColumnUid);
        var editSettings = Sheet.GetEditSettings(cell);

        var cellEditInfo = new CellEditInfo()
        {
            ClientX = e.ClientX,
            ClientY = e.ClientY,
            EditSettings = editSettings,
            Cell = cell,
            Row = row,
            Column = column
        };

        await StartEdit.InvokeAsync(cellEditInfo);
    }
    
    private string CellStyle(SheetRow row, SheetColumn column, SheetCell cell)
    {
        return StyleBuilder.GetCellStyle(Sheet, row, column, cell, IsHiddenCellsVisible);
    }
    
    private string GetHtmlSpacing(int indent)
    {
        var spacing = string.Empty;

        for (int i = 0; i < indent; i++)
            spacing += "\u00A0";

        return spacing;
    }
}