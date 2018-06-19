using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PL_ServiceOnline.Converter;

namespace Converter.Test
{
    [TestClass]
    public class StringToColourStringConverterTest
    {
        StringToColourString stcs = new StringToColourString();

        [TestMethod]
        public void TestStringToColourString()
        {
            Assert.AreEqual("SpringGreen", stcs.Convert("Y", null, null, null));
        }

        [TestMethod]
        public void TestStringToColourString2()
        {
            Assert.AreEqual("DarkRed", stcs.Convert("N", null, null, null));
        }

        [TestMethod]
        public void TestStringToColourString3()
        {
            Assert.AreEqual("DarkRed", stcs.Convert("No", null, null, null));
        }

        [TestMethod]
        public void TestStringToColourString4()
        {
            Assert.AreEqual("DarkRed", stcs.Convert(null, null, null, null));
        }
    }
}

//public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//{
//    if ((string)value == "Y")
//    {
//        return "SpringGreen";
//    }
//    return "DarkRed";
//}