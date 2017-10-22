using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using RusEnginersDb_SHARED;
using Microsoft.VisualBasic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace RusEnginersDb_SERVER
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>

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
            Bitmap bmp = new Bitmap(source.PixelWidth, source.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            BitmapData data = bmp.LockBits(new Rectangle(System.Drawing.Point.Empty, bmp.Size), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            source.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }

        public static void GetBitmap(out Bitmap logo)
        {
            Bitmap tmp;

            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "Image (.jpg)|*.jpg|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog();

            if (!string.IsNullOrWhiteSpace(openFileDialog1.FileName))
            {
                try
                {
                    tmp = new Bitmap(openFileDialog1.FileName);
                    logo = tmp;
                    return;
                }
                catch (Exception ex)
                {
                    Interaction.MsgBox("Не удается добавить картинку! " + ex.Message);
                }
            }

            logo = App.GetBitmap(new BitmapImage(new Uri("pack://application:,,,/RusEnginersDb_SERVER;component/Images/DefaultIcon.png")));
        }

        static ServerData sdata = null;
        static object sdatalocker = new Object();

        public static ServerData SData
        {
            get
            {
                lock (sdatalocker)
                {
                    return sdata;
                }
            }
            set
            {
                lock (sdatalocker)
                {
                    sdata = value;
                }
            }
        }

        public static void SaveData(string path)
        {
            lock (sdatalocker)
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        formatter.Serialize(fs, sdata);
                        fs.Close();
                    }
                }
                catch (Exception ex)
                {
                    Interaction.MsgBox("Неудается провести сериализацию: " + ex.Message);
                }
            }
        }

        public static void LoadData(string path)
        {
            lock (sdatalocker)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream fs = new FileStream(path, FileMode.Open);
                try
                {
                    sdata = (ServerData)formatter.Deserialize(fs);
                }
                catch (Exception ex)
                {
                    Interaction.MsgBox("Не удается десериализовать: " + ex.Message);
                }
                fs.Close();
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SData = new ServerData();

            SData.Items.Add(
                    new Item("GX960", "Привод", "ДВС",
                    "Honda", 1000, 5, 60, "lolka",
                    new Bitmap(@"C:\1.jpg"),  //Иконка
                    new Bitmap[] { new Bitmap(@"C:\1.jpg"), new Bitmap(@"C:\1.jpg"), new Bitmap(@"C:\1.jpg"), new Bitmap(@"C:\1.jpg") },//Массив картинок
                    new Dictionary<string, int> { { "Диаметр", 100 } }, //Свойства
                    new Link[] { new Link("Yandex", "http://yandex.ru"), new Link("Скайп", "skype://") }
            ));

            MainWindow w = new MainWindow();
            w.ShowDialog();
        }
    }
}
