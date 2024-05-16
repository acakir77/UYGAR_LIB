using System;
using UYGAR.Data.Base;

namespace UYGAR.Data.Criterias
{
    /// <summary>
    /// Verilen kriteri "de�il"ler.
    /// </summary>
    [Serializable]
    public class NotCriteria : Criteria
    {

        public Criteria m_criteria;
      
        /// <summary>
        /// Verilen kriteri "de�il"ler.
        /// </summary>
        /// <param name="criteria">Kriter</param>
        public NotCriteria(Criteria criteria)
        {
            this.m_criteria = criteria;
        }

        public NotCriteria()
        {
        }

        /// <summary>
        /// Sql ko�ul par�ac��� �retir.
        /// </summary>
        /// <param name="info">Ko�ulun uygulanaca�� BaseObject'in Type' �</param>
        /// <param name="type"></param>
        /// <param name="connection"></param>
        /// <returns>Sql ko�ul par�ac���</returns>
        public override string PopulateSQLCriteria(System.Type type, DbConnectionBase connection)
        {
            return string.Format("({0} ({1}))", "NOT", this.m_criteria.PopulateSQLCriteria(type, connection));
        }
    }
}
