using HubCloud.BlazorSheet.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HubCloud.BlazorSheet.Core.Interfaces
{
    public interface IStringMethods
    {
        UniversalValue Substring(int startIndex);
        UniversalValue Substring(int startIndex, int length);
        UniversalValue ToUpper();
        UniversalValue ToLower();
        UniversalValue IndexOf(string value);
        UniversalValue Replace(string oldValue, string newValue);
    }
}
