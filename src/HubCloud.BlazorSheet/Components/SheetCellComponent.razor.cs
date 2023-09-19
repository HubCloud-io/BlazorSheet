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

    [Parameter] public EventCallback<CellEditInfo> StartEdit { get; set; }

    [Parameter] public EventCallback<SheetCell> Clicked { get; set; }

    public string Id => $"cell_{Cell.Uid}";

    private async Task OnCellClick(MouseEventArgs e, SheetCell cell)
    {
        await Clicked.InvokeAsync(Cell);
    }

    private async Task OnCellDblClick(MouseEventArgs e, SheetCell cell)
    {
        if (Regime == SheetRegimes.Design)
        {
            var jsCallService = new JsCallService(JsRuntime);
            var domRect = await jsCallService.GetElementCoordinates(Id);

            if (domRect == null)
                return;

            var editSettings = new SheetCellEditSettings()
            {
                ControlKind = CellControlKinds.TextInput,
                CellDataType = (int)CellDataTypes.String,
            };
            
            var cellEditInfo = new CellEditInfo()
            {
                DomRect = domRect,
                EditSettings = editSettings,
                Cell = cell,
            };

            await StartEdit.InvokeAsync(cellEditInfo);
        }
        else if (Regime == SheetRegimes.InputForm)
        {
            if (!cell.EditSettingsUid.HasValue)
            {
                return;
            }

            var jsCallService = new JsCallService(JsRuntime);
            var domRect = await jsCallService.GetElementCoordinates(Id);

            if (domRect == null)
                return;


            var editSettings = Sheet.GetEditSettings(cell);

            var cellEditInfo = new CellEditInfo()
            {
                DomRect = domRect,
                EditSettings = editSettings,
                Cell = cell,
            };

            if (cell.ValidationFailed)
                cell.ValidationFailed = false;
            
            await StartEdit.InvokeAsync(cellEditInfo);
        }
        
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

    private string ControlPresentation(CellControlKinds controlKind)
    {
        string presentation;

        switch (controlKind)
        {
           
            case CellControlKinds.TextInput:
                presentation = "Text";
                break;
            case CellControlKinds.NumberInput:
                presentation = "Number";
                break;
            case CellControlKinds.DateInput:
                presentation = "Date";
                break;
            case CellControlKinds.DateTimeInput:
                presentation = "DateTime";
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