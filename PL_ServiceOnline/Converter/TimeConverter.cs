using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PL_ServiceOnline.Converter
{
    public class TimeConverter : IValueConverter
    {
        private DateTime datePickerDate;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            datePickerDate = (DateTime)value;
            return value;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return datePickerDate;
            var timePickerDate = (DateTime)value;
            
            if (timePickerDate.Year != datePickerDate.Year
                || timePickerDate.Month != datePickerDate.Month
                || timePickerDate.Day != datePickerDate.Day)
            {
                var result = new DateTime(datePickerDate.Year,
                     datePickerDate.Month,
                     datePickerDate.Day,
                     timePickerDate.Hour,
                     timePickerDate.Minute,
                     timePickerDate.Second
                     );
                
                return result;
            }

            return timePickerDate;
        }
    }
}
