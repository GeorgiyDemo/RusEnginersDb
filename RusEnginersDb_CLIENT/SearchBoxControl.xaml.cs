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

namespace RusEnginersDb
{
    /// <summary>
    /// Логика взаимодействия для SearchBoxControl.xaml
    /// </summary>
    public partial class SearchBoxControl : UserControl
    {

        Action<string> search;

        public Action<string> SearchDelegate
        {
            set
            {
                search = value;
            }
        }

        private string Stext
        {
            get;set;
        }

        public SearchBoxControl()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Stext = (sender as TextBox).Text;
        }

        private void TextBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            search?.Invoke(Stext);
        }
    }
}
