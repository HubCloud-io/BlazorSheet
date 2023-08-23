using BBComponents.Abstract;

namespace HubCloud.BlazorSheet.Infrastructure;

public interface IComboBoxDataProviderFactory
{
    IComboBoxDataProvider<int> Create(int cellDataType);
}