using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RusEnginersDb_SERVER
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool IsMenuOpened = false;

        public void MenuItemClick()
        {
            if (IsMenuOpened)
            {
                Storyboard sb = Resources["sbHideLeftMenu"] as Storyboard;
                sb.Begin(SlideMenu);
            }
            else
            {
                Storyboard sb = Resources["sbShowLeftMenu"] as Storyboard;
                sb.Begin(SlideMenu);
            }

            IsMenuOpened = !IsMenuOpened;
        }

        public MainWindow()
        {
            InitializeComponent();

            IPHostEntry IPHost = Dns.GetHostEntry(Dns.GetHostName());

            foreach(var item in IPHost.AddressList)
            {

                IpTextBox.Text += item.ToString() + " "+ item.AddressFamily.ToString() + "\n";
            }
            IpTextBox.Text += "InterNetwork - локальный адресс";
        }
    }
}
