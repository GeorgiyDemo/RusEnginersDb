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
using RusEnginersDb_SHARED;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace RusEnginersDb
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

        public void Items()
        {
            ItemEditorWindow iew = new ItemEditorWindow();
            iew.ShowDialog();
        }

        public void Mans()
        {
            ManEditorWindow mew = new ManEditorWindow();
            mew.ShowDialog();
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

        private void SaveData(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            // Set filter options and filter index.
            saveFileDialog1.Filter = "Binary (.bin)|*.bin";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.ShowDialog();

            if (!string.IsNullOrWhiteSpace(saveFileDialog1.FileName))
            {
                App.SaveData(saveFileDialog1.FileName);
            }
        }


        private void LoadData(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "Binary (.bin)|*.bin";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog();

            if (!string.IsNullOrWhiteSpace(openFileDialog1.FileName))
            {
                App.LoadData(openFileDialog1.FileName);
            }
        }
    }
}
