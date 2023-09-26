using Company.WebApplication1.DataProviders;
using Company.WebApplication1.Models;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.WasmExamples.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.WasmExamples.Client.Pages;

public partial class SheetTransferPage: ComponentBase
{
    private int _rowsCount = 1000;
    private int _columnsCount = 4;
    private bool _isWaiting = false;
    
    private Sheet _sheet;
 

    [Inject]
    public HttpClient Http { get; set; }

    protected override void OnAfterRender(bool firstRender)
    {
        Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}: Sheet rendered");
    }

    private async Task OnGetDataClick()
    {
        _isWaiting = true;
        Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}: Start transfer");
        
        var provider = new SheetTransferDataProvider(Http);

        var callDto = new SheetTransferCallDto() {RowsCount = _rowsCount, ColumnsCount = _columnsCount};
        var responseDto = await provider.BuildAsync(callDto);

        Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}: End transfer");
        
        _sheet = responseDto.Data;
        _sheet.InitLookUp();
        _sheet.PrepareCellText();
        
        Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}: Sheet prepared");
        
        _isWaiting = false;
    }

    private async Task OnGetDataDTOClick()
    {
        _isWaiting = true;
        Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}: Start transfer");
        
        var provider = new SheetTransferDataProvider(Http);

        var callDto = new SheetTransferCallDto() {RowsCount = _rowsCount, ColumnsCount = _columnsCount};
        var responseDto = await provider.BuildByDtoAsync(callDto);

        Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}: End transfer");
        
        _sheet = new Sheet(responseDto.Data);
        
        Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}: Sheet prepared");
        
        _isWaiting = false;
    }
}