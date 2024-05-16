using System;
using System.Reflection;
using UYGAR.Data.Base;

namespace UYGAR.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true)]
    public sealed class AssociationAttribute : Attribute
    {
        private string _name;
        private Type _objectType;
        private bool m_isCanBeNull = true;

        public bool IsCanBeNull
        {
            get { return m_isCanBeNull; }
            set { m_isCanBeNull = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Type ObjectType
        {
            get { return _objectType; }
            set { _objectType = value; }
        }


        public AssociationAttribute(string _name)
        {
            this._name = _name;
        }

        public AssociationAttribute(string _name, System.Type _objectType)
        {
            this._name = _name;
            this._objectType = _objectType;
        }

        public static string GetName(PropertyInfo propertyInfo)
        {
            AssociationAttribute attribute =
               AssociationAttribute.GetAssociationAttribute(propertyInfo);
            if (attribute == null)
                return null;
            //else if (attribute.Name != string.Empty)
            //    return attribute.Name;
            else
                if (!propertyInfo.PropertyType.Name.Equals("ModelCollection`1"))
                {
                    if (propertyInfo.PropertyType.IsSubclassOf(typeof(Model)))
                        return string.Format("{0}_ID", propertyInfo.Name);
                    else
                        return propertyInfo.Name;
                }
                else return string.Empty;
        }
        public static AssociationAttribute GetAssociationAttribute(System.Reflection.PropertyInfo info)
        {
            AssociationAttribute[] attributes =
                info.GetCustomAttributes(typeof(AssociationAttribute), true) as AssociationAttribute[];

            if (attributes.Length > 1)
                throw new InvalidOperationException(string.Format("[AssociationAttribute] Attribute yalnızca 1 tane olabilir. {0}", info.Name));
            if (attributes.Length == 1)
                return attributes[0];
            else
                return null;
        }
        public static AssociationAttribute[] GetAssociationAttribute(System.Type info)
        {
            AssociationAttribute[] attributes =
                info.GetCustomAttributes(typeof(AssociationAttribute), true) as AssociationAttribute[];
            if (attributes.Length != 0)
                return attributes;
            else
                return null;

        }
        public static PropertyInfo GetPropertyInfo(System.Type type, string associationName)
        {

            foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (AssociationAttribute.GetAssociationAttribute(propertyInfo) != null && AssociationAttribute.GetAssociationAttribute(propertyInfo).Name.Equals(associationName))
                    return propertyInfo;
            }
            return null;
        }

    }
}
