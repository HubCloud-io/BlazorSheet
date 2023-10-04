using BBComponents.Abstract;
using HubCloud.BlazorSheet.Infrastructure;

namespace HubCloud.BlazorSheet.WasmExamples.Client.DataProviders;

public class ComboBoxDataProviderFactory: IComboBoxDataProviderFactory
{
    public IComboBoxDataProvider<int> Create(int dataType, string itemsSource)
    {
        var dataProvider = new ComboBoxDataProvider(dataType);

        return dataProvider;
    }
}