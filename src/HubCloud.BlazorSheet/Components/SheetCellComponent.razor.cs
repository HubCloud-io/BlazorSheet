using System.Security.Cryptography.X509Certificates;
using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.Editors;
using HubCloud.BlazorSheet.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace HubCloud.BlazorSheet.Components;

public partial class SheetCellComponent : ComponentBase
{
    private ElementReference _cellElement;

    [Inject] public IJSRuntime JsRuntime { get; set; }

    [Parameter] public Sheet Sheet { get; set; }

    [Parameter] public SheetRow Row { get; set; }

    [Parameter] public SheetColumn Column { get; set; }

    [Parameter] public SheetCell Cell { get; set; }

    [Parameter] public SheetRegimes Regime { get; set; }

    [Parameter] public bool IsHiddenCellsVisible { get; set; }

    [Parameter] public HashSet<Guid> SelectedIdentifiers { get; set; }

    [Parameter] public CellStyleBuilder StyleBuilder { get; set; }

    [Parameter] public EventCallback<SheetCell> StartEdit { get; set; }

    [Parameter] public EventCallback<SheetCell> Clicked { get; set; }

    public string Id => $"cell_{Cell.Uid}";

    protected override void OnAfterRender(bool firstRender)
    {
#if (DEBUG)
        Console.WriteLine($"{DateTime.Now:yyyy-MM-dd:hh:mm:ss.fff} - Cell {Cell.Value} Rendered.");
#endif
    }

    private async Task OnCellClick(MouseEventArgs e, SheetCell cell)
    {
        await Clicked.InvokeAsync(Cell);
    }

    private async Task OnCellDblClick(MouseEventArgs e, SheetCell cell)
    {
        await StartEdit.InvokeAsync(cell);
    }

    private string CellStyle(SheetRow row, SheetColumn column, SheetCell cell)
    {
        return StyleBuilder.GetCellStyle(Sheet, row, column, cell, IsHiddenCellsVisible);
    }

    public string CellClass(SheetCell cell)
    {
        var result = "hc-sheet-cell";

        if (cell.ValidationFailed)
            return result += " hc-sheet-cell__non-valid";

        if (SelectedIdentifiers.Contains(cell.Uid))
            return result += " hc-sheet-cell__active";

        return result;
    }

    private string GetHtmlSpacing(int indent)
    {
        var spacing = string.Empty;

        for (int i = 0; i < indent; i++)
            spacing += "\u00A0";

        return spacing;
    }

    private string ControlPresentation(SheetCellEditSettings editSettings)
    {
        string presentation;
        var controlKind = editSettings.ControlKind;

        switch (controlKind)
        {
            case CellControlKinds.TextInput:
                presentation = "T";
                break;
            case CellControlKinds.NumberInput:
                presentation = $"N:{editSettings.NumberDigits}";
                break;
            case CellControlKinds.DateInput:
                presentation = "D";
                break;
            case CellControlKinds.DateTimeInput:
                presentation = "D";
                break;
            case CellControlKinds.CheckBox:
                presentation = "Flag";
                break;
            default:
                presentation = controlKind.ToString();
                break;
        }

        return presentation;
    }
}