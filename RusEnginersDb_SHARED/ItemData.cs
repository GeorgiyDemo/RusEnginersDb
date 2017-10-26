using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RusEnginersDb_SHARED
{
    //В этом файле содержатся все классы для хранения примитивов (элемент, производитель)

    class ServerToClientNameConverter : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            typeName = typeName.Replace("RusEnginersDb_CLIENT", "RusEnginersDb_SERVER");
            assemblyName = assemblyName.Replace("RusEnginersDb_CLIENT", "RusEnginersDb_SERVER");
            return Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));
        }
    
    }

    class ClientToServerTypeNameConverter : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            typeName = typeName.Replace("RusEnginersDb_SERVER", "RusEnginersDb_CLIENT");
            assemblyName = assemblyName.Replace("RusEnginersDb_SERVER", "RusEnginersDb_CLIENT");
            return Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));
        }
    }

    [Serializable]
    public class ItemListArchive
    {
        public ItemListArchive(Item[] itemarr, Manufacturer[] manarr)
        {
            Items = new List<Item>();
            Mans = new List<Manufacturer>();

            foreach(var item in itemarr)
            {
                Items.Add(item);
            }

            foreach(var item in manarr)
            {
                Mans.Add(item);
            }
        }
        public List<Item> Items { get; private set; }
        public List<Manufacturer> Mans { get; private set; }
    }

    [Serializable]
    public class Manufacturer : IComparable
    {
        public Manufacturer(Bitmap icon, string name)
        {
            Init(icon, name, 0, "нет информации");
        }

        public Manufacturer(Bitmap icon, string name, int rate)
        {
            Init(icon, name, rate, "нет информации");
        }

        public Manufacturer(Bitmap icon, string name, int rate, string info)
        {
            Init(icon, name, rate, info);
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

    [Serializable]
    public class Item : IComparable, ICloneable
    {

        public Item(string name, string category, string subcategory, string man, int price, int rate, int delivery, string info, Bitmap icon)
        {
            Bitmap[] noBitmaps = { };
            Dictionary<string, int> nooptions = new Dictionary<string, int>();
            Link[] nolist = { };
            Init(name, category, subcategory, man, price, rate, delivery, info, icon, noBitmaps, nooptions, nolist);
        }

        public Item(string name, string category, string subcategory, string man, int price, int rate, int delivery, string info, Bitmap icon, Link[] links)
        {
            Bitmap[] noBitmaps = { };
            Dictionary<string, int> nooptions = new Dictionary<string, int>();
            Init(name, category, subcategory, man, price, rate, delivery, info, icon, noBitmaps, nooptions,links);
        }

        public Item(string name, string category, string subcategory, string man, int price, int rate, int delivery, string info, Bitmap icon, Bitmap[] Bitmaps, Link[] links)
        {
            Dictionary<string, int> nooptions = new Dictionary<string, int>();
            Init(name, category, subcategory, man, price, rate, delivery, info, icon, Bitmaps, nooptions, links);
        }

        public Item(string name, string category, string subcategory, string man, int price, int rate, int delivery, string info, Bitmap icon, Bitmap[] Bitmaps, Dictionary<string, int> options, Link[] links)
        {
            Init(name, category, subcategory, man, price, rate, delivery, info, icon, Bitmaps, options, links);
        }

        private void Init(string name, string category, string subcategory, string man, int price, int rate, int delivery, string info, Bitmap icon, Bitmap[] Bitmaps, Dictionary<string, int> options, Link[] links)
        {
            this.bitmaps = new List<Bitmap>();
            this.links = new List<Link>();

            this.Bitmap = icon;
            this.Name = name;
            this.Category = category;
            this.Subcategory = subcategory;
            this.Manufacturer = man;
            this.Price = price;
            this.Bitmaps = Bitmaps;
            this.Options = options;
            this.Rate = rate;
            this.Delivery = delivery;
            this.Info = info;
            this.Links = links;
        }

        public int CompareTo(object obj)
        {
            return String.Compare(this.Category + " " + this.Subcategory, (obj as Item).Category + " " + (obj as Item).Subcategory);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public Bitmap Bitmap { get; private set; }

        public string Name { get; private set; }
        public string Category { get; private set; }
        public string Subcategory { get; private set; }
        public string Manufacturer { get; private set; }
        public int Price { get; private set; }
        public int Rate { get; private set; }
        public int Delivery { get; private set; }
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
            private set
            {
                bitmaps.Clear();
                foreach (var item in value)
                {
                    bitmaps.Add(item);
                }
            }
        }

        List<Link> links;
        public Link[] Links
        {
            get
            {
                return links.ToArray();
            }
            private set
            {
                links.Clear();
                foreach(var item in value)
                {
                    links.Add(item);
                }
            }
        }
    }

    [Serializable]
    public class Link
    {
        public Link(string title,string url)
        {
            this.Title = title;
            this.Url = url;
        }

        public string Title { get; private set; }
        public string Url { get; private set; }
    }
}
