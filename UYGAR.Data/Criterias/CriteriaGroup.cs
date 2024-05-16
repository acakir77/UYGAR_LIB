using System;
using System.Collections.Generic;
using System.Text;
using UYGAR.Data.Base;

namespace UYGAR.Data.Criterias
{
    /// <summary>
    /// kriterler aras�ndaki mant�ksal ili�ki t�r�n� tan�mlar
    /// </summary>
    public enum GroupType
    {
        /// <summary>
        /// Mant�ksal "VE", "AND"
        /// </summary>
        And,

        /// <summary>
        /// Mant�ksal "YA DA", "OR"
        /// </summary>
        Or
    }

    /// <summary>
    /// Bir ko�ulun birden fazla kriterden olu�abilmesi i�in kullan�l�r. 
    /// Birden fazla kriterin SQL c�mlesinde "and" ya da "or" kullan�larak gruplanmas�n� sa�lar.
    /// </summary>


    [Serializable]  
    public class CriteriaGroup : Criteria
    {
        List<Criteria> m_criteriaList = new List<Criteria>();
        GroupType m_groupType = GroupType.And;

        /// <summary>
        /// Kriterlerin kolleksiyonu
        /// </summary>
        public CriteriaGroup()
        {

        }
        public List<Criteria> CriteriaList
        {
            get { return m_criteriaList; }
            set { m_criteriaList = value; }
        }


        /// <summary>
        /// Grup tipi, "and" ya da "or"
        /// </summary>

        public GroupType GroupType
        {
            get { return m_groupType; }
            set { m_groupType = value; }
        }

        /// <summary>
        /// Gruptaki kriterlerin "PopulateSQLCriteria()" yordamlar�n� �al��t�r�r ve grup tipine g�re yerle�tirir.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public override string PopulateSQLCriteria(System.Type type, DbConnectionBase connection)
        {
            if (CriteriaList.Count > 0)
            {
                System.Text.StringBuilder sqlCriteria = new StringBuilder();
                sqlCriteria.Append("(");

                List<Criteria> criterias = CriteriaList;
                for (int i = 0; i < criterias.Count; i++)
                {
                    string subCriteria = criterias[i].PopulateSQLCriteria(type,connection);

                    if (subCriteria != null && subCriteria != string.Empty)
                    {
                        sqlCriteria.Append(subCriteria);
                        if (i < (criterias.Count - 1))
                            switch (this.GroupType)
                            {
                                case GroupType.And:
                                    sqlCriteria.Append(" AND ");
                                    break;
                                case GroupType.Or:
                                    sqlCriteria.Append(" OR ");
                                    break;
                            }
                    }
                }
                sqlCriteria.Append(")");

                return sqlCriteria.ToString();
            }

            return string.Empty;
        }
    }
}
