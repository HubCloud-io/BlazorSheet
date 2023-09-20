﻿using System.Text.RegularExpressions;
using HubCloud.BlazorSheet.Core.Models;

namespace HubCloud.BlazorSheet.Infrastructure;

public class ItemsSourceParametersHelper
{
    private readonly string _itemsSource;
    private readonly Sheet _sheet;
    
    public ItemsSourceParametersHelper(Sheet sheet, string itemsSource)
    {
        _itemsSource = itemsSource;
        _sheet = sheet;
    }

    public string Execute()
    {
        var result = _itemsSource;
        
        var parameters = Parse(_itemsSource);

        foreach (var parameter in parameters)
        {
            var address = new SheetCellAddress(parameter);
            var cell =  _sheet.GetCell(address);

            if (cell != null)
            {
                result = result.Replace( $"&{parameter}", cell.Value.ToString());
            }
        }

        return result;
    }
    
    public List<string> Parse(string input)
    {
        var parameters = new List<string>();
        var pattern = @"&\w+";

        // Use Regex.Match to find the first match in the input string
        var match = Regex.Match(input, pattern);
        
        if (match.Success)
        {
            // Extract and print the matched string (without the leading &)
            var extractedString = match.Value.Substring(1);
            parameters.Add(extractedString);
        }
       

        return parameters;
    }
}