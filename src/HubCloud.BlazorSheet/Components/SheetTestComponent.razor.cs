using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.Infrastructure;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.Components;

public partial class SheetTestComponent: ComponentBase
{

    private const int LeftSideCellWidth = 40;
    private const int TopSideCellHeight = 30;
    private const string CellHiddenBackground = "#cccccc";
    
    private bool _isHiddenCellsVisible;
    private HashSet<Guid> _selectedIdentifiers = new HashSet<Guid>();
    private CellStyleBuilder _cellStyleBuilder;
    
    [Parameter]
    public Sheet Sheet { get; set; }
    [Parameter] public SheetRegimes Regime { get; set; }

    [Parameter] public string MaxHeight { get; set; }
    [Parameter] public string MaxWidth { get; set; }
    
    public string TableId => $"table_{Sheet.Uid}";

    protected override void OnInitialized()
    {
        _cellStyleBuilder = new CellStyleBuilder
        {
            LeftSideCellWidth = LeftSideCellWidth,
            TopSideCellHeight = TopSideCellHeight
        };
    }

    protected override void OnAfterRender(bool firstRender)
    {
        Console.WriteLine($"Sheet test rendered: {DateTime.Now: yyyy-MM-dd:hh:mm:ss.fff}");
    }
    
    private bool ShouldCellBeDisplayed(SheetColumn column, SheetRow row, SheetCell cell)
    {
        return ((column.IsHidden || row.IsHidden) && !_isHiddenCellsVisible) || cell.HiddenByJoin || row.IsCollapsed ||
               column.IsCollapsed;
    }

    private void OnCellClicked()
    {
        
    }

    private void OnCellStartEdit()
    {
        
    }
}