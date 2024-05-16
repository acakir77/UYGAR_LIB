using System;

namespace UYGAR.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredAttribute : Attribute
    {
        public RequiredAttribute()
        { }

        public static RequiredAttribute GetRequiredAttribute(System.Reflection.PropertyInfo info)
        {
            RequiredAttribute[] attributes =
                info.GetCustomAttributes(typeof(RequiredAttribute), true) as RequiredAttribute[];

            if (attributes.Length > 1)
                throw new InvalidOperationException(string.Format("[Required] Attribute yalnızca 1 tane olabilir. {0}", info.Name));
            if (attributes.Length == 1)
                return attributes[0];
            else
                return null;
        }
    }
}
