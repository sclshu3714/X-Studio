using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace XStudio.App.Converters
{
    public class NullToBooleanConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            bool IsNull = value == null;
            if (!IsNull && value is string str) { 
                IsNull = string.IsNullOrWhiteSpace(str);
            }
            return IsNull;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
