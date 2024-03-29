﻿using System.Text.RegularExpressions;

namespace HubCloud.BlazorSheet.EvalEngine.Helpers
{
    public static class RegexHelper
    {
        public static readonly Regex AddressRegex
            = new Regex(@"R\[*-*\d*\]*C\[*-*\d*\]*", RegexOptions.Compiled);
        
        public static readonly Regex AddressRangeRegex 
            = new Regex(@"R\[*-*\d*\]*C\[*-*\d*\]*:R\[*-*\d*\]*C\[*-*\d*\]*", RegexOptions.Compiled);
        
        public static readonly Regex ExcelAddressRegex
            = new Regex(@"\$?[A-Z]+\$?\d+", RegexOptions.Compiled);
        
        public static readonly Regex ExcelAddressRangeRegex
            = new Regex(@"\$?[A-Z]+\$?\d+:\$?[A-Z]+\$?\d+", RegexOptions.Compiled);
    }
}