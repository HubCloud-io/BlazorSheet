using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.ExamplesShared.WorkbookBuilders;
using HubCloud.BlazorSheet.Infrastructure;
using HubCloud.BlazorSheet.ServerSideExamples.Infrastructure;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.ServerSideExamples.Pages;

public partial class SheetFreezedColumnsPage: ComponentBase
{
    private Sheet _sheet;
    private IComboBoxDataProviderFactory _dataProviderFactory;

    protected override void OnInitialized()
    {
        _dataProviderFactory = new ComboBoxDataProviderFactory();
        
        var builder = new WorkbookFreezedAreaBuilder();
        var workbook = builder.Build();

        _sheet = workbook.FirstSheet;
    }
}