using HubCloud.BlazorSheet.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HubCloud.BlazorSheet.Core.Interfaces
{
    public interface IDateTimeProperties
    {
        UniversalValue Day { get; }
        UniversalValue Hour { get; }
        UniversalValue Month { get; }
        UniversalValue Minute { get; }
        UniversalValue Second { get; }
        UniversalValue Year { get; }
    }
}
