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
using RusEnginersDb_SHARED;

namespace RusEnginersDb_CLIENT
{
    /// <summary>
    /// Логика взаимодействия для AddItemWindow.xaml
    /// </summary>
    public partial class AddItemWindow : Window
    {
        private Item tmp;

        public AddItemWindow(Item i)
        {
            tmp = i;
            InitializeComponent();
            ItemControl.ShowItemInfo(i);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProjectWindow.AddItem(tmp);
            Close();
        }
    }
}
