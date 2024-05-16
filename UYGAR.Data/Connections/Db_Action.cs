using Devart.Data.MySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using UYGAR.Data.Attributes;
using UYGAR.Data.Base;

namespace UYGAR.Data.Connection
{
    [Table(Name = "DBACTION", ProjectName = "UYGAR")]
    public class DbAction : Model
    {
        #region Public Static Fields

        public static string F_CREATEDATE = "CREATEDATE";
        public static string F_DbActionType = "DbActionType";
        public static string F_UserId = "UserId";
        public static string F_ObjectTableName = "ObjectTableName";
        public static string F_Time = "Time";
        #endregion

        #region Public Properties

        [Column]
        [Indexed(GroupNumber = 1, IsUnique = false)]
        [Indexed(GroupNumber = 5, IsUnique = false)]
        public DbActionType DbActionType
        { get; set; }

        [Column]
        [Indexed(GroupNumber = 2, IsUnique = false)]
        [Indexed(GroupNumber = 5, IsUnique = false)]
        public int UserId
        { get; set; }

        [Column]
        [Indexed(GroupNumber = 3, IsUnique = false)]
        public int ObjectOID
        { get; set; }

        [Column]
        [Indexed(GroupNumber = 4, IsUnique = false)]
        public string ObjectTableName
        { get; set; }

        [Size(4000)]
        [Column]
        public string Query
        { get; set; }

        [Size(4000)]
        [Column]
        public string PrevQuery
        { get; set; }

        [Column]
        public DateTime Time
        { get; set; }

        [Column]
        public bool IsArchive
        { get; set; }

        [Column]
        public string ClientIPAddress
        { get; set; }
        [Column]
        public string ClientMacAddress
        { get; set; }

        [Column]
        [Size(4000)]
        public string ParametersValue
        { get; set; }
        #endregion
        public override int Populatereader(IDataReader reader, int retval)
        {
            int i = base.Populatereader(reader, retval);
            using (IDataReaderValues values = new IDataReaderValues())
            {
                DbActionType = values.GetEnum<DbActionType>(reader, i++);
                UserId = values.GetInt(reader, i++);
                ObjectOID = values.GetInt(reader, i++);
                ObjectTableName = values.GetString(reader, i++);
                Query = values.GetString(reader, i++);
                PrevQuery = values.GetString(reader, i++);
                Time = values.GetDatetime(reader, i++);
                IsArchive = values.GetBoolen(reader, i++);
                ClientIPAddress = values.GetString(reader, i++);
                ClientMacAddress = values.GetString(reader, i++);
                ParametersValue = values.GetString(reader, i++);
            }
            return i;
        }

        #region Methods

