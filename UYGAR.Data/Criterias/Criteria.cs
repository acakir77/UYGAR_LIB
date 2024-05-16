using System;
using System.Xml.Serialization;
using UYGAR.Data.Base;

namespace UYGAR.Data.Criterias
{
    /// <summary>
    /// SQL kriteri vermek için kullanýlýr
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(CriteriaGroup)), XmlInclude(typeof(CompareCriteria))]
    [XmlInclude(typeof(IsNullCriteria)), XmlInclude(typeof(QueryCriteria)), XmlInclude(typeof(Criteria))]
    [XmlInclude(typeof(InCriteria)), XmlInclude(typeof(NotCriteria)), XmlInclude(typeof(InnerCriteria))]
    public class Criteria
    {
         string m_criteria;
        /// <summary>
        /// Kullanýlmýyor
        /// </summary>
        public Criteria() { }

        /// <summary>
        /// SQL kriteri vermek için kullanýlýr
        /// </summary>
        /// <param name="sqlCriteria"></param>
        public Criteria(string sqlCriteria)
        {
            m_criteria = sqlCriteria;
        }

        /// <summary>
        /// Kriter nesnesinin SQL koþulunun ilgili bölümünü oluþturmasý için çaðýrýlýr.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual string PopulateSQLCriteria(System.Type type, DbConnectionBase connection)
        {
            return this.m_criteria;
        }
    }
}
