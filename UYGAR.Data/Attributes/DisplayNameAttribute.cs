using System;
using System.Reflection;
using UYGAR.Data.Base;

namespace UYGAR.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DisplayNameAttribute : System.Attribute
    {
        private string m_name = string.Empty;

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public DisplayNameAttribute(string name)
        {
            this.m_name = name;
        }

        public static string GetName(PropertyInfo propertyInfo)
        {
            DisplayNameAttribute attribute =
               DisplayNameAttribute.GetDisplayNameAttribute(propertyInfo);
            if (attribute == null)
                return null;
            else if (attribute.Name != string.Empty)
                return attribute.Name;
            else
                if (propertyInfo.PropertyType.BaseType == typeof(Model))
                    return string.Format("{0}_ID", propertyInfo.Name);
                else
                    return propertyInfo.Name;
        }

        public static DisplayNameAttribute GetDisplayNameAttribute(System.Reflection.PropertyInfo info)
        {
            DisplayNameAttribute[] attributes =
                info.GetCustomAttributes(typeof(DisplayNameAttribute), true) as DisplayNameAttribute[];

            if (attributes.Length > 1)
                throw new InvalidOperationException(string.Format("[DisplayName] Attribute yalnızca 1 tane olabilir. {0}", info.Name));
            if (attributes.Length == 1)
                return attributes[0];
            else
                return null;
        }
    }
}
