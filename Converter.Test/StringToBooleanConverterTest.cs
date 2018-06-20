using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PL_ServiceOnline.Converter;

namespace Converter.Test
{
    [TestClass]
    public class StringToBooleanConverterTest
    {
        StringToBoolean stb = new StringToBoolean();

        [TestMethod]
        public void TestStringToBooleanYes()
        {
            Assert.IsTrue((bool)stb.Convert("Y", null, null, null));
        }

        [TestMethod]
        public void TestStringToBooleanYes1()
        {
            Assert.IsTrue((bool)stb.Convert("Ja", null, null, null));
        }

        [TestMethod]
        public void TestStringToBooleanNo()
        {
            Assert.IsFalse((bool)stb.Convert("N", null, null, null));
        }

        [TestMethod]
        public void TestStringToBooleanNo2()
        {
            Assert.IsFalse((bool)stb.Convert("Nein", null, null, null));
        }

        [TestMethod]
        public void TestStringToBooleanNo3()
        {
            Assert.IsFalse((bool)stb.Convert("No", null, null, null));
        }

        [TestMethod]
        public void TestStringToBooleanNo4()
        {
            Assert.IsFalse((bool)stb.Convert(null, null, null, null));
        }

        [TestMethod]
        public void TestStringToBooleanBackTrue()
        {
            Assert.AreEqual("Y", stb.ConvertBack(true, null, null, null));
        }

        [TestMethod]
        public void TestStringToBooleanBackFalse()
        {
            Assert.AreEqual("N", stb.ConvertBack(false, null, null, null));
        }
    }
}