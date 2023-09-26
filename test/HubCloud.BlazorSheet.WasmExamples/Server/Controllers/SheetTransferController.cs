using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.WasmExamples.Shared.Helpers;
using HubCloud.BlazorSheet.WasmExamples.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace HubCloud.BlazorSheet.WasmExamples.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class SheetTransferController : Controller
{
    [HttpPost("Build")]
    public IActionResult Build([FromBody] SheetTransferCallDto callDto)
    {
        Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}: SheetTransfer.Back. Build started");
        
        var responseDto = new ResponseDto<Sheet>();
        
        var builder = new SheetWithCellNamesBuilder();
        var sheetSettings =  builder.BuildSettings(callDto.RowsCount, callDto.ColumnsCount);

        var sheet = new Sheet(sheetSettings);

        responseDto.Data = sheet;
        
        Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}: SheetTransfer.Back. Build finished");

        return Ok(responseDto);
    }
}