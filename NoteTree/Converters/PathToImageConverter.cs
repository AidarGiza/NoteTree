using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using System;
using System.Globalization;

namespace NoteTree.Converters
{
    public class PathToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            string name = value.ToString();
            object outObj = new object();

            if (App.Current.Resources.MergedDictionaries[0].TryGetResource(name, out outObj))
            {
                Bitmap bitmap = outObj as Bitmap;

                if (bitmap != null) return bitmap;
                else return new object();
            }
            return new object();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
            //return new object();
        }
    }
}
