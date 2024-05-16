using System;
using System.Configuration;
using UYGAR.Data.Connections;

namespace UYGAR.Data.Base
{
    [Serializable]
    public abstract class ModelRaporBase : ICloneable
    {
        private DbConnectionBase _conn = null;
        public DbConnectionBase Conn
        {
            get
            {

                if (_conn == null)
                {
                    try
                    {

                        string dbTaype = ConfigurationManager.AppSettings["DbType"];
                        if (dbTaype.Equals("SQL"))
                            _conn = new DbConnectionSql();
                        if (dbTaype.Equals("MYSQL"))
                            _conn = new DbConnectionMySql();
                        if (dbTaype.Equals("POSTGRE"))
                            _conn = new DbConnectionPostgreSql();


                    }
                    catch (System.Exception)
                    {


                    }
                }
                return _conn;
            }
        }
        public virtual int PopulateReader(System.Data.Common.DbDataReader reader)
        {
            int i = 0;
            return i;
        }


        #region ICloneable Members

        public object Clone()
        {
            ModelRaporBase OB = (ModelRaporBase)MemberwiseClone();
            return OB;
        }

        #endregion
    }
}
