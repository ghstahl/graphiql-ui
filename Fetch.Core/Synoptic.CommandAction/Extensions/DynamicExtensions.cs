using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;

namespace Synoptic.Extensions
{
    internal static class DynamicExtensions
    {
        public static string ToPascalCase(this string theString)
        {
            // If there are 0 or 1 characters, just return the string.
            if (theString == null) return theString;
            if (theString.Length < 2) return theString.ToUpper();

            // Split the string into words.
            string[] words = theString.Split(
                new char[] { },
                StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            string result = "";
            foreach (string word in words)
            {
                result +=
                    word.Substring(0, 1).ToUpper() +
                    word.Substring(1);
            }

            return result;
        }
        public static object ToObject(this IDictionary<string, object> source, Type someObjectType)
        {
            object someObject = Activator.CreateInstance(someObjectType);

            foreach (KeyValuePair<string, object> item in source)
            {
                var key = item.Key.ToPascalCase();
                var property = someObjectType.GetProperty(key);
                var isExpando = item.Value.GetType() == typeof(System.Dynamic.ExpandoObject);
                if (property != null)
                {

                    if (property.PropertyType == typeof(string) && isExpando)
                    {
                        var svalue = JsonConvert.SerializeObject(item.Value);
                        property.SetValue(someObject, svalue, null);
                    }
                    else
                    {
                        property.SetValue(someObject, item.Value, null);
                    }
                  
                }
            }

            return someObject;
        }

        public static T ToObject<T>(this IDictionary<string, object> source)
            where T : class, new()
        {
            T someObject = new T();
            Type someObjectType = someObject.GetType();

            foreach (KeyValuePair<string, object> item in source)
            {
                someObjectType.GetProperty(item.Key).SetValue(someObject, item.Value, null);
            }

            return someObject;
        }

        public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );

        }
    }
}