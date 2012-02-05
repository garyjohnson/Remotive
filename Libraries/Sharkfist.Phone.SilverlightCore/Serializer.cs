using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;

namespace Sharkfist.Phone.SilverlightCore
{
    public static class Serializer
    {
        public static void SaveToBinaryFile<T>(T value, string fileName) where T : class
        {
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(fileName,
                    System.IO.FileMode.Create, file))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                    serializer.WriteObject(stream, value);
                }
            }
        }

        public static bool TrySaveToBinaryFile<T>(T value, string fileName) where T : class
        {
            bool result = false;

            try
            {
                SaveToBinaryFile<T>(value, fileName);
                result = true;
            }
            catch (Exception) { }

            return result;
        }

        public static T LoadBinaryFromFile<T>(string fileName) where T : class
        {
            T returnValue = null;

            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(fileName,
                    System.IO.FileMode.OpenOrCreate, file))
                {
                    if (stream.Length > 0)
                    {
                        DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                        returnValue = serializer.ReadObject(stream) as T;
                    }
                }
            }

            return returnValue;
        }

        public static bool TryLoadBinaryFromFile<T>(string fileName, out T value) where T : class
        {
            bool result = false;
            value = null;

            try
            {
                value = LoadBinaryFromFile<T>(fileName);
                result = true;
            }
            catch (Exception) { }

            return result;
        }

        public static string SerializeToXml<T>(object value)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);
            serializer.Serialize(writer, value);

            return builder.ToString();
        }

        public static T DeserializeFromXml<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringReader reader = new StringReader(xml);
            return (T)serializer.Deserialize(reader);
        }

        public static string SerializeToQueryString<T>(object value)
        {
            StringBuilder queryString = new StringBuilder();

            Type targetType = typeof(T);
            PropertyInfo[] properties = targetType.GetProperties(BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);

            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];
                object propertyValue = null;

                try
                {
                    propertyValue = property.GetValue((T)value, null);
                }
                catch (Exception)
                {
                    // Do nothing
                }

                string stringValue = string.Empty;
                if (propertyValue != null)
                {
                    stringValue = HttpUtility.UrlEncode(propertyValue.ToString());
                }

                queryString.Append(string.Format("{0}={1}",
                    HttpUtility.UrlEncode(property.Name), stringValue));

                if (i < properties.Length - 1)
                {
                    queryString.Append("&");
                }
            }

            return queryString.ToString();
        }

        public static T DeserializeFromQueryString<T>(IDictionary<string,string> queryString)
        {
            Type targetType = typeof(T);
            T value = Activator.CreateInstance<T>();

            foreach (string propertyName in queryString.Keys)
            {
                PropertyInfo info = targetType.GetProperty(propertyName);
                if (info == null)
                {
                    continue;
                }

                try
                {
                    if (!string.IsNullOrEmpty(queryString[propertyName]))
                    {
                        info.SetValue(value,
                            Convert.ChangeType(HttpUtility.UrlDecode(queryString[propertyName]),
                            info.PropertyType, CultureInfo.CurrentCulture), null);
                    }
                }
                catch (InvalidCastException) { /* Do nothing */}
            }

            return value;
        }
    }
}
