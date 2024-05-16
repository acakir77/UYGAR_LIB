using System;
using System.Reflection;
using UYGAR.Data.Base;

namespace UYGAR.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class DisplayEnumNameAttribute : System.Attribute
    {

        public string Name { get; set; }
        public DisplayEnumNameAttribute(string name)
        {
            this.Name = name;
        }

        public static string GetName(FieldInfo propertyInfo)
        {
            DisplayEnumNameAttribute attribute =
               DisplayEnumNameAttribute.GetDisplayNameAttribute(propertyInfo);
            if (attribute == null)
                return string.Empty;
            else if (attribute.Name != string.Empty)
                return attribute.Name;
            else
                if (propertyInfo.FieldType.BaseType == typeof(Model))
                    return string.Format("{0}_ID", propertyInfo.Name);
                else
                    return propertyInfo.Name;
        }
        public static string GetCaption(Enum Enum)
        {
            FieldInfo itemFiled = Enum.GetType().GetField(Enum.ToString());
            string caption = DisplayEnumNameAttribute.GetName(itemFiled);
            return caption;

        }
        public static DisplayEnumNameAttribute GetDisplayNameAttribute(System.Reflection.FieldInfo info)
        {
            DisplayEnumNameAttribute[] attributes =
                info.GetCustomAttributes(typeof(DisplayEnumNameAttribute), true) as DisplayEnumNameAttribute[];

            if (attributes.Length > 1)
                throw new InvalidOperationException(string.Format("[DisplayName] Attribute yalnızca 1 tane olabilir. {0}", info.Name));
            if (attributes.Length == 1)
                return attributes[0];
            else
                return null;
        }

    }
}
