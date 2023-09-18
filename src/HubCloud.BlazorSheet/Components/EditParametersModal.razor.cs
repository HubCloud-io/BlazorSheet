using System.Text;
using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Interfaces;
using HubCloud.BlazorSheet.Core.Models;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.Components;

public partial class EditParametersModal: ComponentBase
{

    private SheetCellEditSettings _editSettings;
    
    private List<Tuple<CellControlKinds, string>> _controlKindSource;
    private List<Tuple<int, string>> _dataTypeSource;
    
    [Parameter]
    public SheetCellEditSettings EditSettings { get; set; }
    
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
    public IDataTypeDataProvider DataTypeDataProvider { get; set; }

    
    [Parameter]
    public EventCallback<SheetCellEditSettings> Closed { get; set; }

    protected override void OnInitialized()
    {
        if (DataTypeDataProvider != null)
        {
            _dataTypeSource = DataTypeDataProvider.GetItems().ToList();
        }
        
        _controlKindSource = new List<Tuple<CellControlKinds, string>>();
        _controlKindSource.Add(new Tuple<CellControlKinds, string>(CellControlKinds.Undefined, "No control"));
        _controlKindSource.Add(new Tuple<CellControlKinds, string>(CellControlKinds.TextInput, "Text input"));
        _controlKindSource.Add(new Tuple<CellControlKinds, string>(CellControlKinds.NumberInput, "Number input"));
        _controlKindSource.Add(new Tuple<CellControlKinds, string>(CellControlKinds.DateInput, "Date input"));
        _controlKindSource.Add(new Tuple<CellControlKinds, string>(CellControlKinds.CheckBox, "Check box"));
        _controlKindSource.Add(new Tuple<CellControlKinds, string>(CellControlKinds.Select, "Select"));
    }

    protected override void OnParametersSet()
    {
        _editSettings = EditSettings.ConcreteClone();
    }

    private void OnControlKindChanged()
    {
        _editSettings.CellDataType = SheetCellEditSettings.GetDataType(_editSettings.ControlKind);
    }
    
    private async Task OnOkClick()
    {
        await Closed.InvokeAsync(_editSettings);
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