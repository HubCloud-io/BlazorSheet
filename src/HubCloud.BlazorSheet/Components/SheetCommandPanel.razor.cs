using System.Text;
using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Models;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.Components;

public partial class SheetCommandPanel:ComponentBase
{
    private List<Tuple<string, string>> _textAlignSource;
    private List<Tuple<string, CellFormatTypes>>? _cellFormatSource;
    private List<Tuple<CellBorderTypes, string>> _borderTypesSource;

    [Parameter]
    public SheetCommandPanelStyleModel StyleModel { get; set; }
    
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
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>("Нет формата", CellFormatTypes.None));
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>("Целое число", CellFormatTypes.F0));
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>("Число 2 знака после запятой", CellFormatTypes.F));
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>("Число 3 знака после запятой", CellFormatTypes.F3));
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>("Дата", CellFormatTypes.Date));
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>("Дата время", CellFormatTypes.DateTime));
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>("Произвольный", CellFormatTypes.Custom));
    }

    protected override void OnParametersSet()
    {
    }

    private async Task OnBoldClick()
    {
        StyleModel.IsBold = !StyleModel.IsBold;
        await Changed.InvokeAsync(null);
    }
    
    private async Task OnItalicClick()
    {
        StyleModel.IsItalic = !StyleModel.IsItalic;
        await Changed.InvokeAsync(null);
    }

    private async Task OnColorChange(ChangeEventArgs e)
    {
        StyleModel.Color = e.Value?.ToString();
        await Changed.InvokeAsync(null);
    }
    
    private async Task OnBackgroundColorChange(ChangeEventArgs e)
    {
        StyleModel.BackgroundColor = e.Value?.ToString();
        await Changed.InvokeAsync(null);
    }

    private async Task OnBorderColorChange(ChangeEventArgs e)
    {
        StyleModel.BorderColor = e.Value?.ToString();
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
        StyleModel.CustomFormat = StyleModel.CustomFormat.Trim();

        await Changed.InvokeAsync(null);
    }

    private async Task OnCellFormatChanged(CellFormatTypes formatType)
    {
        if (formatType == CellFormatTypes.Custom)
            return;

        StyleModel.CustomFormat = string.Empty;
        await Changed.InvokeAsync(null);
    }
}