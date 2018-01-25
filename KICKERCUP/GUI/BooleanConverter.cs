using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI
{
    /// <summary>
    /// BooleanConverter
    /// konvertiert Boolean-Werte in Ja/Nein
    /// bzw. Sieger (falls true)
    /// benutzt in den DataGrids des Tournament_Managements/Ranking_List
    /// </summary>
    /// 
    public class BooleanConverterYesNo : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = System.Convert.ToBoolean(value);

            if (b)
                return "Ja";

            else
                return "Nein";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleanConverterWinner : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = System.Convert.ToBoolean(value);

            if (b)
                return "Sieger";

            else
                return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}