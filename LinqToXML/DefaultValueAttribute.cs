using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqToXML
{
    [global::System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class DefaultValueAttribute : Attribute
    {
        readonly object defaultValue; readonly Type valueType;
        public DefaultValueAttribute(object defaultValue, Type valueType)
        {
            this.valueType = valueType;
            this.defaultValue = Convert.ChangeType(defaultValue, valueType);
        }
        public Type ValueType
        {
            get { return valueType; }
        }
        public object DefaultValue { get { return defaultValue; } }
    }

    public static class DefaultValueAttributeHelper
    {
        public static void SetDefaultValues(this object obj)
        {
            Type t = obj.GetType();
            var properties = t.GetProperties();
            if (properties != null && properties.Length > 0)
                foreach (var item in properties)
                {
                    var attributes = item.GetCustomAttributes(typeof(DefaultValueAttribute), true);
                    if (attributes != null && attributes.Length > 0)
                        foreach (var attr in attributes)
                        {
                            DefaultValueAttribute val = ((DefaultValueAttribute)attr);
                            if (item.GetValue(obj, null) == null || (val.ValueType == typeof(string) && item.GetValue(obj, null).ToString() == string.Empty) || ((val.ValueType != typeof(string)) && (item.GetValue(obj, null).ToString() == "0")))
                                item.SetValue(obj, val.DefaultValue, null);
                        }
                }

            var fields = t.GetFields();
            if (fields != null && fields.Length > 0)
                foreach (var item in fields)
                {
                    var attributes = item.GetCustomAttributes(typeof(DefaultValueAttribute), true);
                    if (attributes != null && attributes.Length > 0)
                        foreach (var attr in attributes)
                        {
                            DefaultValueAttribute val = ((DefaultValueAttribute)attr);
                            if (item.GetValue(obj) == null || (val.ValueType == typeof(string) && item.GetValue(obj).ToString() == string.Empty) || ((val.ValueType != typeof(string)) && (item.GetValue(obj).ToString() == "0")))
                                item.SetValue(obj, val.DefaultValue);
                        }
                }
        }
    }
}
