using System.Text;
using HubCloud.BlazorSheet.Core.Consts;
using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Interfaces;
using HubCloud.BlazorSheet.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace HubCloud.BlazorSheet.Components;

public partial class SheetCommandPanel:ComponentBase
{
    private const string TextAlignLeft = "left";
    private const string TextAlignJustify = "justify";
    private const string TextAlignRight = "right";

    private bool _isCollapseExpandAllRows;
    private bool _isCollapseExpandAllColumns;
    
    private bool _isEditSettingsOpen;
    private double _clientX;
    private double _clientY;

    private string _currentTextAlign;
    private CellBorderTypes _currentBorderType;
    private CellFormatTypes _currentFormatType;
    private int _currentBorderWidth;

    private List<Tuple<CellControlKinds, string>> _controlKindSource;
    private List<Tuple<int, string>> _dataTypeSource;

    [Parameter]
    public EventCallback Changed { get; set; }
    [Parameter]
    public EventCallback<int> FreezedRowsChanged { get; set; }
    [Parameter]
    public EventCallback<int> FreezedColumnsChanged { get; set; }

    [Parameter]
    public EventCallback FormatChanged { get; set; }

    [Parameter]
    public EventCallback FormulaChanged { get; set; }

    [Parameter]
    public EventCallback ExportClicked { get; set; }

    [Parameter]
    public EventCallback ImportClicked { get; set; }

    [Parameter]
    public EventCallback OpenCellLinkModal { get; set; }

    [Parameter]
    public EventCallback SplitJoinCells { get; set; }

    [Parameter]
    public EventCallback GroupRows { get; set; }

    [Parameter]
    public EventCallback GroupColumns { get; set; }

    [Parameter]
    public EventCallback UngroupRows { get; set; }

    [Parameter]
    public EventCallback UngroupAllRows { get; set; }

    [Parameter]
    public EventCallback UngroupColumns { get; set; }

    [Parameter]
    public EventCallback UngroupAllColumns { get; set; }

    [Parameter]
    public EventCallback<bool> CollapseExpandAllRows { get; set; }

    [Parameter]
    public EventCallback<bool> CollapseExpandAllColumns { get; set; }
    
    [Parameter]
    public EventCallback<SheetCellEditSettings> EditSettingsChanged { get; set; }

    [Parameter]
    public EventCallback RememberCellSettings { get; set; }

    [Parameter]
    public EventCallback ApplyRememberedCellSettings { get; set; }

    [Parameter]
    public SheetCommandPanelModel Model { get; set; }

    [Parameter]
    public bool CanCellsBeJoined { get; set; }

    [Parameter]
    public bool CanRowsBeGrouped { get; set; }

    [Parameter]
    public bool CanColumnsBeGrouped { get; set; }

    [Parameter]
    public bool CanRowsBeUngrouped { get; set; }

    [Parameter]
    public bool CanColumnsBeUngrouped { get; set; }

    [Parameter]
    public int SelectedCellsCount { get; set; }
    
    [Parameter]
    public IDataTypeDataProvider DataTypeDataProvider { get; set; }

    [Parameter]
    public int SelectedCellRowNumber { get; set; }

    [Parameter]
    public int SelectedCellColumnNumber { get; set; }

    protected override void OnInitialized()
    {
        if (DataTypeDataProvider != null)
            _dataTypeSource = DataTypeDataProvider.GetItems().ToList();

        _controlKindSource = new List<Tuple<CellControlKinds, string>>
        {
            new Tuple<CellControlKinds, string>(CellControlKinds.Undefined, "No control"),
            new Tuple<CellControlKinds, string>(CellControlKinds.TextInput, "Text input"),
            new Tuple<CellControlKinds, string>(CellControlKinds.NumberInput, "Number input"),
            new Tuple<CellControlKinds, string>(CellControlKinds.DateInput, "Date input"),
            new Tuple<CellControlKinds, string>(CellControlKinds.CheckBox, "Check box"),
            new Tuple<CellControlKinds, string>(CellControlKinds.Select, "Select")
        };
    }

