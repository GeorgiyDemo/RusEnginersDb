using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RusEnginersDb_SHARED
{
    //В данном файле описываются все структуры проекта

    [Serializable]
    public class Project
    {
        public Project(string Name, string Comment, Bitmap Bitmap)
        {
            History = new List<HistoryItem>();

            //Инициализация списка
            CurrentList = new ObservableCollection<Item>();

            this.Name = Name;
            this.Comment = Comment;
            this.Bitmap = Bitmap;
        }

        public void SaveItemsToHistory(string comment)
        {
            History.Add(new HistoryItem(comment, DateTime.Now, CurrentList.ToArray()));
        }

        public string Name { get; private set; }
        public string Comment { get; private set; }
        public Bitmap Bitmap { get; private set; }

        public List<HistoryItem> History { get; private set; }
        public ObservableCollection<Item> CurrentList { get; set; }
    }

    [Serializable]
    public class HistoryItem
    {
        public HistoryItem(string Comment, DateTime ItemTime, Item[] ItemList)
        {
            this.ItemList = new ObservableCollection<Item>();

            this.ItemTime = ItemTime;
            this.Comment = Comment;
            foreach(var item in ItemList)
            {
                this.ItemList.Add(item);
            }
        }
        public DateTime ItemTime { get; private set; }
        public string Comment { get; private set; }
        public ObservableCollection<Item> ItemList { get; private set; }
    }
}
