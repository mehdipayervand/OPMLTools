// -----------------------------------------------------------------------
// <copyright file="ImageConverter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace OPMLtools.Common.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ImageConverter : IValueConverter
    {
		#region Methods (2) 

		// Public Methods (2) 

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return value;
            switch (value.ToString().ToLower())
            {
                case "error":
                    {
                        return new BitmapImage(new Uri("/OPMLtools;Component/Images/error.png", UriKind.Relative));
                    }
                case "info":
                    {
                        return new BitmapImage(new Uri("/OPMLtools;Component/Images/info.png", UriKind.Relative));
                    }
                case "warn":
                    {
                        return new BitmapImage(new Uri("/OPMLtools;Component/Images/warn.png", UriKind.Relative));
                    }
                default:
                    return new BitmapImage(new Uri("/OPMLtools;Component/Images/info.png", UriKind.Relative));
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

		#endregion Methods 
    }
}
