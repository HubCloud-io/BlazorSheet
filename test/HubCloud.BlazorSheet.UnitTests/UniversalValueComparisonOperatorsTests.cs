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
        public void ImplicitCast_UvBool_BoolValue()
        {
            var uv = new UniversalValue(true);
            bool flag = uv;
            
            Assert.AreEqual(true, flag);
        }
        
        [Test]
        public void ExplicitCast_UvBool_BoolValue()
        {
            var uv = new UniversalValue(true);
            var flag = (bool)uv;
            
            Assert.AreEqual(true, flag);
        }
        
        [Test]
        public void ExplicitCast_BoolUv_UniversalValue()
        {
            var flag = true;
            var uv = (UniversalValue)flag;
            
            Assert.AreEqual(true, uv.Value);
        }
        
        [Test]
        public void ImplicitCast_BoolUv_UniversalValue()
        {
            var flag = true;
            UniversalValue uv = flag;
            
            Assert.AreEqual(true, uv.Value);
        }
        
        [Test]
        public void Equal_UvDateTimeAndUvDateTime_BoolValue()
        {
            var uValue1 = new UniversalValue(new DateTime(2023,10, 6));
            var uValue2 = new UniversalValue(new DateTime(2023,10, 6));

            var result = uValue1 == uValue2;

            Assert.AreEqual(true, result.Value);
        }
        
        [Test]
        public void Equal_UvDateTimeAndDateTime_BoolValue()
        {
            var uValue1 = new UniversalValue(new DateTime(2023,10, 6));
            var uValue2 = new DateTime(2023,10, 6);

            var result = uValue1 == uValue2;

            Assert.AreEqual(true, result.Value);
        }
        
        [Test]
        public void Equal_UvIndAndUvInt_BoolValue()
        {
            var uValue1 = new UniversalValue(1);
            var uValue2 = new UniversalValue(1);

            var result = uValue1 == uValue2;

            Assert.AreEqual(true, result.Value);
        }
        
        [Test]
        public void Equal_UvIndAndInt_BoolValue()
        {
            var uValue1 = new UniversalValue(1);
            var value2 = 1;

            var result = uValue1 == value2;

            Assert.AreEqual(true, result.Value);
        }
        
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
