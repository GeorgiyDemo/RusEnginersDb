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
    /// Логика взаимодействия для GetItemsByCategoryWindow.xaml
    /// </summary>
    public partial class GetItemsByCategoryWindow : Window
    {
        HashSet<string> categorylist=new HashSet<string>();
        HashSet<string> subcategorylist = new HashSet<string>();

        List<Item> tmp;

        public GetItemsByCategoryWindow()
        {
            InitializeComponent();

            DbKeeper db = new DbKeeper();
            
            tmp = db.GetItemList();

            foreach(var item in tmp)
            {
                categorylist.Add(item.Category);
            }

            CategotyListBox.ItemsSource = categorylist;
        }

        private void CategoryChoosed(object sender, SelectionChangedEventArgs e)
        {
            var selected = CategotyListBox.SelectedItem;
            if (selected == null) return;

            subcategorylist.Clear();

            foreach(var item in tmp)
            {
                if(item.Category == selected.ToString())
                {
                    subcategorylist.Add(item.Subcategory);
                }
            }

            SubcategoryListBox.ItemsSource = null;
            SubcategoryListBox.ItemsSource = subcategorylist;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CategotyListBox.SelectedItem == null) return;

            string category = CategotyListBox.SelectedItem.ToString();
            string subcategory = (SubcategoryListBox.SelectedItem!=null)?SubcategoryListBox.SelectedItem.ToString():String.Empty;

            for(int i = tmp.Count - 1; i < 0; i--)
            {
                if (tmp[i].Category != category)
                {
                    tmp.RemoveAt(i);
                }
                if (subcategory != String.Empty && tmp[i].Subcategory != subcategory)
                {
                    tmp.RemoveAt(i);
                }
            }

            Hide();
            SearchItemWindow siw = new SearchItemWindow(tmp.ToArray());
            siw.ShowDialog();
            Close();
        }
    }
}
