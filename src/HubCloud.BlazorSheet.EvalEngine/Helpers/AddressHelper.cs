using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using HubCloud.BlazorSheet.EvalEngine.Engine;

namespace HubCloud.BlazorSheet.EvalEngine.Helpers
{
    public static class AddressHelper
    {
        private static readonly Regex AddressRegex = RegexHelper.AddressRegex;

        public static string ConvertExcelToRowCellAddress(string excelAddress)
        {
            if (string.IsNullOrEmpty(excelAddress) || AddressRegex.IsMatch(excelAddress))
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

        public static string ConvertR1C1ToA1Address(string sheetAddress, int currentRow, int currentCol)
        {
            if (string.IsNullOrEmpty(sheetAddress) || !AddressRegex.IsMatch(sheetAddress))
                return sheetAddress;

            var address = ProcessR1C1Address(sheetAddress, currentRow, currentCol);
            var arr = address
                .TrimStart('"')
                .TrimEnd('"')
                .Split(new[] {'R', 'C', 'r', 'c'}, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length != 2)
                return address;

            var row = arr[0];
            var col = arr[1];

            var columnLetter = GetColumnLetter(col);
            return $"{columnLetter}{row}";
        }

        public static string ProcessR1C1Address(string sheetAddress, int currentRow, int currentCol)
        {
            var rcValues = GetRCValues(sheetAddress);

            var r = ProcessValues(rcValues.Item1, currentRow);
            var c = ProcessValues(rcValues.Item2, currentCol);

            return $"R{r}C{c}";
        }

        #region private methods

        private static string ProcessValues(string value, int current)
        {
            // RC1 case
            if (string.IsNullOrEmpty(value))
                return current.ToString();

            // R[1]C1 case
            if (value.Contains("[") && value.Contains("]"))
            {
                var trimmedValue = value.Trim(new[] {'[', ']'});
                if (!int.TryParse(trimmedValue, out var intValue))
                    return value;

                return (current + intValue).ToString();
            }

            return value;
        }

        /// <summary>
        /// item1 - row, item2 - col
        /// </summary>
        /// <param name="addressR1C1"></param>
        /// <returns></returns>
        private static Tuple<string, string> GetRCValues(string addressR1C1)
        {
            var address = addressR1C1?.ToUpper().Trim().Trim('\"');
            if (string.IsNullOrEmpty(address) || !address.Contains("R") || !address.Contains("C"))
                return null;

            var cIndex = address.IndexOf("C", StringComparison.InvariantCulture);
            var rVal = address.Substring(0, cIndex).Trim('R');
            var cVal = string.Empty;
            if (cIndex < address.Length - 1)
                cVal = address.Substring(cIndex, address.Length - cIndex).Trim('C');

            return new Tuple<string, string>(rVal, cVal);
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
        
        public static string GetColumnLetter(string column)
        {
            if (!int.TryParse(column, out var colNum))
                return "";

            var columnName = "";
            while (colNum > 0)
            {
                var m = (colNum - 1) % 26;
                columnName = Convert.ToChar('A' + m) + columnName;
                colNum = (colNum - m) / 26;
            } 

            return columnName;
        }

        #endregion
    }
}