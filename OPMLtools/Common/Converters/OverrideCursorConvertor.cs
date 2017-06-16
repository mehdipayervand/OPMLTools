using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;

namespace OPMLtools.Common.Converters
{
    class OverrideCursorConvertor:IValueConverter
    {
		#region Methods (2) 

		// Public Methods (2) 

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value==null)
                return value;
            if ((bool)value)
            {
                return Cursors.Wait;
            }
            return Cursors.Arrow;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

		#endregion Methods 
    }
}
