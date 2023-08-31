using HubCloud.BlazorSheet.Editors;
using Microsoft.JSInterop;

namespace HubCloud.BlazorSheet.Infrastructure;

public class JsCallService
{
    private readonly IJSRuntime _jsRuntime;

    public JsCallService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task FocusElementAsync(string elementId)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("blazorSheet.focusElement", elementId);
        }
        catch(Exception e)
        {
            Console.WriteLine($"blazorSheet. Cannot focus element. Message: {e.Message}");
        }
    }

    public async Task<DomRect> GetElementCoordinates(string elementId)
    {
        DomRect domRect = null;
        try
        {
            domRect = await _jsRuntime.InvokeAsync<DomRect>("blazorSheet.getElementCoordinates", elementId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"BlazorSheet. Cannot get element coordinates. Message: {ex.Message}");
        }

        return domRect;
    }

    public async Task DisableArrowScroll()
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("blazorSheet.disableArrowScroll");
        }
        catch (Exception e)
        {
            Console.WriteLine($"BlazorSheet.DisableArrowScroll. Message: {e.Message}");
        }
    }
    
}