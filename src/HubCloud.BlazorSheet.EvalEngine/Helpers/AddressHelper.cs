using System;
using System.Text;
using System.Text.RegularExpressions;

namespace HubCloud.BlazorSheet.EvalEngine.Helpers
{
    public class AddressHelper
    {
        private static Regex _addressRegex = new Regex(@"R-*\d*C-*\d*", RegexOptions.Compiled);
        
        public static string ConvertAddress(string excelAddress)
        {
            if (string.IsNullOrEmpty(excelAddress) || _addressRegex.IsMatch(excelAddress))
                return excelAddress;

            var resultAddress = new StringBuilder();
            var i = 0;
            var colAddress = new StringBuilder();
            while (i < excelAddress.Length && char.IsLetter(excelAddress[i]))
            {
                colAddress.Append(excelAddress[i++]);
            }

            if (colAddress.Length == 0 || 
                !int.TryParse(excelAddress.Substring(i, excelAddress.Length - i), out var rowAddress))
                return excelAddress;

            resultAddress.Append($"R{rowAddress}C{GetColumnNumber(colAddress.ToString())}");
            
            return resultAddress.ToString();
        }

        private static string GetColumnNumber(string columnName)
        {
            double result = 0;
            columnName = columnName.ToUpper();
            var currentNumber = columnName.Length - 1;
            for (var i = 0; i < columnName.Length; i++)
            {
                var currentCharVal = columnName[i] - 64;
                result += currentCharVal * Math.Pow(26, currentNumber--);
            }
            
            return result.ToString("#");
        }
    }
}