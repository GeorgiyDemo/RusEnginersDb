using RusEnginersDb_SHARED;
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
    /// Логика взаимодействия для ManEditorWindow.xaml
    /// </summary>
    public partial class ManEditorWindow : Window
    {
        public ManEditorWindow()
        {
            InitializeComponent();
            ManDataGrid.ItemsSource = App.SData.Mans;
        }

        private void RemoveMan(object sender, RoutedEventArgs e)
        {
            var item = ManDataGrid.SelectedItem as Manufacturer;
            if (item == null) return;
            App.SData.Mans.Remove(item);
        }

        private void AddMan(object sender, RoutedEventArgs e)
        {
            AddManWindow amw = new AddManWindow();
            amw.ShowDialog();
            if (amw.RetMan == null) return;
            App.SData.Mans.Add(amw.RetMan);
        }
    }
}
