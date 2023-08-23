using System;
using System.Collections.Generic;

namespace HubCloud.BlazorSheet.Core.Interfaces
{
    public interface IDataTypeDataProvider
    {
        IEnumerable<Tuple<int, string>> GetItems();
    }
}