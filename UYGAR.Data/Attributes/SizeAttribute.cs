using System;

namespace UYGAR.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SizeAttribute : Attribute
    {
        #region Private Fields

        private int m_minSize = 0;
        private int m_maxSize = 100;
        private byte m_precision = 18;
        private byte m_scale = 4;

        #endregion

        #region Public Fields

        public int MinSize
        {
            get { return m_minSize; }
            set { m_minSize = value; }
        }

        public int MaxSize
        {
            get { return m_maxSize; }
            set { m_maxSize = value; }
        }

        /// <summary>
        /// Decimal alanların tamsayı uzunluk bilgisini taşır.
        /// </summary>
        public byte Precision
        {
            get { return m_precision; }
            set { m_precision = value; }
        }

        /// <summary>
        /// Decimal alanlarýn ondalık uzunluk bilgisini taþýr.
        /// </summary>
        public byte Scale
        {
            get { return m_scale; }
            set { m_scale = value; }
        }

        #endregion

        #region Constructors
        public SizeAttribute(int maxSize)
        {
            this.m_maxSize = maxSize;
        }


        public SizeAttribute(int minSize, int maxSize)
            : this(maxSize)
        {
            this.m_minSize = minSize;
        }


        public SizeAttribute()
        {

        }

        #endregion

        #region Methods

        public static int GetMaxSize(System.Reflection.PropertyInfo info)
        {
            SizeAttribute[] attributes =
                info.GetCustomAttributes(typeof(SizeAttribute), true) as SizeAttribute[];

            if (attributes.Length > 0)
                return attributes[0].MaxSize;
            else
                return 0;
        }

        public static int GetMaxSize(Type classType, string memberName)
        {
            System.Reflection.PropertyInfo info = classType.GetProperty(memberName);
            System.Diagnostics.Trace.Assert(info != null, string.Format("Property '{0}' not found.", memberName));
            return GetMaxSize(info);
        }

        public static SizeAttribute GetSize(System.Reflection.PropertyInfo info)
        {

            SizeAttribute[] attributes =
                info.GetCustomAttributes(typeof(SizeAttribute), true) as SizeAttribute[];

            if (attributes.Length > 0)
                return attributes[0];
            else
                return null;
        }

        #endregion
    }
}
