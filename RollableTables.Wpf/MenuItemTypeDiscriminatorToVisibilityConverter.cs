using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RollableTables;

public class MenuItemTypeDiscriminatorToVisibilityConverter : IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        return ((MenuItemTypeDiscriminator)value) == MenuItemTypeDiscriminator.MenuItemLevelViewModel
                   ? Visibility.Visible
                   : Visibility.Collapsed;
    }

    public object ConvertBack(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}