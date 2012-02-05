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
using System.ComponentModel;
using System.Windows.Resources;
using System.Reflection;

namespace Sharkfist.Phone.SilverlightCore
{ 
    public class StringToTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType.IsAssignableFrom(typeof(string));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            Type type = null;
            string text = value as string;
            if (text != null)
            {
                foreach (AssemblyPart part in Deployment.Current.Parts)
                {
                    type = GetAssemblyType(part.Source.Replace(".dll", string.Empty), text);
                    if (type != null)
                    {
                        break;
                    }
                }
            }

            return type;
        }

        private static Type GetAssemblyType(string assemblyName, string className)
        {
            return Assembly.Load(assemblyName).GetType(className);
        }
    }
}
