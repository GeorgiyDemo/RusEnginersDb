using Microsoft.VisualBasic;
using RusEnginersDb_SHARED;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RusEnginersDb_CLIENT
{
    /// <summary>
    /// Логика взаимодействия для DatabaseManagerWindow.xaml
    /// </summary>
    public partial class DatabaseManagerWindow : Window
    {
        ObservableCollection<string> paths = new ObservableCollection<string>();
        readonly string path;

        public DatabaseManagerWindow()
        {
            path = new FileInfo(Assembly.GetEntryAssembly().Location).Directory.ToString() + "\\Db";

            InitializeComponent();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            UpdatePaths();

            PathListBox.ItemsSource = paths;

        }

        private void UpdatePaths()
        {
            paths.Clear();
            foreach (var item in Directory.GetFiles(path))
            {
                if (item.Substring(item.Length - 3) == "bin")
                {
                    paths.Add(item.ToString());
                }
            }
        }

        private string DownloadNewDb()
        {
            string name = path +"\\" + DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss") + ".bin";
            Interaction.MsgBox(name);
            try
            {
                DataBaseDownloaderWindow dbdw = new DataBaseDownloaderWindow(ServerSettingsWindow.listurl,name);
                dbdw.ShowDialog();
            }
            catch(Exception ex)
            {
                Interaction.MsgBox("Не удается скачать базу данных! " + ex.Message);
                name = null;
            }
            UpdatePaths();
            return name;
        }

        private void LoadDb(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    ItemListArchive tmp = (ItemListArchive)formatter.Deserialize(fs);
                    App.Db.Item = tmp.Items;
                    App.Db.Man = tmp.Mans;
                    Interaction.MsgBox(tmp.Items.First().Name);
                    //Close();
                }
            }
            catch (Exception ex)
            {
                Interaction.MsgBox("Не удается подгрузить: " + ex.Message);
            }

        }

        private void LoadDbButtonClick(object sender, RoutedEventArgs e)
        {
            if (paths.Count == 0)
            {
                DownloadNewDb();
                LoadDb(paths.Last());
            }
            var item = PathListBox.SelectedItem as string;
            if (item == null) return;
            LoadDb(item);
        }

        private void DownloadNewDbButton(object sender, MouseButtonEventArgs e)
        {
            DownloadNewDb();
        }
    }
}
