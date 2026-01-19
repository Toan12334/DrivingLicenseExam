using System;
using System.Globalization;
using System.Windows.Data;
using DrivingLicenseExam.Enums;

namespace DrivingLicenseExam.Converters
{
    public class EnumToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && value.ToString() == parameter?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value && parameter != null)
                return Enum.Parse(typeof(AnswerOption), parameter.ToString());

            return Binding.DoNothing;
        }
    }
}
