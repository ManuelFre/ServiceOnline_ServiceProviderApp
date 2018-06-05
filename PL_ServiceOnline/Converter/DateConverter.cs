using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PL_ServiceOnline.Converter
{
    [Obsolete("ForNow")]
    public class DateConverter : IValueConverter
    {
        private DateTime timePickerDate;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            timePickerDate = (DateTime)value;
            return value;
        }

        //TODO: Check each step to look for time / date changes... solution idea... maybe also a time converter needed to safe the date when time changes...

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return timePickerDate;
            var datePickerDate = (DateTime)value;

            //compare relevant parts manually
            if (datePickerDate.Hour != timePickerDate.Hour
                || datePickerDate.Minute != timePickerDate.Minute
                || datePickerDate.Second != timePickerDate.Second)
            {
                //correct the date picker value
                var result = new DateTime(datePickerDate.Year,
                     datePickerDate.Month,
                     datePickerDate.Day,
                     timePickerDate.Hour,
                     timePickerDate.Minute,
                     timePickerDate.Second);

                //return, because this event handler will be executed a second time
                return result;
            }

            return datePickerDate;
        }
    }
}
