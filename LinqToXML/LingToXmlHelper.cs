using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqToXML
{
    public static class LingToXmlHelper
    {
        readonly static string fileName = AppDomain.CurrentDomain.BaseDirectory + "{0}.xml";

        public static void Add<T>(this T obj) where T : class
        {
            Type type = typeof(T);
            string _fileName = string.Format(fileName, type.Name);
            XMLFactory<T> factory = new XMLFactory<T>(_fileName);
            factory.Add(obj);
        }

        public static void Update<T>(this T obj) where T : class
        {
            Type type = typeof(T);
            string _fileName = string.Format(fileName, type.Name);
            XMLFactory<T> factory = new XMLFactory<T>(_fileName);
            factory.Update(obj);
        }

        public static void Delete<T>(this T obj, int id) where T : class
        {
            Type type = typeof(T);
            string _fileName = string.Format(fileName, type.Name);
            XMLFactory<T> factory = new XMLFactory<T>(_fileName);
            factory.Delete(id);
        }

        public static T Get<T>(this T obj, int id) where T : class
        {
            Type type = typeof(T);
            string _fileName = string.Format(fileName, type.Name);
            XMLFactory<T> factory = new XMLFactory<T>(_fileName);
            return factory.GetByID(id);
        }

        public static List<T> GetAll<T>(this T obj) where T : class
        {
            Type type = typeof(T);
            string _fileName = string.Format(fileName, type.Name);
            XMLFactory<T> factory = new XMLFactory<T>(_fileName);
            return factory.GetAll();
        }
    }
}
