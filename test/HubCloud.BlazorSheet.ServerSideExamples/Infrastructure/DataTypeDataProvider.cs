using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Interfaces;

namespace HubCloud.BlazorSheet.ServerSideExamples.Infrastructure;

public class DataTypeDataProvider: IDataTypeDataProvider
{
    public IEnumerable<Tuple<int, string>> GetItems()
    {
        var collection = new List<Tuple<int, string>>();

        collection.Add(new Tuple<int, string>((int)CellDataTypes.Undefined, "Undefined"));
        collection.Add(new Tuple<int, string>((int)CellDataTypes.String, "String"));
        collection.Add(new Tuple<int, string>((int)CellDataTypes.Number, "Number"));
        collection.Add(new Tuple<int, string>((int)CellDataTypes.DateTime, "DateTime"));
        collection.Add(new Tuple<int, string>((int)CellDataTypes.Bool, "Bool"));
        collection.Add(new Tuple<int, string>(23, "Department"));
        collection.Add(new Tuple<int, string>(26, "Product"));
        collection.Add(new Tuple<int, string>(27, "Person"));
        
        return collection;
    }
}