using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Phone.Shell;
using System.Xml;

namespace Sharkfist.Phone.SilverlightCore
{
    public static class Configuration
    {
        public static readonly object _syncLock = new object();

        public static void SaveStateSetting(string key, object value)
        {
            if (PhoneApplicationService.Current.State.ContainsKey(key))
            {
                PhoneApplicationService.Current.State[key] = value;
            }
            else
            {
                PhoneApplicationService.Current.State.Add(key, value);
            }
        }

        public static object LoadStateSetting(string key)
        {
            if (PhoneApplicationService.Current.State.ContainsKey(key))
            {
                return PhoneApplicationService.Current.State[key];
            }

            return null;
        }

        public static void SaveStateSetting<T>(string key, T value)
        {
            if (PhoneApplicationService.Current.State.ContainsKey(key))
            {
                PhoneApplicationService.Current.State[key] = value;
            }
            else
            {
                PhoneApplicationService.Current.State.Add(key, value);
            }
        }

        public static bool TryLoadStateSetting<T>(string key, out T value)
        {
            if (PhoneApplicationService.Current.State.ContainsKey(key))
            {
                value = (T)PhoneApplicationService.Current.State[key];
                return true;
            }

            value = default(T);
            return false;
        }

        public static object LoadStateSetting(string key, object defaultValue)
        {
            object value = LoadStateSetting(key);
            if (value != null)
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        public static void SaveSetting(string key, string value)
        {
            lock (_syncLock)
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
                {
                    IsolatedStorageSettings.ApplicationSettings[key] = value;
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings.Add(key, value);
                }
            }
        }

        public static string LoadSetting(string key)
        {
            lock (_syncLock)
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
                {
                    return (string)IsolatedStorageSettings.ApplicationSettings[key];
                }

                throw new InvalidOperationException(string.Format("No setting named {0} exists.", key));
            }
        }

