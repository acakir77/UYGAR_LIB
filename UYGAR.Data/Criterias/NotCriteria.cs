using System;
using UYGAR.Data.Base;

namespace UYGAR.Data.Criterias
{
    /// <summary>
    /// Verilen kriteri "deðil"ler.
    /// </summary>
    [Serializable]
    public class NotCriteria : Criteria
    {

        public Criteria m_criteria;
      
        /// <summary>
        /// Verilen kriteri "deðil"ler.
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
        /// Sql koþul parçacýðý üretir.
        /// </summary>
        /// <param name="info">Koþulun uygulanacaðý BaseObject'in Type' ý</param>
        /// <param name="type"></param>
        /// <param name="connection"></param>
        /// <returns>Sql koþul parçacýðý</returns>
        public override string PopulateSQLCriteria(System.Type type, DbConnectionBase connection)
        {
            return string.Format("({0} ({1}))", "NOT", this.m_criteria.PopulateSQLCriteria(type, connection));
        }
    }
}
