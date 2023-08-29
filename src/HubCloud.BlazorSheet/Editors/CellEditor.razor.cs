using System.Collections.Specialized;
using HubCloud.BlazorSheet.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace HubCloud.BlazorSheet.Editors;

public partial class CellEditor : ComponentBase
{
    [Parameter] public CellEditInfo CellEditInfo { get; set; }

    [Parameter] public bool IsDisabled { get; set; }

    [Parameter] public EventCallback<SheetCell> Changed { get; set; }
    
    [Parameter] public EventCallback<SheetCell> EditCancelled { get; set; }

    [Inject] public IJSRuntime JsRuntime { get; set; }

    public string InputId => $"input_{CellEditInfo.Cell.Uid}";

    public string EditorStyle
    {
        get
        {
            var style = $"border: 1px solid blue;  position: fixed; width: " +
                        $"{CellEditInfo.DomRect.Width}px; height: {CellEditInfo.DomRect.Height}px; top: {CellEditInfo.DomRect.Top}px; left: {CellEditInfo.DomRect.Left}px;";
            return style;
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                await JsRuntime.InvokeVoidAsync("focusElement", InputId);
            }
            catch(Exception e)
            {
                Console.WriteLine($"OnAfterRenderAsync. Cannot focus element. Message: {e.Message}");
            }
        }
    }

    public async Task OnChanged()
    {
        CellEditInfo.Cell.ApplyFormat();
        if (string.IsNullOrEmpty(CellEditInfo.Cell.Text))
        {
            CellEditInfo.Cell.Text = CellEditInfo.Cell.Value.ToString();
        }

        await Changed.InvokeAsync(CellEditInfo.Cell);
    }

    private async Task OnSelectChanged(SelectResultArgs args)
    {
        CellEditInfo.Cell.Text = args.Text;
        await Changed.InvokeAsync(CellEditInfo.Cell);
    }

    private async Task OnEditCancelled()
    {
        await EditCancelled.InvokeAsync(CellEditInfo.Cell);
    }
}