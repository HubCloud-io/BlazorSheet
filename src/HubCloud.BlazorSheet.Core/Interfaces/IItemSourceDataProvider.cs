using System;
using System.Collections.Generic;

namespace HubCloud.BlazorSheet.Core.Interfaces
{
    public interface IItemsSourceDataProvider
    {
        IEnumerable<Tuple<string, string>> GetItems();
    }
}