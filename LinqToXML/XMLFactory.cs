using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Reflection;
using System.IO;

namespace LinqToXML
{
    public class XMLFactory<T> where T : class
    {
        public string xmlPath { get; set; }
        Type type { get; set; }

        public XMLFactory(string _xmlPath)
        {
            xmlPath = _xmlPath;
            type = typeof(T);
            if (!File.Exists(_xmlPath))
            {
                XElement rootNode = new XElement("ArrayOf" + type.Name, "");
                rootNode.Save(xmlPath);
            }
        }

        public List<Name_Value> GetAttributeName_Value(T obj)
        {
            List<Name_Value> name_values = new List<Name_Value>();

            FieldInfo[] fieldInfos = type.GetFields();
            foreach (FieldInfo info in fieldInfos)
            {
                name_values.Add(new Name_Value() { Name = info.Name, Value = type.InvokeMember(info.Name, BindingFlags.GetField, null, obj, null), type = Types.Field });
            }

            PropertyInfo[] propertyInfos = type.GetProperties();
            foreach (PropertyInfo info in propertyInfos)
            {
                name_values.Add(new Name_Value() { Name = info.Name, Value = info.GetValue(obj, null), type = Types.Property });
            }

            return name_values;
        }

        public int GetNewID()
        {
            int id = 0;

            try
            {
                XElement rootNode = XElement.Load(xmlPath);
                IEnumerable<XElement> xelements =
                    from node in rootNode.Descendants(type.Name)
                    select node;
                List<int> IDs = xelements.Select(row => Convert.ToInt32(row.Attribute(/*type.Name +*/ "ID").Value)).ToList();

                if (xelements.Count() > 0)
                {
                    id = IDs.Max(row => row) + 1;
                }
                else
                    id = 1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
            return id;
        }

        public T CreateInstance()
        {
            Assembly asm = type.Assembly;//Assembly.GetExecutingAssembly();
            return (T)asm.CreateInstance(type.FullName);
        }

        public List<T> GetAll()
        {
            List<T> lstObj = new List<T>();
            try
            {
                XElement rootNode = XElement.Load(xmlPath);
                IEnumerable<XElement> objs =
                    from node in rootNode.Descendants(type.Name)
                    select node;

                FieldInfo[] fieldInfos = type.GetFields();
                PropertyInfo[] propertyInfos = type.GetProperties();

                foreach (XElement item in objs.ToList())
                {
                    T obj = CreateInstance();
                    foreach (FieldInfo info in fieldInfos)
                    {
                        type.InvokeMember(info.Name, BindingFlags.SetField, null, obj, new object[] { Convert.ChangeType(item.Element(info.Name).Value, info.FieldType) });
                    }
                    foreach (PropertyInfo info in propertyInfos)
                    {
                        type.InvokeMember(info.Name, BindingFlags.SetProperty, null, obj, new object[] { Convert.ChangeType(item.Attribute(info.Name).Value, info.PropertyType) });
                    }

                    lstObj.Add(obj);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return lstObj;
        }

        public T GetByID(int id)
        {
            T obj = CreateInstance();
            try
            {
                XElement rootNode = XElement.Load(xmlPath);
                IEnumerable<XElement> objs =
                    from node in rootNode.Descendants(type.Name)
                    where Convert.ToInt32(node.Attribute(/*type.Name + */"ID").Value) == id
                    select node;
                XElement item = objs.FirstOrDefault();

                if (item == null)
                    return obj;

                FieldInfo[] fieldInfos = type.GetFields();
                PropertyInfo[] propertyInfos = type.GetProperties();

                foreach (FieldInfo info in fieldInfos)
                {
                    type.InvokeMember(info.Name, BindingFlags.SetField, null, obj, new object[] { Convert.ChangeType(item.Element(info.Name).Value, info.FieldType) });
                }
                foreach (PropertyInfo info in propertyInfos)
                {
                    type.InvokeMember(info.Name, BindingFlags.SetProperty, null, obj, new object[] { Convert.ChangeType(item.Attribute(info.Name).Value, info.PropertyType) });
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return obj;
        }

        public void Add(T obj)
        {
            try
            {
                obj.SetDefaultValues();
                XElement rootNode = XElement.Load(xmlPath);
                XElement newNode = new XElement(type.Name);

                int id = GetNewID();
                PropertyInfo info = null;
                info = type.GetProperty(/*type.Name +*/ "ID");
                if (info != null)
                {
                    info.SetValue(obj, Convert.ChangeType(id, info.PropertyType), null);
                }
                else
                    throw new Exception(/*type.Name +*/ "ID 被用于做主键使用，他必须是属性且存在"/*，其命名为类名加ID！*/);

                List<Name_Value> lstSource = GetAttributeName_Value(obj);
                foreach (Name_Value item in lstSource)
                {
                    if (item.type == Types.Field)
                    {
                        newNode.Add(new XElement(item.Name, item.Value));
                    }
                    else if (item.type == Types.Property)
                    {
                        newNode.Add(new XAttribute(item.Name, item.Value));
                    }
                }

                rootNode.Add(newNode);
                rootNode.Save(xmlPath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public void Update(T obj)
        {
            try
            {
                obj.SetDefaultValues();
                PropertyInfo info = type.GetProperty(/*type.Name + */"ID");
                int id = (int)info.GetValue(obj, null);

                XElement rootNode = XElement.Load(xmlPath);
                IEnumerable<XElement> nodes =
                    from node in rootNode.Descendants(type.Name)
                    where Convert.ToInt32(node.Attribute(/*type.Name + */"ID").Value) == id
                    select node;
                XElement node_Update = nodes.FirstOrDefault();

                if (node_Update == null)
                    return;

                List<Name_Value> lstSource = GetAttributeName_Value(obj);

                foreach (Name_Value item in lstSource)
                {
                    if (item.type == Types.Field)
                    {
                        node_Update.Element(item.Name).SetValue(item.Value);
                    }
                    else if (item.type == Types.Property)
                    {
                        node_Update.Attribute(item.Name).SetValue(item.Value);
                    }
                }

                rootNode.Save(xmlPath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public void Delete(int id)
        {
            try
            {
                XElement rootNode = XElement.Load(xmlPath);
                IEnumerable<XElement> nodes =
                    from node in rootNode.Descendants(type.Name)
                    where Convert.ToInt32(node.Attribute(/*type.Name + */"ID").Value) == id
                    select node;
                XElement node_Delete = nodes.FirstOrDefault();

                if (node_Delete == null)
                    return;
                node_Delete.Remove();
                rootNode.Save(xmlPath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
    }

    public struct Name_Value
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public Types type { get; set; }
    }

    public enum Types
    {
        Property,
        Field
    }
}
