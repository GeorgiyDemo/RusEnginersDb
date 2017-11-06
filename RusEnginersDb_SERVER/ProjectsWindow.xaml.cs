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
    /// Логика взаимодействия для ProjectsWindow.xaml
    /// </summary>
    public partial class ProjectsWindow : Window
    {
        public ProjectsWindow()
        {
            InitializeComponent();
            ProjectListBox.ItemsSource = App.SData.Projects;
        }

        private void ProjectListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = ProjectListBox.SelectedIndex;
            if (item == -1) return;
            System.Diagnostics.Process.Start("http://127.0.0.1/?Project="+App.SData.Projects.ElementAt(item).Key);
        }
    }
}
