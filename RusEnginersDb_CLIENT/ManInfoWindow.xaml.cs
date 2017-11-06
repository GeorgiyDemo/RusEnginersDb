using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RusEnginersDb_SHARED;

namespace RusEnginersDb
{
    /// <summary>
    /// Логика взаимодействия для ManInfoWindow.xaml
    /// </summary>
    public partial class ManInfoWindow : Window
    {
        public ManInfoWindow(Manufacturer man)
        {
            InitializeComponent();
            ManImage.Source = App.ConvertToBitmapSource(man.Bitmap);
            ManName.Text = man.Name;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < man.Rate; i++)
            {
                sb.Append("★");
            }
            ManRate.Text = sb.ToString();
            ManComment.Text = man.Info;
        }
    }
}
