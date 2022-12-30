using System;
using System.Globalization;
using System.Security;
using System.Windows;
using System.Windows.Data;

namespace WPFRJCode.Extensions
{
    /// <summary>
    /// Конвертирует SecurityString в обычный String и наоборот
    /// </summary>
    public class SecurityStringToStringConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new System.Net.NetworkCredential(string.Empty, ((SecureString)value)).Password;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
