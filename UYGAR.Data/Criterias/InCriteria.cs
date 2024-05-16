using System;
using System.Reflection;
using System.Text;
using UYGAR.Data.Attributes;
using UYGAR.Data.Base;

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
    public class InCriteria : Criteria
    {
        private string m_memberName;
        private Criteria m_criteria;

        public Criteria Criteria1
        {
            get { return m_criteria; }
            set { m_criteria = value; }
        }
        private string _multiselectValue;
        private ModelCollection<Model> _MyCollection = new ModelCollection<Model>();

        public ModelCollection<Model> MyCollection
        {
            get { return _MyCollection; }
            set { _MyCollection = value; }
        }

        public string MultiselectValue
        {
            get { return _multiselectValue; }
            set { _multiselectValue = value; }
        }
        /// <summary>
        /// Kritere konu olan �yenin ad�
        /// </summary>

        public string MemberName
        {
            get { return m_memberName; }
            set { m_memberName = value; }
        }

        /// <summary>
        /// BaseObject cinsinden bir �yenin, referans oldu�u alt nesneye ait ko�ullar� tan�mlar.
        /// </summary>
        /// <param name="persistentMemberName">�ye ad�.</param>
        /// <param name="criteria">�lgili nesneye ait alt ko�ul.</param>
        public InCriteria(string persistentMemberName, string _MultiselectVales)
        {
            this.m_memberName = persistentMemberName;
            this.MultiselectValue = _MultiselectVales;

        }
        public InCriteria(string persistentMemberName, Criteria Criter)
        {
            this.m_memberName = persistentMemberName;
            this.Criteria1 = Criter;

        }
        public InCriteria()
        {


        }
        /// <summary>
        /// Kriter nesnesinin SQL ko�ulunun ilgili b�l�m�n� olu�turmas� i�in �a��r�l�r.
        /// </summary>
        /// <param name="info">Ko�ulun uygulanaca�� BaseObject'in Type'�</param>
        /// <returns>Sql ko�ul par�ac���</returns>
        public override string PopulateSQLCriteria(System.Type type, DbConnectionBase connection)
        {
            string fieldName = string.Empty;
            string sqlpart = string.Empty;
            string tableName = TableAttribute.GetName(type);
            PropertyInfo info = type.GetProperty(this.MemberName);
            if (info.PropertyType.IsSubclassOf(typeof(Model)))
                fieldName = string.Format("{0}.{1}_ID", tableName, this.MemberName);
            else
                fieldName = string.Format("{0}.{1}", tableName, this.MemberName);


            PropertyInfo pInfo = type.GetProperty(this.MemberName);

            System.Diagnostics.Trace.Assert(pInfo != null, string.Format("S�n�f i�erisinde '{0}' belirtilen �ye bulunamad�", this.MemberName));
            string innerTableName = TableAttribute.GetName(pInfo.PropertyType);
            if (!string.IsNullOrEmpty(this.MultiselectValue))
            {
                string[] MultiselectVaulesplit = MultiselectValue.Split(',');
                StringBuilder BuilderValue = new StringBuilder();
                for (int i = 0; i < MultiselectVaulesplit.Length; i++)
                {
                    BuilderValue.AppendFormat("'{0}'",MultiselectVaulesplit[i].Trim());
                    if ((i + 1) != MultiselectVaulesplit.Length)
                        BuilderValue.Append(",");
                }
                sqlpart =
                    string.Format(" {0} in ({1})", fieldName, BuilderValue);
            }
            if (this.Criteria1 != null)
            {
                sqlpart =
                string.Format(" {0} in (select OID from {3}.{1} where ( {2} ))",
                fieldName, innerTableName,
                this.m_criteria.PopulateSQLCriteria(pInfo.PropertyType,connection),connection.Shema);

            }


            return sqlpart;
        }
    }
}
