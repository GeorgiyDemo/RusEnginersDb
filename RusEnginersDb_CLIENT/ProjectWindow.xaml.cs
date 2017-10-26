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
using Microsoft.VisualBasic;
using System.Windows.Media.Animation;
using RusEnginersDb_SHARED;

namespace RusEnginersDb_CLIENT
{
    /// <summary>
    /// Логика взаимодействия для ProjectWindow.xaml
    /// </summary>
    public partial class ProjectWindow : Window
    {
        private static Project project;
        public static void AddItem(Item i)
        {
            project.CurrentList.Add(i);
        }

        bool IsMenuOpened = false;

        public ProjectWindow(Project proj)
        {
            project = proj;
            Init();
        }

        public ProjectWindow(Bitmap Image, string Name, string Comment)
        {
            //Создать новый проект
            project = new Project(Name, Comment, Image);
            Init();
        }

        public ProjectWindow(string Name,string Comment)
        {
            project = new Project(Name, Comment, App.GetBitmap(new BitmapImage(new Uri("pack://application:,,,/RusEnginersDb;component/Images/DefaultIcon.png"))));
            Init();
        }

        private void Init()
        {
            //Выполнять инициализацию формы тут из-за разных конструкторов

            do
            {
                DatabaseManagerWindow dbmw = new DatabaseManagerWindow();
                dbmw.ShowDialog();
            }
            while (App.Db.Item.Count == 0 || App.Db.Man.Count == 0);

            InitializeComponent();

            ProjectBitmap.Source = App.ConvertToBitmapSource(project.Bitmap);
            ProjectName.Text = project.Name;
            ProjectComment.Text = project.Comment;

            if (project.CurrentList.Count == 0)
            {
                project.CurrentList.Add(App.Db.Item.First());
            }
            ItemList.ItemsSource = project.CurrentList;

            /*FileStream fs = new FileStream(@"C:\tmp\1.bin", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(fs, project);
            fs.Close();*/

            ItemInfo.ShowItemInfo(project.CurrentList.First());
        }

        private void ItemClick()
        {
            var selected = ItemList.SelectedItem as Item;
            if (selected == null) return;

            ItemInfo.ShowItemInfo(selected);
        }

        public void MenuItemClick()
        {
            if (IsMenuOpened)
            {
                Storyboard sb = Resources["sbHideLeftMenu"] as Storyboard;
                sb.Begin(SlideMenu);
            }
            else
            {
                Storyboard sb = Resources["sbShowLeftMenu"] as Storyboard;
                sb.Begin(SlideMenu);
            }

            IsMenuOpened = !IsMenuOpened;
        }

        private void Search()
        {
            SearchItemWindow siw = new SearchItemWindow(App.Db.GetItemList().ToArray());
            siw.ShowDialog();
        }

        private void Manufacturer()
        {
            GetItemsByManWindow gibmw = new GetItemsByManWindow();
            gibmw.ShowDialog();
        }

        private void Category()
        {
            GetItemsByCategoryWindow gibcw = new GetItemsByCategoryWindow();
            gibcw.ShowDialog();
        }

        private void AddItem()
        {
            Interaction.MsgBox("Для добавления элемента воспользуйтесь поиском из меню слева!");
        }
        private void RemoveItem()
        {
            var item = ItemList.SelectedItem;
            if (item == null) return;

            project.CurrentList.Remove(item as Item);
        }

        private void SaveMenuClick(object sender, RoutedEventArgs e)
        {
            SaveManager sm = new SaveManager(project);
            sm.ShowDialog();
        }

        private void Chart()
        {
            ChartWindow cw = new ChartWindow(project);
            cw.ShowDialog();
        }
    }
}
