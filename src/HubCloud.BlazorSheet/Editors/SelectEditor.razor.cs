using BBComponents.Abstract;
using Microsoft.AspNetCore.Components;

namespace HubCloud.BlazorSheet.Editors;

public partial class SelectEditor: ComponentBase
{
    private List<Tuple<string, string>> _source = new List<Tuple<string, string>>();

    private string _value;
    
    [Parameter]
    public string Id { get; set; }
    
    /// <summary>
    /// Selected value.
    /// </summary>
    [Parameter]
    public int Value { get; set; } 
    
    [Parameter]
    public IComboBoxDataProvider<int> DataProvider { get; set; }
    
    [Parameter]
    public bool IsDisabled { get; set; }
    
    /// <summary>
    /// Event call back for value changed.
    /// </summary>
    [Parameter]
    public EventCallback<int> ValueChanged { get; set; }

    /// <summary>
    /// Duplicate event call back for value changed. 
    /// It is necessary to have possibility catch changed even whet we use @bind-Value.
    /// </summary>
    [Parameter]
    public EventCallback<int> Changed { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (DataProvider != null)
        {
            var collection = await DataProvider.GetCollectionAsync();
            _source.Add(new Tuple<string, string>("0", "===Not selected==="));
            foreach (var selectItem in collection)
            {
                _source.Add(new Tuple<string, string>(selectItem.Value.ToString(), selectItem.Text));
            }
        }
    }

    protected override void OnParametersSet()
    {
        _value = Value.ToString();
    }

    private async Task OnValueChanged(ChangeEventArgs e)
    {
        _value = e.Value?.ToString();

        if (int.TryParse(_value, out var intValue))
        {
            await ValueChanged.InvokeAsync(intValue);
            await Changed.InvokeAsync(intValue);
        }
        
    }
}