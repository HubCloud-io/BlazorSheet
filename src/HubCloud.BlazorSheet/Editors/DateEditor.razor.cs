using HubCloud.BlazorSheet.Core.Models;
using Microsoft.AspNetCore.Components;
using ExpressoFunctions.FunctionLibrary;
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

    private async Task OnValueChange(ChangeEventArgs e)
    {

        var strValue = e.Value?.ToString();
        
        if (DateTime.TryParse(strValue, out var dateTimeValue))
        {
            _value = strValue;

            await ValueChanged.InvokeAsync(dateTimeValue);
            await Changed.InvokeAsync(dateTimeValue);
        }
    }

    private async Task OnInputKeyDown(KeyboardEventArgs e)
    {
        await EditCancelled.InvokeAsync(null);
    }
}