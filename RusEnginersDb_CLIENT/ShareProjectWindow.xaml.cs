using Microsoft.VisualBasic;
using RusEnginersDb_SHARED;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
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
    /// Логика взаимодействия для ShareProjectWindow.xaml
    /// </summary>
    public partial class ShareProjectWindow : Window
    {

        private void DataUploaded(string retcode)
        {
            string url = ServerSettingsWindow.serverurl + "?Project=" + retcode;
            QrCodeImage.Source=App.ConvertToBitmapSource(App.GenerateQRCode(url));
            LinkTextBox.Text = url;
        }

        public ShareProjectWindow(Project proj)
        {
            InitializeComponent();

            try
            {
                AsyncExecute ae = new AsyncExecute(() =>
                {
                    WebClient wc = new WebClient();
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        formatter.Serialize(ms, proj);
                        byte[] result = wc.UploadData(new Uri(ServerSettingsWindow.serverurl + "?UploadProject=1"), ms.ToArray());
                        Application.Current.Dispatcher.Invoke(()=> DataUploaded(Encoding.Unicode.GetString(result)));
                    }
                });
                ae.ShowDialog();
            }
            catch(Exception ex)
            {
                Interaction.MsgBox("Не удается выгрузить файл! " + ex.Message);
                Close();
            }
        }

    }
}
