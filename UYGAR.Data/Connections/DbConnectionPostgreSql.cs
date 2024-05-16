using Devart.Data.PostgreSql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UYGAR.Data.Attributes;
using UYGAR.Data.Base;
using UYGAR.Data.Connection;
using UYGAR.Data.Criterias;
using UYGAR.Exceptions;

namespace UYGAR.Data.Connections
{

    [Serializable]
    public class DbConnectionPostgreSql : DbConnectionBase
    {


        public override string DbConnectionString => ConfigurationManager.AppSettings["PostgreConn"];
        public override string DbName => ConfigurationManager.AppSettings["PostgreDbName"];
        public override string Shema => ConfigurationManager.AppSettings["PgShema"];

        public override void InsertObject(Model _object)
        {
            try
            {



                var sqlStr = new StringBuilder();
                var valuesStr = new StringBuilder();
                var parametrseListesi = new List<DbParameter>();
                var objectType = _object.GetType();
                int Oid = Convert.ToInt32(GetSequencerNumber($"sq_{TableAttribute.GetName(objectType).ToLower(new CultureInfo("en-US", false))}"));
                sqlStr.Append(String.Format("INSERT INTO {0}.{1} (", Shema, TableAttribute.GetName(objectType).ToLower(new CultureInfo("en-US", false))));
                valuesStr.Append(" Values(");
                sqlStr.Append(string.Format("{0} ,", "OID").ToLower(new CultureInfo("en-US", false)));
                valuesStr.Append(string.Format("@{0},", "OID").ToLower(new CultureInfo("en-US", false)));
                var paramoid = new PgSqlParameter { DbType = DbType.Int32, Value = Oid, ParameterName = string.Format("@{0}", "OID").ToLower(new CultureInfo("en-US", false)) };
                parametrseListesi.Add(paramoid);

                sqlStr.Append(string.Format("{0} ,", "CreateDate").ToLower(new CultureInfo("en-US", false)));
                valuesStr.Append(string.Format("@{0},", "CreateDate").ToLower(new CultureInfo("en-US", false)));
                var paramCreatateDateTime = new PgSqlParameter { DbType = DbType.DateTime, Value = DateTime.Now, ParameterName = string.Format("@{0}", "CreateDate").ToLower(new CultureInfo("en-US", false)) };
                parametrseListesi.Add(paramCreatateDateTime);
                sqlStr.Append(string.Format("{0},", "CreateUserId").ToLower(new CultureInfo("en-US", false)));
                valuesStr.Append(string.Format("@{0},", "CreateUserId").ToLower(new CultureInfo("en-US", false)));
                var paramCreatateUserId = new PgSqlParameter { DbType = DbType.Int32, Value = TaxPayerDescendant.UserId, ParameterName = string.Format("@{0}", "CreateUserId").ToLower(new CultureInfo("en-US", false)) };
                parametrseListesi.Add(paramCreatateUserId);
                sqlStr.Append(string.Format("{0},", "UpdateUserId").ToLower(new CultureInfo("en-US", false)));
                valuesStr.Append(string.Format("@{0},", "UpdateUserId").ToLower(new CultureInfo("en-US", false)));
                var paramUpdateUserId = new PgSqlParameter { DbType = DbType.Int32, Value = TaxPayerDescendant.UserId, ParameterName = string.Format("@{0}", "UpdateUserId").ToLower(new CultureInfo("en-US", false)) };
                parametrseListesi.Add(paramUpdateUserId);

                sqlStr.Append(string.Format("{0},", "Rowversion").ToLower(new CultureInfo("en-US", false)));
                valuesStr.Append(string.Format("@{0},", "Rowversion").ToLower(new CultureInfo("en-US", false)));
                var paramRowversion = new PgSqlParameter { DbType = DbType.Int32, Value = 1, ParameterName = string.Format("@{0}", "Rowversion").ToLower(new CultureInfo("en-US", false)) };
                parametrseListesi.Add(paramRowversion);
                sqlStr.Append(string.Format("{0} ", "UpdateDate").ToLower(new CultureInfo("en-US", false)));
                valuesStr.Append(string.Format("@{0}", "UpdateDate").ToLower(new CultureInfo("en-US", false)));
                var paramUpdateDateTime = new PgSqlParameter { DbType = DbType.DateTime, Value = DateTime.Now, ParameterName = string.Format("@{0}", "UpdateDate").ToLower(new CultureInfo("en-US", false)) };
                parametrseListesi.Add(paramUpdateDateTime);




                int count = 0;
                var propertyInfos = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                Array.ForEach(propertyInfos, propertyInfo =>
                {
                    if ((ColumnAttribute.GetColumnAttribute(propertyInfo) != null || AssociationAttribute.GetName(propertyInfo) != null) & !string.IsNullOrEmpty(TypeName(propertyInfo)))
                    {
                        sqlStr.Append(",");
                        valuesStr.Append(",");
                        if (propertyInfo.PropertyType.IsSubclassOf(typeof(Model)))
                        {
                            sqlStr.Append(string.Format("{0}_ID", propertyInfo.Name.ToLower(new CultureInfo("en-US", false))));
                            string paramnae = string.Format("@{0}", string.Format("{0}_ID", propertyInfo.Name.ToLower(new CultureInfo("en-US", false))));
                            valuesStr.Append(paramnae);
                            var param = new PgSqlParameter { ParameterName = paramnae, DbType = DbType.Int32 };
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
                            sqlStr.Append(string.Format("{0}", propertyInfo.Name.ToLower(new CultureInfo("en-US", false))));
                            valuesStr.AppendFormat("@{0}", propertyInfo.Name.ToLower(new CultureInfo("en-US", false)));
                            var param = new PgSqlParameter { ParameterName = string.Format("@{0}", propertyInfo.Name.ToLower(new CultureInfo("en-US", false))), Value = GetPropertyInfoValue(propertyInfo, _object), DbType = GetValueType(propertyInfo) };
                            parametrseListesi.Add(param);
                        }
                        count++;
                    }
                });

                if (count > 0)
                {
                    sqlStr.Append(") ");
                    valuesStr.Append(");");
                    sqlStr.Append(valuesStr);
                    ExecuteScalarInsert(sqlStr.ToString(), parametrseListesi);
                    //if (Db_ActionAttribute.IsInsertRequired(objectType))
                    //    DbAction.Write(DbActionType.Insert, TaxPayerDescendant.UserId, Oid, TableAttribute.GetName(objectType), sqlStr.ToString(), string.Empty, DateTime.Now, TaxPayerDescendant.IpAdres, TaxPayerDescendant.MacAdress, parametrseListesi, this);
                    _object.OID = Oid;
                    _object.Rowversion = 1;
                    _object.OID = Oid;
                    _object.Rowversion = 1;
                    _object.CreateUserId = TaxPayerDescendant.UserId;
                    _object.UpdateUserId = TaxPayerDescendant.UserId;
                    _object.CreateDate = DateTime.Now;
                    _object.UpdateDate = DateTime.Now;


                }

            }


            catch (Exception ex)
            {

                if (ex.Message.StartsWith("Duplicate entry"))
                {
                    throw new UygulamaHatasiException("Bu Kayıt İle Aynı Özellikleri Taşıyan Kayıt Zaten Sistemde Mevcut!!");
                }
                else
                {
                    throw ex;
                }
            }
        }
        public void InsertObjectWithID(Model _object)
        {
            try
            {


                int Oid = _object.OID;
                var sqlStr = new StringBuilder();
                var valuesStr = new StringBuilder();
                var parametrseListesi = new List<DbParameter>();
                var objectType = _object.GetType();
                sqlStr.Append(String.Format("INSERT INTO {0} (", TableAttribute.GetName(objectType)));
                valuesStr.Append(" Values(");
                sqlStr.Append(string.Format("{0} ,", "OID"));
                valuesStr.Append(string.Format("@{0},", "OID"));
                var paramOid = new PgSqlParameter { DbType = DbType.Int32, Value = _object.OID, ParameterName = string.Format("@{0}", "OID") };
                parametrseListesi.Add(paramOid);
                sqlStr.Append(string.Format("{0} ,", "CreateDate"));
                valuesStr.Append(string.Format("@{0},", "CreateDate"));
                var paramCreatateDateTime = new PgSqlParameter { DbType = DbType.DateTime, Value = _object.CreateDate, ParameterName = string.Format("@{0}", "CreateDate") };
                parametrseListesi.Add(paramCreatateDateTime);
                sqlStr.Append(string.Format("{0},", "CreateUserId"));
                valuesStr.Append(string.Format("@{0},", "CreateUserId"));
                var paramCreatateUserId = new PgSqlParameter { DbType = DbType.Int32, Value = _object.CreateUserId, ParameterName = string.Format("@{0}", "CreateUserId") };
                parametrseListesi.Add(paramCreatateUserId);
                sqlStr.Append(string.Format("{0},", "UpdateUserId"));
                valuesStr.Append(string.Format("@{0},", "UpdateUserId"));
                var paramUpdateUserId = new PgSqlParameter { DbType = DbType.Int32, Value = _object.UpdateUserId, ParameterName = string.Format("@{0}", "UpdateUserId") };
                parametrseListesi.Add(paramUpdateUserId);

                sqlStr.Append(string.Format("{0},", "Rowversion"));
                valuesStr.Append(string.Format("@{0},", "Rowversion"));
                var paramRowversion = new PgSqlParameter { DbType = DbType.Int32, Value = _object.Rowversion, ParameterName = string.Format("@{0}", "Rowversion") };
                parametrseListesi.Add(paramRowversion);
                sqlStr.Append(string.Format("{0} ", "UpdateDate"));
                valuesStr.Append(string.Format("@{0}", "UpdateDate"));
                var paramUpdateDateTime = new PgSqlParameter { DbType = DbType.DateTime, Value = _object.UpdateDate, ParameterName = string.Format("@{0}", "UpdateDate") };
                parametrseListesi.Add(paramUpdateDateTime);




                int count = 0;
                var propertyInfos = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                Array.ForEach(propertyInfos, propertyInfo =>
                {
                    if ((ColumnAttribute.GetColumnAttribute(propertyInfo) != null || AssociationAttribute.GetName(propertyInfo) != null) & !string.IsNullOrEmpty(TypeName(propertyInfo)))
                    {
                        sqlStr.Append(",");
                        valuesStr.Append(",");
                        if (propertyInfo.PropertyType.IsSubclassOf(typeof(Model)))
                        {
                            sqlStr.Append(string.Format("{0}_ID", propertyInfo.Name));
                            string paramnae = string.Format("@{0}", string.Format("{0}_ID", propertyInfo.Name));
                            valuesStr.Append(paramnae);
                            var param = new PgSqlParameter { ParameterName = paramnae, DbType = DbType.Int32 };
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
                            sqlStr.Append(string.Format("{0}", propertyInfo.Name));
                            valuesStr.AppendFormat("@{0}", propertyInfo.Name);
                            var param = new PgSqlParameter { ParameterName = string.Format("@{0}", propertyInfo.Name), Value = GetPropertyInfoValue(propertyInfo, _object), DbType = GetValueType(propertyInfo) };
                            parametrseListesi.Add(param);
                        }
                        count++;
                    }
                });

                if (count > 0)
                {
                    sqlStr.Append(") ");
                    valuesStr.Append(");");
                    sqlStr.Append(valuesStr);
                    ExecuteScalarInsert(sqlStr.ToString(), parametrseListesi);
                    //if (Db_ActionAttribute.IsInsertRequired(objectType))
                    //    DbAction.Write(DbActionType.Insert, TaxPayerDescendant.UserId, Oid, TableAttribute.GetName(objectType), sqlStr.ToString(), string.Empty, DateTime.Now, TaxPayerDescendant.IpAdres, TaxPayerDescendant.MacAdress, parametrseListesi, this);
                    //_object.OID = Oid;
                    //_object.Rowversion = 1;


                }

            }


            catch (Exception ex)
            {

                if (ex.Message.StartsWith("Duplicate entry"))
                {
                    throw new UygulamaHatasiException("Bu Kayıt İle Aynı Özellikleri Taşıyan Kayıt Zaten Sistemde Mevcut!!");
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
                var sqlStr = new StringBuilder();
                var parametrseListesi = new List<DbParameter>();
                var objectType = _object.GetType();
                sqlStr.Append(String.Format("UPDATE {0} Set ", TableAttribute.GetName(objectType)));
                sqlStr.Append(string.Format("{0}={1}", "UpdateUserId", string.Format("@{0},", "UpdateUserId")));
                var paramCreatateUserId = new PgSqlParameter { DbType = DbType.Int32, Value = TaxPayerDescendant.UserId, ParameterName = string.Format("@{0}", "UpdateUserId") };
                parametrseListesi.Add(paramCreatateUserId);
                _object.Rowversion = _object.Rowversion + 1;
                sqlStr.Append(string.Format("{0}={1}", "Rowversion", string.Format("@{0},", "Rowversion")));
                var paramRowversion = new PgSqlParameter { DbType = DbType.Int32, Value = _object.Rowversion, ParameterName = string.Format("@{0}", "Rowversion") };
                parametrseListesi.Add(paramRowversion);
                sqlStr.Append(string.Format("{0}={1}", "UpdateDate", string.Format("@{0}", "UpdateDate")));
                var paramUpdateDateTime = new PgSqlParameter { DbType = DbType.DateTime, Value = DateTime.Now, ParameterName = string.Format("@{0}", "UpdateDate") };
                parametrseListesi.Add(paramUpdateDateTime);
                var paramOid = new PgSqlParameter { DbType = DbType.Int32, Value = _object.OID, ParameterName = string.Format("@{0}", "OID") };
                parametrseListesi.Add(paramOid);
                var paramNewRowversion = new PgSqlParameter { DbType = DbType.Int32, Value = oldRowVersion, ParameterName = string.Format("@{0}", "NewRowversion") };
                var propertyInfos = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                parametrseListesi.Add(paramNewRowversion);
                Array.ForEach(propertyInfos, propertyInfo =>
                {
                    if ((ColumnAttribute.GetColumnAttribute(propertyInfo) != null) & !string.IsNullOrEmpty(TypeName(propertyInfo)))
                    {
                        sqlStr.Append(",");
                        if (propertyInfo.PropertyType.IsSubclassOf(typeof(Model)))
                        {
                            sqlStr.Append(string.Format("{0}_ID={1}", propertyInfo.Name, string.Format("@{0}", string.Format("{0}_ID", propertyInfo.Name))));
                            var param = new PgSqlParameter { ParameterName = string.Format("@{0}", string.Format("{0}_ID", propertyInfo.Name)), DbType = DbType.Int32 };
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
                            sqlStr.Append(string.Format("{0}={1}", propertyInfo.Name, string.Format("@{0}", propertyInfo.Name)));
                            var param = new PgSqlParameter { ParameterName = string.Format("@{0}", propertyInfo.Name), Value = GetPropertyInfoValue(propertyInfo, _object), DbType = GetValueType(propertyInfo) };
                            parametrseListesi.Add(param);
                        }

                    }
                });
                sqlStr.Append("  Where OID = @OID AND Rowversion=@NewRowversion");
                int isUpdatet = ExecuteScalar(sqlStr.ToString(), parametrseListesi);
                if (isUpdatet.Equals(0))
                    throw new DbKayitDegistirilmisException();
                //if (Db_ActionAttribute.IsUpdateRequired(objectType))
                //    DbAction.Write(DbActionType.Update, TaxPayerDescendant.UserId, _object.OID, TableAttribute.GetName(objectType), sqlStr.ToString(), db_old_query(_object), DateTime.Now, TaxPayerDescendant.IpAdres, TaxPayerDescendant.MacAdress, parametrseListesi, this);


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public override void DeleteObject(Model _object)
        {
            var sqlStr = new StringBuilder();
            Type objectType = _object.GetType();
            if (_object.OID.Equals(-1))
                throw new UygulamaHatasiException("VeriTabanında Kayıtlı Olamayan Bir Kaydı Silemezsiniz..");
            sqlStr.Append(String.Format("DELETE FROM {0} WHERE OID=@OID AND Rowversion=@NewRowversion", TableAttribute.GetName(objectType)));
            string oldQuery = db_old_query(_object);
            try
            {
                var parametrseListesi = new List<DbParameter>();
                var paramNewRowversion = new PgSqlParameter { DbType = DbType.Int32, Value = _object.Rowversion, ParameterName = string.Format("@{0}", "NewRowversion") };
                parametrseListesi.Add(paramNewRowversion);
                var paramOid = new PgSqlParameter { DbType = DbType.Int32, Value = _object.OID, ParameterName = string.Format("@{0}", "OID") };
                parametrseListesi.Add(paramOid);

                int isDelete = ExecuteScalar(sqlStr.ToString(), parametrseListesi);
                if (isDelete.Equals(0))
                    throw new DbKayitDegistirilmisException();
                //if (Db_ActionAttribute.IsDeleteRequired(_object.GetType()))
                //    DbAction.Write(DbActionType.Update, TaxPayerDescendant.UserId, _object.OID, TableAttribute.GetName(objectType), sqlStr.ToString(), db_old_query(_object), DateTime.Now, TaxPayerDescendant.IpAdres, TaxPayerDescendant.MacAdress, parametrseListesi, this);
            }

            catch (Exception ex)
            {
                if (ex.Message.StartsWith(string.Format("Cannot delete or update a parent row: a foreign key constraint fails")))
                {

                    throw new DBSilinemezException(ex);
                }

                else
                {
                    throw ex;

                }
            }
        }
        public override int ExecuteScalarInsert(string query, List<DbParameter> parameters)
        {
            try
            {

                int retval;
                var connection = new PgSqlConnection(DbConnectionString);
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (var cmdS = new PgSqlCommand())
                {
                    parameters.ForEach(item => cmdS.Parameters.Add(item));
                    cmdS.CommandText = query;
                    cmdS.Connection = connection;
                    retval = Convert.ToInt32(cmdS.ExecuteScalar());


                }
                connection.Close();
                connection.Dispose();


                return retval;







            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public override int ExecuteScalar(string query, List<DbParameter> parametrs)
        {
            try
            {
                int retval;

                var connection = new PgSqlConnection(DbConnectionString);
                if (connection.State != ConnectionState.Open)
                    connection.Open();


                using (var cmdS = new PgSqlCommand())
                {

                    parametrs.ForEach(item => cmdS.Parameters.Add(item));
                    cmdS.CommandText = query;
                    cmdS.Connection = connection;
                    retval = cmdS.ExecuteNonQuery();

                }
                connection.Close();
                connection.Dispose();
                return retval;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public override object ExecuteScalar(string query)
        {
            try
            {
                var connection = new PgSqlConnection(DbConnectionString);
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                object returneddata = null;
                using (var cmdS = new PgSqlCommand())
                {

                    cmdS.CommandText = query;
                    cmdS.Connection = connection;
                    returneddata = cmdS.ExecuteScalar();
                }
                connection.Close();
                connection.Dispose();
                return returneddata;




            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public override string ConvertFieldName(string newName)
        {
            newName = newName.ToLower(new CultureInfo("en-US", false));
            return base.ConvertFieldName(newName);
        }
        public override string db_old_query(Model _object)
        {
            StringBuilder SQLStr = new StringBuilder();
            StringBuilder ValuesStr = new StringBuilder();

            Type objectType = _object.GetType();

            SQLStr.Append(String.Format("INSERT INTO \"{0}\" (", TableAttribute.GetName(objectType)));
            ValuesStr.Append(" Values(");

            int OId = _object.OID;
            SQLStr.Append(string.Format("\"{0}\",\"{1}\"", "OID", "CreateDate"));

            ValuesStr.Append(string.Format("{0},to_timestamp('{1}','YYYY.MM.DD H24:MI:SS')",
                OId,
                DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss")));
            int Count = 0;
            foreach (PropertyInfo propertyInfo in objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {

                if ((ColumnAttribute.GetColumnAttribute(propertyInfo) != null || AssociationAttribute.GetName(propertyInfo) != null) & !string.IsNullOrEmpty(TypeName(propertyInfo)))
                {

                    SQLStr.Append(",");
                    ValuesStr.Append(",");


                    if (propertyInfo.PropertyType.BaseType == typeof(Model))
                        SQLStr.Append(string.Format("\"{0}_ID\"", propertyInfo.Name));
                    else
                        SQLStr.Append(string.Format("\"{0}\"", propertyInfo.Name));

                    if (propertyInfo.PropertyType == typeof(string))
                        if (propertyInfo.GetValue(_object, null) == null)
                            ValuesStr.Append("''");
                        else
                            ValuesStr.Append(string.Format("'{0}'", propertyInfo.GetValue(_object, null).ToString().Replace("'", "`")));
                    else if (propertyInfo.PropertyType == typeof(DateTime))
                        ValuesStr.Append(String.Format("to_timestamp('{0}','DD.MM.YYYY H24:MI:SS')", propertyInfo.GetValue(_object, null)));
                    else if (propertyInfo.PropertyType.BaseType == typeof(Model))
                    {
                        int ReferencedId = ((Model)propertyInfo.GetValue(_object, null)) == null ? 0 : ((Model)propertyInfo.GetValue(_object, null)).OID;
                        if (ReferencedId == -1)
                            ((Model)propertyInfo.GetValue(_object, null)).Save(this);
                        if (ReferencedId == 0)
                            ValuesStr.Append("null");
                        else
                            ValuesStr.Append(string.Format("{0}", ((Model)propertyInfo.GetValue(_object, null)).OID));
                    }
                    else if (propertyInfo.PropertyType == typeof(bool) || propertyInfo.PropertyType == typeof(Boolean))
                        ValuesStr.Append(string.Format("'{0}'", (bool)propertyInfo.GetValue(_object, null) == true ? 1 : 0));
                    else if (propertyInfo.PropertyType.IsEnum)
                        ValuesStr.Append(string.Format("{0}", Convert.ToInt32(propertyInfo.GetValue(_object, null))));
                    else
                        ValuesStr.Append(string.Format("{0}", propertyInfo.GetValue(_object, null)));
                    Count++;
                }
            }
            if (Count > 0)
            {
                SQLStr.Append(") ");
                ValuesStr.Append(")");
                SQLStr.Append(ValuesStr);
            }
            return SQLStr.ToString();
        }
        public override string ToDbProviderString(Type targetType, object value)
        {
            System.Diagnostics.Trace.Assert(value != null, "Value is cant be null !!!");


            if (targetType == typeof(bool))
            {
                if (Convert.ToBoolean(value))
                    return "TRUE";
                else
                    return "FALSE";
            }


            if (targetType == typeof(DateTime))
            {
                DateTime dateTime = Convert.ToDateTime(value);
                return string.Format("CONVERT('{0}',DATE)",
                     string.Format("{0}.{1}.{2} {3}:{4}:{5}", dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second));
            }
            if (targetType == typeof(string) || targetType == typeof(Guid))
                return string.Format("'{0}'", value);

            if (targetType.IsEnum)
            {
                if (!(value is Enum))
                    throw new InvalidOperationException(string.Format("Value must be Enum. Bu value type is: {0}", value.GetType().FullName));

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
                    return "TRUE";
                else
                    return "FALSE";
            }


            if (targetType == typeof(DateTime))
            {
                DateTime dateTime = Convert.ToDateTime(value);
                return string.Format("CONVERT('{0}',DATETIME)",
                     string.Format("{0}.{1}.{2} {3}:{4}:{5}", dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second));
            }
            if (targetType == typeof(string) || targetType == typeof(Guid))
                return string.Format("'{0}'", value);

            if (targetType.IsEnum)
            {
                if (!(value is Enum))
                    throw new InvalidOperationException(string.Format("Value must be Enum. Bu value type is: {0}", value.GetType().FullName));

                return (Convert.ToInt32(value)).ToString();
            }

            if (targetType == typeof(decimal))
                return (Convert.ToDecimal(value)).ToString(new CultureInfo("en-US"));
            return value.ToString();
        }
        public override T LoadObject<T>(Criteria criteria)
        {
            StringBuilder builder = new StringBuilder();

            var _object = new T();
            builder.Append(GetSelectQuery(typeof(T)));
            string criteriasql = criteria.PopulateSQLCriteria(typeof(T), this);
            if (!string.IsNullOrEmpty(criteriasql))
                builder.Append(string.Format(" WHERE {0}", criteriasql));

            var connection = new PgSqlConnection(DbConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            string query = builder.ToString();
            query = query.Replace("WHERE ()", ""); LastQuery = query;
            _object = null;
            using (PgSqlCommand CmdS = new PgSqlCommand(query, connection))
            {




                using (PgSqlDataReader reader = CmdS.ExecuteReader(CommandBehavior.CloseConnection))
                {


                    if (reader.Read())
                    {
                        int i = 0;
                        _object = new T();
                        _object.Populatereader(reader, i);



                    }


                }

            }
            connection.Close();
            connection.Dispose();
            return _object;
        }
        public override T LoadObject<T>(Criteria criteria, Order orders)
        {
            var grupuFealdName = new StringBuilder();
            if (orders.Count != 0)
            {
                if (orders.Count == 1)
                {
                    var obj = (T)Activator.CreateInstance(typeof(T));
                    PropertyInfo info = obj.GetType().GetProperty(orders[0]);
                    grupuFealdName.AppendFormat(
                        info.PropertyType.IsSubclassOf(typeof(Model)) ? "{1}.{0}_ID" : "{1}.{0}", orders[0], TableAttribute.GetName(typeof(T)));
                }
                else
                {

                    for (int i = 0; i < orders.Count; i++)
                    {
                        var obj = (T)Activator.CreateInstance(typeof(T));
                        PropertyInfo info = obj.GetType().GetProperty(orders[i]);
                        if (info.PropertyType.IsSubclassOf(typeof(Model)))
                        {
                            grupuFealdName.AppendFormat((i + 1) != orders.Count ? "{0}_ID," : "{0}_ID", orders[i]);
                        }
                        else
                        {
                            grupuFealdName.AppendFormat((i + 1) != orders.Count ? "{0}," : "{0}", orders[i]);
                        }

                    }
                }
            }
            StringBuilder builder = new StringBuilder();

            var _object = new T();
            builder.Append(GetSelectQuery(typeof(T)));

            string criteriasql = criteria.PopulateSQLCriteria(typeof(T), this);
            if (!string.IsNullOrEmpty(criteriasql))
                builder.Append(string.Format(" WHERE {0}", criteriasql));
            builder.AppendFormat(" ORDER BY {0} {1}", grupuFealdName, orders.IsDesc ? "DESC" : "ASC");
            var connection = new PgSqlConnection(DbConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            string query = builder.ToString();
            query = query.Replace("WHERE ()", "");
            LastQuery = query;
            _object = null;
            using (PgSqlCommand CmdS = new PgSqlCommand(query, connection))
            {




                using (PgSqlDataReader reader = CmdS.ExecuteReader(CommandBehavior.CloseConnection))
                {


                    if (reader.Read())
                    {
                        int i = 0;
                        _object = new T();
                        _object.Populatereader(reader, i);



                    }


                }
            }
            connection.Close();
            connection.Dispose();



            return _object;
        }
        public override ModelCollection<TBaseObject> LoadObjects<TBaseObject>(Criteria criteria)
        {

            try
            {
                var builder = new StringBuilder();
                var lst = new ModelCollection<TBaseObject>();
                var onj = new TBaseObject();
                builder.Append(GetSelectQuery(typeof(TBaseObject)));
                //builder.Append(onj.PopulateExecuteString());
                string criteriasql = criteria.PopulateSQLCriteria(typeof(TBaseObject), this);
                if (!string.IsNullOrEmpty(criteriasql))
                    builder.Append(string.Format(" WHERE {0}", criteriasql));
                var connection = new PgSqlConnection(DbConnectionString);
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                string query = builder.ToString();
                query = query.Replace("WHERE ()", "");
                LastQuery = query;
                using (var cmdS = new PgSqlCommand(query, connection))
                {
                    using (var reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            int i = 0;
                            var returned = new TBaseObject();
                            returned.Populatereader(reader, i);
                            lst.Add(returned);
                        }



                    }
                }
                connection.Close();
                connection.Dispose();

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
                var builder = new StringBuilder();
                var lst = new ModelCollection<Model>();
                builder.Append(GetSelectQuery(type));

                string criteriasql = criteria.PopulateSQLCriteria(type, this);
                if (!string.IsNullOrEmpty(criteriasql))
                    builder.Append(string.Format(" WHERE {0}", criteriasql));


                var connection = new PgSqlConnection(DbConnectionString);
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                string query = builder.ToString();
                query = query.Replace("WHERE ()", "");
                LastQuery = query;
                using (var cmdS = new PgSqlCommand(query, connection))
                {
                    using (var reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
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
                connection.Close();
                connection.Dispose();

                return lst;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public override DataTable ExecuteDataTable(string query)
        {
            try
            {
                var connection = new PgSqlConnection(DbConnectionString);
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                DataTable _table = new DataTable();
                using (var CmdS = new PgSqlCommand())
                {

                    CmdS.CommandText = query;
                    CmdS.Connection = connection;

                    using (PgSqlDataAdapter AdapterS = new PgSqlDataAdapter(CmdS))
                    {
                        AdapterS.Fill(_table);
                    }



                }
                connection.Close();
                connection.Dispose();
                return _table;



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
                    builder.Append($" WHERE {criteriasql}");
                var connection = new PgSqlConnection(DbConnectionString);
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                string query = builder.ToString();
                query = query.Replace("WHERE ()", "");
                using (PgSqlCommand cmdS = new PgSqlCommand(query, connection))
                {
                    using (PgSqlDataAdapter adapet = new PgSqlDataAdapter(cmdS))
                    {
                        DataTable table = new DataTable();
                        adapet.Fill(table);
                        set.Tables.Add(table);
                    }
                }
                connection.Close();
                connection.Dispose();
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
                var connection = new PgSqlConnection(DbConnectionString);
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (PgSqlCommand cmdS = new PgSqlCommand(query, connection))
                {
                    using (PgSqlDataAdapter adapet = new PgSqlDataAdapter(cmdS))
                    {
                        DataTable table = new DataTable();
                        adapet.Fill(table);
                        set.Tables.Add(table);
                    }
                }
                connection.Close();
                connection.Dispose();
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

                var connection = new PgSqlConnection(DbConnectionString);
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                DataTable table = new DataTable();
                using (PgSqlCommand cmdS = new PgSqlCommand(query, connection))
                {
                    using (PgSqlDataAdapter adapet = new PgSqlDataAdapter(cmdS))
                    {

                        adapet.Fill(table);

                    }
                }
                connection.Close();
                connection.Dispose();
                return table;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public override ModelCollection<TBaseObject> LoadObjects<TBaseObject>(Criteria criteria, Order orders)
        {
            var grupuFealdName = new StringBuilder();
            if (orders.Count != 0)
            {
                if (orders.Count == 1)
                {
                    var obj = (TBaseObject)Activator.CreateInstance(typeof(TBaseObject));
                    PropertyInfo info = obj.GetType().GetProperty(orders[0]);
                    grupuFealdName.AppendFormat(
                        info.PropertyType.IsSubclassOf(typeof(Model)) ? "{1}.{0}_ID" : "{1}.{0}", orders[0], TableAttribute.GetName(typeof(TBaseObject)));
                }
                else
                {

                    for (int i = 0; i < orders.Count; i++)
                    {
                        var obj = (TBaseObject)Activator.CreateInstance(typeof(TBaseObject));
                        PropertyInfo info = obj.GetType().GetProperty(orders[i]);
                        if (info.PropertyType.IsSubclassOf(typeof(Model)))
                        {
                            grupuFealdName.AppendFormat((i + 1) != orders.Count ? "{0}_ID," : "{0}_ID", orders[i]);
                        }
                        else
                        {
                            grupuFealdName.AppendFormat((i + 1) != orders.Count ? "{0}," : "{0}", orders[i]);
                        }

                    }
                }
            }

            var builder = new StringBuilder();
            var lst = new ModelCollection<TBaseObject>();
            var onj = new TBaseObject();
            builder.Append(GetSelectQuery(typeof(TBaseObject)));
            builder.Append(onj.PopulateExecuteString());
            string criteriasql = criteria.PopulateSQLCriteria(typeof(TBaseObject), this);
            if (!string.IsNullOrEmpty(criteriasql))
                builder.Append(string.Format(" WHERE {0}", criteriasql));
            builder.AppendFormat(" ORDER BY {0} {1}", grupuFealdName, orders.IsDesc ? "DESC" : "ASC");
            var connection = new PgSqlConnection(DbConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            string query = builder.ToString();
            query = query.Replace("WHERE ()", "");
            LastQuery = query;
            using (var cmdS = new PgSqlCommand(query, connection))
            {

                using (PgSqlDataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        int i = 0;
                        var returned = new TBaseObject();
                        returned.Populatereader(reader, i);
                        lst.Add(returned);
                    }



                }
            }
            connection.Close();
            connection.Dispose();

            return lst;
        }
        public override string GetMaxValue(Type baseObjectType, string coloumName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(String.Format("SELECT MAX({0}) from {1} ", coloumName, TableAttribute.GetName(baseObjectType)));
            DataTable returnedTable = ExecuteDataTable(builder.ToString());
            if (returnedTable != null)
                return string.Format("{0}", returnedTable.Rows[0][0]);
            else
                return string.Empty;
        }

        public override string GetMaxValue(Type baseObjectType, string coloumName, Criteria criteria)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(String.Format("SELECT MAX({0}) from {1}", coloumName, TableAttribute.GetName(baseObjectType)));
            string criteriasql = criteria.PopulateSQLCriteria(baseObjectType, this);
            if (!string.IsNullOrEmpty(criteriasql))
                builder.Append(string.Format("   WHERE {0}", criteriasql));
            DataTable returnedTable = ExecuteDataTable(builder.ToString());
            if (returnedTable != null)
                return string.Format("{0}", returnedTable.Rows[0][0]);
            else
                return string.Empty;
        }
        public override string GetMinValue(Type baseObjectType, string coloumName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(String.Format("SELECT MIN({0}) from {1} ", coloumName, TableAttribute.GetName(baseObjectType)));
            DataTable returnedTable = ExecuteDataTable(builder.ToString());
            if (returnedTable != null)
                return string.Format("{0}", returnedTable.Rows[0][0]);
            else
                return string.Empty;
        }
        public override string GetMinValue(Type baseObjectType, string coloumName, Criteria criteria)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(String.Format("SELECT MIN({0}) from {1}", coloumName, TableAttribute.GetName(baseObjectType)));
            string criteriasql = criteria.PopulateSQLCriteria(baseObjectType, this);
            if (!string.IsNullOrEmpty(criteriasql))
                builder.Append(string.Format("   WHERE {0}", criteriasql));
            DataTable returnedTable = ExecuteDataTable(builder.ToString());
            if (returnedTable != null)
                return string.Format("{0}", returnedTable.Rows[0][0]);
            else
                return string.Empty;
        }
        public override decimal GetSumValue(Type baseObjectType, string coloumName)
        {
            decimal deger = 0;
            var builder = new StringBuilder();
            builder.Append(string.Format("SELECT SUM({0}) from {1}", coloumName, TableAttribute.GetName(baseObjectType)));
            var connection = new PgSqlConnection(DbConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            using (PgSqlCommand cmdS = new PgSqlCommand(builder.ToString(), connection))
            {
                using (PgSqlDataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
                {


                    while (reader.Read())
                    {
                        deger = reader.GetDecimal(0);

                    }
                }
            }
            connection.Close();
            connection.Dispose();

            return deger;
        }
        public override decimal GetSumValue(Type baseObjectType, string coloumName, Criteria criteria)
        {
            decimal deger = 0;
            var builder = new StringBuilder();
            builder.Append(String.Format("SELECT ISNULL(SUM({0}),0) from {1}", coloumName, TableAttribute.GetName(baseObjectType)));
            string criteriasql = criteria.PopulateSQLCriteria(baseObjectType, this);
            if (!string.IsNullOrEmpty(criteriasql))
                builder.Append(string.Format("    WHERE {0}", criteriasql));
            var connection = new PgSqlConnection(DbConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            using (PgSqlCommand cmdS = new PgSqlCommand(builder.ToString(), connection))
            {
                using (PgSqlDataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
                {


                    while (reader.Read())
                    {
                        deger = reader.GetDecimal(0);

                    }
                }
            }
            connection.Close();
            connection.Dispose();
            return deger;
        }
        public override T LoadRaportBaseObje<T>(string sql)
        {
            T obje = null;

            var connection = new PgSqlConnection(DbConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            using (PgSqlCommand cmdS = new PgSqlCommand(sql, connection))
            {
                using (PgSqlDataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
                {


                    while (reader.Read())
                    {
                        obje = new T();
                        obje.PopulateReader(reader);

                    }
                }
            }
            connection.Close();
            connection.Dispose();
            return obje;

        }

        public override List<T> LoadRaportBaseObjects<T>(string sql)
        {
            List<T> lst = new List<T>();
            var connection = new PgSqlConnection(DbConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            using (PgSqlCommand cmdS = new PgSqlCommand(sql, connection))
            {
                using (PgSqlDataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
                {


                    while (reader.Read())
                    {
                        T _object = new T();
                        _object.PopulateReader(reader);
                        lst.Add(_object);
                    }
                }
            }
            connection.Close();
            connection.Dispose();
            return lst;
        }
        private DbType GetValueType(PropertyInfo info)
        {
            if (info.PropertyType == typeof(string))
                return DbType.String;
            if (info.PropertyType == typeof(byte[]))
                return DbType.Binary;
            if (info.PropertyType == typeof(DateTime))
                return DbType.DateTime;
            if (info.PropertyType == typeof(int) || info.PropertyType == typeof(Int16) || info.PropertyType == typeof(Int32))
                return DbType.Int32;
            if (info.PropertyType.IsEnum)
                return DbType.Int32;
            if (info.PropertyType == typeof(decimal) || info.PropertyType == typeof(Decimal))
                return DbType.Decimal;
            if (info.PropertyType == typeof(bool) || info.PropertyType == typeof(Boolean))
                return DbType.Boolean;
            return DbType.String;

        }
        public override string GetTypeName(Type type, int size)
        {
            if (type == typeof(long))
                return "bigint";

            if (type == typeof(byte) || type == typeof(Int16))
                return "integer";

            if (type == typeof(Int32))
                return "integer";

            if (type == typeof(string))
            {
                if (size != 0)
                {
                    if (size <= 250)
                        return string.Format("varchar({0})", size);
                    else
                        return "varchar(1000)";

                }
                else
                {
                    return "varchar(1000)";
                }
            }

            if (type == typeof(DateTime))
                return "TIMESTAMP";

            if (type == typeof(Nullable<DateTime>))
                return "TIMESTAMP";

            if (type == typeof(double))
                return "decimal(16, 8)";

            if (type == typeof(decimal))
                return "decimal(16, 8)";

            if (type == typeof(bool))
                return "bit";
            if (type == typeof(Guid))
                return "varchar(1000)";

            if (type.IsSubclassOf(typeof(Model)))
                return "integer";

            if (type.IsEnum)
                return "integer";
            if (type == typeof(byte[]))
                return "BYTEA";
            return null;
        }
        public override string GetSelectQuery(Type modeltype)
        {
            List<string> tables = new List<string>();
            PropertyInfo[] infos = modeltype.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Array.Sort(infos, new DeclarationOrderComparator());
            StringBuilder builder = new StringBuilder();
            StringBuilder buiderJoins = new StringBuilder();
            string baseTableName = TableAttribute.GetName(modeltype).ToLower(new CultureInfo("en-US", false));
            builder.AppendFormat("SELECT {0}.{1}.oid,", Shema, baseTableName);
            builder.AppendFormat("{0}{1}.createdate,", Shema, baseTableName);
            builder.AppendFormat("{0}{1}.rowversion,", Shema, baseTableName);
            builder.AppendFormat("{0}{1}.updatedate,", Shema, baseTableName);
            builder.AppendFormat("{0}{1}.createuserid,", Shema, baseTableName);
            builder.AppendFormat("{0}{1}.updateuserid,", Shema, baseTableName);
            tables.Add(baseTableName);
            int i = 0;
            Array.ForEach(infos, info =>
            {
                int k = i;
                if ((ColumnAttribute.GetColumnAttribute(info) != null || AssociationAttribute.GetName(info) != null) & !string.IsNullOrEmpty(TypeName(info)))
                    if (info.PropertyType.IsSubclassOf(typeof(Model)))
                    {
                        string tempTableName = TableAttribute.GetName(info.PropertyType).ToLower(new CultureInfo("en-US", false));

                        if (tables.Contains(tempTableName))
                        {
                            builder.AppendFormat(PopulatequeryString(info.PropertyType, $"{tempTableName}_{i}", k));
                            buiderJoins.AppendFormat(" LEFT OUTER JOIN {0} AS {3}   ON {1}.{2}={3}.OID", tempTableName, baseTableName,
                                $"{info.Name}_ID", $"{tempTableName}_{i}");
                        }
                        else
                        {
                            builder.AppendFormat(PopulatequeryString(info.PropertyType, tempTableName, k));
                            buiderJoins.AppendFormat(" LEFT OUTER JOIN {0}  ON {1}.{2}={0}.OID", tempTableName, baseTableName,
                                $"{info.Name}_ID");
                        }
                        tables.Add(tempTableName);
                    }
                    else
                        builder.AppendFormat("{0}.{1},", baseTableName, info.Name.ToLower(new CultureInfo("en-US", false)));
                i = i + 1;
            });
            builder.Replace(",", "", builder.Length - 1, 1);
            builder.AppendFormat(" FROM {0}.{1}  ", Shema, baseTableName);
            if (buiderJoins.Length != 0)
                buiderJoins.Replace(",", "", buiderJoins.Length - 1, 1);
            builder.Append(buiderJoins);
            return builder.ToString();
        }

        public override string PopulatequeryString(Type modelType, string tableName, int i)
        {
            tableName = tableName.ToLower(new CultureInfo("en-US", false));
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat($"{Shema}{tableName}.oid AS {tableName}_{i++},");
            builder.AppendFormat($"{Shema}{tableName}.createdate  AS {tableName}_{i++},");
            builder.AppendFormat($"{Shema}{tableName}.rowversion  AS {tableName}_{i++},");
            builder.AppendFormat($"{Shema}{tableName}.UpdateDate  AS {tableName}_{i++},");
            builder.AppendFormat($"{Shema}{tableName}.createuserid  AS {tableName} _ {i++},");
            builder.AppendFormat($"{Shema}{tableName}.updateuserid  AS {tableName} _ {i++},");
            PropertyInfo[] infos = modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            Array.Sort(infos, new DeclarationOrderComparator()); Array.ForEach(infos, info =>
            {

                if ((ColumnAttribute.GetColumnAttribute(info) != null || AssociationAttribute.GetName(info) != null) & !string.IsNullOrEmpty(TypeName(info)))
                    builder.AppendFormat("{4}.{0}.{1} AS {0}_{2} ,", tableName, ColumnAttribute.GetName(info).ToLower(new CultureInfo("en-US", false)), i++,Shema);


            });
            return builder.ToString();
        }

        public override void CreateSchema(Type objectType)
        {
            if (objectType == null)
                throw new UygulamaHatasiException("Oluşturlamak istenen Nesne NULL olamaz");
            if (objectType.IsSubclassOf(typeof(Model)))
            {
                string temtableName = TableAttribute.GetName(objectType).ToLower(new CultureInfo("en-US", false));
                DeleteallForeignKey(temtableName);
                DelletAllIndex(temtableName, DbName);

                var propertyinfos = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                AddTable(temtableName, propertyinfos, "");

                foreach (PropertyInfo info in propertyinfos)
                {
                    if (info.PropertyType.IsSubclassOf(typeof(Model)))
                        AddForeignKey(temtableName, info);
                }
                bool _IsUnique = false;
                Hashtable _Table = IndexedAttribute.GetIndexedAttributes(objectType);
                foreach (int _Keys in _Table.Keys)
                {
                    List<IndexedAttribute> _atributes = _Table[_Keys] as List<IndexedAttribute>;
                    List<string> _Listesi = new List<string>();
                    foreach (IndexedAttribute _IndexedAtri in _atributes)
                    {
                        if (!_IndexedAtri.Property.PropertyType.IsSubclassOf(typeof(Model)))
                        {
                            if (_IndexedAtri.Property.PropertyType == typeof(String))
                            {
                                _Listesi.Add(string.Format("{0}", _IndexedAtri.Property.Name.ToLower(new CultureInfo("en-US", false))));
                            }
                            else
                                _Listesi.Add(_IndexedAtri.Property.Name.ToLower(new CultureInfo("en-US", false)));
                        }
                        else
                            _Listesi.Add(string.Format("{0}_ID", _IndexedAtri.Property.Name.ToLower(new CultureInfo("en-US", false))));
                        _IsUnique = _IndexedAtri.IsUnique;
                    }
                    AddIndex(TableAttribute.GetName(objectType).ToLower(new CultureInfo("en-US", false)), _Listesi, _IsUnique);
                }
            }
            else if (objectType.IsEnum)
            {
                AddEnumTable(string.Format("enum_{0}", objectType.Name.ToLower(new CultureInfo("en-US", false))), objectType, objectType.GetFields(BindingFlags.Static | BindingFlags.Public));
            }
            else
            {
                throw new Exception(string.Format("Type: '{0}' is not BaseObject class", objectType.GetType().ToString()));
            }
        }
        private void DeleteallForeignKey(string tableName)
        {
            string sql = $@"SELECT
    tc.table_schema, 
    tc.constraint_name, 
    tc.table_name, 
    kcu.column_name, 
    ccu.table_schema AS foreign_table_schema,
    ccu.table_name AS foreign_table_name,
    ccu.column_name AS foreign_column_name 
FROM information_schema.table_constraints AS tc 
JOIN information_schema.key_column_usage AS kcu
    ON tc.constraint_name = kcu.constraint_name
    AND tc.table_schema = kcu.table_schema
JOIN information_schema.constraint_column_usage AS ccu
    ON ccu.constraint_name = tc.constraint_name
WHERE tc.constraint_type = 'FOREIGN KEY'
    AND tc.table_schema='{Shema}'
    AND tc.table_name='{tableName}'";
            DataTable table = ExecuteDataTable(sql);

            for (int i = 0; i < table.Rows.Count; i++)
            {
                string dropsql = string.Format("ALTER TABLE {0} DROP CONSTRAINT  {1};", tableName, table.Rows[i][1]);
                ExecuteScalar(dropsql);
            }


        }
        public override void AddTable(string tableName, PropertyInfo[] propertyInfos, string description)
        {
            try
            {

                DataTable table = null;


                if (IsTableAvailable(tableName, DbName))
                {
                    table = GetMetaData(tableName);
                    if (!IsSequencerAvailable(tableName))
                        CreateSequencer(tableName);
                }
                else
                {
                    CreateSequencer(tableName);
                    CreateTable(tableName, DbName);


                }
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    try
                    {




                        if (ColumnAttribute.GetColumnAttribute(propertyInfo) != null || AssociationAttribute.GetAssociationAttribute(propertyInfo) != null)
                        {
                            string columnName = ColumnAttribute.GetColumnAttribute(propertyInfo) != null ? ColumnAttribute.GetName(propertyInfo).ToLower(new CultureInfo("en-US", false)) : AssociationAttribute.GetName(propertyInfo).ToLower(new CultureInfo("en-US", false));
                            if (table != null && IsFieldAvaible(table, columnName))
                            {
                                string fName = ColumnAttribute.GetName(propertyInfo); //this.GetFieldName(tableName, propertyInfo);

                                if (IsFieldUpdateRequired(table, propertyInfo))
                                    UpdateField(tableName, DbName, ColumnAttribute.GetName(propertyInfo), TypeName(propertyInfo));
                                else
                                {
                                    if (propertyInfo.PropertyType.IsSubclassOf(typeof(Model)))
                                    {

                                        //AddForeignKey(tableName, propertyInfo);

                                    }
                                }

                            }
                            else if (TypeName(propertyInfo) != null)
                            {

                                AddField(tableName, columnName.ToLower(new CultureInfo("en-US", false)), TypeName(propertyInfo), ColumnAttribute.GetIsCanBeNull(propertyInfo), GetAfterFieldName(tableName));
                                //AddForeignKey(tableName, propertyInfo);
                            }
                        }

                    }
                    catch (Exception )
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
            string sql = $@"select * from pg_tables where schemaname='{schemaname}' and tablename='{tablename}'";
            return ExecuteScalar(sql) == null ? false : true; ;
        }
        public bool IsSequencerAvailable(string tablename)
        {
            bool retuned = true;
            try
            {
                string sql = string.Format("SELECT relname FROM pg_class WHERE relkind = 'S' AND relnamespace IN (SELECT oid FROM pg_namespace WHERE nspname NOT LIKE 'pg_%' AND nspname != 'information_schema') AND relname='sq_{0}'", tablename);

                DataTable table = ExecuteDataTable(sql);
                if (table != null && table.Rows.Count > 0)
                    retuned = true;
                else
                    retuned = false;
            }
            catch (Exception)
            {
                retuned = false;
            }
            return retuned;
        }
        public void CreateSequencer(string tablename)
        {
            string sql = $"CREATE SEQUENCE {Shema}.sq_{tablename} INCREMENT 1  MINVALUE 1 MAXVALUE 9999999999999  START 1 CACHE 1;";
            ExecuteScalar(sql);
        }
        public override void AddField(string tableName, string fieldName, string type, bool canBeNull, string afterField)
        {
            InsertField(tableName, Shema, fieldName, type, canBeNull, GetAfterFieldName(tableName));
            //base.AddField(tableName, fieldName, type, canBeNull, afterField);
        }
        public override void CreateTable(string tablename, string description)
        {
            string sql = $"CREATE TABLE {Shema}.{tablename} (oid   int8 NOT NULL DEFAULT nextval('{Shema}.sq_{tablename}'::regclass),createdate TIMESTAMP DEFAULT now(),rowversion INTEGER NOT NULL,updatedate TIMESTAMP DEFAULT now() ,createuserId INTEGER,updateuserId INTEGER)";
            ExecuteScalar(sql);
            sql = $"ALTER TABLE {Shema}.{tablename} ADD PRIMARY KEY (oid)";

            ExecuteScalar(sql);
        }
        public override void UpdateField(string tableName, string schemaname, string fieldName, string type)
        {
            string sql = $@"ALTER TABLE {schemaname}.{tableName} ALTER COLUMN {fieldName} TYPE {type};";
            ExecuteScalar(sql);
        }
        public override DataTable GetMetaData(string tablename)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("name"));
            table.Columns.Add(new DataColumn("type"));
            table.Columns.Add(new DataColumn("size"));


            string sql = string.Format(@"SELECT * FROM {0} LIMIT 1", tablename);

            DataTable _table = ExecuteDataTable(sql);
            foreach (DataColumn colum in _table.Columns)
            {
                DataRow row = table.NewRow();
                row[0] = colum.ColumnName;
                row[1] = colum.DataType.Name;
                row[2] = colum.DefaultValue;
                table.Rows.Add(row);
            }

            return table;
        }
        private string GetAfterFieldName(string tableName)
        {
            string sql = string.Format("select COLUMN_NAME from information_schema.COLUMNS where TABLE_SCHEMA='{1}' AND TABLE_NAME='{0}' ORDER BY ORDINAL_POSITION DESC LIMIT 1", tableName, Shema);
            string returnvalue = string.Empty;
            var connection = new PgSqlConnection(DbConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            using (var command = new PgSqlCommand(sql, connection))
            {
                using (PgSqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (reader.Read())
                        returnvalue = reader.GetString(0);
                }
            }

            connection.Close();
            connection.Dispose();
            return returnvalue;

        }
        public override void InsertField(string tableName, string schemaname, string fieldName, string type, bool canBeNull, string afterField)
        {
            string sql = $@"ALTER TABLE {schemaname}.{tableName} ADD  {fieldName} {type};";
            ExecuteScalar(sql);
            if (!canBeNull)
            {
                sql = $@"ALTER TABLE {schemaname}.{tableName} ALTER COLUMN {fieldName} {type}  NOT NULL;";
                ExecuteScalar(sql);
            }
        }
        public override void AddForeignKey(string tableName, PropertyInfo propertyInfo)
        {
            int forenkeyint = 0;
            if (propertyInfo.PropertyType.IsSubclassOf(typeof(Model)))
            {
                string columnName = ColumnAttribute.GetColumnAttribute(propertyInfo) != null ? ColumnAttribute.GetName(propertyInfo) : AssociationAttribute.GetName(propertyInfo);
                StringBuilder _SS = new StringBuilder();
                _SS.Append("SELECT tc.constraint_name, tc.table_name, kcu.column_name, ccu.table_name AS foreign_table_name,ccu.column_name AS foreign_column_name FROM");
                _SS.Append("  information_schema.table_constraints AS tc JOIN information_schema.key_column_usage AS kcu ON tc.constraint_name = kcu.constraint_name JOIN information_schema.constraint_column_usage AS ccu ON ccu.constraint_name = tc.constraint_name");
                _SS.AppendFormat(" WHERE constraint_type = 'FOREIGN KEY' AND tc.table_name='{0}' AND kcu.column_name='{1}'", tableName, columnName);
                DataTable reader = ExecuteDataTable(_SS.ToString());
                for (int i = 0; i < reader.Rows.Count; i++)
                {
                    forenkeyint++;
                }
                if (forenkeyint == 0)
                {
                    string fkeyname = string.Format("fk_{0}", GetSequencerNumber("f_key"));

                    AddForeignKey(fkeyname, string.Format("{0}", Shema), TableAttribute.GetName(propertyInfo.PropertyType).ToLower(new CultureInfo("en-US", false)), "OID", tableName, columnName);
                }
            }
        }
        public override void AddForeignKey(string fkeyName, string schemaName, string tableName, string masterFieldName, string detailTableName, string detailFieldName)
        {
            string sql;

            sql = $"ALTER TABLE {schemaName}.{detailTableName} ADD CONSTRAINT {fkeyName} FOREIGN KEY ({detailFieldName}) REFERENCES {schemaName}.{tableName}({masterFieldName}) ";
            ExecuteScalar(sql);
        }
        public override string GetSequencerNumber(string keyname)
        {
            string returnedValu = string.Empty;
            string sql = $"select nextval('{Shema}.{keyname}'::regclass)";
            var connection = new PgSqlConnection(DbConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            using (var command = new PgSqlCommand(sql, connection))
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
            connection.Close();
            connection.Dispose();
            return returnedValu;
        }
        public override void DelletAllIndex(string tablename, string schemaName)
        {
            DataTable _INDEX_TABLE = new DataTable();
            StringBuilder _Bulder = new StringBuilder();
            string _SQl = string.Format("SELECT relname FROM pg_class WHERE oid IN (SELECT indexrelid FROM pg_index c LEFT JOIN pg_class t ON  c.indrelid = t.oid LEFT JOIN pg_attribute a ON a.attrelid = t.oid AND a.attnum = ANY (indkey) LEFT JOIN pg_catalog.pg_namespace n ON n.oid = pg_class.relnamespace  WHERE t.relname = '{0}' and a.attname!= 'oid' and n.nspname='{1}') GROUP BY relname ", tablename, schemaName);
            _INDEX_TABLE = ExecuteDataTable(_SQl.ToString());
            for (int i = 0; i < _INDEX_TABLE.Rows.Count; i++)
            {
                string _IndexColumn_Name = string.Format("{0}", _INDEX_TABLE.Rows[i][0]);
                ExecuteScalar(string.Format("DROP INDEX {0}.{1}", schemaName, _IndexColumn_Name));
            }
        }
        public override void AddIndex(string tableName, List<string> grupIndexList, bool isUnique)
        {
            try
            {
                StringBuilder _Builder = new StringBuilder();
                string sql;

                for (int i = 0; i < grupIndexList.Count; i++)
                {
                    _Builder.AppendFormat("{0}", grupIndexList[i]);
                    if (i != (grupIndexList.Count - 1))
                        _Builder.Append(",");
                }
                string IndexName = string.Format("indx_{0}", this.GetSequencerNumber("indx"));
                if (isUnique)
                    sql = string.Format("CREATE UNIQUE INDEX {0} ON {1}.{2} USING btree ( {3} ) ", IndexName, Shema, tableName, _Builder);
                else
                    sql = string.Format("CREATE  INDEX {0} ON {1}.{2} USING btree ( {3} )", IndexName, Shema, tableName, _Builder);

                ExecuteScalar(sql);
            }
            catch (Exception )
            {

            
            }
           
        }
        private void AddFKIndex(string tableName, List<string> grupIndexList)
        {
       
            StringBuilder _Builder = new StringBuilder();
            string sql;

            for (int i = 0; i < grupIndexList.Count; i++)
            {
                _Builder.AppendFormat("{0}", grupIndexList[i]);
                if (i != (grupIndexList.Count - 1))
                    _Builder.Append(",");
            }
            string IndexName = string.Format("fk_index{0}", this.GetSequencerNumber("indx"));
            sql = string.Format("CREATE  INDEX {0} ON {1}.{2} USING btree ( {3} )", IndexName, Shema, tableName, _Builder);
            ExecuteScalar(sql);
         

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
                ExecuteScalar(string.Format("DROP TABLE IF EXISTS {0}", tableName));
                string sql = String.Format("CREATE TABLE {0} (NAME          VARCHAR(100) NOT NULL,VALUE         INT NOT NULL,DISPLAYNAME   VARCHAR(100)) ENGINE = InnoDB ROW_FORMAT = DEFAULT CHARACTER SET latin5,COLLATE latin5_turkish_ci;", tableName);

                ExecuteScalar(sql);
                Array.ForEach(fieldInfos, fInfo =>
                {
                    string enumName = fInfo.Name;
                    string enumCaption = DisplayEnumNameAttribute.GetName(fInfo);
                    int enumValue = Convert.ToInt32(Enum.Parse(objectType, enumName));
                    sql = string.Format("INSERT INTO {0}  VALUES('{1}',{2},'{3}')", tableName, enumName, enumValue, enumCaption);
                    ExecuteScalar(sql);
                });
            }
        }


    }
}