        public static string LoadSetting(string key, string defaultValue)
        {
            lock (_syncLock)
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
                {
                    return (string)IsolatedStorageSettings.ApplicationSettings[key];
                }

                return defaultValue;
            }
        }

        public static void SaveSetting<T>(string key, T value)
        {
            lock (_syncLock)
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
                {
                    IsolatedStorageSettings.ApplicationSettings[key] = value;
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings.Add(key, value);
                }
            }
        }

        public static T LoadSetting<T>(string key)
        {
            lock (_syncLock)
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
                {
                    return (T)IsolatedStorageSettings.ApplicationSettings[key];
                }

                throw new InvalidOperationException(string.Format("No setting named {0} exists.", key));
            }
        }

        public static T LoadSetting<T>(string key, T defaultValue)
        {
            lock (_syncLock)
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
                {
                    return (T)IsolatedStorageSettings.ApplicationSettings[key];
                }

                return defaultValue;
            }
        }

        public static bool TryLoadSetting<T>(string key, out T value)
        {
            lock (_syncLock)
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
                {
                    value = (T)IsolatedStorageSettings.ApplicationSettings[key];
                    return true;
                }

                value = default(T);
                return false;
            }
        }

        public static void RemoveSetting(string key)
        {
            lock (_syncLock)
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
                {
                    IsolatedStorageSettings.ApplicationSettings.Remove(key);
                }
            }
        }

        //public static void SaveSetting(string key, string value)
        //{
        //    XElement settings = GetSettingsElement();

        //    var setting = (from item in settings.Descendants("setting")
        //                       where item.Attribute("key").Value.Equals(key)
        //                       select item).FirstOrDefault();

        //    if(setting != null)
        //    {
        //        setting.Attribute("value").Value = value;
        //    }
        //    else
        //    {
        //        setting = new XElement("setting");
        //        XAttribute keyAttribute = new XAttribute("key", key);
        //        XAttribute valueAttribute = new XAttribute("value", value);
        //        setting.Add(keyAttribute, valueAttribute);
        //        settings.Add(setting);
        //    }

        //    using(Stream settingsStream = GetSettingsFileStream())
        //    {
        //        settings.Save(settingsStream);
        //    }
        //}

        //public static void SaveSetting<T>(string key, T value)
        //{
        //    XElement settings = GetSettingsElement();

        //    var setting = (from item in settings.Descendants("setting")
        //                   where item.Attribute("key").Value.Equals(key)
        //                   select item).First();

        //    StringBuilder builder = new StringBuilder();
        //    StringWriter writer = new StringWriter(builder);
        //    XmlSerializer serializer = new XmlSerializer(typeof(T));
        //    serializer.Serialize(writer, value);

        //    if (setting != null) 
        //    {
        //        setting.Attribute("value").Value = builder.ToString();
        //    }
        //    else
        //    {
        //        setting = new XElement("setting");
        //        XAttribute keyAttribute = new XAttribute("key", key);
        //        XAttribute valueAttribute = new XAttribute("value", builder.ToString());
        //        setting.Add(keyAttribute, valueAttribute);
        //        settings.Add(setting);
        //    }

        //    using (Stream settingsStream = GetSettingsFileStream())
        //    {
        //        settings.Save(settingsStream);
        //    }
        //}

        //public static string LoadSetting(string key)
        //{
        //    XElement settings = GetSettingsElement();
            
        //    var setting = (from item in settings.Descendants("setting")
        //                   where item.Attribute("key").Value.Equals(key)
        //                   select item).First();

        //     return setting.Attribute("value").Value;
        //}

        //public static string LoadSetting(string key, string defaultValue)
        //{
        //    XElement settings = GetSettingsElement();
        //    string test = settings.ToString();

        //    var setting = (from item in settings.Descendants("setting")
        //                   where item.Attribute("key").Value.Equals(key)
        //                   select item).FirstOrDefault();

        //    if (setting == null || setting.Attribute("value") == null)
        //    {
        //        return defaultValue;
        //    }
          
        //    return setting.Attribute("value").Value;
        //}

        //public static T LoadSetting<T>(string key)
        //{
        //    return LoadSetting<T>(key, default(T));
        //}

        //public static T LoadSetting<T>(string key, object defaultValue)
        //{
        //    XElement settings = GetSettingsElement();
            
        //    var setting = (from item in settings.Descendants("setting")
        //                   where item.Attribute("key").Value.Equals(key)
        //                   select item).First();

        //    StringReader reader = new StringReader(setting.Attribute("value").Value);
        //    XmlSerializer serializer = new XmlSerializer(typeof(T));
        //    return (T)serializer.Deserialize(reader);
        //}

        //private static XElement GetSettingsElement()
        //{
        //    XElement settings = null;
            
        //    using(Stream fileStream = GetSettingsFileStream())
        //    {
        //        try
        //        {
        //            settings = XElement.Load(fileStream);
        //        }
        //        catch(XmlException ex)
        //        {
        //            ex.ToString();

        //            // Start clean
        //            XElement root = new XElement("settings");
        //            root.Save(fileStream);
                    
        //            settings = root;
        //        }
        //    }

        //    return settings;
        //}

        //private static Stream GetSettingsFileStream()
        //{
        //    // TODO: Why am I doing this here? Was there a bug in isolated storage or something?
        //    // Need to figure out and get rid of this shitty code
        //    try
        //    {
        //        try
        //        {
        //            return new IsolatedStorageFileStream("app.settings", FileMode.OpenOrCreate, IsolatedStorageFile.GetUserStoreForApplication());
        //        }
        //        catch (Exception)
        //        {
        //            return new IsolatedStorageFileStream("app.settings", FileMode.OpenOrCreate, IsolatedStorageFile.GetUserStoreForApplication());
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        return new IsolatedStorageFileStream("app.settings", FileMode.OpenOrCreate, IsolatedStorageFile.GetUserStoreForApplication());
        //    }
        //}
    }
}
