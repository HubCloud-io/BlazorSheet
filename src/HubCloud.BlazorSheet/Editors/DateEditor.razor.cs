using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace HubCloud.BlazorSheet.Editors;

public partial class DateEditor : ComponentBase
{
    private string _value;
    private bool _wasInit;
   
    [Parameter]
    public string Id { get; set; }
    
    [Parameter]
    public DateTime Value { get; set; } 
    
    [Parameter]
    public bool IsDisabled { get; set; }
    
    [Parameter]
    public EventCallback<DateTime> ValueChanged { get; set; }
    
    [Parameter]
    public EventCallback<DateTime> Changed { get; set; }
    
    [Parameter]
    public EventCallback EditCancelled { get; set; }

    protected override void OnParametersSet()
    {
        
        if (!_wasInit && Value == DateTime.MinValue)
        {
            _value = "";
        }
        else
        {
            _value = Value.ToString("yyyy'-'MM'-'dd");
        }

        _wasInit = true;
    }

    private void OnValueChange(ChangeEventArgs e)
    {
        _value = e.Value?.ToString();
    }

    private async Task OnInputKeyDown(KeyboardEventArgs e)
    {
        if (e.Key.ToUpper() == "ESCAPE")
            await EditCancelled.InvokeAsync(null);
        if (e.Key.ToUpper() == "ENTER")
            await InputValueFinished();
    }

    private async Task OnFocusOut()
    {
        await InputValueFinished();
    }

    private async Task InputValueFinished()
    {
        if (DateTime.TryParse(_value, out var dateTimeValue))
        {
            await ValueChanged.InvokeAsync(dateTimeValue);
            await Changed.InvokeAsync(dateTimeValue);
        }
    }
}