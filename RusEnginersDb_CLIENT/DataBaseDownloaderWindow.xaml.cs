using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для DataBaseDownloaderWindow.xaml
    /// </summary>
    public partial class DataBaseDownloaderWindow : Window
    {
        WebClient client;
        string url;
        string path;
        bool IsComplated = false;

        internal int DownloadProcessBarValue
        {
            set
            {
                Dispatcher.Invoke(new Action(() => { DownloadProcessBar.Value = value; }));
            }
        }

        public DataBaseDownloaderWindow(string url, string path)
        {
            client = new WebClient();
            this.url = url;
            this.path = path;
            InitializeComponent();

            client.DownloadProgressChanged += (o, e) => { DownloadProcessBarValue = e.ProgressPercentage; };
            client.DownloadFileCompleted += (o, e) => 
            {
                if (e.Error != null)
                {
                    Interaction.MsgBox(e.Error.ToString());
                }
                else
                {
                    IsComplated = true;
                }
                App.Current.Dispatcher.Invoke(() =>Close());
            };
        }

        private void CancelDownload(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!IsComplated)
            {
                client.CancelAsync();
                client.Disposed += (o,ev) => File.Delete(path);
            }
            
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {

            client.DownloadFileAsync(new Uri(url), path);
        }
    }
}
