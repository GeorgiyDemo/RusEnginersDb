using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RusEnginersDb_SHARED;

namespace RusEnginersDb_CLIENT
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    /// 

    public class ConvertBitmapToBitmapImage : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return App.ConvertToBitmapSource(value as Bitmap);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public partial class App : Application
    {
        public static BitmapSource ConvertToBitmapSource(Bitmap bitmap)
        {
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap
            (
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );
            return bitmapSource;
        }

        public static Bitmap GetBitmap(BitmapSource source)
        {
            Bitmap bmp = new Bitmap(source.PixelWidth,source.PixelHeight,System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            BitmapData data = bmp.LockBits( new Rectangle(System.Drawing.Point.Empty, bmp.Size),ImageLockMode.WriteOnly,System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            source.CopyPixels(Int32Rect.Empty,data.Scan0,data.Height * data.Stride, data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            DbKeeper db = new DbKeeper();
            db.Item.Add(
                new Item("GX960", "Привод", "ДВС",
                "Honda", 1000, 5, "lolka",
                new Bitmap(@"C:\1.jpg"),  //Иконка
                new Bitmap[]{ new Bitmap(@"C:\1.jpg") },//Массив картинок
                new Dictionary<string, int> {
                    { "Диаметр вала", 100 },
                    { "Вес", 200 },
                }) 
            );

            db.Man.Add(new Manufacturer(new Bitmap(@"C:\1.jpg"), "Honda"));

            ChooseProject cp = new ChooseProject();
            cp.ShowDialog();
        }
    }
}
