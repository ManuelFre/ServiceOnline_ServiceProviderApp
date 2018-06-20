using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PL_ServiceOnline.Converter;

namespace Converter.Test
{
    [TestClass]
    public class AllIncStringToStringColorConverterTest
    {
        AllIncStringToStringColor allInc = new AllIncStringToStringColor();

        [TestMethod]
        public void TestAllIncStringToStringColor()
        {
            Assert.AreEqual("LightCyan", allInc.Convert("Ja", null, null,null));
        }

        [TestMethod]
        public void TestAllIncStringToStringColor2()
        {
            Assert.AreEqual("LightPink", allInc.Convert("Nein", null, null, null));
        }
        [TestMethod]
        public void TestAllIncStringToStringColor3()
        {
            Assert.AreEqual("LightPink", allInc.Convert(null, null, null, null));
        }
    }
}
