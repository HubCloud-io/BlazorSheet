using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.ExamplesShared.WorkbookBuilders;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.WasmExamples.Client.Pages;

public partial class SheetFreezedColumnsPage: ComponentBase
{
    private Sheet _sheet;

    protected override void OnInitialized()
    {
        var builder = new WorkbookFreezedAreaBuilder();
        var workbook = builder.Build();

        _sheet = workbook.FirstSheet;
    }
}