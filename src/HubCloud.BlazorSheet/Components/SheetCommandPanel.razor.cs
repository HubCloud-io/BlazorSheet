using System.Text;
using HubCloud.BlazorSheet.Core.Consts;
using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Interfaces;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace HubCloud.BlazorSheet.Components;

public partial class SheetCommandPanel:ComponentBase
{
    private bool _isCollapseExpandAllRows;
    private bool _isCollapseExpandAllColumns;
    
    private bool _isEditSettingsOpen;
    private double _clientX;
    private double _clientY;

    private List<Tuple<string, string>> _textAlignSource;
    private List<Tuple<string, CellFormatTypes>> _cellFormatSource;
    private List<Tuple<CellBorderTypes, string>> _borderTypesSource;
    private List<Tuple<CellControlKinds, string>> _controlKindSource;
    private List<Tuple<int, string>> _dataTypeSource;

    [Parameter]
    public SheetCommandPanelModel Model { get; set; }
    
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
    public EventCallback UngroupColumns { get; set; }

    [Parameter]
    public EventCallback<bool> CollapseExpandAllRows { get; set; }

    [Parameter]
    public EventCallback<bool> CollapseExpandAllColumns { get; set; }
    
    [Parameter]
    public EventCallback<SheetCellEditSettings> EditSettingsChanged { get; set; }

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
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>($"Number: {CellFormatConsts.Integer}", CellFormatTypes.Integer));
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>($"Number: {CellFormatConsts.IntegerTwoDecimalPlaces}", CellFormatTypes.IntegerTwoDecimalPlaces));
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>($"Number: {CellFormatConsts.IntegerThreeDecimalPlaces}", CellFormatTypes.IntegerThreeDecimalPlaces));
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>($"Number: {CellFormatConsts.IntegerWithSpaces}", CellFormatTypes.IntegerWithSpaces));
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>($"Number: {CellFormatConsts.IntegerWithSpacesTwoDecimalPlaces}", CellFormatTypes.IntegerWithSpacesTwoDecimalPlaces));
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>($"Number: {CellFormatConsts.IntegerWithSpacesThreeDecimalPlaces}", CellFormatTypes.IntegerWithSpacesThreeDecimalPlaces));
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>($"Number: {CellFormatConsts.IntegerNegativeWithSpaces}", CellFormatTypes.IntegerNegativeWithSpaces));
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>($"Number: {CellFormatConsts.IntegerNegativeWithSpacesTwoDecimalPlaces}", CellFormatTypes.IntegerNegativeWithSpacesTwoDecimalPlaces));
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>($"Number: {CellFormatConsts.IntegerNegativeWithSpacesThreeDecimalPlaces}", CellFormatTypes.IntegerNegativeWithSpacesThreeDecimalPlaces));
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>($"Date: {CellFormatConsts.Date}", CellFormatTypes.Date));
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>($"Date&Time: {CellFormatConsts.DateTime}", CellFormatTypes.DateTime));
        _cellFormatSource.Add(new Tuple<string, CellFormatTypes>("Custom", CellFormatTypes.Custom));

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

    private async Task OnFreezedRowsChanged()
    {
        await FreezedRowsChanged.InvokeAsync(Model.FreezedRows);
    }

    private async Task OnFreezedColumnsChanged()
    {
        await FreezedColumnsChanged.InvokeAsync(Model.FreezedColumns);
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

    private async Task OnFormatChanged(CellFormatTypes formatType)
    {
        if (formatType == CellFormatTypes.Custom)
            return;

        Model.CustomFormat = string.Empty;
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

    private async void OnUngroupColumns()
    {
        await UngroupColumns.InvokeAsync();
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
}