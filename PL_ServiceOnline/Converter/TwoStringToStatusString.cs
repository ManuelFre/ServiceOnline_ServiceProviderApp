using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PL_ServiceOnline.Converter
{
    class TwoStringToStatusString : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if((string)values[1] == "Y")
            {
                return "Abgeschlossen";
            }
            else if((string)values[0] == "Y")
            {
                return "Angenommen";
            }

                return "Nicht bestätigt";
         
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
