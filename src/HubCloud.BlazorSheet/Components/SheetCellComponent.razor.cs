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
    private int _prevIndent = 0;
    private int _prevColspan = 0;
    private int _prevRowspan = 0;
    private bool _prevRowIsHidden = false;
    private bool _prevColumnIsHidden = false;
    private string _prevText = string.Empty;
    private string _prevRowHeight = string.Empty;
    private string _prevStringValue = string.Empty;
    private string _prevColumnWidth = string.Empty;
    private string _prevCellClass = string.Empty;
    private string _prevCellStyle = string.Empty;

    private string _currentCellClass = string.Empty;
    private string _currentCellStyle = string.Empty;
    
    private bool _shouldRender;
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

    protected override bool ShouldRender() => _shouldRender;

    protected override void OnParametersSet()
    {
        _currentCellClass = CellClass();
        _currentCellStyle = CellStyle();

        if (Regime == SheetRegimes.InputForm && 
            Cell != null && 
            Column != null &&
            Row != null)
        {
            _shouldRender = Cell.Text != _prevText ||
                            Cell.StringValue != _prevStringValue ||
                            Cell.Indent != _prevIndent ||
                            Cell.Colspan != _prevColspan ||
                            Cell.Rowspan != _prevRowspan ||
                            Column.Width != _prevColumnWidth ||
                            Column.IsHidden != _prevColumnIsHidden ||
                            Row.Height != _prevRowHeight ||
                            Row.IsHidden != _prevRowIsHidden ||
                            _currentCellClass != _prevCellClass ||
                            _currentCellStyle != _prevCellStyle;

            _prevIndent = Cell.Indent;
            _prevColspan = Cell.Colspan;
            _prevRowspan = Cell.Rowspan;
            _prevText = Cell.Text;
            _prevStringValue = Cell.StringValue;
            _prevColumnIsHidden = Column.IsHidden;
            _prevColumnWidth = Column.Width;
            _prevRowIsHidden = Row.IsHidden;
            _prevRowHeight = Row.Height;
            _prevCellClass = _currentCellClass;
            _prevCellStyle = _currentCellStyle;

            _prevText = Cell?.Text;
        }
        else
            _shouldRender = true;
    }

    protected override void OnAfterRender(bool firstRender)
    {
#if (DEBUG)
        Console.WriteLine($"{DateTime.Now:yyyy-MM-dd:hh:mm:ss.fff} - Cell {Cell.Value} Rendered.");
#endif
    }

    private async Task OnCellClick(MouseEventArgs e)
    {
        await Clicked.InvokeAsync(Cell);
    }

    private async Task OnCellDblClick(MouseEventArgs e)
    {
        await StartEdit.InvokeAsync(Cell);
    }

    private string CellStyle()
    {
        return StyleBuilder.GetCellStyle(Sheet, Row, Column, Cell, IsHiddenCellsVisible);
    }

    public string CellClass()
    {
        var result = "hc-sheet-cell";

        if (Cell.ValidationFailed)
            return result += " hc-sheet-cell__non-valid";

        if (SelectedIdentifiers.Contains(Cell.Uid))
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