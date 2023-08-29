using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.Infrastructure;
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
            
            var jsCallService = new JsCallService(JsRuntime);
            var domRect = await jsCallService.GetElementCoordinates($"cell_{Cell.Uid}");
         
        }
        
        _cellUid = Cell.Uid;
    }
}