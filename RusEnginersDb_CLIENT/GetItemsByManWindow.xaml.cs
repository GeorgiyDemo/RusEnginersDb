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
    /// Логика взаимодействия для GetItemsByManWindow.xaml
    /// </summary>
    public partial class GetItemsByManWindow : Window
    {
        List<Manufacturer> mlist;
        DbKeeper db;

        public GetItemsByManWindow()
        {
            InitializeComponent();
            db = new DbKeeper();
            mlist = db.GetManList();
            mlist.Sort();

            ManList.ItemsSource = mlist;
        }

        private void ManList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = ManList.SelectedItem as Manufacturer;
            if (selected == null) return;

            List<Item> tmp = new List<Item>();

            foreach(var item in db.GetItemList())
            {
                if (item.Manufacturer == selected.Name)
                {
                    tmp.Add(item);
                }
            }

            SearchItemWindow siw = new SearchItemWindow(tmp.ToArray());
            Hide();
            siw.ShowDialog();
            Close();
        }
    }
}
