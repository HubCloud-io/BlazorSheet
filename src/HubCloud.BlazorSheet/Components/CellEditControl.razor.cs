using BBComponents.Abstract;
using HubCloud.BlazorSheet.Core.Models;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.Components;

public partial class CellEditControl: ComponentBase
{
    private const int MinDropdownWidth = 200;
    
    [Parameter]
    public SheetCellEditSettings EditSettings { get; set; }
    
    [Parameter]
    public SheetCell Cell { get; set; }
    
    [Parameter]
    public bool IsDisabled { get; set; }
    
    [Parameter]
    public int ColumnWidth { get; set; }
    
    [Parameter]
    public IComboBoxDataProvider<int> ComboBoxDataProvider { get; set; }

    [Parameter]
    public EventCallback<SheetCell> Changed { get; set; }

    public int DropdownWidth => ColumnWidth < MinDropdownWidth ? MinDropdownWidth : ColumnWidth;

    public async Task OnChanged()
    {
        await Changed.InvokeAsync(Cell);
    }
}