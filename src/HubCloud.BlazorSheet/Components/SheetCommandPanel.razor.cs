using System.Text;
using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Models;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.Components;

public partial class SheetCommandPanel:ComponentBase
{
    private bool _isCellLinkInputModalOpen;

    private List<Tuple<string, string>> _textAlignSource;
    private List<Tuple<string, CellFormatTypes>> _cellFormatSource;
    private List<Tuple<CellBorderTypes, string>> _borderTypesSource;
    private List<Tuple<CellControlKinds, string>> _controlKindSource;

    [Parameter]
    public SheetCommandPanelModel Model { get; set; }
    
    [Parameter]
    public EventCallback Changed { get; set; }

    [Parameter]
    public EventCallback ExportClicked { get; set; }

    [Parameter]
    public EventCallback ImportClicked { get; set; }
    
    protected override void OnInitialized()
    {
        _textAlignSource = new List<Tuple<string, string>>();
        _textAlignSource.Add(new Tuple<string, string>("Left", "left"));
        _textAlignSource.Add(new Tuple<string, string>("Center", "center"));
        _textAlignSource.Add(new Tuple<string, string>("Right", "right"));

        _borderTypesSource = new List<Tuple<CellBorderTypes, string>>();
        _borderTypesSource.Add(new Tuple<CellBorderTypes, string>(CellBorderTypes.None, "None"));
        _borderTypesSource.Add(new Tuple<CellBorderTypes, string>(CellBorderTypes.Top, "Top"));
        _borderTypesSource.Add(new Tuple<CellBorderTypes, string>(CellBorderTypes.Left, "Left"));
        _borderTypesSource.Add(new Tuple<CellBorderTypes, string>(CellBorderTypes.Bottom, "Bottom"));
        _borderTypesSource.Add(new Tuple<CellBorderTypes, string>(CellBorderTypes.Right, "Right"));
        _borderTypesSource.Add(new Tuple<CellBorderTypes, string>(CellBorderTypes.All, "All"));

        _cellFormatSource = new List<Tuple<string, CellFormatTypes>>();
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>("No format", CellFormatTypes.None));
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>("Number 100", CellFormatTypes.Integer));
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>("Number 100.12", CellFormatTypes.IntegerTwoDecimalPlaces));
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>("Number 100.123", CellFormatTypes.IntegerThreeDecimalPlaces));
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>("Date", CellFormatTypes.Date));
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>("Date&Time", CellFormatTypes.DateTime));
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>("Custom", CellFormatTypes.Custom));

        _controlKindSource = new List<Tuple<CellControlKinds, string>>();
        _controlKindSource.Add(new Tuple<CellControlKinds, string>(CellControlKinds.Undefined, "No control"));
        _controlKindSource.Add(new Tuple<CellControlKinds, string>(CellControlKinds.TextInput, "Text input"));
        _controlKindSource.Add(new Tuple<CellControlKinds, string>(CellControlKinds.NumberInput, "Number input"));
        _controlKindSource.Add(new Tuple<CellControlKinds, string>(CellControlKinds.DateInput, "Date input"));
        _controlKindSource.Add(new Tuple<CellControlKinds, string>(CellControlKinds.DateTimeInput, "Date&Time input"));
    }

    protected override void OnParametersSet()
    {
    }

    private async Task OnBoldClick()
    {
        Model.IsBold = !Model.IsBold;
        await Changed.InvokeAsync(null);
    }
    
    private async Task OnItalicClick()
    {
        Model.IsItalic = !Model.IsItalic;
        await Changed.InvokeAsync(null);
    }

    private async Task OnColorChange(ChangeEventArgs e)
    {
        Model.Color = e.Value?.ToString();
        await Changed.InvokeAsync(null);
    }
    
    private async Task OnBackgroundColorChange(ChangeEventArgs e)
    {
        Model.BackgroundColor = e.Value?.ToString();
        await Changed.InvokeAsync(null);
    }

    private async Task OnBorderColorChange(ChangeEventArgs e)
    {
        Model.BorderColor = e.Value?.ToString();
        await Changed.InvokeAsync(null);
    }

    private async Task OnSettingsChanged()
    {
        await Changed.InvokeAsync(null);
    }
    
    private static string ToggleButtonStyle(bool flag)
    {
        var sb = new StringBuilder();

        sb.Append("btn btn-sm ");
        sb.Append(flag ? "btn-primary" : "btn-outline-primary");

        return sb.ToString();
    }

    private async Task OnExport()
    {
        await ExportClicked.InvokeAsync(null);
    }

    private async Task OnImport()
    {
        await ImportClicked.InvokeAsync(null);
    }

    private async Task OnCustomFormatInputChanged()
    {
        Model.CustomFormat = Model.CustomFormat.Trim();

        await Changed.InvokeAsync(null);
    }

    private async Task OnCellFormatChanged(CellFormatTypes formatType)
    {
        if (formatType == CellFormatTypes.Custom)
            return;

        Model.CustomFormat = string.Empty;
        await Changed.InvokeAsync(null);
    }

    private async void OnCellLinkInputModalClosed(CellLink cellLink)
    {
        _isCellLinkInputModalOpen = false;

        if (cellLink == null) 
            return;

        Model.TextLinkInputText = cellLink.Text;
        Model.LinkInputText = cellLink.Link;

        await Changed.InvokeAsync(null);
    }

    private void OpenCellLinkInputModal()
    {
        if (Model.ClientX != 0 && Model.ClientY != 0)
            _isCellLinkInputModalOpen = true;
        else
            _isCellLinkInputModalOpen = false;
    }
}