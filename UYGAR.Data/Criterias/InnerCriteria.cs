using System;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using UYGAR.Data.Attributes;
using UYGAR.Data.Base;
using UYGAR.Data.Configuration;

namespace UYGAR.Data.Criterias
{
    /// <summary>
    /// Kar��la�t�rma kriteri.
    /// Bir �yenin referans oldu�u BaseObject cinsinden nesneye dair ko�ullar� tan�mlamak i�in kullan�l�r.
    /// </summary>
    /// <remarks>
    /// BaseObject cinsinden bir �yenin, referans oldu�u alt nesneye ait ko�ullar� tan�mlamak i�in kullan�l�r.
    /// </remarks>
    [Serializable]
    [XmlInclude(typeof(InnerCriteria))]
    public class InnerCriteria : Criteria
    {
        private string m_memberName;
        private string _multiselectValue;
        public Criteria Criteria1 { get; set; }
        private Order m_order;

        public Order order
        {
            get { return m_order; }
            set { m_order = value; }
        }
        /// <summary>
        /// Kritere konu olan �yenin ad�
        /// </summary>

        public string MemberName
        {
            get { return m_memberName; }
            set { m_memberName = value; }
        }

        public string MultiselectValue
        {
            get { return _multiselectValue; }
            set { _multiselectValue = value; }
        }
        public InnerCriteria()
        {

        }

        /// <summary>
        /// BaseObject cinsinden bir �yenin, referans oldu�u alt nesneye ait ko�ullar� tan�mlar.
        /// </summary>
        /// <param name="persistentMemberName">�ye ad�.</param>
        /// <param name="criteria">�lgili nesneye ait alt ko�ul.</param>
        public InnerCriteria(string persistentMemberName, Criteria criteria)
        {
            this.m_memberName = persistentMemberName;
            this.Criteria1 = criteria;
        }
        public InnerCriteria(string persistentMemberName, Criteria criteria, Order Order)
        {
            this.m_memberName = persistentMemberName;
            this.Criteria1 = criteria;
            this.order = Order;
        }
        public InnerCriteria(string persistentMemberName, string _MultiselectVales)
        {
            this.m_memberName = persistentMemberName;
            this.MultiselectValue = _MultiselectVales;

        }
        /// <summary>
        /// Kriter nesnesinin SQL ko�ulunun ilgili b�l�m�n� olu�turmas� i�in �a��r�l�r.
        /// </summary>
        /// <param name="info">Ko�ulun uygulanaca�� BaseObject'in Type'�</param>
        /// <returns>Sql ko�ul par�ac���</returns>
        public override string PopulateSQLCriteria(System.Type type, DbConnectionBase connection)
        {
            ApplicationConfig config = new ApplicationConfig("connection.config");
            StringBuilder GrupuFealdName = new StringBuilder();
            //StringBuilder propertyNames = new StringBuilder();
            StringBuilder builder = new StringBuilder();
            string sqlpart = string.Empty;
            string tableName = TableAttribute.GetName(type);
            string fieldName = string.Format("{0}.{1}_ID", tableName, this.MemberName);

            PropertyInfo pInfo = type.GetProperty(this.MemberName);

            System.Diagnostics.Trace.Assert(pInfo != null, string.Format("S�n�f i�erisinde '{0}' belirtilen �ye bulunamad�", this.MemberName));
            string innerTableName = TableAttribute.GetName(pInfo.PropertyType);


            if (order != null)
            {
                if (order.Count == 1)
                {
                    var obj = (Model)Activator.CreateInstance(pInfo.PropertyType);
                    var info = obj.GetType().GetProperty(this.order[0]);
                    GrupuFealdName.AppendFormat(info.PropertyType.IsSubclassOf(typeof(Model)) ? "{0}_ID" : "{0}",
                        this.order[0]);
                }
                else
                {

                    for (int i = 0; i < this.order.Count; i++)
                    {
                        var obj = (Model)Activator.CreateInstance(typeof(Model));
                        PropertyInfo info = obj.GetType().GetProperty(this.order[i]);
                        if (info.PropertyType.IsSubclassOf(typeof(Model)))
                        {
                            GrupuFealdName.AppendFormat((i + 1) != this.order.Count ? "{0}_ID," : "{0}_ID",
                                this.order[i]);
                        }
                        else
                        {
                            GrupuFealdName.AppendFormat((i + 1) != this.order.Count ? "{0}," : "{0}", this.order[i]);
                        }

                    }
                }
                if (order.IsDesc)
                    builder.AppendFormat(" ORDER BY {0} {1}", GrupuFealdName, "DESC");
                sqlpart =
                  string.Format(" {0} in (select OID from {4}.{1} where ( {2} ) {3} )",
                  fieldName, innerTableName,
                  Criteria1.PopulateSQLCriteria(pInfo.PropertyType, connection), builder,connection.Shema);
            }
            else if (!string.IsNullOrEmpty(this.MultiselectValue))
            {
                sqlpart =
                    string.Format(" {0} in ({1})", fieldName, MultiselectValue);
            }
            else
            {
                sqlpart =
                 string.Format("{0} in (select OID from {1} where ( {2} ))",
                 fieldName, innerTableName,
                 this.Criteria1.PopulateSQLCriteria(pInfo.PropertyType, connection));
            }
            return sqlpart;
        }
    }
}
