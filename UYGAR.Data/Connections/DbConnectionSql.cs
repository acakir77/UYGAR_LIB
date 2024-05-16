using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using UYGAR.Data.Attributes;
using UYGAR.Data.Base;
using UYGAR.Data.Connection;
using UYGAR.Data.Criterias;
using UYGAR.Exceptions;
using UYGAR.Log.Client;
using UYGAR.Log.Shared;

namespace UYGAR.Data.Connections
{
    [Serializable]
    public class DbConnectionSql : DbConnectionBase
    {

        public override string DbConnectionString => string.Format(ConfigurationManager.AppSettings["ConnStr"], TaxPayerDescendant.KullaniciAdi);
        public override string Shema => $"{ConfigurationManager.AppSettings["ConnSchema"]}";
        public override string DbName => ConfigurationManager.AppSettings["mysqlDbName"];
        public override void InsertObject(Model _object)
        {
            try
            {


                _object.CreateUserId = TaxPayerDescendant.UserId;
                _object.UpdateUserId = TaxPayerDescendant.UserId;
                StringBuilder sqlStr = new StringBuilder();
                StringBuilder valuesStr = new StringBuilder();
                List<DbParameter> parametrseListesi = new List<DbParameter>();
                Type objectType = _object.GetType();
                sqlStr.Append($"INSERT INTO \"{TableAttribute.GetName(objectType)}\" (");
                valuesStr.Append(" Values(");
                sqlStr.Append($"[{"CreateDate"}],");
                valuesStr.Append($"@{"CreateDate"},");
                SqlParameter paramCreatateDateTime = new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Value = DateTime.Now,
                    ParameterName = $"@{"CreateDate"}"
                };
                parametrseListesi.Add(paramCreatateDateTime);
                sqlStr.Append($"[{"CreateUserId"}],");
                valuesStr.Append($"@{"CreateUserId"},");
                SqlParameter paramCreatateUserId = new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    Value = TaxPayerDescendant.UserId,
                    ParameterName = $"@{"CreateUserId"}"
                };
                parametrseListesi.Add(paramCreatateUserId);
                sqlStr.Append($"[{"Rowversion"}],");
                valuesStr.Append($"@{"Rowversion"},");
                SqlParameter paramRowversion = new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    Value = 1,
                    ParameterName = $"@{"Rowversion"}"
                };
                parametrseListesi.Add(paramRowversion);
                sqlStr.Append($"[{"UpdateDate"}],");
                valuesStr.Append($"@{"UpdateDate"},");
                SqlParameter paramUpdateDateTime = new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Value = DateTime.Now,
                    ParameterName = $"@{"UpdateDate"}"
                };
                parametrseListesi.Add(paramUpdateDateTime);
                sqlStr.Append($"[{"UpdateUserId"}],");
                valuesStr.Append($"@{"UpdateUserId"},");
                SqlParameter paramUpdateUserId = new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    Value = TaxPayerDescendant.UserId,
                    ParameterName = $"@{"UpdateUserId"}"
                };
                parametrseListesi.Add(paramUpdateUserId);
                sqlStr.Append($"[{"DeleteUserId"}],");
                valuesStr.Append($"@{"DeleteUserId"},");
                SqlParameter paramDeleteUserId = new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    Value = 0,
                    ParameterName = $"@{"DeleteUserId"}"
                };
                parametrseListesi.Add(paramDeleteUserId);
                sqlStr.Append($"[{"DeleteTime"}],");
                valuesStr.Append($"@{"DeleteTime"},");
                SqlParameter paramDeleteTime = new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Value = new DateTime(1901, 1, 1),
                    ParameterName = $"@{"DeleteTime"}"
                };
                parametrseListesi.Add(paramDeleteTime);
                sqlStr.Append($"[{"IsDeleted"}]");
                valuesStr.Append($"@{"IsDeleted"}");
                SqlParameter paramIsDeleted = new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Value = false,
                    ParameterName = $"@{"IsDeleted"}"
                };
                parametrseListesi.Add(paramIsDeleted);

                int count = 0;
                PropertyInfo[] propertyInfos = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                Array.ForEach(propertyInfos, propertyInfo =>
                {
                    if ((ColumnAttribute.GetColumnAttribute(propertyInfo) != null ||
                         AssociationAttribute.GetName(propertyInfo) != null) &
                        !string.IsNullOrEmpty(TypeName(propertyInfo)))
                    {
                        sqlStr.Append(",");
                        valuesStr.Append(",");
                        if (propertyInfo.PropertyType.IsSubclassOf(typeof(Model)))
                        {
                            sqlStr.Append($"[{propertyInfo.Name}_ID]");
                            string paramnae = $"@{$"{propertyInfo.Name}_ID"}";
                            valuesStr.Append(paramnae);
                            SqlParameter param = new SqlParameter { ParameterName = paramnae, SqlDbType = SqlDbType.Int };
                            if (propertyInfo.GetValue(_object, null) != null)
                                param.Value = ((Model)propertyInfo.GetValue(_object, null)).OID;
                            else
                            {
                                param.IsNullable = true;
                                param.Value = DBNull.Value;
                            }
                            parametrseListesi.Add(param);
                        }
                        else
                        {
                            sqlStr.Append(string.Format("[{0}]", propertyInfo.Name));
                            valuesStr.AppendFormat("@{0}", propertyInfo.Name);
                            SqlParameter param = new SqlParameter
                            {

                                ParameterName = string.Format("@{0}", propertyInfo.Name),
                                Value = GetPropertyInfoValue(propertyInfo, _object),
                                SqlDbType = GetValueType(propertyInfo),



                            };

                            parametrseListesi.Add(param);
                        }
                        count++;
                    }
                });

                if (count > 0)
                {
                    sqlStr.Append(") ");
                    valuesStr.Append(")");
                    sqlStr.Append(valuesStr);
                    sqlStr.Append(" SELECT SCOPE_IDENTITY()");
                    int oId = ExecuteScalarInsert(sqlStr.ToString(), parametrseListesi);
                    InsertLog(DbActionType.Insert, 1, oId, TableAttribute.GetName(objectType), objectType, sqlStr.ToString(), DateTime.Now, parametrseListesi);

                    //if (Db_ActionAttribute.IsInsertRequired(objectType))
                    //    DbAction.Write(DbActionType.Insert, TaxPayerDescendant.UserId, oId,
                    //        TableAttribute.GetName(objectType), sqlStr.ToString(), string.Empty, DateTime.Now,
                    //        TaxPayerDescendant.IpAdres, TaxPayerDescendant.MacAdress, parametrseListesi, this);
                    _object.OID = oId;
                    _object.Rowversion = 1;
          
                    _object.CreateUserId = TaxPayerDescendant.UserId;
                    _object.UpdateUserId = TaxPayerDescendant.UserId;
                    _object.CreateDate = DateTime.Now;
                    _object.UpdateDate = DateTime.Now;


                }


            }


            catch (Exception ex)
            {

                if (ex.Message.StartsWith("Cannot insert duplicate key row"))
                {
                    throw new UygulamaHatasiException(
                        "Bu Kayıt İle Aynı Özellikleri Taşıyan Kayıt Zaten Sistemde Mevcut!!");
                }
                else
                {
                    throw ex;
                }
            }
        }

        public override void UpdateObject(Model _object)
        {
            try
            {

                int oldRowVersion = _object.Rowversion;
                _object.UpdateUserId = TaxPayerDescendant.UserId;
                StringBuilder sqlStr = new StringBuilder();
                List<DbParameter> parametrseListesi = new List<DbParameter>();
                Type objectType = _object.GetType();
                sqlStr.Append($"UPDATE \"dbo\".\"{TableAttribute.GetName(objectType)}\" Set ");
                SqlParameter paramCreatateDateTime = new SqlParameter { SqlDbType = SqlDbType.DateTime, Value = DateTime.Now, ParameterName = $"@{"CreateDate"}" };
                parametrseListesi.Add(paramCreatateDateTime);
                _object.Rowversion = _object.Rowversion + 1;
                sqlStr.Append($"[{"Rowversion"}]={$"@{"Rowversion"},"}");
                SqlParameter paramRowversion = new SqlParameter { SqlDbType = SqlDbType.Int, Value = _object.Rowversion, ParameterName = $"@{"Rowversion"}" };
                parametrseListesi.Add(paramRowversion);
                sqlStr.Append($"[{"UpdateDate"}]={$"@{"UpdateDate"},"}");
                SqlParameter paramUpdateDateTime = new SqlParameter { SqlDbType = SqlDbType.DateTime, Value = DateTime.Now, ParameterName = $"@{"UpdateDate"}" };
                parametrseListesi.Add(paramUpdateDateTime);
                sqlStr.Append($"[{"UpdateUserId"}]={$"@{"UpdateUserId"}"}");
                SqlParameter paramUpdateUserId = new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    Value = TaxPayerDescendant.UserId,
                    ParameterName =
                    $"@{"UpdateUserId"}"
                };
                parametrseListesi.Add(paramUpdateUserId);
                PropertyInfo[] propertyInfos = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                Array.ForEach(propertyInfos, propertyInfo =>
                {
                    if ((ColumnAttribute.GetColumnAttribute(propertyInfo) != null || AssociationAttribute.GetName(propertyInfo) != null) & !string.IsNullOrEmpty(TypeName(propertyInfo)))
                    {
                        sqlStr.Append(",");
                        if (propertyInfo.PropertyType.IsSubclassOf(typeof(Model)))
                        {
                            sqlStr.Append(
                                $"[{propertyInfo.Name}_ID]={$"@{$"{propertyInfo.Name}_ID"}"}");
                            SqlParameter param = new SqlParameter
                            {
                                ParameterName =
                                $"@{$"{propertyInfo.Name}_ID"}",
                                SqlDbType = SqlDbType.Int
                            };
                            if (propertyInfo.GetValue(_object, null) != null)
                                param.Value = ((Model)propertyInfo.GetValue(_object, null)).OID;
                            else
                            {
                                param.IsNullable = true;
                                param.Value = DBNull.Value;
                            }
                            parametrseListesi.Add(param);
                        }
                        else
                        {
                            sqlStr.Append($"[{propertyInfo.Name}]={$"@{propertyInfo.Name}"}");
                            SqlParameter param = new SqlParameter { ParameterName = $"@{propertyInfo.Name}", Value = GetPropertyInfoValue(propertyInfo, _object), SqlDbType = GetValueType(propertyInfo) };
                            parametrseListesi.Add(param);
                        }

                    }
                });
                sqlStr.Append($" Where \"OID\" = {_object.OID} AND \"Rowversion\"={oldRowVersion}");
                int isUpdatet = ExecuteScalar(sqlStr.ToString(), parametrseListesi);
                if (isUpdatet.Equals(0))
                    throw new DbKayitDegistirilmisException();
                InsertLog(DbActionType.Update, oldRowVersion, _object.OID, TableAttribute.GetName(objectType), objectType, sqlStr.ToString(), DateTime.Now, parametrseListesi);

                //if (Db_ActionAttribute.IsUpdateRequired(objectType))
                //    DbAction.Write(DbActionType.Update, TaxPayerDescendant.UserId, _object.OID, TableAttribute.GetName(objectType), sqlStr.ToString(), string.Empty, DateTime.Now, TaxPayerDescendant.IpAdres, TaxPayerDescendant.MacAdress, parametrseListesi, this);


            }
            catch (Exception ex)
            {
                if (!(ex is KYSException))
                {
                    DTOExeptionModel model = new DTOExeptionModel();
                    model.UserId = TaxPayerDescendant.UserId;
                    model.ExecptionString = ex.Message;
                    model.ExecptionTrace = ex.Source;
                    model.Time = DateTime.Now;
                    model.ClientIPAddress = TaxPayerDescendant.IpAdres;
                    model.ClientMacAddress = TaxPayerDescendant.MacAdress;
                    model.ClientMachineName = TaxPayerDescendant.ComputareName;
                    model.FormFullName = TaxPayerDescendant.FormName;
                    model.FormCaption = TaxPayerDescendant.FormCapion;
                    LogClient.PushExeptionLog(model);
                }

                throw ex;
            }
        }


        public override void DeleteObject(Model _object)
        {
            StringBuilder sqlStr = new StringBuilder();
            Type objectType = _object.GetType();
            sqlStr.AppendFormat(@"SELECT
  KCU1.TABLE_NAME AS FK_TABLE_NAME
 ,KCU1.COLUMN_NAME AS FK_COLUMN_NAME
FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS RC
INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU1
  ON KCU1.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG
    AND KCU1.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA
    AND KCU1.CONSTRAINT_NAME = RC.CONSTRAINT_NAME
INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU2
  ON KCU2.CONSTRAINT_CATALOG = RC.UNIQUE_CONSTRAINT_CATALOG
    AND KCU2.CONSTRAINT_SCHEMA = RC.UNIQUE_CONSTRAINT_SCHEMA
    AND KCU2.CONSTRAINT_NAME = RC.UNIQUE_CONSTRAINT_NAME
    AND KCU2.ORDINAL_POSITION = KCU1.ORDINAL_POSITION
WHERE KCU2.TABLE_NAME = '{0}'", TableAttribute.GetName(objectType));
            var dattable = LoadToDataTable(sqlStr.ToString());
            for (int i = 0; i < dattable.Rows.Count; i++)
            {
                string query = string.Format(@"select OID from {0} where {1}={2} AND {0}.IsDeleted=0", dattable.Rows[i][0], dattable.Rows[i][1], _object.OID);
                var datacountdata = LoadToDataTable(query);
                if (datacountdata.Rows.Count > 0)
                {

                    throw new DBSilinemezException(new Exception(string.Format(@"{0}   Tablosunda {1} Nolu Kayıtta Bu Kayıt Kullanıldığında Silinemez!!", dattable.Rows[i][0], datacountdata.Rows[0][0])));
                }


            }
            sqlStr = new StringBuilder();
            _object.IsDeleted = true;
            _object.DeleteTime = DateTime.Now;
            _object.DeleteUserId = TaxPayerDescendant.UserId;
            List<DbParameter> parametrseListesi = new List<DbParameter>();
            sqlStr.Append(String.Format("UPDATE {0} Set ", TableAttribute.GetName(objectType)));
            sqlStr.Append(string.Format("{0}={1}", "DeleteUserId", string.Format("@{0},", "DeleteUserId")));
            var paramDeleteUserId = new SqlParameter { DbType = DbType.Int32, Value = TaxPayerDescendant.UserId, ParameterName = string.Format("@{0}", "DeleteUserId") };
            parametrseListesi.Add(paramDeleteUserId);
            sqlStr.Append(string.Format("{0}={1}", "DeleteTime", string.Format("@{0},", "DeleteTime")));
            var paramDeleteTime = new SqlParameter { DbType = DbType.DateTime, Value = DateTime.Now, ParameterName = string.Format("@{0}", "DeleteTime") };
            parametrseListesi.Add(paramDeleteTime);
            sqlStr.Append(string.Format("{0}={1}", "IsDeleted", string.Format("@{0}", "IsDeleted")));
            var paramIsDeleted = new SqlParameter { DbType = DbType.Boolean, Value = true, ParameterName = string.Format("@{0}", "IsDeleted") };
            parametrseListesi.Add(paramIsDeleted);
            sqlStr.Append($@" Where OID ={_object.OID} AND Rowversion={_object.Rowversion} ");

            int isUpdatet = ExecuteScalar(sqlStr.ToString(), parametrseListesi);
            if (isUpdatet.Equals(0))
            {
                throw new DbKayitDegistirilmisException();
            }
            InsertLog(DbActionType.Delete, _object.Rowversion, _object.OID, TableAttribute.GetName(objectType), objectType, sqlStr.ToString(), DateTime.Now, new List<DbParameter>());


        }
        private SqlDbType GetValueType(PropertyInfo info)
        {
            if (info.PropertyType == typeof(string))
                return SqlDbType.NVarChar;
            if (info.PropertyType == typeof(byte[]))
                return SqlDbType.Image;
            if (info.PropertyType == typeof(DateTime))
                return SqlDbType.DateTime;
            if (info.PropertyType == typeof(int) || info.PropertyType == typeof(Int16) || info.PropertyType == typeof(Int32))
                return SqlDbType.Int;
            if (info.PropertyType.IsEnum)
                return SqlDbType.Int;
            if (info.PropertyType == typeof(decimal) || info.PropertyType == typeof(Decimal))
                return SqlDbType.Decimal;
            if (info.PropertyType == typeof(double) || info.PropertyType == typeof(Double))
                return SqlDbType.Float;
            return SqlDbType.NVarChar;

        }
        public override string GetTypeName(Type type, int size)
        {
            if (type == typeof(long))
                return "bigint";

            if (type == typeof(byte) || type == typeof(Int16))
                return "smallint";

            if (type == typeof(Int32))
                return "int";

            if (type == typeof(string))
                if (size <= 1000)
                    return $"nvarchar({(size != 0 ? size : 1000)})";
                else
                    return "text";

            if (type == typeof(DateTime))
                return "datetime";

            if (type == typeof(DateTime?))
                return "datetime";

            if (type == typeof(double))
                return "float(53)";

            if (type == typeof(decimal))
                return "decimal(18, 8)";

            if (type == typeof(bool))
                return "bit";

            if (type == typeof(byte[]))
                return "image";

            if (type == typeof(Guid))
                return "uniqueidentifier";
            if (type.IsEnum)
                return "INTEGER";
            if (type.IsSubclassOf(typeof(Model)))
                return "INTEGER";
            return null;
        }

        public override int ExecuteScalarInsert(string query, List<DbParameter> parameters)
        {
            int retval = -1;
            try
            {

                using (SqlConnection newconnection = new SqlConnection(DbConnectionString))
                {

                    if (newconnection.State != ConnectionState.Open)
                        newconnection.Open();

                    using (SqlCommand cmdS = new SqlCommand(query, newconnection))
                    {
                        parameters.ForEach(item => cmdS.Parameters.Add(item));
                        retval = Convert.ToInt32(cmdS.ExecuteScalar());

                    }
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
            return retval;
        }

        public override int ExecuteScalar(string query, List<DbParameter> parametrs)
        {
            int retval = 0;
            try
            {

                using (SqlConnection newconnection = new SqlConnection(DbConnectionString))
                {
                    if (newconnection.State != ConnectionState.Open)
                        newconnection.Open();

                    using (SqlCommand cmdS = new SqlCommand(query, newconnection))
                    {
                        parametrs.ForEach(item => cmdS.Parameters.Add(item));
                        retval = cmdS.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                if (!(ex is KYSException))
                {
                    DTOExeptionModel model = new DTOExeptionModel();
                    model.UserId = TaxPayerDescendant.UserId;
                    model.ExecptionString = ex.Message;
                    model.ExecptionTrace = ex.Source;
                    model.Time = DateTime.Now;
                    model.ClientIPAddress = TaxPayerDescendant.IpAdres;
                    model.ClientMacAddress = TaxPayerDescendant.MacAdress;
                    model.ClientMachineName = TaxPayerDescendant.ComputareName;
                    model.FormFullName = TaxPayerDescendant.FormName;
                    model.FormCaption = TaxPayerDescendant.FormCapion;
                    LogClient.PushExeptionLog(model);
                }
                throw ex;
            }
            return retval;
        }

        public override object ExecuteScalar(string query)
        {
            try
            {

                using (var newconnection = new SqlConnection(DbConnectionString))
                {
                    if (newconnection.State != ConnectionState.Open)
                        newconnection.Open();
                    object returneddata = null;
                    using (var cmdS = new SqlCommand())
                    {

                        cmdS.CommandText = query;
                        cmdS.Connection = newconnection;
                        returneddata = cmdS.ExecuteScalar();
                    }
                    newconnection.Close();
                    newconnection.Dispose();
                    return returneddata;



                }
            }
            catch (Exception ex)
            {
                if (!(ex is KYSException))
                {
                    DTOExeptionModel model = new DTOExeptionModel();
                    model.UserId = TaxPayerDescendant.UserId;
                    model.ExecptionString = ex.Message;
                    model.ExecptionTrace = ex.Source;
                    model.Time = DateTime.Now;
                    model.ClientIPAddress = TaxPayerDescendant.IpAdres;
                    model.ClientMacAddress = TaxPayerDescendant.MacAdress;
                    model.ClientMachineName = TaxPayerDescendant.ComputareName;
                    model.FormFullName = TaxPayerDescendant.FormName;
                    model.FormCaption = TaxPayerDescendant.FormCapion;
                    LogClient.PushExeptionLog(model);
                }

                throw ex;
            }
        }

        public override DataTable ExecuteDataTable(string query)
        {
            try
            {
                //SqlConnectionStringBuilder sConnB = new SqlConnectionStringBuilder();


                using (var newconnection = new SqlConnection(DbConnectionString))
                {
                    if (newconnection.State != ConnectionState.Open)
                        newconnection.Open();

                    using (var cmdS = new SqlCommand(query, newconnection))
                    {


                        DataTable table = new DataTable();
                        using (SqlDataAdapter adapterS = new SqlDataAdapter(cmdS))
                        {
                            adapterS.Fill(table);
                        }
                        newconnection.Close();
                        newconnection.Dispose();
                        return table;

                    }

                }

            }
            catch (Exception ex)
            {
                if (!(ex is KYSException))
                {
                    DTOExeptionModel model = new DTOExeptionModel();
                    model.UserId = TaxPayerDescendant.UserId;
                    model.ExecptionString = ex.Message;
                    model.ExecptionTrace = ex.Source;
                    model.Time = DateTime.Now;
                    model.ClientIPAddress = TaxPayerDescendant.IpAdres;
                    model.ClientMacAddress = TaxPayerDescendant.MacAdress;
                    model.ClientMachineName = TaxPayerDescendant.ComputareName;
                    model.FormFullName = TaxPayerDescendant.FormName;
                    model.FormCaption = TaxPayerDescendant.FormCapion;
                    LogClient.PushExeptionLog(model);
                }

                throw ex;
            }
        }

        public override string db_old_query(Model _object)
        {
            StringBuilder sqlStr = new StringBuilder();
            StringBuilder valuesStr = new StringBuilder();

            Type objectType = _object.GetType();


            sqlStr.Append($"INSERT INTO \"{TableAttribute.GetName(objectType)}\" (");
            valuesStr.Append(" Values(");

            int oId = _object.OID;
            sqlStr.Append($"\"{"OID"}\",\"{"CreateDate"}\"");

            valuesStr.Append($"{oId},CAST('{DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss")}'AS DATE)");
            int count = 0;
            foreach (PropertyInfo propertyInfo in objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(propertyInfo => (ColumnAttribute.GetColumnAttribute(propertyInfo) != null || AssociationAttribute.GetName(propertyInfo) != null) & !string.IsNullOrEmpty(TypeName(propertyInfo))))
            {
                sqlStr.Append(",");
                valuesStr.Append(",");


                if (propertyInfo.PropertyType.BaseType == typeof(Model))
                    sqlStr.Append($"\"{propertyInfo.Name}_ID\"");
                else
                    sqlStr.Append($"\"{propertyInfo.Name}\"");

                if (propertyInfo.PropertyType == typeof(string))
                    if (propertyInfo.GetValue(_object, null) == null)
                        valuesStr.Append("''");
                    else
                        valuesStr.Append(string.Format("'{0}'", propertyInfo.GetValue(_object, null).ToString().Replace("'", "`")));
                else if (propertyInfo.PropertyType == typeof(DateTime))
                    valuesStr.Append($"CAST('{propertyInfo.GetValue(_object, null)}' AS DATE)");
                else if (propertyInfo.PropertyType.BaseType == typeof(Model))
                {
                    int referencedId = ((Model)propertyInfo.GetValue(_object, null)) == null ? 0 : ((Model)propertyInfo.GetValue(_object, null)).OID;
                    if (referencedId == -1)
                        ((Model)propertyInfo.GetValue(_object, null)).Save(this);
                    valuesStr.Append(referencedId == 0
                        ? "null"
                        : $"{((Model)propertyInfo.GetValue(_object, null)).OID}");
                }
                else if (propertyInfo.PropertyType == typeof(bool) || propertyInfo.PropertyType == typeof(Boolean))
                    valuesStr.Append($"'{((bool)propertyInfo.GetValue(_object, null) == true ? 1 : 0)}'");
                else if (propertyInfo.PropertyType.IsEnum)
                    valuesStr.Append($"{Convert.ToInt32(propertyInfo.GetValue(_object, null))}");
                else
                    valuesStr.Append($"{propertyInfo.GetValue(_object, null)}");
                count++;
            }
            if (count > 0)
            {
                sqlStr.Append(") ");
                valuesStr.Append(")");
                sqlStr.Append(valuesStr);
            }
            return sqlStr.ToString();
        }

        public override string ToDbProviderString(Type targetType, object value)
        {
            System.Diagnostics.Trace.Assert(value != null, "Value is cant be null !!!");
            if (targetType == typeof(bool))
            {
                if (Convert.ToBoolean(value))
                    return "'True'";
                else
                    return "'False'";
            }
            if (targetType == typeof(DateTime))
            {

                DateTimeFormatInfo fmt = (new CultureInfo("tr-TR")).DateTimeFormat;
                return $"Convert(DATE,'{((DateTime)value).ToString(fmt)}',{"104"})";
            }
            if (targetType == typeof(string) || targetType == typeof(Guid))
                return $"'{value}'";

            if (targetType.IsEnum)
            {
                if (!(value is Enum))
                    throw new InvalidOperationException(
                        $"Value must be Enum. Bu value type is: {value.GetType().FullName}");

                return (Convert.ToInt32(value)).ToString();
            }

            if (targetType == typeof(decimal))
                return (Convert.ToDecimal(value)).ToString(new CultureInfo("en-US"));
            return value.ToString();
        }
        public override string ToDbProviderString(Type targetType, object value, bool isdatetime)
        {
            System.Diagnostics.Trace.Assert(value != null, "Value is cant be null !!!");
            if (targetType == typeof(bool))
            {
                if (Convert.ToBoolean(value))
                    return "'True'";
                else
                    return "'False'";
            }
            if (targetType == typeof(DateTime))
            {

                DateTimeFormatInfo fmt = (new CultureInfo("tr-TR")).DateTimeFormat;
                return $"Convert(SMALLDATETIME,'{((DateTime)value).ToString(fmt)}',{"104"})";
            }
            if (targetType == typeof(string) || targetType == typeof(Guid))
                return $"'{value}'";

            if (targetType.IsEnum)
            {
                if (!(value is Enum))
                    throw new InvalidOperationException(
                        $"Value must be Enum. Bu value type is: {value.GetType().FullName}");

                return (Convert.ToInt32(value)).ToString();
            }

            if (targetType == typeof(decimal))
                return (Convert.ToDecimal(value)).ToString(new CultureInfo("en-US"));
            return value.ToString();
        }
        #region LoadData

        public override T LoadObject<T>(Criteria criteria)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(GetSelectQuery(typeof(T)));
            T _object = null;
            string criteriasql = criteria.PopulateSQLCriteria(typeof(T), this);
            if (!string.IsNullOrEmpty(criteriasql))
                builder.Append($" AND {criteriasql}");
            string query = builder.ToString();
            query = query.Replace("WHERE ()", "");
            using (SqlConnection newconnection = new SqlConnection(DbConnectionString))
            {
                if (newconnection.State != ConnectionState.Open)
                    newconnection.Open();

                using (SqlCommand cmdS = new SqlCommand(query, newconnection))
                {
                    using (SqlDataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.Read())
                        {
                            var i = 0;
                            _object = new T();
                            _object.Populatereader(reader, i);



                        }


                    }
                }
            }
            if (_object != null)
                InsertLog(DbActionType.Select, _object != null ? _object.Rowversion : 0, _object != null ? _object.OID : 0, TableAttribute.GetName(typeof(T)), typeof(T), query, DateTime.Now, new List<DbParameter>());
            return _object;
        }

        public override T LoadObject<T>(Criteria criteria, Order orders)
        {
            StringBuilder grupuFealdName = new StringBuilder();
            if (orders.Count != 0)
            {
                if (orders.Count == 1)
                {
                    PropertyInfo info = typeof(T).GetProperty(orders[0]);
                    grupuFealdName.AppendFormat(

                    info.PropertyType.IsSubclassOf(typeof(Model)) ? "{0}.{1}_ID" : "{0}.{1}", TableAttribute.GetName(typeof(T)), orders[0]);
                }
                else
                {


                    for (int i = 0; i < orders.Count; i++)
                    {
                        T obj = (T)Activator.CreateInstance(typeof(T));
                        PropertyInfo info = obj.GetType().GetProperty(orders[i]);
                        if (info.PropertyType.IsSubclassOf(typeof(Model)))
                        {
                            grupuFealdName.AppendFormat((i + 1) != orders.Count ? "{0}.{1}_ID," : "{0}.{1}_ID",
                                TableAttribute.GetName(typeof(T)), orders[i]);
                        }
                        else
                        {
                            grupuFealdName.AppendFormat((i + 1) != orders.Count ? "{0}.{1}," : "{0}.{1}", TableAttribute.GetName(typeof(T)), orders[i]);
                        }

                    }
                }
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(GetSelectQuery(typeof(T)));
            T _object = null;

            string criteriasql = criteria.PopulateSQLCriteria(typeof(T), this);
            if (!string.IsNullOrEmpty(criteriasql))
                builder.Append($" AND {criteriasql}");
            builder.AppendFormat(" ORDER BY {0} {1}", grupuFealdName, orders.IsDesc ? "DESC" : "ASC");
            string query = builder.ToString();
            query = query.Replace("WHERE ()", "");
            using (SqlConnection newconnection = new SqlConnection(DbConnectionString))
            {
                if (newconnection.State != ConnectionState.Open)
                    newconnection.Open();

                using (SqlCommand cmdS = new SqlCommand(query, newconnection))
                {



                    using (SqlDataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
                    {


                        if (reader.Read())
                        {
                            int i = 0;
                            _object = new T();
                            _object.Populatereader(reader, i);



                        }


                    }
                }
            }
            if (_object != null)
                InsertLog(DbActionType.Select, _object != null ? _object.Rowversion : 0, _object != null ? _object.OID : 0, TableAttribute.GetName(typeof(T)), typeof(T), query, DateTime.Now, new List<DbParameter>());
            return _object;
        }

        public override ModelCollection<TBaseObject> LoadObjects<TBaseObject>(Criteria criteria)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                ModelCollection<TBaseObject> lst = new ModelCollection<TBaseObject>();
                builder.Append(GetSelectQuery(typeof(TBaseObject)));
                string criteriasql = criteria.PopulateSQLCriteria(typeof(TBaseObject), this);
                if (!string.IsNullOrEmpty(criteriasql))
                    builder.Append($" AND {criteriasql}");
                string query = builder.ToString();
                query = query.Replace("WHERE ()", "");
                using (SqlConnection newconnection = new SqlConnection(DbConnectionString))
                {
                    if (newconnection.State != ConnectionState.Open)
                        newconnection.Open();

                    using (SqlCommand cmdS = new SqlCommand(query, newconnection))
                    {
                        using (SqlDataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (reader.Read())
                            {
                                int i = 0;
                                TBaseObject returned = new TBaseObject();
                                returned.Populatereader(reader, i);
                                lst.Add(returned);
                            }



                        }
                    }
                }
                if (lst.Count > 0)
                    InsertLog(DbActionType.Select, 0, 0, TableAttribute.GetName(typeof(TBaseObject)), typeof(TBaseObject), query, DateTime.Now, new List<DbParameter>());
                return lst;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public override ModelCollection<Model> LoadObjects(Type type, Criteria criteria)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                ModelCollection<Model> lst = new ModelCollection<Model>();
                builder.Append(GetSelectQuery(type));
                string criteriasql = criteria.PopulateSQLCriteria(type, this);
                if (!string.IsNullOrEmpty(criteriasql))
                    builder.Append($" AND {criteriasql}");
                string query = builder.ToString();
                query = query.Replace("WHERE ()", "");
                using (SqlConnection newconnection = new SqlConnection(DbConnectionString))
                {
                    if (newconnection.State != ConnectionState.Open)
                        newconnection.Open();

                    using (SqlCommand cmdS = new SqlCommand(query, newconnection))
                    {
                        using (SqlDataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (reader.Read())
                            {
                                int i = 0;
                                using (Model returned = Activator.CreateInstance(type) as Model)
                                {
                                    if (returned != null)
                                    {
                                        returned.Populatereader(reader, i);
                                        lst.Add(returned);
                                    }
                                }
                            }



                        }
                    }
                }
                if (lst.Count > 0)
                    InsertLog(DbActionType.Select, 0, 0, TableAttribute.GetName(type), type, query, DateTime.Now, new List<DbParameter>());
                return lst;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public override DataSet LoadToDataSet(Type type, Criteria criter)
        {
            try
            {
                DataSet set = new DataSet();
                StringBuilder builder = new StringBuilder();
                ModelCollection<Model> lst = new ModelCollection<Model>();
                builder.Append(GetSelectQuery(type));
                string criteriasql = criter.PopulateSQLCriteria(type, this);
                if (!string.IsNullOrEmpty(criteriasql))
                    builder.Append($" AND {criteriasql}");
                using (SqlConnection newconnection = new SqlConnection(DbConnectionString))
                {
                    if (newconnection.State != ConnectionState.Open)
                        newconnection.Open();
                    string query = builder.ToString();
                    query = query.Replace("WHERE ()", "");
                    using (SqlCommand cmdS = new SqlCommand(query, newconnection))
                    {
                        using (SqlDataAdapter adapet = new SqlDataAdapter(cmdS))
                        {
                            DataTable table = new DataTable();
                            adapet.Fill(table);
                            set.Tables.Add(table);
                        }
                    }
                }
                return set;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public override DataSet LoadToDataSet(string query)
        {
            try
            {
                DataSet set = new DataSet();
                using (SqlConnection newconnection = new SqlConnection(DbConnectionString))
                {
                    if (newconnection.State != ConnectionState.Open)
                        newconnection.Open();

                    using (SqlCommand cmdS = new SqlCommand(query, newconnection))
                    {
                        using (SqlDataAdapter adapet = new SqlDataAdapter(cmdS))
                        {
                            DataTable table = new DataTable();
                            adapet.Fill(table);
                            set.Tables.Add(table);
                        }
                    }
                }
                return set;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public override DataTable LoadToDataTable(string query)
        {
            try
            {

                using (SqlConnection newconnection = new SqlConnection(DbConnectionString))
                {
                    if (newconnection.State != ConnectionState.Open)
                        newconnection.Open();

                    using (SqlCommand cmdS = new SqlCommand(query, newconnection))
                    {
                        using (SqlDataAdapter adapet = new SqlDataAdapter(cmdS))
                        {
                            DataTable table = new DataTable();
                            adapet.Fill(table);
                            return table;
                        }
                    }
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public override ModelCollection<TBaseObject> LoadObjects<TBaseObject>(Criteria criteria, Order orders)
        {
            StringBuilder grupuFealdName = new StringBuilder();
            if (orders.Count != 0)
            {
                if (orders.Count == 1)
                {
                    TBaseObject obj = (TBaseObject)Activator.CreateInstance(typeof(TBaseObject));

                    PropertyInfo info = obj.GetType().GetProperty(orders[0]);
                    grupuFealdName.AppendFormat(
                        info.PropertyType.IsSubclassOf(typeof(Model)) ? "{0}.{1}_ID" : "{0}.{1}", TableAttribute.GetName(obj.GetType()), orders[0]);
                }
                else
                {

                    for (int i = 0; i < orders.Count; i++)
                    {
                        TBaseObject obj = (TBaseObject)Activator.CreateInstance(typeof(TBaseObject));
                        PropertyInfo info = obj.GetType().GetProperty(orders[i]);
                        if (info.PropertyType.IsSubclassOf(typeof(Model)))
                        {
                            grupuFealdName.AppendFormat((i + 1) != orders.Count ? "{0}.{1}_ID," : "{0}.{1}_ID",
                                TableAttribute.GetName(obj.GetType()), orders[i]);
                        }
                        else
                        {
                            grupuFealdName.AppendFormat((i + 1) != orders.Count ? "{0}.{1}," : "{0}.{1}", TableAttribute.GetName(obj.GetType()), orders[i]);
                        }

                    }
                }
            }

            StringBuilder builder = new StringBuilder();
            ModelCollection<TBaseObject> lst = new ModelCollection<TBaseObject>();
            builder.Append(GetSelectQuery(typeof(TBaseObject)));
            string criteriasql = criteria.PopulateSQLCriteria(typeof(TBaseObject), this);
            if (!string.IsNullOrEmpty(criteriasql))
                builder.Append($" AND {criteriasql}");
            builder.AppendFormat(" ORDER BY {0} {1}", grupuFealdName, orders.IsDesc ? "DESC" : "ASC");
            string query = builder.ToString();
            query = query.Replace("WHERE ()", "");
            using (SqlConnection newconnection = new SqlConnection(DbConnectionString))
            {
                if (newconnection.State != ConnectionState.Open)
                    newconnection.Open();

                using (SqlCommand CmdS = new SqlCommand(query, newconnection))
                {
                    using (SqlDataReader reader = CmdS.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            int i = 0;
                            TBaseObject returned = new TBaseObject();
                            returned.Populatereader(reader, i);
                            lst.Add(returned);
                        }



                    }
                }
            }
            if (lst.Count > 0)
                InsertLog(DbActionType.Select, 0, 0, TableAttribute.GetName(typeof(TBaseObject)), typeof(TBaseObject), query, DateTime.Now, new List<DbParameter>());
            return lst;
        }

        public override string GetMaxValue(Type baseObjectType, string coloumName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"SELECT MAX(\"{coloumName}\") from \"{Shema}\".\"{TableAttribute.GetName(baseObjectType)}\"  WHERE {TableAttribute.GetName(baseObjectType)}.IsDeleted=0");
            DataTable returnedTable = ExecuteDataTable(builder.ToString());
            return returnedTable != null ? $"{returnedTable.Rows[0][0]}" : string.Empty;
        }

        public override string GetMaxValue(Type baseObjectType, string coloumName, Criteria criteria)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(
                $"SELECT MAX(\"{coloumName}\") from \"{TableAttribute.GetName(baseObjectType)}\" WHERE {TableAttribute.GetName(baseObjectType)}.IsDeleted=0");
            string criteriasql = criteria.PopulateSQLCriteria(baseObjectType, this);
            if (!string.IsNullOrEmpty(criteriasql))
                builder.Append($"AND {criteriasql}");
            DataTable returnedTable = ExecuteDataTable(builder.ToString());
            if (returnedTable != null)
                return $"{returnedTable.Rows[0][0]}";
            else
                return string.Empty;
        }

        public override string GetMinValue(Type baseObjectType, string coloumName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"SELECT MIN({coloumName}) from {TableAttribute.GetName(baseObjectType)} WHERE {TableAttribute.GetName(baseObjectType)}.IsDeleted=0 ");
            DataTable returnedTable = ExecuteDataTable(builder.ToString());
            return returnedTable != null ? $"{returnedTable.Rows[0][0]}" : string.Empty;
        }

        public override string GetMinValue(Type baseObjectType, string coloumName, Criteria criteria)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(
                $"SELECT MIN(\"{coloumName}\") from \"{TableAttribute.GetName(baseObjectType)}\" WHERE {TableAttribute.GetName(baseObjectType)}.IsDeleted=0");
            string criteriasql = criteria.PopulateSQLCriteria(baseObjectType, this);
            if (!string.IsNullOrEmpty(criteriasql))
                builder.Append($"AND {criteriasql}");
            DataTable returnedTable = ExecuteDataTable(builder.ToString());
            if (returnedTable != null)
                return $"{returnedTable.Rows[0][0]}";
            else
                return string.Empty;
        }
        public override decimal GetSumValue(Type baseObjectType, string coloumName)
        {
            decimal deger = 0;
            var builder = new StringBuilder();
            builder.Append(string.Format("SELECT SUM({0}) from {1} WHERE {1}.IsDeleted=0", coloumName, TableAttribute.GetName(baseObjectType)));
            using (SqlConnection newconnection = new SqlConnection(DbConnectionString))
            {
                if (newconnection.State != ConnectionState.Open)
                    newconnection.Open();
                using (SqlCommand cmdS = new SqlCommand(builder.ToString(), newconnection))
                {
                    using (SqlDataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
                    {


                        while (reader.Read())
                        {
                            deger = reader.GetDecimal(0);

                        }
                    }
                }
            }
            return deger;
        }
        public override decimal GetSumValue(Type baseObjectType, string coloumName, Criteria criteria)
        {
            decimal deger = 0;
            var builder = new StringBuilder();
            builder.Append(String.Format("SELECT ISNULL(SUM({0}),0) from {1} WHERE {1}.IsDeleted=0", coloumName, TableAttribute.GetName(baseObjectType)));
            string criteriasql = criteria.PopulateSQLCriteria(baseObjectType, this);
            if (!string.IsNullOrEmpty(criteriasql))
                builder.Append(string.Format("    AND {0}", criteriasql));
            using (SqlConnection newconnection = new SqlConnection(DbConnectionString))
            {
                if (newconnection.State != ConnectionState.Open)
                    newconnection.Open();
                using (SqlCommand cmdS = new SqlCommand(builder.ToString(), newconnection))
                {
                    using (SqlDataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
                    {


                        while (reader.Read())
                        {
                            deger = reader.GetDecimal(0);

                        }
                    }
                }
            }
            return deger;
        }
        public override T LoadRaportBaseObje<T>(string sql)
        {
            T obje = null;
            using (SqlConnection newconnection = new SqlConnection(DbConnectionString))
            {
              
                if (newconnection.State != ConnectionState.Open)
                    newconnection.Open();
                using (SqlCommand cmdS = new SqlCommand(sql, newconnection))
                {
                    using (SqlDataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
                    {


                        while (reader.Read())
                        {
                            obje = new T();
                            obje.PopulateReader(reader);

                        }
                    }
                }
            }
            InsertLog(DbActionType.Select, 0, 0, typeof(T).Name, typeof(T), sql, DateTime.Now, new List<DbParameter>());
            return obje;

        }

        public override List<T> LoadRaportBaseObjects<T>(string sql)
        {
            List<T> lst = new List<T>();
            using (SqlConnection newconnection = new SqlConnection(DbConnectionString))
            {

                if (newconnection.State != ConnectionState.Open)
                    newconnection.Open();
                using (SqlCommand cmdS = new SqlCommand(sql, newconnection))
                {
                    using (SqlDataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
                    {


                        while (reader.Read())
                        {
                            T _object = new T();
                            _object.PopulateReader(reader);
                            lst.Add(_object);
                        }
                    }
                }
            }
            InsertLog(DbActionType.Select, 0, 0, typeof(T).Name, typeof(T), sql, DateTime.Now, new List<DbParameter>());
            return lst;
        }
        public override List<T> LoadRaportBaseObjects<T>(string sql, List<QueryParameters> dbParameters)
        {



            List<T> lst = new List<T>();
            List<DbParameter> dd = new List<DbParameter>();
            using (SqlConnection newconnection = new SqlConnection(DbConnectionString))
            {

                if (newconnection.State != ConnectionState.Open)
                    newconnection.Open();
                using (SqlCommand cmdS = new SqlCommand(sql, newconnection))
                {
                    foreach (var item in dbParameters)
                    {
                        SqlParameter param = new SqlParameter();
                        param.ParameterName = item.ParametrName;
                        param.Value = item.Value;
                        param.DbType = item.DbType;
                        dd.Add(param);
                        cmdS.Parameters.Add(param);
                    }
                    using (SqlDataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
                    {


                        while (reader.Read())
                        {
                            T _object = new T();
                            _object.PopulateReader(reader);
                            lst.Add(_object);
                        }
                    }
                }
            }
            InsertLog(DbActionType.Select, 0, 0, typeof(T).Name, typeof(T), sql, DateTime.Now, dd);
            return lst;
        }
        public override string GetSelectQuery(Type modeltype)
        {
            List<string> tables = new List<string>();
            PropertyInfo[] infos = modeltype.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Array.Sort(infos, new DeclarationOrderComparator());
            StringBuilder builder = new StringBuilder();
            StringBuilder buiderJoins = new StringBuilder();
            string baseTableName = TableAttribute.GetName(modeltype);
            builder.AppendFormat("SELECT {0}.OID,", baseTableName);
            builder.AppendFormat("{0}.CreateDate,", baseTableName);
            builder.AppendFormat("{0}.Rowversion,", baseTableName);
            builder.AppendFormat("{0}.UpdateDate,", baseTableName);
            builder.AppendFormat("{0}.CreateUserId,", baseTableName);
            builder.AppendFormat("{0}.UpdateUserId,", baseTableName);
            builder.AppendFormat("{0}.DeleteUserId,", baseTableName);
            builder.AppendFormat("{0}.DeleteTime,", baseTableName);
            builder.AppendFormat("{0}.IsDeleted,", baseTableName);

            tables.Add(baseTableName);
            int i = 0;
            Array.ForEach(infos, info =>
            {
                int k = i;
                if ((ColumnAttribute.GetColumnAttribute(info) != null || AssociationAttribute.GetName(info) != null) & !string.IsNullOrEmpty(TypeName(info)))
                    if (info.PropertyType.IsSubclassOf(typeof(Model)))
                    {
                        string tempTableName = TableAttribute.GetName(info.PropertyType);

                        if (tables.Contains(tempTableName))
                        {
                            builder.AppendFormat(PopulatequeryString(info.PropertyType, $"{tempTableName}_{i}", k));
                            buiderJoins.AppendFormat(" LEFT OUTER JOIN {0} AS {3} WITH(NOLOCK)  ON {1}.{2}={3}.OID AND {3}.IsDeleted=0", tempTableName, baseTableName,
                                $"{info.Name}_ID", $"{tempTableName}_{i}");
                        }
                        else
                        {
                            builder.AppendFormat(PopulatequeryString(info.PropertyType, tempTableName, k));
                            buiderJoins.AppendFormat(" LEFT OUTER JOIN {0} WITH(NOLOCK) ON {1}.{2}={0}.OID AND {0}.IsDeleted=0", tempTableName, baseTableName,
                                $"{info.Name}_ID");
                        }
                        tables.Add(tempTableName);
                    }
                    else
                        builder.AppendFormat("{0}.{1},", baseTableName, info.Name);
                i = i + 1;
            });
            builder.Replace(",", "", builder.Length - 1, 1);
            builder.AppendFormat(" FROM {0} WITH(NOLOCK) ", baseTableName);
            if (buiderJoins.Length != 0)
                buiderJoins.Replace(",", "", buiderJoins.Length - 1, 1);
            builder.Append(buiderJoins);
            builder.AppendFormat(" WHERE {0}.IsDeleted=0", baseTableName);
            return builder.ToString();
        }

        public override string PopulatequeryString(Type modelType, string tableName, int i)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0}.OID AS {0}_{1},", tableName, i++);
            builder.AppendFormat("{0}.CreateDate  AS {0}_{1},", tableName, i++);
            builder.AppendFormat("{0}.Rowversion  AS {0}_{1},", tableName, i++);
            builder.AppendFormat("{0}.UpdateDate  AS {0}_{1},", tableName, i++);
            builder.AppendFormat("{0}.CreateUserId  AS {0}_{1},", tableName, i++);
            builder.AppendFormat("{0}.UpdateUserId  AS {0}_{1},", tableName, i++);
            builder.AppendFormat("{0}.DeleteUserId  AS {0}_{1},", tableName, i++);
            builder.AppendFormat("{0}.DeleteTime  AS {0}_{1},", tableName, i++);
            builder.AppendFormat("{0}.IsDeleted  AS {0}_{1},", tableName, i++);
            PropertyInfo[] infos = modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            Array.Sort(infos, new DeclarationOrderComparator()); Array.ForEach(infos, info =>
            {

                if ((ColumnAttribute.GetColumnAttribute(info) != null || AssociationAttribute.GetName(info) != null) & !string.IsNullOrEmpty(TypeName(info)))
                    builder.AppendFormat("{0}.{1} AS {0}_{2} ,", tableName, ColumnAttribute.GetName(info), i++);

            });
            return builder.ToString();
        }

        #endregion
        #region CreateTable

        public override void CreateSchema(Type objectType)
        {
            if (objectType == null)
                throw new UygulamaHatasiException("Oluşturlamak istenen Nesne NULL olamaz");
            if (objectType.IsSubclassOf(typeof(Model)))
            {
                AddTable(TableAttribute.GetName(objectType), objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance), TableAttribute.GetDescription(objectType));
                DelletAllIndex(TableAttribute.GetName(objectType), Shema);
                bool isUnique = false;
                Hashtable table = IndexedAttribute.GetIndexedAttributes(objectType);
                foreach (int keys in table.Keys)
                {
                    List<IndexedAttribute> atributes = table[keys] as List<IndexedAttribute>;
                    List<string> listesi = new List<string>();
                    if (atributes != null)
                        foreach (IndexedAttribute indexedAtri in atributes)
                        {
                            listesi.Add(!indexedAtri.Property.PropertyType.IsSubclassOf(typeof(Model))
                                ? indexedAtri.Property.Name
                                : string.Format("{0}_ID", indexedAtri.Property.Name));
                            isUnique = indexedAtri.IsUnique;
                        }
                    if (isUnique)
                    {
                        listesi.Add("IsDeleted");
                        listesi.Add("DeleteTime");
                    }
                    AddIndex(TableAttribute.GetName(objectType), listesi, isUnique);
                }
            }
            else if (objectType.IsEnum)
            {
                AddEnumTable(string.Format("ENUM_{0}", objectType.Name.ToUpper(new CultureInfo("en-EN"))), objectType, objectType.GetFields(BindingFlags.Static | BindingFlags.Public));
            }
            else
            {
                throw new Exception(string.Format("Type: '{0}' is not BaseObject class", objectType));
            }
        }

        public override void AddTable(string tableName, PropertyInfo[] propertyInfos, string description)
        {
            try
            {


                DataTable table = null;

                if (IsTableAvailable(tableName, Shema))
                {
                    table = GetMetaData(tableName);

                }
                else
                {
                    CreateTable(tableName, description);

                }
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    try
                    {




                        if (ColumnAttribute.GetColumnAttribute(propertyInfo) != null || AssociationAttribute.GetAssociationAttribute(propertyInfo) != null)
                        {
                            string columnName = ColumnAttribute.GetColumnAttribute(propertyInfo) != null ? ColumnAttribute.GetName(propertyInfo) : AssociationAttribute.GetName(propertyInfo);
                            if (table != null && IsFieldAvaible(table, columnName))
                            {

                                if (IsFieldUpdateRequired(table, propertyInfo))
                                    UpdateField(tableName, Shema, ColumnAttribute.GetName(propertyInfo), TypeName(propertyInfo));
                                else
                                {
                                    if (propertyInfo.PropertyType.IsSubclassOf(typeof(Model)))
                                    {

                                        AddForeignKey(tableName, propertyInfo);

                                    }
                                }

                            }
                            else if (TypeName(propertyInfo) != null)
                            {
                                AddField(tableName, columnName, TypeName(propertyInfo), ColumnAttribute.GetIsCanBeNull(propertyInfo), "");
                                AddForeignKey(tableName, propertyInfo);
                            }
                        }

                    }
                    catch (Exception)
                    {


                    }
                }
            }
            catch (Exception )
            {


            }
        }

        public override bool IsTableAvailable(string tablename, string schemaname)
        {
            string sql = $"select count(*) from dbo.sysobjects (nolock)  where id = object_id(N'[{tablename}]') and OBJECTPROPERTY(id, N'IsUserTable') = 1 ";
            return (int)ExecuteScalar(sql) > 0;
        }

        public override DataTable GetMetaData(string tablename)
        {
            string sql = $"SELECT COLUMN_NAME as [name], DATA_TYPE as [type], ISNULL(CHARACTER_MAXIMUM_LENGTH, 0) as [size] from INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tablename}'";
            var table = ExecuteDataTable(sql);
            return table;
        }

        public override void CreateTable(string tablename, string description)
        {
            string sql = $"CREATE TABLE {Shema}.{tablename} (OID INT NOT NULL IDENTITY (1, 1), CreateDate DATETIME,Rowversion INT,UpdateDate DATETIME,CreateUserId INT,UpdateUserId INT,DeleteUserId INT NOT NULL DEFAULT 0,DeleteTime DATETIME NOT NULL DEFAULT ('1901-01-01 00:00:00'),IsDeleted BIT NOT NULL DEFAULT 0)";
            ExecuteScalar(sql);
            if (!string.IsNullOrEmpty(description))
            {
                sql =
                    $@"EXEC sys.sp_addextendedproperty N'MS_Description'
                               ,'{description}'
                               ,'SCHEMA'
                               ,N'dbo'
                               ,'TABLE'
                               ,N'{tablename}'";
                ExecuteScalar(sql);
            }

            sql = $"ALTER TABLE {Shema}.{tablename} ADD PRIMARY KEY (\"OID\")";
            ExecuteScalar(sql);
        }

        public override void UpdateField(string tableName, string schemaname, string fieldName, string type)
        {

            string sql = $@"ALTER TABLE ""{schemaname}"".""{tableName}"" ALTER COLUMN ""{fieldName}"" TYPE {type};";
            ExecuteScalar(sql);
        }

        public override void AddForeignKey(string tableName, PropertyInfo propertyInfo)
        {
            int forenkeyint = 0;
            if (propertyInfo.PropertyType.IsSubclassOf(typeof(Model)))
            {


                string columnName = ColumnAttribute.GetColumnAttribute(propertyInfo) != null ? ColumnAttribute.GetName(propertyInfo) : AssociationAttribute.GetName(propertyInfo);
                string ss = $"SELECT A.TABLE_NAME, A.CONSTRAINT_NAME, B.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS A, INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE B WHERE CONSTRAINT_TYPE <> 'PRIMARY KEY' AND A.CONSTRAINT_NAME = B.CONSTRAINT_NAME AND A.TABLE_SCHEMA='{"dbo"}' AND A.TABLE_NAME='{tableName}' AND  B.COLUMN_NAME='{columnName}' ORDER BY A.TABLE_NAME ";
                DataTable reader = ExecuteDataTable(ss);
                for (int i = 0; i < reader.Rows.Count; i++)
                {
                    forenkeyint++;
                }
                if (forenkeyint == 0)
                {

                    string fkeyname = $"fk_{GetSequencerNumber("F_KEY")}";
                    AddForeignKey(fkeyname, Shema, TableAttribute.GetName(propertyInfo.PropertyType), "OID", tableName, columnName);
                }
            }
        }

        public override void AddForeignKey(string fkeyName, string schemaName, string tableName, string masterFieldName, string detailTableName,
            string detailFieldName)
        {
            var sql = string.Format(detailTableName == tableName ? @"ALTER TABLE ""{0}"".""{1}"" ADD CONSTRAINT ""{2}"" FOREIGN KEY (""{3}"") REFERENCES ""{0}"".""{4}""(""{5}"")" : @"ALTER TABLE ""{0}"".""{1}"" ADD CONSTRAINT ""{2}"" FOREIGN KEY (""{3}"") REFERENCES ""{0}"".""{4}""(""{5}"") ", schemaName, detailTableName, fkeyName, detailFieldName, tableName, masterFieldName);

            ExecuteScalar(sql);
        }

        public override string GetSequencerNumber(string keyname)
        {
            string returnedValu = string.Empty;
            string sql = $@"DECLARE @a INT      set @a=(SELECT  MAX(VALUE +1) FROM {SequencerTablename} WHERE NAME='{keyname}');  UPDATE {SequencerTablename} SET  VALUE = @a WHERE NAME='{keyname}'; select @a;";
            using (var newconnection = new SqlConnection(DbConnectionString))
            {
                if (newconnection.State != ConnectionState.Open) newconnection.Open();
                using (var command = new SqlCommand(sql, newconnection))
                {
                    using (var datareader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (datareader.Read())
                        {
                            var data = datareader.GetValue(0);
                            returnedValu = data.ToString();
                        }
                    }

                }
            }
            return returnedValu;
        }

        public override void InsertField(string tableName, string schemaname, string fieldName, string type, bool canBeNull, string afterField)
        {
            string sql = $@"ALTER TABLE ""{schemaname}"".""{tableName}"" ADD  ""{fieldName}"" {type};";
            ExecuteScalar(sql);
            if (!canBeNull)
            {
                sql = $@"ALTER TABLE ""{schemaname}"".""{tableName}"" ALTER COLUMN ""{fieldName}"" {type}  NOT NULL;";
                ExecuteScalar(sql);
            }
        }
        public override void InsertLog(DbActionType dbActionType, int RowVersion, int objectOid, string objectTableName, Type ObjectType, string query, DateTime time, List<DbParameter> parametrs)
        {
            List<DTO_DB_Parameter> pamsliste = new List<DTO_DB_Parameter>();

            foreach (var item in parametrs)
            {
                DTO_DB_Parameter pams = new DTO_DB_Parameter();
                pams.ParameterName = item.ParameterName;
                pams.Value = item.Value;
                pams.DbType = item.DbType.ToString();
                pamsliste.Add(pams);
            }

            LogClient.PushQueryLog(new BasePublishParametre
            {
                ClientIPAddress = TaxPayerDescendant.IpAdres,
                ClientMacAddress = TaxPayerDescendant.MacAdress,
                ClientMachineName = TaxPayerDescendant.MacAdress,
                CreateDate = time,
                CreateUserId = TaxPayerDescendant.UserId,
                ObjectTableName = objectTableName,
                UserId = TaxPayerDescendant.UserId,
                DbActionType = (int)dbActionType,
                Query = query,
                ObjectOID = objectOid,

                DbDtoParameter = pamsliste,
                DeleteTime = time,
                DeleteUserId = TaxPayerDescendant.UserId,

                FormCaption = TaxPayerDescendant.FormCapion,
                FormFullName = TaxPayerDescendant.FormName,
                IsDeleted = false,
                ObjectRowVersion = RowVersion,
                ObjectTypeFullName = ObjectType.FullName,
                Rowversion = RowVersion,
                Time = time,
                UpdateDate = time,
                UpdateUserId = TaxPayerDescendant.UserId
            });
        }
        //public override void DelletAllIndex(string tablename, string schemaName)
        //{
        //    StringBuilder bulder = new StringBuilder();
        //    string tempIndexName = string.Empty;
        //    bulder.Append("select s.name as SHEMANAME, t.name as TABLENAME, i.name As INDEXNAME, c.name as COLUMSNAME ");
        //    bulder.Append("from sys.tables t inner join sys.schemas s on t.schema_id = s.schema_id inner join sys.indexes i on i.object_id = t.object_id ");
        //    bulder.Append("inner join sys.index_columns ic on ic.object_id = t.object_id inner join sys.columns c on c.object_id = t.object_id and ");
        //    bulder.Append("ic.column_id = c.column_id where i.index_id > 0  and i.type in (1, 2) and i.is_primary_key = 0 and i.is_unique_constraint = 0 ");
        //    bulder.Append("and i.is_disabled = 0 and i.is_hypothetical = 0 and ic.key_ordinal > 0 ");
        //    bulder.AppendFormat(" and t.name='{0}' and s.name='{1}' and  c.name<>'OID'", tablename, schemaName);
        //    var indextable = ExecuteDataTable(bulder.ToString());
        //    for (int i = 0; i < indextable.Rows.Count; i++)
        //    {
        //        string indexColumnName = $"{indextable.Rows[i][$"{"INDEXNAME"}"]}";
        //        if (!tempIndexName.Equals(indexColumnName))
        //        {
        //            ExecuteScalar($"DROP INDEX \"{indexColumnName}\" ON \"{"dbo"}\".\"{tablename}\"");
        //            tempIndexName = indexColumnName;
        //        }
        //    }

        //}
        public override void DelletAllIndex(string tablename, string schemaName)
        {
            string query = string.Format(@"SELECT
  i.name as INDEXNAME
FROM sys.indexes i
INNER JOIN sys.tables t
  ON t.object_id = i.object_id
WHERE t.name = '{0}'
AND i.is_primary_key = 0
AND i.type = 2", tablename);
            var indextable = ExecuteDataTable(query);
            for (int i = 0; i < indextable.Rows.Count; i++)
            {
                string indexColumnName = $"{indextable.Rows[i][$"{"INDEXNAME"}"]}";
                ExecuteScalar($"DROP INDEX \"{indexColumnName}\" ON \"{"dbo"}\".\"{tablename}\"");
                //if (!tempIndexName.Equals(indexColumnName))
                //{
                //    ExecuteScalar($"DROP INDEX \"{indexColumnName}\" ON \"{"dbo"}\".\"{tablename}\"");
                //    tempIndexName = indexColumnName;
                //}
            }

            //base.DelletAllIndex(tablename, schemaName);
        }
        public override void AddIndex(string tableName, List<string> grupIndexList, bool isUnique)
        {

            StringBuilder builder = new StringBuilder();
            StringBuilder builderString = new StringBuilder();
            for (int i = 0; i < grupIndexList.Count; i++)
            {
                builder.AppendFormat("\"{0}\"", grupIndexList[i]);
                if (i != (grupIndexList.Count - 1))
                    builder.Append(",");
            }
            string indexName = $"INDX_{GetSequencerNumber("INDX")}";
            if (isUnique)
            {
                builderString.AppendFormat("CREATE UNIQUE NONCLUSTERED INDEX [{0}] ON [{2}]  ({3}) WITH ( PAD_INDEX = OFF,", indexName, "dbo", tableName, builder);
                builderString.Append("IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = OFF, ALLOW_PAGE_LOCKS = OFF)");

            }
            else
            {
                builderString.AppendFormat("CREATE  NONCLUSTERED INDEX [{0}] ON [{2}] ({3}) WITH ( PAD_INDEX = OFF,", indexName, "dbo", tableName, builder);
                builderString.Append("IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = OFF, ALLOW_PAGE_LOCKS = OFF)");

            }
            ExecuteScalar(builderString.ToString());
        }

        public override void AddField(string tableName, string fieldName, string type, bool canBeNull, string afterField)
        {
            InsertField(tableName, Shema, fieldName, type, canBeNull, afterField);
        }

        public override void AddEnumTable(string tableName, Type objectType, FieldInfo[] fieldInfos)
        {
            string varmi = "";
            Array.ForEach(fieldInfos, fInfo =>
            {
                string enumName = fInfo.Name;
                varmi = DisplayEnumNameAttribute.GetName(fInfo);


            });
            if (!string.IsNullOrEmpty(varmi))
            {
                if (!IsTableAvailable(tableName, "dbo"))
                {
                    string sql = String.Format("CREATE TABLE \"{0}\".\"{1}\" (\"NAME\" {2} NOT NULL, \"VALUE\" {3} NOT NULL,\"DISPLAYNAME\" {2})", "dbo", tableName, GetTypeName(typeof(string), 100), GetTypeName(typeof(int), 100));

                    ExecuteScalar(sql);
                    Array.ForEach(fieldInfos, fInfo =>
                    {
                        string enumName = fInfo.Name;
                        string enumCaption = DisplayEnumNameAttribute.GetName(fInfo);
                        int enumValue = Convert.ToInt32(Enum.Parse(objectType, enumName));
                        sql = string.Format($"INSERT INTO {Shema}.{tableName} VALUES('{{0}}',{{1}},'{{2}}')", enumName, enumValue, enumCaption);
                        ExecuteScalar(sql);
                    });
                }
                else
                {

                    string ifIsAvaibleField = $"Delete  from {Shema}.{tableName}";
                    ExecuteScalar(ifIsAvaibleField);
                    Array.ForEach(fieldInfos, fInfo =>
                    {
                        string enumName1 = fInfo.Name;
                        string enumCaption1 = DisplayEnumNameAttribute.GetName(fInfo);
                        int enumValue1 = Convert.ToInt32(Enum.Parse(objectType, enumName1));
                        string sql1 = string.Format($"INSERT INTO {Shema}.{tableName} VALUES('{{0}}',{{1}},'{{2}}')", enumName1, enumValue1, enumCaption1);
                        ExecuteScalar(sql1);
                    });


                }
            }
        }


        #endregion

    }
}
