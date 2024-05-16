using System;

namespace UYGAR.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ValidEmailAttribute : Attribute
    {
        public ValidEmailAttribute()
        { }

        public static ValidEmailAttribute GetCRequiredAttribute(System.Reflection.PropertyInfo info)
        {
            ValidEmailAttribute[] attributes =
                info.GetCustomAttributes(typeof(ValidEmailAttribute), true) as ValidEmailAttribute[];

            if (attributes.Length > 1)
                throw new InvalidOperationException(string.Format("[ValidEmail] Attribute yalnızca 1 tane olabilir. {0}", info.Name));
            if (attributes.Length == 1)
                return attributes[0];
            else
                return null;
        }
    }
}
