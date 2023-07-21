using System;
using System.Collections.Generic;
using HubCloud.BlazorSheet.Core.Interfaces;

namespace HubCloud.BlazorSheet.ExamplesShared.Infrastructure
{
    public class ItemsSourceProvider: IItemsSourceDataProvider
    {
        public IEnumerable<Tuple<string, string>> GetItems()
        {
            var collection = new List<Tuple<string, string>>();

            collection.Add(new Tuple<string, string>("department", "Department"));
            collection.Add(new Tuple<string, string>("product", "Product"));
            collection.Add(new Tuple<string, string>("person", "Person"));
        
            return collection;
        }
    }
}