using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HubCloud.BlazorSheet.EvalEngine.Helpers
{
    public class AddressHelper
    {
        private static Regex _addressRegex = new Regex(@"R-*\d*C-*\d*", RegexOptions.Compiled);
        
        public static string ConvertExcelToRowCellAddress(string excelAddress)
        {
            if (string.IsNullOrEmpty(excelAddress) || _addressRegex.IsMatch(excelAddress))
                return excelAddress;

            excelAddress = excelAddress.Replace("$", "");
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

        public static string ConvertRowCellToExcelAddress(string sheetAddress)
        {
            if (string.IsNullOrEmpty(sheetAddress) || !_addressRegex.IsMatch(sheetAddress))
                return sheetAddress;
            
            var arr = sheetAddress.Split(new[] {'R', 'C', 'r', 'c'}, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length != 2)
                return sheetAddress;

            var row = arr[0];
            var col = arr[1];

            var address = $"{GetColumnLetter(col)}{row}";
            return address;
        }

        #region private methods
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

        private static string GetColumnLetter(string column)
        {
            if (!int.TryParse(column, out var colNum))
                return "";

            var list = new List<int>();
            var d = colNum;
            do
            {
                list.Add(d % 26);
                d = d / 26;
            } while (d != 0);
            
            list.Reverse();

            var sb = new StringBuilder();
            foreach (var item in list)
            {
                var ch = (char) (item + 64);
                sb.Append(ch);
            }

            return sb.ToString();
        }
        #endregion
    }
}