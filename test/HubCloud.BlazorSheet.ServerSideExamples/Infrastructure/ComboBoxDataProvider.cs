using BBComponents.Abstract;
using BBComponents.Models;

namespace HubCloud.BlazorSheet.ServerSideExamples.Infrastructure;

public class ComboBoxDataProvider:IComboBoxDataProvider<int>
{
    private int _itemsCount = 100;
    private List<SelectItem<int>> _collection;
    private int _dataType;

    public ComboBoxDataProvider(int dataType)
    {
        _dataType = dataType;
        Init();
    }
    
    public async Task<List<SelectItem<int>>> GetCollectionAsync()
    {
        var collection =  await Task.Run(() => _collection);

        return collection;
    }

    public async Task<SelectItem<int>> GetItemAsync(int key)
    {
        var item = await Task.Run(() => _collection.FirstOrDefault(x => x.Value == key));

        return item;
    }
    
    private void Init()
    {
        _collection = new List<SelectItem<int>>();

        for (var i = 1; i <= _itemsCount; i++)
        {
            var item = new SelectItem<int>();
            item.Value = i;
            item.Text = $"Item {_dataType} {i}";
            
            _collection.Add(item);
        }
    }
}