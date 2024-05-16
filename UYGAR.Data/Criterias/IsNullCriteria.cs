using System;
using System.Reflection;
using UYGAR.Data.Attributes;
using UYGAR.Data.Base;

namespace UYGAR.Data.Criterias
{
    /// <summary>
    /// Null operatoru, bir �zelli�in alabilece�i de�erin null olmas�n� sorgular.
    /// </summary>
    [Serializable]
    public class IsNullCriteria : Criteria
    {
        private string m_memberName;
        /// <summary>
        /// Kritere konu olan �yenin ad�
        /// </summary>
         
        public string MemberName
        {
            get { return m_memberName; }
            set { m_memberName = value; }
        }
        public IsNullCriteria()
        {

        }
        /// <summary>
        /// Null operatoru, bir �zelli�in alabilece�i de�erin null olmas�n� sorgular.
        /// </summary>
        /// <param name="persistentMemberName"></param>
        public IsNullCriteria(string persistentMemberName)
        {
            this.m_memberName = persistentMemberName;
        }
     
        /// <summary>
        /// Sql ko�ul par�ac��� �retir.
        /// </summary>
        /// <param name="info">Ko�ulun uygulanaca�� Model'in Type' �</param>
        /// <returns>Sql ko�ul par�ac���</returns>
        public override string PopulateSQLCriteria(System.Type type, DbConnectionBase connection)
        {
            PropertyInfo member = type.GetProperty(this.MemberName);
            string fieldName=string.Empty;
            string tableName = (string)TableAttribute.GetName(type);
            if (member.PropertyType.IsSubclassOf(typeof(Model)))
             fieldName = string.Format("{0}.{1}", tableName,string.Format("{0}_ID",MemberName));
            else
             fieldName = string.Format("{0}.{1}", tableName,MemberName);

            return string.Format("{0} {1}",fieldName, "is null");
        }
    }
}
