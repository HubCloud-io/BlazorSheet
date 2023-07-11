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
            
            return excelAddress;
        }
    }
}