using HubCloud.BlazorSheet.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubCloud.BlazorSheet.UnitTests
{
    [TestFixture]
    public class UniversalValueStringMethodsTests
    {
        [TestCase("qwerty", 1, "werty")]
        public void Substring_StartIndex_SubstringValue(string value, int startIndex, string expectedValue)
        {
            var uv = new UniversalValue(value);

            var result = uv.Substring(startIndex);

            Assert.AreEqual(expectedValue, result.Value);
        }

        [TestCase("qwerty", 0, 1, "q")]
        public void Substring_StartIndexLength_SubstringValue(string value, int startIndex, int length, string expectedValue)
        {
            var uv = new UniversalValue(value);

            var result = uv.Substring(startIndex, length);

            Assert.AreEqual(expectedValue, result.Value);
        }

        [TestCase("qwerty", "QWERTY")]
        public void ToUpper_ToUpperValue(string value, string expectedValue)
        {
            var uv = new UniversalValue(value);

            var result = uv.ToUpper();

            Assert.AreEqual(expectedValue, result.Value);
        }

        [TestCase("QWERTY", "qwerty")]
        public void ToLower_ToLowerValue(string value, string expectedValue)
        {
            var uv = new UniversalValue(value);

            var result = uv.ToLower();

            Assert.AreEqual(expectedValue, result.Value);
        }

        [TestCase("QWERTY", "W", 1)]
        public void IndexOf_StringValue_IndexOfValue(string value, string indexOfValue, int expectedIndex)
        {
            var uv = new UniversalValue(value);

            var result = uv.IndexOf(indexOfValue);

            Assert.AreEqual(expectedIndex, result.Value);
        }

        [TestCase("QWERTY", "WERT", "____", "Q____Y")]
        public void Replace_OldValueNewValue_ReplaceValue(string value, string oldValue, string newValue, string expectedValue)
        {
            var uv = new UniversalValue(value);

            var result = uv.Replace(oldValue, newValue);

            Assert.AreEqual(expectedValue, result.Value);
        }
    }
}
