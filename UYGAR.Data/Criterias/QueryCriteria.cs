using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UYGAR.Data.Base;

namespace UYGAR.Data.Criterias
{
    [Serializable]
    [XmlInclude(typeof(QueryCriteria))]
    public class QueryCriteria : Criteria
    {
        public string Criter { get; set; }
        public QueryCriteria()
        {

        }
        public QueryCriteria(string sqlCriteria)
        {
            Criter = sqlCriteria;
        }
        public override string PopulateSQLCriteria(Type type, DbConnectionBase connection)
        {
            return this.Criter;
        }
    }
}
