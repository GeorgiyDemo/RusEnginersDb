using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RusEnginersDb_SHARED;
using System.Drawing;

namespace RusEnginersDb_CLIENT
{
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
                foreach (var item1 in itemlist)
                {
                    foreach (var item2 in item1.Options)
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
            if (manlist == null)
            {
                manlist = new List<Manufacturer>();
                itemlist = new List<Item>();
                Item.Add(
                    new Item("GX960", "Привод", "ДВС",
                    "Honda", 1000, 5, 60, "lolka",
                    new Bitmap(@"C:\1.jpg"),  //Иконка
                    new Bitmap[] { new Bitmap(@"C:\1.jpg"), new Bitmap(@"C:\1.jpg"), new Bitmap(@"C:\1.jpg"), new Bitmap(@"C:\1.jpg") },//Массив картинок
                    new Link[] { new Link("Yandex","http://yandex.ru"), new Link("Скайп","skype://")}
                ));

                Man.Add(new Manufacturer(new Bitmap(@"C:\1.jpg"), "Honda"));

            }
        }
    }
}
