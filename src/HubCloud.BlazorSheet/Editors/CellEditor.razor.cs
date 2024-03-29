﻿using System.Collections.Specialized;
using System.Globalization;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace HubCloud.BlazorSheet.Editors;

public partial class CellEditor : ComponentBase
{

    private JsCallService _jsCallService;
    
    [Parameter] public CellEditInfo CellEditInfo { get; set; }

    [Parameter] public bool IsDisabled { get; set; }

    [Parameter] public EventCallback<SheetCell> Changed { get; set; }
    
    [Parameter] public EventCallback<SheetCell> EditCancelled { get; set; }

    [Inject] public IJSRuntime JsRuntime { get; set; }

    public string InputId => $"input_{CellEditInfo.Cell.Uid}";

    public string EditorStyle
    {
        get
        {
            var height =(int)Math.Floor( CellEditInfo.DomRect.Height );
            var width = (int) Math.Floor(CellEditInfo.DomRect.Width);
            var topStr = CellEditInfo.DomRect.Top.ToString(CultureInfo.InvariantCulture);
            var leftStr = CellEditInfo.DomRect.Left.ToString(CultureInfo.InvariantCulture);
            
            var style = $"border: 1px solid blue;  position: fixed; z-index:100000; width: " +
                        $"{width}px; height: {height}px; top: {topStr}px; left: {leftStr}px;";
            
            return style;
        }
    }

    protected override void OnInitialized()
    {
        _jsCallService = new JsCallService(JsRuntime);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await _jsCallService.FocusElementAsync(InputId);
        }
    }

    public async Task OnChanged()
    {
        CellEditInfo.Cell.ApplyFormat();
        if (string.IsNullOrEmpty(CellEditInfo.Cell.Text))
        {
            CellEditInfo.Cell.Text = CellEditInfo.Cell.Value.ToString();
        }

        await Changed.InvokeAsync(CellEditInfo.Cell);
    }

    private async Task OnSelectChanged(SelectResultArgs args)
    {
        CellEditInfo.Cell.Text = args.Text;
        await Changed.InvokeAsync(CellEditInfo.Cell);
    }

    private async Task OnTextInputChanged()
    {
        CellEditInfo.Cell.Text = CellEditInfo.Cell.Value.ToString();
        await Changed.InvokeAsync(CellEditInfo.Cell);
    }

    private async Task OnEditCancelled()
    {
        await EditCancelled.InvokeAsync(CellEditInfo.Cell);
    }
}