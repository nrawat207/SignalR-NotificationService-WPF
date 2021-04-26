﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace JobNotificationsClient.Infrastructure.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visible = value is bool && (bool)value;
            return visible ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
