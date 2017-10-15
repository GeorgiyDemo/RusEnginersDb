using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RusEnginersDb_SHARED
{
    //В этом файле содержатся все классы для хранения примитивов (элемент, производитель)
    class DbKeeper
    {
        private static readonly object manlock = new Object();
        private static List<Manufacturer> manlist = null;

        private static readonly object itemlock = new Object();
        private static List<Item> itemlist = null;

        public List<Manufacturer> GetManList()
        {
            return Man;
        }

        public List<Item> GetItemList()
        {
            return Item;
        }

        public HashSet<string> GetParamsList()
        {
            HashSet<string> temp = new HashSet<string>();
            lock (itemlock)
            {
                foreach(var item1 in itemlist)
                {
                    foreach(var item2 in item1.Options)
                    {
                        temp.Add(item2.Key);
                    }
                }
            }
            temp.Add("Не задано");
            return temp;
        }

        public List<Manufacturer> Man
        {
            get
            {
                return manlist;
            }
            set
            {
                lock (manlock)
                {
                    manlist = value;
                }
            }
        }

        public List<Item> Item
        {
            get
            {
                return itemlist;
            }
            set
            {
                lock (itemlock)
                {
                    itemlist = value;
                }
            }
        }

        public Manufacturer GetManufacturerByName(string name)
        {
            foreach (var item in manlist)
            {
                if (item.Name.ToLower() == name.ToLower()) return item;
            }
            return null;
        }

        public DbKeeper()
        {
            //Инициализация с подгрузкой из бд
            if (manlist == null) manlist = new List<Manufacturer>();
            if (itemlist == null) itemlist = new List<Item>();
        }
    }

    public class Manufacturer : IComparable
    {
        public Manufacturer(Bitmap icon, string name)
        {
            Init(icon, name, 0, "нет информации");
        }

        private void Init(Bitmap icon, string name, int rate, string info)
        {
            this.Bitmap = icon;
            this.Name = name;
            this.Rate = rate;
            this.Info = info;
        }

        public int CompareTo(object obj)
        {
            return String.Compare(this.Name, (obj as Manufacturer).Name);
        }

        public Bitmap Bitmap { get; private set; }
        public string Name { get; private set; }
        public int Rate { get; private set; }
        public string Info { get; private set; }
    }

    public class Item : IComparable
    {

        public Item(string name, string category, string subcategory, string man, int price, int rate, string info, Bitmap icon)
        {
            Bitmap[] noBitmaps = { };
            Dictionary<string, int> nooptions = new Dictionary<string, int>();
            Init(name, category, subcategory, man, price, rate, info, icon, noBitmaps, nooptions);
        }

        public Item(string name, string category, string subcategory, string man, int price, int rate, string info, Bitmap icon, Bitmap[] Bitmaps)
        {
            Dictionary<string, int> nooptions = new Dictionary<string, int>();
            Init(name, category, subcategory, man, price, rate, info, icon, Bitmaps, nooptions);
        }

        public Item(string name, string category, string subcategory, string man, int price, int rate, string info, Bitmap icon, Bitmap[] Bitmaps, Dictionary<string, int> options)
        {
            Init(name, category, subcategory, man, price, rate, info, icon, Bitmaps, options);
        }

        private void Init(string name, string category, string subcategory, string man, int price, int rate, string info, Bitmap icon, Bitmap[] Bitmaps, Dictionary<string, int> options)
        {
            this.bitmaps = new List<Bitmap>();

            this.Bitmap = icon;
            this.Name = name;
            this.Category = category;
            this.Subcategory = subcategory;
            this.Manufacturer = man;
            this.Price = price;
            this.Bitmaps = Bitmaps;
            this.Options = options;
            this.Rate = rate;
            this.Info = info;
        }

        public int CompareTo(object obj)
        {
            return String.Compare(this.Category + " " + this.Subcategory, (obj as Item).Category + " " + (obj as Item).Subcategory);
        }

        public Bitmap Bitmap { get; private set; }

        public string Name { get; private set; }
        public string Category { get; private set; }
        public string Subcategory { get; private set; }
        public string Manufacturer { get; private set; }
        public int Price { get; private set; }
        public int Rate { get; private set; }
        public string Info { get; private set; }

        Dictionary<string, int> options;
        public Dictionary<string, int> Options
        {
            get
            {
                Dictionary<string, int> temp = options; 
                return temp;
            }
            private set
            {
                options = value;
            }
        }

        List<Bitmap> bitmaps;

        public Bitmap[] Bitmaps
        {
            get
            {
                return bitmaps.ToArray();
            }
            set
            {
                bitmaps.Clear();
                foreach (var item in value)
                {
                    bitmaps.Add(item);
                }
            }
        }
    }
}
