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
using System.Runtime.CompilerServices;


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

        static DbKeeper db;
        static readonly Object dblock = new Object();
        public static DbKeeper Db
        {
            get
            {
                lock (dblock)
                {
                    return db;
                }
            }
            set
            {
                lock (dblock)
                {
                    db = value;
                }
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            db = new DbKeeper();

            if (db.LastPath == null)
            {
                //Первый запуск, подгрузка бд обязательна
                DatabaseManagerWindow dmw = new DatabaseManagerWindow();
                dmw.ShowDialog();
            }

            if (db.Man.Count == 0 || db.Item.Count == 0) Current.Shutdown();

            ChooseProject cp = new ChooseProject();
            cp.ShowDialog();
        }
    }
}
