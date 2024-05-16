using System;
using System.Reflection;
using UYGAR.Data.Base;

namespace UYGAR.Data.Attributes
{
    /// <summary>
    /// Bir Property nin Tabloda bir kolona karşılık geldiğini belirtir.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    [Serializable]
    public class ColumnAttribute : Attribute
    {
        #region Private Fields

        private string m_name = string.Empty;  
        private bool m_isIndexed = false;
        private bool m_isCanBeNull = true;


        #endregion

        #region Public Fields

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public bool IsCanBeNull
        {
            get { return m_isCanBeNull; }
            set { m_isCanBeNull = value; }
        }

        //public string DbType
        //{
        //    get { return m_dbType; }
        //    set { m_dbType = value; }
        //}


        //public bool IsPrimaryKey
        //{
        //    get { return m_isPrimaryKey; }
        //    set { m_isPrimaryKey = value; }
        //}


        //public bool IsIdentity
        //{
        //    get { return m_isIdentity; }
        //    set { m_isIdentity = value; }
        //}

        public bool IsIndexed
        {
            get { return m_isIndexed; }
            set { m_isIndexed = value; }
        }
        #endregion

        #region Methods

        public static string GetName(PropertyInfo propertyInfo)
        {
            ColumnAttribute attribute =
               ColumnAttribute.GetColumnAttribute(propertyInfo);
            if (attribute == null)
                return null;
            else if (attribute.Name != string.Empty)
                return attribute.Name;
            else
                if (propertyInfo.PropertyType.IsSubclassOf(typeof(Model)))
                    return $"{propertyInfo.Name}_ID";
                else
                    return propertyInfo.Name;
        }
     
        //public static string GetDbType(PropertyInfo propertyInfo)
        //{
        //    ColumnAttribute attribute =
        //       ColumnAttribute.GetColumnAttribute(propertyInfo);
        //    if (attribute == null)
        //        return null;
        //    else if (attribute.DbType != string.Empty)
        //        return attribute.DbType;
        //    else
        //        return propertyInfo.PropertyType.Name; //Bak
        //}

        public static bool GetIsCanBeNull(PropertyInfo propertyInfo)
        {
            ColumnAttribute attribute =
               ColumnAttribute.GetColumnAttribute(propertyInfo);
            if (attribute == null)
                return true;
            return attribute.IsCanBeNull;
        }

        //public static bool GetIsPrimaryKey(PropertyInfo propertyInfo)
        //{
        //    ColumnAttribute attribute =
        //       ColumnAttribute.GetColumnAttribute(propertyInfo);
        //    if (attribute == null)
        //        return false;
        //    return attribute.IsPrimaryKey;
        //}

        //public static bool GetIsIdentity(PropertyInfo propertyInfo)
        //{
        //    ColumnAttribute attribute =
        //       ColumnAttribute.GetColumnAttribute(propertyInfo);
        //    if (attribute == null)
        //        return false;
        //    return attribute.IsIdentity;
        //}

        public static bool GetIsIndexed(PropertyInfo propertyInfo)
        {
            ColumnAttribute attribute =
               ColumnAttribute.GetColumnAttribute(propertyInfo);
            if (attribute == null)
                return false;
            return attribute.IsIndexed;
        }

        public static ColumnAttribute GetColumnAttribute(System.Reflection.PropertyInfo info)
        {
            ColumnAttribute[] attributes =
                info.GetCustomAttributes(typeof(ColumnAttribute), true) as ColumnAttribute[];

            if (attributes.Length > 1)
                throw new InvalidOperationException(string.Format("[Column] Attribute yalnızca 1 tane olabilir. {0}", info.Name));
            if (attributes.Length == 1)
                return attributes[0];
            else
                return null;
        }
        #endregion

        #region Constructor

        public ColumnAttribute()
        {
        }

        #endregion
    }
}
