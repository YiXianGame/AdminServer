using System;
using System.Globalization;
using System.Windows.Data;

namespace Pack.Element
{
    public class Converts : IValueConverter
    {
        //当值从绑定源传播给绑定目标时，调用方法Convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value) return "0";
            else return "1";
        }
        //当值从绑定目标传播给绑定源时，调用此方法ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = value as string;
            if (result.Equals("0")) return true;
            else return false;
        }
    }
}