    protected override void OnParametersSet()
    {
        if (Model == null)
            return;

        switch (Model.TextAlign)
        {
            case SheetConsts.TextAlignLeft:
                _currentTextAlign = TextAlignLeft;
                break;
            case SheetConsts.TextAlignCenter:
                _currentTextAlign = TextAlignJustify;
                break;
            case SheetConsts.TextAlignRight:
                _currentTextAlign = TextAlignRight;
                break;
            default:
                _currentTextAlign = TextAlignLeft;
                break;
        }

        _currentBorderType = Model.BorderType;
        _currentFormatType = Model.FormatType;

        if (Model.BorderWidth <= 1)
            _currentBorderWidth = 1;
        else if (Model.BorderWidth <= 3)
            _currentBorderWidth = 3;
        else if (Model.BorderWidth <= 5 || Model.BorderWidth > 5)
            _currentBorderWidth = 5;
        else
            _currentBorderWidth = 1;
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

    private async Task OnTextAlignChanged(string textAlign)
    {
        _currentTextAlign = textAlign;

        switch (textAlign)
        {
            case TextAlignLeft:
                Model.TextAlign = SheetConsts.TextAlignLeft;
                break;
            case TextAlignJustify:
                Model.TextAlign = SheetConsts.TextAlignCenter;
                break;
            case TextAlignRight:
                Model.TextAlign = SheetConsts.TextAlignRight;
                break;
            default:
                Model.TextAlign = SheetConsts.TextAlignLeft;
                break;
        }

        await Changed.InvokeAsync();
    }

    private async Task OnSetFreezedRows(int freezedRowsCount)
    {
        await FreezedRowsChanged.InvokeAsync(freezedRowsCount);
    }

    private async Task OnSetFreezedColumns(int freezedColumnsCount)
    {
        await FreezedColumnsChanged.InvokeAsync(freezedColumnsCount);
    }

    private string ToggleButtonStyle(bool flag)
    {
        var sb = new StringBuilder();

        sb.Append("btn ");
        sb.Append(flag ? "btn-primary" : "btn-outline-primary");

        return sb.ToString();
    }

    private string GetActiveTextAlign(string textAlign)
    {
        var active = string.Empty;

        if (_currentTextAlign == textAlign)
            active = "active";

        return active;
    }

    private string GetActiveBorderType(CellBorderTypes borderType)
    {
        var active = string.Empty;

        if (_currentBorderType == borderType)
            active = "active";

        return active;
    }

    private string GetActiveBorderWidth(int borderWidth)
    {
        var active = string.Empty;

        if (_currentBorderWidth == borderWidth)
            active = "active";

        return active;
    }

    private string GetActiveFormat(CellFormatTypes formatType)
    {
        var active = string.Empty;

        if (_currentFormatType == formatType)
            active = "active";

        return active;
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

        await Changed.InvokeAsync();
        await FormatChanged.InvokeAsync();
    }

    private async Task OnFormulaChanged()
    {
        await FormulaChanged.InvokeAsync();
    }

    private async void OnOpenCellLinkModal()
    {
        await OpenCellLinkModal.InvokeAsync();
    }

    private async void OnSplitJoinCells()
    {
        await SplitJoinCells.InvokeAsync();
    }

    private bool IsButtonSplitJoinDisabled()
    {
        if (SelectedCellsCount == 0) 
            return true;

        if (SelectedCellsCount == 1)
            return false;

        if (SelectedCellsCount > 1 && !CanCellsBeJoined)
            return true;

        return false;
    }

    private async void OnGroupRows()
    {
        await GroupRows.InvokeAsync();
    }

    private async void OnGroupColumns()
    {
        await GroupColumns.InvokeAsync();
    }

    private async void OnUngroupRows()
    {
        await UngroupRows.InvokeAsync();
    }

    private async void OnUngroupAllRows()
    {
        await UngroupAllRows.InvokeAsync();
    }

    private async void OnUngroupColumns()
    {
        await UngroupColumns.InvokeAsync();
    }

    private async void OnUngroupAllColumns()
    {
        await UngroupAllColumns.InvokeAsync();
    }

    private async void OnCollapseExpandAllRows()
    {
        _isCollapseExpandAllRows = !_isCollapseExpandAllRows;
        await CollapseExpandAllRows.InvokeAsync(_isCollapseExpandAllRows);
    }

    private async void OnCollapseExpandAllColumns()
    {
        _isCollapseExpandAllColumns = !_isCollapseExpandAllColumns;
        await CollapseExpandAllColumns.InvokeAsync(_isCollapseExpandAllColumns);
    }

    private void OnEditSettingsClick(MouseEventArgs e)
    {
        _isEditSettingsOpen = true;

        _clientX = e.ClientX;
        _clientY = e.ClientY;
    }

    private async Task OnEditParametersClosed(SheetCellEditSettings editSettings)
    {
        _isEditSettingsOpen = false;

        if (editSettings != null)
        {
            Model.EditSettings = editSettings;
            await EditSettingsChanged.InvokeAsync(Model.EditSettings);
        }
    }

    private async Task OnSetBorder(CellBorderTypes borderType)
    {
        _currentBorderType = borderType;
        Model.BorderType = borderType;

        await Changed.InvokeAsync();
    }

    private async Task OnSetBorderWidth(int borderWidth)
    {
        Model.BorderWidth = borderWidth;
        await Changed.InvokeAsync();
    }

    private async Task OnSetFormat(CellFormatTypes formatType)
    {
        _currentFormatType = formatType;
        Model.FormatType = formatType;

        if (formatType == CellFormatTypes.Custom)
            return;

        Model.CustomFormat = string.Empty;
        await FormatChanged.InvokeAsync();
    }

    private string GetSplitJoinItemLabel()
    {
        if (SelectedCellsCount > 1)
            return "Join";
        else
            return "Split";
    }
}