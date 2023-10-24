using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.WasmExamples.Shared.Helpers;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.WasmExamples.Client.Pages;

public partial class SheetViewPage: ComponentBase
{
    private int _rowsCount = 2000;
    private int _columnsCount = 20;
    private bool _isVirtualizationUsed = true;
    
    private Sheet _sheet;
    private SheetSettings _sheetSettings;
    
    protected override void OnAfterRender(bool firstRender)
    {
        Console.WriteLine($"{DateTime.Now:yyyy-MM-dd:hh:mm:ss.fff} - Sheet Rendered.");
    }

    private void OnRenderClick()
    {
        Console.WriteLine($"{DateTime.Now:yyyy-MM-dd:hh:mm:ss.fff} - Start sheet build.");

        var builder = new SheetWithCellNamesBuilder();
        _sheetSettings =  builder.BuildSettings(_rowsCount, _columnsCount);

        _sheet = new Sheet(_sheetSettings);
        
        Console.WriteLine($"{DateTime.Now:yyyy-MM-dd:hh:mm:ss.fff} - End sheet build.");
    }
    
}