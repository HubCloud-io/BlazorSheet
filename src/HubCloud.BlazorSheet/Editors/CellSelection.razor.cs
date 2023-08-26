using HubCloud.BlazorSheet.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace HubCloud.BlazorSheet.Editors;

public partial class CellSelection: ComponentBase
{

    private Guid _cellUid;
    private DomRect _domRect;
    
    [Parameter]
    public SheetCell Cell { get; set; }
    
    [Inject] 
    public IJSRuntime JsRuntime { get; set; }
    
    public string EditorStyle
    {
        get
        {
            var style = $"border: 1px solid blue;  position: fixed; width: " +
                        $"{_domRect.Width}px; height: {_domRect.Height}px; top: {_domRect.Top}px; left: {_domRect.Left}px;";
            return style;
        }
    }
    
    protected override async Task OnParametersSetAsync()
    {
        if (_cellUid != Cell.Uid)
        {
            try
            {
                _domRect = await JsRuntime.InvokeAsync<DomRect>("getElementCoordinates", $"cell_{Cell.Uid}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OnCellDblClick. Cannot get element coordinates. Message: {ex.Message}");
            }
        }
        
        _cellUid = Cell.Uid;
    }
}