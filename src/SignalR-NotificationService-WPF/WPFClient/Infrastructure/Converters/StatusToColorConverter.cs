using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace JobNotificationsClient.Infrastructure.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public SolidColorBrush CompletedColor { get; set; }
        public SolidColorBrush FailedColor { get; set; }
        public SolidColorBrush NullValue { get; set; }
        public SolidColorBrush AssignedColor { get; set; }
        public SolidColorBrush RunningColor { get; set; }

        public StatusToColorConverter()
        {
            CompletedColor = Brushes.LightGreen;
            FailedColor = Brushes.Red;
            AssignedColor = Brushes.LightSlateGray;
            RunningColor = Brushes.LightYellow;
            NullValue = Brushes.Transparent;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return NullValue;
            if (value.ToString() == "Completed") return CompletedColor;
            if (value.ToString() == "Failed") return FailedColor;
            if (value.ToString() == "Running") return RunningColor;
            if (value.ToString() == "Assigned") return AssignedColor;
            return NullValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
