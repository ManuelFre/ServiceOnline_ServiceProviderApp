using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PL_ServiceOnline.Converter;

namespace Converter.Test
{
    [TestClass]
    public class BoolStringToGermanConverterTest
    {
        BoolStringToGerman bstg = new BoolStringToGerman();

        [TestMethod]
        public void TestBoolStringToGermanYes()
        {
            Assert.AreEqual("Ja", bstg.Convert("Y", null, null, null));
        }

        [TestMethod]
        public void TestBoolStringToGermanYes2()
        {
            Assert.AreEqual("Ja", bstg.Convert("Ja", null, null, null));
        }

        [TestMethod]
        public void TestBoolStringToGermanNo()
        {
            Assert.AreEqual("Nein", bstg.Convert("No", null, null, null));
        }

        [TestMethod]
        public void TestBoolStringToGermanNo2()
        {
            Assert.AreEqual("Nein", bstg.Convert("N", null, null, null));
        }

        [TestMethod]
        public void TestBoolStringToGermanNo3()
        {
            Assert.AreEqual("Nein", bstg.Convert("Nein", null, null, null));
        }

        [TestMethod]
        public void TestBoolStringToGermanNo4()
        {
            Assert.AreEqual("Nein", bstg.Convert(null, null, null, null));
        }
    }
}