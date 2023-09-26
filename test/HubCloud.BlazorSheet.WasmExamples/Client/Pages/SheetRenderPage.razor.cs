using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.WasmExamples.Shared.Helpers;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.WasmExamples.Client.Pages;

public partial class SheetRenderPage: ComponentBase
{
    private int _rowsCount = 1000;
    private int _columnsCount = 4;
    
    private Sheet _sheet;
    private SheetSettings _sheetSettings;

    protected override void OnAfterRender(bool firstRender)
    {
        Console.WriteLine($"Rendered: {DateTime.Now:yyyy-MM-dd:hh:mm:ss.fff}");
    }

    private void OnRenderClick()
    {
        Console.WriteLine($"Start sheet build: {DateTime.Now:yyyy-MM-dd:hh:mm:ss.fff}");

        var builder = new SheetWithCellNamesBuilder();
        _sheetSettings =  builder.BuildSettings(_rowsCount, _columnsCount);

        _sheet = new Sheet(_sheetSettings);
        
        Console.WriteLine($"End sheet build: {DateTime.Now:yyyy-MM-dd:hh:mm:ss.fff}");
    }
    
    
    
}