using HubCloud.BlazorSheet.Core.Models;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.WasmExamples.Client.Components;

public partial class SheetTestComponent: ComponentBase
{
    
    [Parameter]
    public Sheet Sheet { get; set; }
    
    protected override void OnAfterRender(bool firstRender)
    {
        Console.WriteLine($"Sheet test rendered: {DateTime.Now: yyyy-MM-dd:hh:mm:ss.fff}");
    }
}