using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
using RusEnginersDb_SHARED;
using System.Collections.ObjectModel;

namespace RusEnginersDb_CLIENT
{
    /// <summary>
    /// Логика взаимодействия для ChooseProject.xaml
    /// </summary>
    /// 

    [Serializable]
    class OpenedProject
    {
        public OpenedProject(string path, Bitmap b)
        {
            this.Path = path;
            this.Icon = b;
        }
        public string Path { get; private set; }
        public Bitmap Icon { get; private set; }
    }

    public partial class ChooseProject : Window
    {
        Bitmap image = null;
        ObservableCollection<OpenedProject> openedlist = null;
        string openedlistpath = null;

        private void SaveOpenedProjectList()
        {
            for(int i = openedlist.Count - 1; i >= 1; i--)
            {
                for (int j = i-1; j >= 0; j--)
                {
                    if (openedlist[i].Path == openedlist[j].Path)
                    {
                        openedlist.RemoveAt(i);
                        break;
                    }
                }
            }

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(openedlistpath, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, openedlist);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                Interaction.MsgBox("Неудается провести сериализацию: " + ex.Message);
            }
        }

        public ChooseProject()
        {
            InitializeComponent();
            openedlistpath = System.IO.Path.GetTempPath() + "rusengdbprojpath.bin";
            if (!File.Exists(openedlistpath))
            {
                openedlist = new ObservableCollection<OpenedProject>();
            }
            else
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream fs = new FileStream(openedlistpath, FileMode.Open);
                try
                {
                    openedlist = (ObservableCollection<OpenedProject>)formatter.Deserialize(fs);
                }
                catch (Exception ex)
                {
                    Interaction.MsgBox("Не удается десериализовать: " + ex.Message);
                    openedlist = new ObservableCollection<OpenedProject>();
                }
                fs.Close();
            }

            ProjectList.ItemsSource = openedlist;
        }

        private void OpenProjectFromPath(string path)
        {
            Project proj;
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);
            try
            {
                proj = (Project)formatter.Deserialize(fs);
            }
            catch (Exception ex)
            {
                Interaction.MsgBox("Не удается десериализовать: " + ex.Message);
                return;
            }
            fs.Close();
            SaveManager.SetSavePath(path);

            openedlist.Add(new OpenedProject(path, proj.Bitmap));
            SaveOpenedProjectList();

            ProjectWindow pw = new ProjectWindow(proj);
            Hide();
            pw.ShowDialog();
            Close();
        }

        private void CreateProject(object sender, RoutedEventArgs e)
        {
            ProjectWindow pw;
            if (image == null)
            {
                pw = new ProjectWindow(NameInput.Text, CommentInput.Text);
            }
            else
            {
                pw = new ProjectWindow(image, NameInput.Text, CommentInput.Text);
            }

            Hide();
            pw.ShowDialog();
        }

        private void OpenProject(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "BINARY (*.bin)|*.bin";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.ShowDialog();

            if (!string.IsNullOrWhiteSpace(openFileDialog1.FileName))
            {
                OpenProjectFromPath(openFileDialog1.FileName);
            }
            else
            {
                Interaction.MsgBox("Путь не указан!");
            }
        }

        private void LoadBitmapClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "IMAGE (*.jpg)|*.jpg|IMAGE (*.png)|*.png";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.ShowDialog();

            if (!string.IsNullOrWhiteSpace(openFileDialog1.FileName))
            {
                try
                {
                    image = new Bitmap(openFileDialog1.FileName);
                    ProjectImage.Source = App.ConvertToBitmapSource(image);
                }
                catch(Exception ex)
                {
                    Interaction.MsgBox("Не удалось получить картинку! "+ex.Message);
                    image = null;
                }
            }
        }

        private void ProjectListItemClick(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListBox).SelectedItem as OpenedProject;
            if (item == null) return;
            OpenProjectFromPath(item.Path);
        }
    }
}
