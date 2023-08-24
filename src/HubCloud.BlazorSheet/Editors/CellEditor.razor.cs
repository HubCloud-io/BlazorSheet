using System.Collections.Specialized;
using HubCloud.BlazorSheet.Core.Models;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.Editors;

public partial class CellEditor: ComponentBase
{
    [Parameter]
    public CellEditInfo CellEditInfo { get; set; }
    
    [Parameter]
    public bool IsDisabled { get; set; }
    
    [Parameter]
    public EventCallback<SheetCell> Changed { get; set; }
    
    public string EditorStyle
    {
        get
        {
            var style = $"border: 1px solid blue;  position: fixed; width: " +
                        $"{CellEditInfo.DomRect.Width}px; height: {CellEditInfo.DomRect.Height}px; top: {CellEditInfo.DomRect.Top}px; left: {CellEditInfo.DomRect.Left}px;";
            return style;
        }
    }

    public async Task OnChanged()
    {
        await Changed.InvokeAsync(CellEditInfo.Cell);
    }
}