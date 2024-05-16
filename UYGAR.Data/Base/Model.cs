using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using UYGAR.Data.Attributes;
using UYGAR.Data.Connection;
using UYGAR.Data.Connections;

namespace UYGAR.Data.Base
{
    [Serializable]
    public abstract class Model : ICloneable, IDisposable, IComparable
    {
        #region StaticField
        public static readonly string F_OID = "OID";
        public static readonly string F_Create_Date = "CreateDate";
        public static readonly string F_Create_UserId = "CreateUserId";
        public static readonly string F_Rowversion = "Rowversion";
        public static readonly string f_Update_Date = "UpdateDate";
        public static readonly string f_UpdateUserId = "UpdateUserId";

        #endregion
        #region Private Fields
        private DateTime _DeleteTime = new DateTime(1901, 1, 1);
        private bool _IsDeleted = false;
        private int _DeleteUserId = 0;
        private int _OID = -1;

        #endregion

        #region Public Fields


        public int OID
        {
            get { return _OID; }
            set { _OID = value; }
        }

        public DateTime CreateDate { get; set; } = DateTime.Now;
        public int CreateUserId { get; set; }
        public int Rowversion { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdateUserId { get; set; }
        public int DeleteUserId { get => _DeleteUserId; set => _DeleteUserId = value; }
        public DateTime DeleteTime { get => _DeleteTime; set => _DeleteTime = value; }
        
        public bool IsDeleted { get => _IsDeleted; set => _IsDeleted = value; }
        public bool Inserted
        {
            get { return !OID.Equals(-1); }

        }


        #endregion
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
        #region Methods

        //public virtual void Save()
        //{
        //    DbConnection conn = new DbConnection();
        //    if (Validate())
        //        conn.SaveObject(this);
        //}

        public virtual void Save(DbConnectionBase connection)
        {
            try
            {

                if (Validate())
                    //Connection = new DbConnectionSql();
                    connection.SaveObject(this);

            }
            catch (Exception ex)
            {
                throw ex;
            }



        }
        public void CreateTable()
        {
            Conn.CreateSchema(this.GetType());
        }
        public virtual void Save()
        {
            try
            {

                if (Validate())
                    //Connection = new DbConnectionSql();
                    Conn.SaveObject(this);

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public virtual bool Validate()
        {

            return true;

        }
        public virtual bool ValidateDelete()
        {

            return true;

        }

        public virtual void Delete(DbConnectionBase connection)
        {
            if (ValidateDelete())
                connection.DeleteObject(this);

        }
        public virtual void Delete()
        {
            if (ValidateDelete())
                Conn.DeleteObject(this);

        }
        public virtual void CreateDefaultRecords()
        {
        }




        public virtual int Populatereader(IDataReader reader, int retval)
        {
            OID = reader.GetInt32(retval++);
            CreateDate = reader.GetDateTime(retval++);
            Rowversion = reader.GetInt32(retval++);
            UpdateDate = reader.GetDateTime(retval++);
            CreateUserId = reader.GetInt32(retval++);
            UpdateUserId = reader.GetInt32(retval++);
            DeleteUserId = reader.GetInt32(retval++);
            DeleteTime = reader.GetDateTime(retval++);
            IsDeleted = reader.GetBoolean(retval++);
            return retval;
        }

        public virtual int PopulatereaderFirst(IDataReader reader, int retval)
        {
            OID = reader.GetInt32(retval++);
            CreateDate = reader.GetDateTime(retval++);
            Rowversion = reader.GetInt32(retval++);
            UpdateDate = reader.GetDateTime(retval++);
            CreateUserId = reader.GetInt32(retval++);
            UpdateUserId = reader.GetInt32(retval++);
            DeleteUserId = reader.GetInt32(retval++);
            DeleteTime = reader.GetDateTime(retval++);
            IsDeleted = reader.GetBoolean(retval++);
            return retval;
        }

        public virtual string PopulateExecuteString()
        {
            return string.Empty;
        }

        #endregion



        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == DBNull.Value) return false;
            if (obj.GetType().IsSubclassOf(typeof(Model)))
                return this.OID.Equals(((Model)obj).OID);
            else return false;
        }
        #region ICloneable Members

        public object Clone()
        {
            Model ob = (Model)MemberwiseClone();
            return ob;
        }
        public object CloneNew()
        {
            Model ob = (Model)MemberwiseClone();
            ob.OID = -1;
            ob.Rowversion = 1;
            return ob;
        }
        public override string ToString()
        {
            return string.Format("{0}", OID);
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        //public override bool Equals(object obj)
        //{
        //    if (obj == null || obj == System.DBNull.Value)
        //        return false;
        //    if (obj.GetType() == typeof(Model))
        //        return this.OID.Equals(((Model)obj).OID);
        //    return false;

        //}
        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 0;
            return OID.CompareTo(((Model)obj).OID);
        }

        #endregion

        #region IComparable Members

        int IComparable.CompareTo(object obj)
        {
            if (obj == null)
                return 0;
            return OID.CompareTo(((Model)obj).OID);
        }

        #endregion
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }








    }
}
