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
using Microsoft.VisualBasic;
using RusEnginersDb_SHARED;

namespace RusEnginersDb_CLIENT
{
    /// <summary>
    /// Логика взаимодействия для ItemInfoControl.xaml
    /// </summary>
    public partial class ItemInfoControl : UserControl
    {
        public void ShowItemInfo(Item item)
        {
            ItemBitmap.Source = App.ConvertToBitmapSource(item.Bitmap);

            ItemName.Text = item.Name;
            ItemCategory.Text = item.Category;
            ItemSubcategory.Text = item.Subcategory;
            ItemMan.Text = item.Manufacturer;
            ItemComment.Text = item.Info;
            ItemDelivary.Text = item.Delivery.ToString();

            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < item.Rate; i++)
            {
                sb.Append("★");
            }
            ItemRate.Text = sb.ToString();

            if (item.Bitmaps.Count() > 0)
            {
                ItemImageList.ItemsSource = item.Bitmaps;
            }
            else
            {
                ItemImageList.Height = 0;
            }

            if (item.Links.Count() > 0)
            {
                ItemLinksList.ItemsSource = item.Links;
            }
            else
            {
                ItemLinksList.Height = 0;
            }
        }

        public ItemInfoControl()
        {
            InitializeComponent();
        }

        private void LinkClicked(object sender, RoutedEventArgs e)
        {
            var item = ((sender as ListBox).SelectedItem as Link).Url;
            if (item == null) return;
            System.Diagnostics.Process.Start(item);
        }

        private void ManInfo(object sender, MouseButtonEventArgs e)
        {
            DbKeeper db = new DbKeeper();
            foreach(var item in db.Man)
            {
                if (item.Name == ItemMan.Text)
                {
                    ManInfoWindow miw = new ManInfoWindow(item);
                    miw.ShowDialog();
                    break;
                }
            }
        }
    }
}
