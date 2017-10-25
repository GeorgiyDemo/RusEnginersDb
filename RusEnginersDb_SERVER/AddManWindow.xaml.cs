using System;
using System.Collections.Generic;
using System.Drawing;
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
using Microsoft.VisualBasic;

namespace RusEnginersDb
{
    /// <summary>
    /// Логика взаимодействия для AddManWindow.xaml
    /// </summary>
    public partial class AddManWindow : Window
    {
        Bitmap logo;

        public Manufacturer RetMan { get; private set; }

        public AddManWindow()
        {
            logo = App.GetBitmap(new BitmapImage(new Uri("pack://application:,,,/RusEnginersDb;component/Images/DefaultIcon.png")));
            InitializeComponent();
        }

        private void ChooseLogo(object sender, RoutedEventArgs e)
        {
            App.GetBitmap(out logo);
            ManBitmap.Source = App.ConvertToBitmapSource(logo);
        }

        private void AddThisMan(object sender, RoutedEventArgs e)
        {
            try
            {
                int r;
                if (!int.TryParse(ManRate.Text, out r)) throw new Exception("Оценка это число!");
                RetMan = new Manufacturer(logo, ManName.Text, r, ManComment.Text);
            }
            catch(Exception ex)
            {
                Interaction.MsgBox("Произошла ошибка: " + ex.Message);
                return;
            }
            Close();
        }
    }
}
