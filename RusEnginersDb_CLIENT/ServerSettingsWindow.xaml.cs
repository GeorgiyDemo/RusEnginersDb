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

namespace RusEnginersDb
{
    /// <summary>
    /// Логика взаимодействия для ServerSettingsWindow.xaml
    /// </summary>
    public partial class ServerSettingsWindow : Window
    {
        public static readonly string serverurl = "http://127.0.0.1:88/";

        public ServerSettingsWindow()
        {
            InitializeComponent();
        }
    }
}
