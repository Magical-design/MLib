using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MLib
{

        public static class WindowManager
        {
            private static Hashtable _RegisterWindow = new Hashtable();

            public static void Regiter<T>(string key)
            {
                _RegisterWindow.Add(key, typeof(T));
            }
            public static void Regiter(string key, Type t)
            {
                if (!_RegisterWindow.ContainsKey(key))
                    _RegisterWindow.Add(key, t);
            }

            public static void Remove(string key)
            {
                if (_RegisterWindow.ContainsKey(key))
                    _RegisterWindow.Remove(key);
            }

            public static void ShowDialog(string key, object VM)
            {
                if (!_RegisterWindow.ContainsKey(key))
                {
                    throw (new Exception("没有注册此键！"));
                }

                var win = (Window)Activator.CreateInstance((Type)_RegisterWindow[key]);
                win.DataContext = VM;
                win.ShowDialog();
            }

        }
    public static class WindowExt
    {
        public static void Register(this Window win, string key)
        {
            WindowManager.Regiter(key, win.GetType());
        }

        public static void Register(this Window win, string key, Type t)
        {
            WindowManager.Regiter(key, t);
        }

        public static void Register<T>(this Window win, string key)
        {
            WindowManager.Regiter<T>(key);
        }
        public static void Remove<T>(this Window win, string key)
        {
            WindowManager.Remove(key);
        }
    }
}
