using System.Text;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.Components;

public partial class ValueInputModal: ComponentBase
{
    private double _value;
    
    [Parameter]
    public string Title { get; set; }
    
    /// <summary>
    /// X position of mouse click.
    /// </summary>
    [Parameter]
    public double ClientX { get; set; }

    /// <summary>
    /// Y position of mouse click.
    /// </summary>
    [Parameter]
    public double ClientY { get; set; }

    [Parameter] public int Width { get; set; } = 200;
    
    [Parameter]
    public double Value { get; set; }
    
    [Parameter]
    public EventCallback Closed { get; set; }

    protected override void OnParametersSet()
    {
        _value = Value;
    }
    private async Task OnOkClick()
    {
        await Closed.InvokeAsync(_value);
    }

    private async Task OnCancelClick()
    {
        await Closed.InvokeAsync(null);
    }
    
    private string ComponentStyle()
    {
        var sb = new StringBuilder();

        sb.Append("position: fixed; ");
        sb.Append("background-color: white; ");
        sb.Append("z-index: 1000; ");
        sb.Append("padding: 3px; ");
        sb.Append("border: 1px solid #b8b8b8; ");
        sb.Append($"width: {Width}px; ");
        
        var topPx = $"{(int)ClientY}px";
        var leftPx = $"{(int)ClientX}px";

        sb.Append($"top: {topPx}; ");
        sb.Append($"left: {leftPx}; ");

        return sb.ToString();
    }
}