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

namespace RusEnginersDb_CLIENT
{
    /// <summary>
    /// Логика взаимодействия для ChooseProject.xaml
    /// </summary>
    public partial class ChooseProject : Window
    {
        Bitmap image = null;

        public ChooseProject()
        {
            InitializeComponent();
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
                Project proj;
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
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
                SaveManager.SetSavePath(openFileDialog1.FileName);

                ProjectWindow pw = new ProjectWindow(proj);
                Hide();
                pw.ShowDialog();
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
    }
}
