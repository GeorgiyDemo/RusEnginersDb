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

namespace RusEnginersDb_SERVER
{
    /// <summary>
    /// Логика взаимодействия для ItemEditorWindow.xaml
    /// </summary>
    public partial class ItemEditorWindow : Window
    {
        public ItemEditorWindow()
        {
            InitializeComponent();

            ItemDataGrid.ItemsSource = null;
            ItemDataGrid.ItemsSource = App.SData.Items;
        }

        private void RemoveItem(object sender, RoutedEventArgs e)
        {
            var item = ItemDataGrid.SelectedItem as Item;
            if (item == null) return;
            App.SData.Items.Remove(item);
        }

        private void AddItem(object sender, RoutedEventArgs e)
        {
            AddItemWindow aiw = new AddItemWindow();
            aiw.ShowDialog();
            var item = aiw.RetItem;
            if (item == null) return;
            App.SData.Items.Add(item);
        }
    }
}
