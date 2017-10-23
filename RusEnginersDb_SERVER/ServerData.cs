﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RusEnginersDb_SHARED;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Net;
using System.IO;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Runtime.Serialization.Formatters.Binary;

namespace RusEnginersDb_SERVER
{

    //Именно этот класс отвечает за раздачу данных
    public class ServerDataShare
    {
        static bool IsInitComplete = false;

        private HttpListener listener;

        void WriteBinary(HttpListenerContext ctx, MemoryStream ms)
        {
            var response = ctx.Response;
            response.ContentLength64 = ms.Length;
            response.SendChunked = false;
            response.ContentType = System.Net.Mime.MediaTypeNames.Application.Octet;
            response.AddHeader("Content-disposition", "attachment; filename=data.bin");

            using (BinaryWriter sw = new BinaryWriter(response.OutputStream))
            {
                sw.Write(ms.ToArray());
            }

            response.StatusCode = (int)HttpStatusCode.OK;
            response.StatusDescription = "OK";
            response.OutputStream.Close();
        }

        void WriteText(HttpListenerContext ctx, string text)
        {
            byte[] str=Encoding.Unicode.GetBytes(text);
            MemoryStream ms = new MemoryStream();
            ms.Write(str,0, str.Length);


            var response = ctx.Response;
            response.ContentLength64 = ms.Length;
            response.SendChunked = false;
            response.ContentType = System.Net.Mime.MediaTypeNames.Text.Html;
            response.ContentEncoding = Encoding.UTF8;

            using (BinaryWriter sw = new BinaryWriter(response.OutputStream))
            {
                sw.Write(ms.ToArray());
            }

            response.StatusCode = (int)HttpStatusCode.OK;
            response.StatusDescription = "OK";
            response.OutputStream.Close();
        }

        public ServerDataShare()
        {
            if (!IsInitComplete)
            {
                listener = new HttpListener();

                try
                {
                    listener.Prefixes.Add("http://*:80/"); //http://localhost:8080/
                    listener.Start();
                }
                catch(HttpListenerException)
                {
                    //Задать запись в реестр для раздачи порта
                    ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe");
                    startInfo.Verb = "runas";
                    startInfo.Arguments = "/c echo 'Нет разрешения на раздачу данных по http. Выполнение этих комманд исправит проблему...'&netsh http add urlacl url=http://*:80/ user=".Replace("'","\"")+System.Security.Principal.WindowsIdentity.GetCurrent().Name+ "&echo 'Если выполнение этих комманд было неудачно, то обратитесь к вашему системному администратору! Программу нужно перезапустить... Для удаления этой настройки введите комманду netsh http delete urlacl url=http://*:80/'&pause".Replace("'","\"")+"&taskkill /f /im "+ Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName)+"&start "+ System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                    Process.Start(startInfo);
                    return;
                }
                

                //Инициализирует обработку запросов в новом потоке
                Task.Run(() =>
                {
                    while (true)
                    {
                        HttpListenerContext context = listener.GetContext();
                        HttpListenerRequest request = context.Request;

                        NameValueCollection parameters = request.QueryString;

                        if (!string.IsNullOrWhiteSpace(parameters["Getlist"]))
                        {
                            Task.Factory.StartNew((ctx) =>
                            {
                                BinaryFormatter formatter = new BinaryFormatter();
                                ServerData data = App.SData;
                                MemoryStream ms = new MemoryStream();
                                formatter.Serialize(ms, data);
                                WriteBinary((HttpListenerContext)ctx, ms);
                                ms.Close();
                                
                            }, context, TaskCreationOptions.LongRunning);
                            continue;
                        }
                        else
                        {
                            Task.Factory.StartNew((ctx) =>
                            {
                                BinaryFormatter formatter = new BinaryFormatter();
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    WriteText((HttpListenerContext)ctx,"<error>Check your parameters</error>");
                                }
                            }, context, TaskCreationOptions.LongRunning);
                            continue;
                        }
                        

                    }
                });
                
            }
            else
            {
                throw new Exception("Уже инициализированно! Обращайтесь через App!");
            }
        }
    }

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
