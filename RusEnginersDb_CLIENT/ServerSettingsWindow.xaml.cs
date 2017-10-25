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

namespace RusEnginersDb_CLIENT
{
    /// <summary>
    /// Логика взаимодействия для ServerSettingsWindow.xaml
    /// </summary>
    public partial class ServerSettingsWindow : Window
    {
        public static readonly string listurl = "http://127.0.0.1/?Getlist=111";

        public ServerSettingsWindow()
        {
            InitializeComponent();
        }
    }
}