        public static bool Write(DbActionType dbActionType, int RowVersion, int objectOid, string objectTableName, Type ObjectType, string query, DateTime time, List<DbParameter> parametrs, DbConnectionBase conn)
        {
            var sqlStr = new StringBuilder();
            var valuesStr = new StringBuilder();
            var parametrseListesi = new List<DbParameter>();

            sqlStr.Append($"INSERT INTO {conn}_LOG.SYS.LogMaster");
            valuesStr.Append(" Values(");
            sqlStr.Append(string.Format("{0} ,", "CreateDate"));
            valuesStr.Append(string.Format("@{0},", "CreateDate"));
            var paramCreatateDateTime = new MySqlParameter { DbType = DbType.DateTime, Value = time, ParameterName = string.Format("@{0}", "CreateDate") };
            parametrseListesi.Add(paramCreatateDateTime);
            sqlStr.Append(string.Format("{0},", "CreateUserId"));
            valuesStr.Append(string.Format("@{0},", "CreateUserId"));
            var paramCreatateUserId = new MySqlParameter { DbType = DbType.Int32, Value = TaxPayerDescendant.UserId, ParameterName = string.Format("@{0}", "CreateUserId") };
            parametrseListesi.Add(paramCreatateUserId);
            sqlStr.Append(string.Format("{0},", "UpdateUserId"));
            valuesStr.Append(string.Format("@{0},", "UpdateUserId"));
            var paramUpdateUserId = new MySqlParameter { DbType = DbType.Int32, Value = TaxPayerDescendant.UserId, ParameterName = string.Format("@{0}", "UpdateUserId") };
            parametrseListesi.Add(paramUpdateUserId);

            sqlStr.Append(string.Format("{0},", "Rowversion"));
            valuesStr.Append(string.Format("@{0},", "Rowversion"));
            var paramRowversion = new MySqlParameter { DbType = DbType.Int32, Value = 1, ParameterName = string.Format("@{0}", "Rowversion") };
            parametrseListesi.Add(paramRowversion);
            sqlStr.Append(string.Format("{0} ,", "UpdateDate"));
            valuesStr.Append(string.Format("@{0},", "UpdateDate"));
            var paramUpdateDateTime = new MySqlParameter { DbType = DbType.DateTime, Value = time, ParameterName = string.Format("@{0}", "UpdateDate") };
            parametrseListesi.Add(paramUpdateDateTime);
            sqlStr.Append(string.Format("{0} ,", "DeleteUserId"));
            valuesStr.Append(string.Format("@{0},", "DeleteUserId"));
            var paramDeleteUserId = new MySqlParameter { DbType = DbType.Int32, Value = 0, ParameterName = string.Format("@{0}", "DeleteUserId") };
            parametrseListesi.Add(paramDeleteUserId);
            sqlStr.Append(string.Format("{0} ,", "DeleteTime"));
            valuesStr.Append(string.Format("@{0},", "DeleteTime"));
            var paramDeleteTime = new MySqlParameter { DbType = DbType.DateTime, Value = new DateTime(1901, 1, 1), ParameterName = string.Format("@{0}", "DeleteTime") };
            parametrseListesi.Add(paramDeleteTime);
            sqlStr.Append(string.Format("{0} ,", "IsDeleted"));
            valuesStr.Append(string.Format("@{0},", "IsDeleted"));
            var paramIsDeleted = new MySqlParameter { DbType = DbType.Boolean, Value = false, ParameterName = string.Format("@{0}", "IsDeleted") };
            parametrseListesi.Add(paramIsDeleted);
            sqlStr.Append(string.Format("{0},", "DbActionType"));
            valuesStr.Append(string.Format("@{0},", "DbActionType"));
            var paramDbActionType = new MySqlParameter { DbType = DbType.Int32, Value = Convert.ToInt32(dbActionType), ParameterName = string.Format("@{0}", "DbActionType") };
            parametrseListesi.Add(paramDbActionType);

            sqlStr.Append(string.Format("{0},", "UserId"));
            valuesStr.Append(string.Format("@{0},", "UserId"));
            var paramUserId = new MySqlParameter { DbType = DbType.Int32, Value = TaxPayerDescendant.UserId, ParameterName = string.Format("@{0}", "UserId") };
            parametrseListesi.Add(paramUserId);

            sqlStr.Append(string.Format("{0},", "ObjectOID"));
            valuesStr.Append(string.Format("@{0},", "ObjectOID"));
            var paramObjectOID = new MySqlParameter { DbType = DbType.Int32, Value = objectOid, ParameterName = string.Format("@{0}", "ObjectOID") };
            parametrseListesi.Add(paramObjectOID);


            sqlStr.Append(string.Format("{0},", "ObjectRowVersion"));
            valuesStr.Append(string.Format("@{0},", "ObjectRowVersion"));
            var paramObjectRowVersion = new MySqlParameter { DbType = DbType.Int32, Value = RowVersion, ParameterName = string.Format("@{0}", "ObjectRowVersion") };
            parametrseListesi.Add(paramObjectRowVersion);

            sqlStr.Append(string.Format("{0},", "ObjectTableName"));
            valuesStr.Append(string.Format("@{0},", "ObjectTableName"));
            var paramObjectTableName = new MySqlParameter { DbType = DbType.String, Value = objectTableName, ParameterName = string.Format("@{0}", "ObjectTableName") };
            parametrseListesi.Add(paramObjectTableName);

            sqlStr.Append(string.Format("{0},", "ObjectTypeFullName"));
            valuesStr.Append(string.Format("@{0},", "ObjectTypeFullName"));
            var paramObjectTypeFullName = new MySqlParameter { DbType = DbType.String, Value = ObjectType.FullName, ParameterName = string.Format("@{0}", "ObjectTypeFullName") };
            parametrseListesi.Add(paramObjectTypeFullName);

            sqlStr.Append(string.Format("{0},", "Query"));
            valuesStr.Append(string.Format("@{0},", "Query"));
            var paramQuery = new MySqlParameter { DbType = DbType.String, Value = query, ParameterName = string.Format("@{0}", "Query") };
            parametrseListesi.Add(paramQuery);

            sqlStr.Append(string.Format("{0},", "Time"));
            valuesStr.Append(string.Format("@{0},", "Time"));
            var paramTime = new MySqlParameter { DbType = DbType.DateTime, Value = time, ParameterName = string.Format("@{0}", "Time") };
            parametrseListesi.Add(paramTime);

            sqlStr.Append(string.Format("{0},", "ClientIPAddress"));
            valuesStr.Append(string.Format("@{0},", "ClientIPAddress"));
            var paramClientIPAddress = new MySqlParameter { DbType = DbType.String, Value = TaxPayerDescendant.IpAdres, ParameterName = string.Format("@{0}", "ClientIPAddress") };
            parametrseListesi.Add(paramClientIPAddress);


            sqlStr.Append(string.Format("{0},", "ClientMacAddress"));
            valuesStr.Append(string.Format("@{0},", "ClientMacAddress"));
            var paramClientMacAddress = new MySqlParameter { DbType = DbType.String, Value = TaxPayerDescendant.MacAdress, ParameterName = string.Format("@{0}", "ClientMacAddress") };
            parametrseListesi.Add(paramClientMacAddress);

            sqlStr.Append(string.Format("{0},", "ClientMachineName"));
            valuesStr.Append(string.Format("@{0},", "ClientMachineName"));
            var paramClientMachineName = new MySqlParameter { DbType = DbType.String, Value = TaxPayerDescendant.ComputareName, ParameterName = string.Format("@{0}", "ClientMachineName") };
            parametrseListesi.Add(paramClientMachineName);

            sqlStr.Append(string.Format("{0},", "FormFullName"));
            valuesStr.Append(string.Format("@{0},", "FormFullName"));
            var paramFormFullName = new MySqlParameter { DbType = DbType.String, Value = TaxPayerDescendant.FormName, ParameterName = string.Format("@{0}", "FormFullName") };
            parametrseListesi.Add(paramFormFullName);


            sqlStr.Append(string.Format("{0}", "FormCaption"));
            valuesStr.Append(string.Format("@{0}", "FormCaption"));
            var paramFormCaption = new MySqlParameter { DbType = DbType.String, Value = TaxPayerDescendant.FormCapion, ParameterName = string.Format("@{0}", "FormCaption") };
            parametrseListesi.Add(paramFormCaption);
            sqlStr.Append(") ");
            valuesStr.Append(");   SELECT LAST_INSERT_ID();");
            sqlStr.Append(valuesStr);
            var oid = conn.ExecuteScalarInsert(sqlStr.ToString(), parametrseListesi);
            foreach (var item in parametrs)
            {
                sqlStr = new StringBuilder();
                valuesStr = new StringBuilder();
                parametrseListesi = new List<DbParameter>();
                sqlStr.Append($"INSERT INTO {conn}_LOG.SYS.LogDetail");
                valuesStr.Append(" Values(");
                sqlStr.Append(string.Format("{0} ,", "CreateDate"));
                valuesStr.Append(string.Format("@{0},", "CreateDate"));
                var paramCreatateDateTimed = new MySqlParameter { DbType = DbType.DateTime, Value = time, ParameterName = string.Format("@{0}", "CreateDate") };
                parametrseListesi.Add(paramCreatateDateTimed);
                sqlStr.Append(string.Format("{0},", "CreateUserId"));
                valuesStr.Append(string.Format("@{0},", "CreateUserId"));
                var paramCreatateUserIdd = new MySqlParameter { DbType = DbType.Int32, Value = TaxPayerDescendant.UserId, ParameterName = string.Format("@{0}", "CreateUserId") };
                parametrseListesi.Add(paramCreatateUserIdd);
                sqlStr.Append(string.Format("{0},", "UpdateUserId"));
                valuesStr.Append(string.Format("@{0},", "UpdateUserId"));
                var paramUpdateUserIdd = new MySqlParameter { DbType = DbType.Int32, Value = TaxPayerDescendant.UserId, ParameterName = string.Format("@{0}", "UpdateUserId") };
                parametrseListesi.Add(paramUpdateUserIdd);

                sqlStr.Append(string.Format("{0},", "Rowversion"));
                valuesStr.Append(string.Format("@{0},", "Rowversion"));
                var paramRowversiond = new MySqlParameter { DbType = DbType.Int32, Value = 1, ParameterName = string.Format("@{0}", "Rowversion") };
                parametrseListesi.Add(paramRowversiond);
                sqlStr.Append(string.Format("{0} ,", "UpdateDate"));
                valuesStr.Append(string.Format("@{0},", "UpdateDate"));
                var paramUpdateDateTimed = new MySqlParameter { DbType = DbType.DateTime, Value = time, ParameterName = string.Format("@{0}", "UpdateDate") };
                parametrseListesi.Add(paramUpdateDateTimed);
                sqlStr.Append(string.Format("{0} ,", "DeleteUserId"));
                valuesStr.Append(string.Format("@{0},", "DeleteUserId"));
                var paramDeleteUserIdd = new MySqlParameter { DbType = DbType.Int32, Value = 0, ParameterName = string.Format("@{0}", "DeleteUserId") };
                parametrseListesi.Add(paramDeleteUserIdd);
                sqlStr.Append(string.Format("{0} ,", "DeleteTime"));
                valuesStr.Append(string.Format("@{0},", "DeleteTime"));
                var paramDeleteTimed = new MySqlParameter { DbType = DbType.DateTime, Value = new DateTime(1901, 1, 1), ParameterName = string.Format("@{0}", "DeleteTime") };
                parametrseListesi.Add(paramDeleteTimed);
                sqlStr.Append(string.Format("{0} ,", "IsDeleted"));
                valuesStr.Append(string.Format("@{0},", "IsDeleted"));
                var paramIsDeletedd = new MySqlParameter { DbType = DbType.Boolean, Value = false, ParameterName = string.Format("@{0}", "IsDeleted") };
                parametrseListesi.Add(paramIsDeletedd);

                sqlStr.Append(string.Format("{0} ,", "Master_ID"));
                valuesStr.Append(string.Format("@{0},", "Master_ID"));
                var paramMaster = new MySqlParameter { DbType = DbType.Int32, Value = oid, ParameterName = string.Format("@{0}", "Master_ID") };
                parametrseListesi.Add(paramMaster);


                sqlStr.Append(string.Format("{0} ,", "ParameterName"));
                valuesStr.Append(string.Format("@{0},", "ParameterName"));
                var paramParameterName = new MySqlParameter { DbType = DbType.String, Value = item.ParameterName, ParameterName = string.Format("@{0}", "ParameterName") };
                parametrseListesi.Add(paramParameterName);

                sqlStr.Append(string.Format("{0} ,", "FieldName"));
                valuesStr.Append(string.Format("@{0},", "FieldName"));
                var paramFieldName = new MySqlParameter { DbType = DbType.String, Value = item.ParameterName.Replace('@', ' '), ParameterName = string.Format("@{0}", "FieldName") };
                parametrseListesi.Add(paramFieldName);

                sqlStr.Append(string.Format("{0} ,", "FieldValue"));
                valuesStr.Append(string.Format("@{0},", "FieldValue"));
                var paramFieldValue = new MySqlParameter { DbType = DbType.String, Value = item.Value.ToString(), ParameterName = string.Format("@{0}", "FieldValue") };
                parametrseListesi.Add(paramFieldValue);

                sqlStr.Append(string.Format("{0} ", "FiledType"));
                valuesStr.Append(string.Format("@{0}", "FiledType"));
                var paramFiledType = new MySqlParameter { DbType = DbType.String, Value = item.DbType.ToString(), ParameterName = string.Format("@{0}", "FiledType") };
                parametrseListesi.Add(paramFiledType);
                sqlStr.Append(") ");
                valuesStr.Append("); SELECT LAST_INSERT_ID();");
                sqlStr.Append(valuesStr);
                conn.ExecuteScalarInsert(sqlStr.ToString(), parametrseListesi);






            }
            return true;
        }
        #endregion

        private static string GetParameterString(List<DbParameter> parameters)
        {
            StringBuilder parametrsBuilder = new StringBuilder();
            parameters.ForEach(item => parametrsBuilder.AppendFormat("ParametreAdı:{0}ParametreDeğeri:{1} ,", item.ParameterName, item.Value));

            return parametrsBuilder.ToString();
        }

    }

    public enum DbActionType : int
    {
        [DisplayEnumName("GÖRÜNTÜLEME")]
        Select = 1,
        [DisplayEnumName("YENİ KAYIT")]
        Insert = 2,
        [DisplayEnumName("GÜNCELLEME")]
        Update = 4,
        [DisplayEnumName("SİLME")]
        Delete = 8
    }
}
