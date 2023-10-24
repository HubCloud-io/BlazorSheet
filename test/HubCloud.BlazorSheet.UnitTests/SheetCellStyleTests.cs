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
    public class SheetCellStyleTests
    {
        [Test]
        public void Copy_ReturnsCopiedStyle()
        {
            var originalStyle = new SheetCellStyle
            {
                BackgroundColor = "red",
                BorderBottom = "1px solid #275081",
                BorderLeft = "1px solid #275081",
                BorderRight = "1px solid #275081",
                BorderTop = "1px solid #275081",
                Color = "#ffffff",
                FontSize = "12px",
                FontStyle = "italic",
                FontWeight = "bold",
                TextAlign = "center"
            };

            var copiedStyle = originalStyle.Copy();
            var result = copiedStyle.IsStyleEqual(originalStyle);

            Assert.AreEqual(true, result);
        }
    }
}
