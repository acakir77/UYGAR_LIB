using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UYGAR.Data.Base
{
    public class QueryParameters
    {
        public string ParametrName { get; set; }
        public DbType DbType { get => dbType; set => dbType = value; }

        private DbType dbType = DbType.Int32;
        public object Value { get; set; }
    }
}
