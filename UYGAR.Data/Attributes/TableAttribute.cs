using System;

namespace UYGAR.Data.Attributes
{
    /// <summary>
    /// Bir Object in veritabanında bir tabloya  karşılık geldiğini belirtir.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class TableAttribute : Attribute
    {
        #region Private Fields
        private string m_name;
        private string m_projectName;
        private string m_description;


        #endregion

        #region Public Fields

        /// <summary>
        /// Proje Adı
        /// </summary>
        public string ProjectName
        {
            get { return m_projectName; }
            set { m_projectName = value; }
        }

        /// <summary>
        /// Tablo Adı
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        #endregion

        #region Methods
        public static string GetProjectName(Type objectType)
        {
            TableAttribute[] attributes =
                       objectType.GetCustomAttributes(typeof(TableAttribute), true) as TableAttribute[];

            if (attributes.Length > 1)
                throw new InvalidOperationException(string.Format("[Table] Attribute yalnızca 1 tane olabilir. {0}", objectType));
            if (attributes.Length == 0)
                return string.Empty;
            if (attributes[0].ProjectName == null)
                return string.Empty;
            return attributes[0].ProjectName;
        }
        public static string GetDescription(Type objectType)
        {
            TableAttribute[] attributes =
                       objectType.GetCustomAttributes(typeof(TableAttribute), true) as TableAttribute[];

            if (attributes.Length > 1)
                throw new InvalidOperationException(string.Format("[Table] Attribute yalnızca 1 tane olabilir. {0}", objectType));
            if (attributes.Length == 0)
                return string.Empty;
            if (attributes[0].Description == null)
                return string.Empty;
            return attributes[0].Description;
        }
     
        public static string GetName(Type objectType)
        {
            TableAttribute[] attributes =
                   objectType.GetCustomAttributes(typeof(TableAttribute), true) as TableAttribute[];

            if (attributes.Length > 1)
                throw new InvalidOperationException(string.Format("[Table] Attribute yalnızca 1 tane olabilir. {0}", objectType));
            if (attributes.Length == 0)
                return objectType.Name;
            if (attributes[0].Name == null)
                return string.IsNullOrEmpty(GetProjectName(objectType)) ? objectType.Name : string.Format("{0}_{1}", GetProjectName(objectType), objectType.Name);
            return string.IsNullOrEmpty(GetProjectName(objectType)) ? attributes[0].Name : string.Format("{0}_{1}", GetProjectName(objectType), attributes[0].Name);
            //
        }
        #endregion

        #region Constructor
        public TableAttribute()
        {
        }
        #endregion

    }
}
