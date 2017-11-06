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
using System.IO;
using Microsoft.Win32;
using Microsoft.VisualBasic;
using System.Runtime.Serialization.Formatters.Binary;

namespace RusEnginersDb
{
    /// <summary>
    /// Логика взаимодействия для SaveManager.xaml
    /// </summary>
    public partial class SaveManager : Window
    {
        static string savepath=null;
        public static void SetSavePath(string path)
        {
            savepath = path;
        }

        Project proj;

        public SaveManager(Project proj)
        {
            InitializeComponent();

            this.proj = proj;

            if (proj.History.Count == 0)
            {
                foreach (var item in proj.CurrentList)
                {
                    SaveInfo.Items.Add("+ "+item.Name);
                }
            }
            else
            {

                //Найдем удаленные
                List<Item> oldlist = new List<Item>();
                foreach(var item0 in proj.History.Last().ItemList)
                {
                    bool found = false;
                    foreach(var item1 in proj.CurrentList)
                    {
                        if (item0.Name == item1.Name)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found) oldlist.Add(item0);
                }

                List<Item> newlist = new List<Item>();
                //Найдем добавленные
                foreach (var item0 in proj.CurrentList)
                {
                    bool found = false;
                    foreach (var item1 in proj.History.Last().ItemList)
                    {
                        if (item0.Name == item1.Name)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found) newlist.Add(item0);
                }

                //Вывод
                foreach(var item in oldlist)
                {
                    SaveInfo.Items.Add("- "+item.Name);
                }
                foreach (var item in newlist)
                {
                    SaveInfo.Items.Add("+ " + item.Name);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Сохранить проект
            proj.SaveItemsToHistory(ItemComment.Text);
            if (savepath == null)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.Filter = "BINARY (*.bin)|*.bin";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.ShowDialog();

                if (!string.IsNullOrWhiteSpace(saveFileDialog1.FileName))
                {
                    savepath = saveFileDialog1.FileName;
                }
                else
                {
                    Interaction.MsgBox("Не указано имя файла!");
                }
            }

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(savepath, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, proj);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                Interaction.MsgBox("Неудается провести сериализацию: " + ex.Message);
                return;
            }

            Interaction.MsgBox("Сохранено!");
            Close();
        }
    }
}
