using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PL_ServiceOnline.Converter
{

    class TwoStringsToColourString : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)values[1] == "Y")
            {
                return "Blue";
            }
            else if ((string)values[0] == "Y")
            {
                return "Magenta";
            }

            return "Red";

        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
