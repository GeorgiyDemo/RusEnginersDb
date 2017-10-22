using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RusEnginersDb_SHARED;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace RusEnginersDb_SERVER
{

    [Serializable]
    public class ServerData
    {
        static bool IsInitComplete = false;

        //Первичный список
        [Serializable]
        private class CategoryItemForBegin
        {
            public CategoryItemForBegin(string c, string s)
            {
                this.Category = c;
                this.Subcategory = s;
            }
            public string Category { get; private set; }
            public string Subcategory { get; private set; }
        }

        readonly List<CategoryItemForBegin> DefaultCategories = new List<CategoryItemForBegin>
        {
            new CategoryItemForBegin("Привод", "Электродвигатель"),
            new CategoryItemForBegin("Привод", "Двигатель внутреннего сгорания"),
            new CategoryItemForBegin("Привод", "Гидромотор"),
            new CategoryItemForBegin("Привод", "Пневмопривод"),

            new CategoryItemForBegin("Насос", "Центробежный"),
            new CategoryItemForBegin("Насос", "Вакуумный водокольцевой"),
            new CategoryItemForBegin("Насос", "Поршневой"),
            new CategoryItemForBegin("Насос", "Плунжерный"),
            new CategoryItemForBegin("Насос", "Шнековый"),
            new CategoryItemForBegin("Насос", "Перистальтический"),
            new CategoryItemForBegin("Насос", "Шестеренный"),
            new CategoryItemForBegin("Насос", "Аксиально-поршневой"),

            new CategoryItemForBegin("Генератор", "Синхронный"),
            new CategoryItemForBegin("Генератор", "Асинхронный"),


            new CategoryItemForBegin("Клапан", "Предохранительный"),
            new CategoryItemForBegin("Клапан", "Регулировочный"),
            new CategoryItemForBegin("Клапан", "Игольчатый (дроссельный)"),

            new CategoryItemForBegin("Гидравлический распределитель", "Многоблочный"),
            new CategoryItemForBegin("Гидравлический распределитель", "Многосекционный"),

            new CategoryItemForBegin("Шаровой кран ВД", "Двухходовый"),
            new CategoryItemForBegin("Шаровой кран ВД", "Трехходовый"),

            new CategoryItemForBegin("Шланговый барабан", "Ручной"),
            new CategoryItemForBegin("Шланговый барабан", "С электроприводом"),
            new CategoryItemForBegin("Шланговый барабан", "С гидроприводом"),
            new CategoryItemForBegin("Шланговый барабан", "С автонамотчиком"),

            new CategoryItemForBegin("Форсунок", "Очистной"),
            new CategoryItemForBegin("Форсунок", "Каналопромывочный"),
            new CategoryItemForBegin("Форсунок", "Гидрообразивный"),
            new CategoryItemForBegin("Форсунок", "Увлажение и туманообразнование"),
            new CategoryItemForBegin("Форсунок", "Пожаротушение"),

            new CategoryItemForBegin("Частотный преобразователь", "Непосредственный"),
            new CategoryItemForBegin("Частотный преобразователь", "Двухзвенный"),

            new CategoryItemForBegin("Компрессор", "Винтовой (ротационный)"),
            new CategoryItemForBegin("Компрессор", "Поршневой"),
        };

        private ObservableCollection<Item> itemlist=null;
        private Object itemlistlock=null;
        public ObservableCollection<Item> Items
        {
            get
            {
                return itemlist;
            }
            set
            {
                lock (itemlistlock)
                {
                    itemlist = value;
                    BindingOperations.EnableCollectionSynchronization(itemlist, itemlistlock);
                }
            }
        }

        private ObservableCollection<Manufacturer> manlist = null;
        private Object manlistlock = null;
        public ObservableCollection<Manufacturer> Mans
        {
            get
            {
                return manlist;
            }
            set
            {
                lock (manlistlock)
                {
                    manlist = value;
                    BindingOperations.EnableCollectionSynchronization(manlist, manlistlock);
                }
            }
        }

        public String[] GetManufacturerArray()
        {
            HashSet<string> ret = new HashSet<string>();
            lock (manlistlock)
            {
                foreach(var item in manlist)
                {
                    ret.Add(item.Name);
                }
            }
            return ret.ToArray();
        }

        public String[] GetCategoryArray()
        {
            HashSet<string> ret = new HashSet<string>();
            lock (itemlistlock)
            {
                foreach(var item in itemlist)
                {
                    ret.Add(item.Category);
                }
            }

            foreach(var item in DefaultCategories)
            {
                ret.Add(item.Category);
            }

            return ret.ToArray();
        }

        public String[] GetSubcategoryArray(string categoty)
        {
            HashSet<string> ret = new HashSet<string>();
            lock (itemlistlock)
            {
                foreach (var item in itemlist)
                {
                    if (item.Category == categoty) ret.Add(item.Subcategory);
                }
            }

            foreach (var item in DefaultCategories)
            {
                if (item.Category == categoty) ret.Add(item.Subcategory);
            }

            return ret.ToArray();
        }

        public ServerData()
        {
            if (IsInitComplete == false)
            {
                IsInitComplete = true;
                itemlist = new ObservableCollection<Item>();
                itemlistlock = new Object();
                manlist = new ObservableCollection<Manufacturer>();
                manlistlock = new Object(); 
            }
            else
            {
                throw new Exception("Уже инициализированно! Обращайтесь через App!");
            }
        }

    }
}
