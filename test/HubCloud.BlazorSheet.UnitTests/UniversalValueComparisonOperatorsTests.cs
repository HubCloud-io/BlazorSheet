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
    public class UniversalValueComparisonOperatorsTests
    {
        [Test]
        public void MoreThan_DecimalAndDecimal_MoreThanValue()
        {
            var uValue1 = new UniversalValue(1M);
            var uValue2 = new UniversalValue(2M);

            var result = uValue1 > uValue2;

            Assert.AreEqual(false, result.Value);
        }

        [Test]
        public void MoreThan_DecimalAndNull_MoreThanValue()
        {
            var uValue1 = new UniversalValue(1M);
            var uValue2 = new UniversalValue(null);

            var result = uValue1 > uValue2;

            Assert.AreEqual(false, result.Value);
        }

        [Test]
        public void MoreThan_NullAndDecimal_MoreThanValue()
        {
            var uValue1 = new UniversalValue(null);
            var uValue2 = new UniversalValue(2M);

            var result = uValue1 > uValue2;

            Assert.AreEqual(false, result.Value);
        }

        [Test]
        public void MoreThan_DecimalAndInt_MoreThanValue()
        {
            var uValue1 = new UniversalValue(1M);
            var uValue2 = new UniversalValue(1);

            var result = uValue1 > uValue2;

            Assert.AreEqual(false, result.Value);
        }

        [Test]
        public void MoreThan_IntAndDecimal_MoreThanValue()
        {
            var uValue1 = new UniversalValue(1);
            var uValue2 = new UniversalValue(1M);

            var result = uValue1 > uValue2;

            Assert.AreEqual(false, result.Value);
        }

        [Test]
        public void LessThan_DecimalAndDecimal_LessThanValue()
        {
            var uValue1 = new UniversalValue(1M);
            var uValue2 = new UniversalValue(2M);

            var result = uValue1 < uValue2;

            Assert.AreEqual(true, result.Value);
        }

        [Test]
        public void LessThan_DecimalAndNull_LessThanValue()
        {
            var uValue1 = new UniversalValue(1M);
            var uValue2 = new UniversalValue(null);

            var result = uValue1 < uValue2;

            Assert.AreEqual(false, result.Value);
        }

        [Test]
        public void LessThan_NullAndDecimal_LessThanValue()
        {
            var uValue1 = new UniversalValue(null);
            var uValue2 = new UniversalValue(2M);

            var result = uValue1 < uValue2;

            Assert.AreEqual(false, result.Value);
        }

        [Test]
        public void LessThan_DecimalAndInt_LessThanValue()
        {
            var uValue1 = new UniversalValue(1M);
            var uValue2 = new UniversalValue(1);

            var result = uValue1 < uValue2;

            Assert.AreEqual(false, result.Value);
        }

        [Test]
        public void LessThan_IntAndDecimal_LessThanValue()
        {
            var uValue1 = new UniversalValue(1);
            var uValue2 = new UniversalValue(1M);

            var result = uValue1 < uValue2;

            Assert.AreEqual(false, result.Value);
        }
    }
}
