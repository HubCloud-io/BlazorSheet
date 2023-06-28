using BBComponents.Abstract;
using HubCloud.BlazorSheet.Infrastructure;

namespace HubCloud.BlazorSheet.ServerSideExamples.Infrastructure;

public class ComboBoxDataProviderFactory: IComboBoxDataProviderFactory
{
    public IComboBoxDataProvider<int> Create(string itemsSource)
    {
        var dataProvider = new ComboBoxDataProvider(itemsSource);

        return dataProvider;
    }
}