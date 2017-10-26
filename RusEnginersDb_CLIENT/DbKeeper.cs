using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RusEnginersDb_SHARED;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Microsoft.VisualBasic;

namespace RusEnginersDb_CLIENT
{
    public class XamlDbProvider
    {
        public static HashSet<string> GetParamsList()
        {
            return App.Db.GetParamsList();
        }
    }

    public class DbKeeper
    {
        private static bool IsInitComplate=false;

        private readonly object manlock = new Object();
        private List<Manufacturer> manlist = null;

        private readonly object itemlock = new Object();
        private List<Item> itemlist = null;

        public string LastPath
        {
            get
            {
                string temppath = System.IO.Path.GetTempPath() + "rusenginersdb_path.bin";
                if (File.Exists(temppath))
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    try
                    {
                        using (FileStream fs = new FileStream(temppath, FileMode.Open))
                        {
                            string ret = (string)formatter.Deserialize(fs);
                            return ret;
                        }
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                return null;
            }
            private set
            {
                string temppath = System.IO.Path.GetTempPath() + "rusenginersdb_path.bin";
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (FileStream fs = new FileStream(temppath, FileMode.OpenOrCreate))
                    {
                        formatter.Serialize(fs, value);
                    }
                }
                catch (Exception) { }
            }
        }
        

        public bool LoadFromPath(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);
            try
            {
                ItemListArchive sdata = (ItemListArchive)formatter.Deserialize(fs);
                Item = sdata.Items;
                Man = sdata.Mans;
                LastPath = path;
                return true;
            }
            catch (Exception ex)
            {
                Interaction.MsgBox("Не удается десериализовать: " + ex.Message);
            }
            fs.Close();
            return false;
        }

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
            if (IsInitComplate == false)
            {
                Man = new List<Manufacturer>();
                Item = new List<Item>();
                IsInitComplate = true;
            }
            else
            {
                throw new Exception("Экземпляр DbKeeper должен быть только один! Используйте App.Db!");
            }
        }
    }
}
