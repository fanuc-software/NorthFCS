using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BFM.WPF.SHWMS.ViewModel
{
    public class ImageConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return new BitmapImage(new Uri($"pack://application:,,,/BFM.WPF.SHWMS;component/SHImage/{value}.png"));

            }
            catch (Exception)
            {

                return new BitmapImage(new Uri($"pack://application:,,,/BFM.WPF.SHWMS;component/SHImage/Logo.png"));

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
