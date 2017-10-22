using RusEnginersDb_SHARED;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;

namespace RusEnginersDb_SERVER
{
    /// <summary>
    /// Логика взаимодействия для AddItemWindow.xaml
    /// </summary>
    public partial class AddItemWindow : Window
    {
        Bitmap logo;
        ObservableCollection<Bitmap> Bitmaps;
        ObservableCollection<Link> Links;
        Dictionary<string, int> Options;

        public Item RetItem { get; private set; }

        public AddItemWindow()
        {
            logo = App.GetBitmap(new BitmapImage(new Uri("pack://application:,,,/RusEnginersDb_SERVER;component/Images/DefaultIcon.png")));
            Bitmaps = new ObservableCollection<Bitmap>();
            Options = new Dictionary<string, int>();
            Links = new ObservableCollection<Link>();

            InitializeComponent();
            ItemMan.ItemsSource = App.SData.GetManufacturerArray();
            ItemCategory.ItemsSource = App.SData.GetCategoryArray();
            ItemImageList.ItemsSource = Bitmaps;
            ItemLinksList.ItemsSource = Links;
        }

        private void ChooseLogo(object sender, RoutedEventArgs e)
        {
            App.GetBitmap(out logo);
            ItemBitmap.Source = App.ConvertToBitmapSource(logo);
        }

        private void UpdateSubcategory(object sender, SelectionChangedEventArgs e)
        {
            ItemSubcategory.ItemsSource = App.SData.GetSubcategoryArray(ItemCategory.SelectedItem as string);
        }

        private void AddThisItem(object sender, RoutedEventArgs e)
        {
            try
            {
                int price;
                int rate;
                int d;
                if (!int.TryParse(ItemPrice.Text, out price)) throw new Exception("Сумма это число");
                if (!int.TryParse(ItemPrice.Text, out rate)) throw new Exception("Оценка это число");
                if (!int.TryParse(ItemPrice.Text, out d)) throw new Exception("Доставка это число");
                Item i = new Item(ItemName.Text, ItemCategory.Text, ItemSubcategory.Text, ItemMan.Text, price, rate, d, ItemComment.Text, logo, Bitmaps.ToArray(),Options, Links.ToArray());
                App.SData.Items.Add(i);
            }
            catch(Exception ex)
            {
                Interaction.MsgBox("Произошла ошибка: " + ex.Message);
                return;
            }
            Close();
        }

        private void RemoveImage(object sender, RoutedEventArgs e)
        {
            var item = ItemImageList.SelectedItem as Bitmap;
            if (item == null) return;
            Bitmaps.Remove(item);
        }

        private void AddImage(object sender, RoutedEventArgs e)
        {
            Bitmap tmp;
            App.GetBitmap(out tmp);
            Bitmaps.Add(tmp);
        }

        private void AddLink(object sender, RoutedEventArgs e)
        {
            Links.Add(new Link(Interaction.InputBox("Название", "Введите НАЗВАНИЕ"), Interaction.InputBox("Ссылка", "Введите ССЫЛКУ")));
        }

        private void RemoveLink(object sender, RoutedEventArgs e)
        {
            var item = ItemLinksList.SelectedItem as Link;
            if (item == null) return;
            Links.Remove(item);
        }

        private void LinkClicked(object sender, RoutedEventArgs e)
        {
            var item = (ItemLinksList.SelectedItem as Link).Url;
            if (item == null) return;
            System.Diagnostics.Process.Start(item);
        }

        private void AddOption(object sender, RoutedEventArgs e)
        {
            try
            {
                string key = Interaction.InputBox("Название", "Введите НАЗВАНИЕ");
                int v;
                if (!int.TryParse(Interaction.InputBox("Число", "Введите ЗНАЧЕНИЕ(число)"), out v)) throw new Exception("Неверное число!");
                Options.Add(key,v);
            }
            catch(Exception ex)
            {
                Interaction.MsgBox("Произошла ошибка: " + ex.Message);
            }
        }

        private void RemoveOption(object sender, RoutedEventArgs e)
        {
            var item = ItemLinksList.SelectedItem as String;
            if (item == null) return;
            Options.Remove(item);
        }
    }
}
