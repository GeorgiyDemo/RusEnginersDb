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
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using RusEnginersDb_SHARED;

namespace RusEnginersDb_CLIENT
{
    /// <summary>
    /// Логика взаимодействия для SearchItemWindow.xaml
    /// </summary>
    public partial class SearchItemWindow : Window
    {
        class SearchParam : INotifyPropertyChanged
        {
            public SearchParam(string param, string sign, int value)
            {
                this.param = param;
                this.sign = sign;
                this.v = value;
            }

            string param;
            public string Param {
                get
                {
                    return param;
                }
                set
                {
                    param = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Param"));
                }
            }

            string sign;
            public string Sign
            {
                get
                {
                    return sign;
                }
                set
                {
                    sign = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sign"));
                }
            }

            int v;
            public int Value
            {
                get
                {
                    return v;
                }
                set
                {
                    v = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        ObservableCollection<SearchParam> SearchParams = new ObservableCollection<SearchParam>();
        Item[] DefaultList = null;
        ObservableCollection<Item> AfterSearchList = new ObservableCollection<Item>();

        void ReloadList()
        {
            AfterSearchList.Clear();
            foreach(var item in DefaultList)
            {
                AfterSearchList.Add(item);
            }
        }

        void SortList()
        {
            List<Item> tmp = new List<Item>();
            foreach(var item in AfterSearchList)
            {
                tmp.Add(item);
            }
            tmp.Sort();
            AfterSearchList.Clear();
            foreach(var item in tmp)
            {
                AfterSearchList.Add(item);
            }
        }

        public SearchItemWindow(Item[] arr)
        {
            InitializeComponent();

            SearchParams.Add(new SearchParam("Не задано", "=", 100));

            SearchParamsDataGrid.ItemsSource = SearchParams;

            DefaultList = arr;
            ReloadList();

            ItemList.ItemsSource = AfterSearchList;
        }

        private void SearchTextChanged(string text)
        {
            AfterSearchList.Clear();

            if (String.IsNullOrWhiteSpace(text))
            {
                ReloadList();
                return;
            }

            foreach(var item in DefaultList)
            {
                StringBuilder s = new StringBuilder();
                s.Append(item.Name);
                s.Append(item.Manufacturer);
                s.Append(item.Category);
                s.Append(item.Subcategory);

                if (s.ToString().ToLower().IndexOf(text.ToLower()) > -1)
                {
                    AfterSearchList.Add(item);
                }
            }

            SortList();
        }

        private void AddSearchClick(object sender, RoutedEventArgs e)
        {
            SearchParams.Add(new SearchParam("Не задано", "=", 100));
        }

        private void RemoveSearchClick(object sender, RoutedEventArgs e)
        {
            var item = SearchParamsDataGrid.SelectedItem as SearchParam;
            if (item == null) return;
            SearchParams.Remove(item);
        }

        
    }

    
}
