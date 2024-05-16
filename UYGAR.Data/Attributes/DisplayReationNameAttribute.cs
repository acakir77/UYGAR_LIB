using System;
using System.Reflection;
using UYGAR.Data.Base;

namespace UYGAR.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ReationNameAttribute : System.Attribute
    {
        private string m_name = string.Empty;

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public ReationNameAttribute(string name)
        {
            this.m_name = name;
        }

        public static string GetName(PropertyInfo propertyInfo)
        {
            ReationNameAttribute attribute =
               ReationNameAttribute.GetDisplayNameAttribute(propertyInfo);
            if (attribute == null)
                return null;
            else if (attribute.Name != string.Empty)
                return attribute.Name;
            else
                if (propertyInfo.PropertyType.IsSubclassOf(typeof(Model)))
                    return string.Format("{0}_ID", propertyInfo.Name);
                else
                    return propertyInfo.Name;
        }

        public static ReationNameAttribute GetDisplayNameAttribute(System.Reflection.PropertyInfo info)
        {
            ReationNameAttribute[] attributes =
                info.GetCustomAttributes(typeof(ReationNameAttribute), true) as ReationNameAttribute[];

            if (attributes.Length > 1)
                throw new InvalidOperationException(string.Format("[DisplayName] Attribute yalnızca 1 tane olabilir. {0}", info.Name));
            if (attributes.Length == 1)
                return attributes[0];
            else
                return null;
        }
    }
}
