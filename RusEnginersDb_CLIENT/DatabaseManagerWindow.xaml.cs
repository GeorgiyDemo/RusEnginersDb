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
        readonly string path;
        ObservableCollection<string> files = new ObservableCollection<string>();


        public DatabaseManagerWindow()
        {
            path = new FileInfo(Assembly.GetEntryAssembly().Location).Directory.ToString() + "\\Db";

            InitializeComponent();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (var item in new DirectoryInfo(path).GetFiles())
                files.Add(item.FullName);

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
           | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = "*.bin";

            Action lambda = ()=>App.Current.Dispatcher.Invoke(() =>
            {
                files.Clear();
                foreach (var item in new DirectoryInfo(path).GetFiles())
                    files.Add(item.FullName);
            });

            watcher.Changed += new FileSystemEventHandler((o, e) => lambda());
            watcher.Deleted += (o,e)=>lambda();
            watcher.EnableRaisingEvents = true;

            PathListBox.ItemsSource = files;

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
            var item = PathListBox.SelectedItem as string;
            if (item == null)
            {
                Interaction.MsgBox("Сначала выберите базу данных!");
            }
            LoadDb(item);
        }

        private void DownloadNewDbButton(object sender, MouseButtonEventArgs e)
        {
            DownloadNewDb();
        }

        private void RemoveItemClick(object sender, RoutedEventArgs e)
        {
            var item = PathListBox.SelectedItem as string;
            if (item == null) return;
            File.Delete(item);
        }
    }
}
