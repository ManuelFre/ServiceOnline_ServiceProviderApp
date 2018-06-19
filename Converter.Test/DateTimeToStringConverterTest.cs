using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PL_ServiceOnline.Converter;

namespace Converter.Test
{
    [TestClass]
    public class DateTimeToStringConverterTest
    {
        DateTimeToString dtts = new DateTimeToString();

        [TestMethod]
        public void TestDateTimeToString()
        {
            Assert.AreEqual("19.06.2018 - 23:41", dtts.Convert(new DateTime(2018,06,19,23,41,55), null, null, null));
        }

        [TestMethod]
        public void TestDateTimeToString2()
        {
            Assert.AreEqual("01.01.2022 - 03:00", dtts.Convert(new DateTime(2022, 01, 01, 03, 00, 00), null, null, null));
        }

        [TestMethod]
        public void TestDateTimeToString3()
        {
            Assert.AreEqual("24.12.1980 - 23:59", dtts.Convert(new DateTime(1980, 12, 24, 23, 59, 59), null, null, null));
        }

        [TestMethod]
        public void TestDateTimeToStringOutOfRange()
        {
            try
            {
                dtts.Convert(DateTime.MinValue.Subtract(new DateTime(0, 0, 1, 0, 0, 0)), null, null, null);
                Assert.Fail();
            }
            catch (Exception) { }
        }

        [TestMethod]
        public void TestDateTimeToStringOutOfRange2()
        {
            try
            {
                dtts.Convert(DateTime.MaxValue.AddDays(1), null, null, null);
                Assert.Fail();
            }
            catch (Exception) { }
        }
    }
}

