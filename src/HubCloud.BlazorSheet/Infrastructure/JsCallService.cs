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
    
}