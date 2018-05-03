using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MazeGame
{
    public class WindowWidthConverter : IValueConverter
    {
        public double A { get; set; }
        public double B { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double a = GetDoubleValue(parameter, 0.0);

            double x = GetDoubleValue(value, 0.0);

            return (a + x);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double a = GetDoubleValue(parameter, 0.0);

            double x = GetDoubleValue(value, 0.0);

            return (a - x);
        }

        private double GetDoubleValue(object parameter, double defaultValue)
        {
            double a;
            if (parameter != null)
                try
                {
                    a = System.Convert.ToDouble(parameter);
                }
                catch
                {
                    a = defaultValue;
                }
            else
                a = defaultValue;
            return a;
        }
    }
}
