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
    public class UniversalValueLogicOperatorsTests
    {
        [TestCase(true, true, true)]
        [TestCase(true, false, false)]
        [TestCase(true, null, null)]
        [TestCase(false, true, false)]
        [TestCase(false, false, false)]
        [TestCase(false, null, false)]
        [TestCase(null, true, null)]
        [TestCase(null, false, false)]
        [TestCase(null, null, null)]
        public void LogicOperationAnd_UniversalValue(bool? value1, bool? value2, bool? expected)
        {
            var uValue1 = new UniversalValue(value1);
            var uValue2 = new UniversalValue(value2);

            var result = uValue1 && uValue2;

            Assert.AreEqual(expected, result.Value);
        }

        [TestCase(true, true, true)]
        [TestCase(true, false, true)]
        [TestCase(true, null, true)]
        [TestCase(false, true, true)]
        [TestCase(false, false, false)]
        [TestCase(false, null, null)]
        [TestCase(null, true, true)]
        [TestCase(null, false, null)]
        [TestCase(null, null, null)]
        public void LogicOperationOr_UniversalValue(bool? value1, bool? value2, bool? expected)
        {
            var uValue1 = new UniversalValue(value1);
            var uValue2 = new UniversalValue(value2);

            var result = uValue1 || uValue2;

            Assert.AreEqual(expected, result.Value);
        }

        [TestCase(5, 5, true)]
        [TestCase("qwerty", "qwerty", true)]
        [TestCase(null, "qwerty", false)]
        [TestCase("qwerty", false, false)]
        [TestCase("qwerty", 5, false)]
        public void Equals_UniversalValueAndUniversalValue_ReturnBool(object value1, object value2, bool expected)
        {
            var uValue1 = new UniversalValue(value1);
            var uValue2 = new UniversalValue(value2);

            var result = uValue1.Equals(uValue2);

            Assert.AreEqual(expected, result);
        }

        [TestCase(5, 5, true)]
        [TestCase(5, null, false)]
        [TestCase("qwerty", "qwerty", true)]
        [TestCase(null, "qwerty", false)]
        [TestCase("qwerty", false, false)]
        [TestCase("qwerty", 5, false)]
        public void Equals_UniversalValueAndObject_ReturnBool(object value1, object value2, bool expected)
        {
            var uValue1 = new UniversalValue(value1);

            var result = uValue1.Equals(value2);

            Assert.AreEqual(expected, result);
        }
    }
}
