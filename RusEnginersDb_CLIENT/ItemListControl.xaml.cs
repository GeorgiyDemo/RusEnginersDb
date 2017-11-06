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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RusEnginersDb
{
    /// <summary>
    /// Логика взаимодействия для ItemListControl.xaml
    /// </summary>
    public partial class ItemListControl : UserControl
    {
        public Action ItemClicked { get; set; }

        public Action AddItem { get; set; }
        public Action RemoveItem { get; set; }

        public IEnumerable<Object> ItemsSource
        {
            set
            {
                ItemList.ItemsSource = value;
            }
        }

        public Object SelectedItem
        {
            get
            {
                return ItemList.SelectedItem;
            }
        }

        public ItemListControl()
        {
            InitializeComponent();
        }

        private void ItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemClicked?.Invoke();
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            AddItem?.Invoke();
        }

        private void RemoveClick(object sender, RoutedEventArgs e)
        {
            RemoveItem?.Invoke();
        }
    }
}
