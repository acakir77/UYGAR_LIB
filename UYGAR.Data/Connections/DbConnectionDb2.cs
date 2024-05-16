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
using System.Text;
using UYGAR.Data.Attributes;
using UYGAR.Data.Base;
using UYGAR.Data.Connection;
using UYGAR.Data.Criterias;
using UYGAR.Exceptions;

namespace UYGAR.Data.Connections
{
    public class DbConnectionDb2 : DbConnectionBase
    {
//        public override string DbConnectionString => $"{ConfigurationManager.AppSettings["Db2Con"]}";
//        public override string Shema => $"{ConfigurationManager.AppSettings["Db2Schema"]}";
//        public override void InsertObject(Model _object)
//        {
//            try
//            {



//                StringBuilder sqlStr = new StringBuilder();
//                StringBuilder valuesStr = new StringBuilder();
//                List<DbParameter> parametrseListesi = new List<DbParameter>();
//                Type objectType = _object.GetType();
//                sqlStr.Append($"INSERT INTO {Shema}.{TableAttribute.GetName(objectType)} (");
//                valuesStr.Append(" VALUES(");
//                sqlStr.Append($"{"CreateDate"},");
//                valuesStr.Append($"@{"CreateDate"},");
//                DB2Parameter paramCreatateDateTime = new DB2Parameter
//                {
//                    DB2Type = DB2Type.Timestamp,
//                    Value = DateTime.Now,
//                    ParameterName = $"@{"CreateDate"}"
//                };
//                parametrseListesi.Add(paramCreatateDateTime);
//                sqlStr.Append($"{"CreateUserId"},");
//                valuesStr.Append($"@{"CreateUserId"},");
//                DB2Parameter paramCreatateUserId = new DB2Parameter
//                {
//                    DB2Type = DB2Type.Integer,
//                    Value = TaxPayerDescendant.UserId,
//                    ParameterName = $"@{"CreateUserId"}"
//                };
//                parametrseListesi.Add(paramCreatateUserId);
//                sqlStr.Append($"{"Rowversion"},");
//                valuesStr.Append($"@{"Rowversion"},");
//                DB2Parameter paramRowversion = new DB2Parameter
//                {
//                    DB2Type = DB2Type.Integer,
//                    Value = 1,
//                    ParameterName = $"@{"Rowversion"}"
//                };
//                parametrseListesi.Add(paramRowversion);
//                sqlStr.Append($"{"UpdateDate"},");
//                valuesStr.Append($"@{"UpdateDate"},");
//                DB2Parameter paramUpdateDateTime = new DB2Parameter
//                {
//                    DB2Type = DB2Type.Timestamp,
//                    Value = DateTime.Now,
//                    ParameterName = $"@{"UpdateDate"}"
//                };
//                parametrseListesi.Add(paramUpdateDateTime);
//                sqlStr.Append($"{"UpdateUserId"}");
//                valuesStr.Append($"@{"UpdateUserId"}");
//                DB2Parameter paramUpdateUserId = new DB2Parameter
//                {
//                    DB2Type = DB2Type.Integer,
//                    Value = TaxPayerDescendant.UserId,
//                    ParameterName = $"@{"UpdateUserId"}"
//                };
//                parametrseListesi.Add(paramUpdateUserId);
//                int count = 0;
//                PropertyInfo[] propertyInfos = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
//                Array.ForEach(propertyInfos, propertyInfo =>
//                {
//                    if ((ColumnAttribute.GetColumnAttribute(propertyInfo) != null ||
//                         AssociationAttribute.GetName(propertyInfo) != null) &
//                        !string.IsNullOrEmpty(TypeName(propertyInfo)))
//                    {
//                        sqlStr.Append(",");
//                        valuesStr.Append(",");
//                        if (propertyInfo.PropertyType.IsSubclassOf(typeof(Model)))
//                        {
//                            sqlStr.Append($"{propertyInfo.Name}_ID");
//                            string paramnae = $"@{$"{propertyInfo.Name}_ID"}";
//                            valuesStr.Append(paramnae);
//                            DB2Parameter param = new DB2Parameter { ParameterName = paramnae, DB2Type = DB2Type.Integer };
//                            if (propertyInfo.GetValue(_object, null) != null)
//                                param.Value = ((Model)propertyInfo.GetValue(_object, null)).OID;
//                            else
//                            {
//                                param.IsNullable = true;
//                                param.Value = DBNull.Value;
//                            }
//                            parametrseListesi.Add(param);
//                        }
//                        else
//                        {
//                            sqlStr.Append($"{propertyInfo.Name}");
//                            valuesStr.AppendFormat("@{0}", propertyInfo.Name);
//                            DB2Parameter param = new DB2Parameter
//                            {
//                                ParameterName = $"@{propertyInfo.Name}",
//                                Value = GetPropertyInfoValue(propertyInfo, _object),
//                                DB2Type = GetValueType(propertyInfo)
//                            };
//                            parametrseListesi.Add(param);
//                        }
//                        count++;
//                    }
//                });

//                if (count > 0)
//                {
//                    sqlStr.Append(") ");
//                    valuesStr.Append(");");
//                    sqlStr.Append(valuesStr);
//                    sqlStr.Append(" SELECT IDENTITY_VAL_LOCAL() FROM SYSIBM.SYSDUMMY1");
//                    int oId = ExecuteScalarInsert(sqlStr.ToString(), parametrseListesi);
//                    if (Db_ActionAttribute.IsInsertRequired(objectType))
//                        DbAction.Write(DbActionType.Insert, TaxPayerDescendant.UserId, oId,
//                            TableAttribute.GetName(objectType), sqlStr.ToString(), string.Empty, DateTime.Now,
//                              TaxPayerDescendant.IpAdres, TaxPayerDescendant.MacAdress, parametrseListesi, this);
//                    _object.OID = oId;
//                    _object.Rowversion = 1;


//                }


//            }


//            catch (Exception ex)
//            {

//                if (ex.Message.StartsWith("from having duplicate values for the index key"))
//                {
//                    throw new UygulamaHatasiException(
//                        "Bu Kayıt İle Aynı Özellikleri Taşıyan Kayıt Zaten Sistemde Mevcut!!");
//                }
//                else
//                {
//                    throw ex;
//                }
//            }
//        }
//        public override void UpdateObject(Model _object)
//        {
//            try
//            {
//                int oldRowVersion = _object.Rowversion;
//                StringBuilder sqlStr = new StringBuilder();
//                List<DbParameter> parametrseListesi = new List<DbParameter>();
//                Type objectType = _object.GetType();
//                sqlStr.Append($"UPDATE {Shema}.{TableAttribute.GetName(objectType)} Set ");
//                DB2Parameter paramCreatateDateTime = new DB2Parameter
//                {
//                    DB2Type = DB2Type.Timestamp,
//                    Value = DateTime.Now,
//                    ParameterName =
//                    $"@{"CreateDate"}"
//                };
//                parametrseListesi.Add(paramCreatateDateTime);
//                _object.Rowversion = _object.Rowversion + 1;
//                sqlStr.Append($"{"Rowversion"}={$"@{"Rowversion"},"}");
//                DB2Parameter paramRowversion = new DB2Parameter
//                {
//                    DB2Type = DB2Type.Integer,
//                    Value = _object.Rowversion,
//                    ParameterName =
//                    $"@{"Rowversion"}"
//                };
//                parametrseListesi.Add(paramRowversion);
//                sqlStr.Append($"{"UpdateDate"}={$"@{"UpdateDate"},"}");
//                DB2Parameter paramUpdateDateTime = new DB2Parameter
//                {
//                    DB2Type = DB2Type.Timestamp,
//                    Value = DateTime.Now,
//                    ParameterName =
//                    $"@{"UpdateDate"}"
//                };
//                parametrseListesi.Add(paramUpdateDateTime);
//                sqlStr.Append($"{"UpdateUserId"}={$"@{"UpdateUserId"}"}");
//                DB2Parameter paramUpdateUserId = new DB2Parameter
//                {
//                    DB2Type = DB2Type.Integer,
//                    Value = TaxPayerDescendant.UserId,
//                    ParameterName =
//                    $"@{"UpdateUserId"}"
//                };
//                parametrseListesi.Add(paramUpdateUserId);
//                PropertyInfo[] propertyInfos = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

//                Array.ForEach(propertyInfos, propertyInfo =>
//                {
//                    if ((ColumnAttribute.GetColumnAttribute(propertyInfo) != null || AssociationAttribute.GetName(propertyInfo) != null) & !string.IsNullOrEmpty(TypeName(propertyInfo)))
//                    {
//                        sqlStr.Append(",");
//                        if (propertyInfo.PropertyType.IsSubclassOf(typeof(Model)))
//                        {
//                            sqlStr.Append($"{propertyInfo.Name}_ID={$"@{$"{propertyInfo.Name}_ID"}"}");
//                            DB2Parameter param = new DB2Parameter
//                            {
//                                ParameterName =
//                                $"@{$"{propertyInfo.Name}_ID"}",
//                                DB2Type = DB2Type.Integer
//                            };
//                            if (propertyInfo.GetValue(_object, null) != null)
//                                param.Value = ((Model)propertyInfo.GetValue(_object, null)).OID;
//                            else
//                            {
//                                param.IsNullable = true;
//                                param.Value = DBNull.Value;
//                            }
//                            parametrseListesi.Add(param);
//                        }
//                        else
//                        {
//                            sqlStr.Append(string.Format("{0}={1}", propertyInfo.Name, $"@{propertyInfo.Name}"));
//                            DB2Parameter param = new DB2Parameter { ParameterName = $"@{propertyInfo.Name}", Value = GetPropertyInfoValue(propertyInfo, _object), DB2Type = GetValueType(propertyInfo) };
//                            parametrseListesi.Add(param);
//                        }

//                    }
//                });
//                sqlStr.Append($" Where OID = {_object.OID} AND Rowversion={oldRowVersion}");
//                int isUpdatet = ExecuteScalar(sqlStr.ToString(), parametrseListesi);
//                if (isUpdatet.Equals(0))
//                    throw new DbKayitDegistirilmisException();
//                if (Db_ActionAttribute.IsUpdateRequired(objectType))
//                    DbAction.Write(DbActionType.Update, TaxPayerDescendant.UserId, _object.OID, TableAttribute.GetName(objectType), sqlStr.ToString(), db_old_query(_object), DateTime.Now, TaxPayerDescendant.IpAdres, TaxPayerDescendant.MacAdress, parametrseListesi, this);


//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//        }
//        public override void DeleteObject(Model _object)
//        {
//            Type objectType = _object.GetType();
//            string qyery = $"DELETE FROM {Shema}.{TableAttribute.GetName(objectType)} where OID={_object.OID}";
//            string oldQuery = db_old_query(_object);
//            try
//            {
//                ExecuteScalar(qyery);
//                if (Db_ActionAttribute.IsDeleteRequired(_object.GetType()))
//                    DbAction.Write(DbActionType.Delete, TaxPayerDescendant.UserId, _object.OID, TableAttribute.GetName(_object.GetType()), qyery, oldQuery, DateTime.Now, TaxPayerDescendant.IpAdres, TaxPayerDescendant.MacAdress, null, this);
//            }

//            catch (Exception ex)
//            {
//                if (ex.Message.Contains("parent row cannot be deleted because the relationship"))
//                {

//                    throw new DBSilinemezException(ex);
//                }

//                else
//                {
//                    throw ex;

//                }
//            }
//        }
//        private DB2Type GetValueType(PropertyInfo info)
//        {
//            if (info.PropertyType == typeof(string))
//                return DB2Type.VarChar;
//            if (info.PropertyType == typeof(byte[]))
//                return DB2Type.Blob;
//            if (info.PropertyType == typeof(DateTime))
//                return DB2Type.Timestamp;
//            if (info.PropertyType == typeof(int) || info.PropertyType == typeof(Int16) || info.PropertyType == typeof(Int32))
//                return DB2Type.Integer;
//            if (info.PropertyType.IsEnum)
//                return DB2Type.Integer;
//            if (info.PropertyType == typeof(decimal) || info.PropertyType == typeof(Decimal))
//                return DB2Type.Decimal;
//            if (info.PropertyType == typeof(double) || info.PropertyType == typeof(Double))
//                return DB2Type.Float;
//            if (info.PropertyType == typeof(bool) || info.PropertyType == typeof(Boolean))
//                return DB2Type.Char;
//            return DB2Type.VarChar;

//        }

//        public override DataTable ExecuteDataTable(string query)
//        {
//            try
//            {
//                using (var newconnection = new DB2Connection(DbConnectionString))
//                {
//                    if (newconnection.State != ConnectionState.Open)
//                        newconnection.Open();

//                    using (var cmdS = new DB2Command(query, newconnection))
//                    {


//                        DataTable table = new DataTable();
//                        using (DB2DataAdapter adapterS = new DB2DataAdapter(cmdS))
//                        {
//                            adapterS.Fill(table);
//                        }
//                        newconnection.Close();
//                        newconnection.Dispose();
//                        return table;

//                    }

//                }

//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//        }

//        public override string GetTypeName(Type type, int size)
//        {
//             if (type == typeof(long))
//                return "BIGINT";

//            if (type == typeof(byte) || type == typeof(Int16))
//                return "SMALLINT";

//            if (type == typeof(Int32))
//                return "INTEGER";

//            if (type == typeof(string))
//                if (size <= 1000)
//                    return $"varchar({(size != 0 ? size : 1000)})";
//                else
//                    return "LONG VARCHAR";

//            if (type == typeof(DateTime))
//                return "TIMESTAMP(4)";

//            if (type == typeof(DateTime?))
//                return "TIMESTAMP(4)";

//            if (type == typeof(double))
//                return "FLOAT(53)";

//            if (type == typeof(decimal))
//                return "DECIMAL(16, 6)";

//            if (type == typeof(bool))
//                return "CHARACTER(1) FOR BIT DATA";

//            if (type == typeof(byte[]))
//                return "BLOB";

//            if (type == typeof(Guid))
//                return "BIGINT";
//            if (type.IsEnum)
//                return "INTEGER";
//            if (type.IsSubclassOf(typeof(Model)))
//                return "INTEGER";
//            return null;
//        }

//        public override int ExecuteScalarInsert(string query, List<DbParameter> parameters)
//        {
//            int retval = -1;
//            try
//            {

//                using (DB2Connection newconnection = new DB2Connection(DbConnectionString))
//                {
//                    if (newconnection.State != ConnectionState.Open)
//                        newconnection.Open();

//                    using (DB2Command cmdS = new DB2Command(query, newconnection))
//                    {
//                        parameters.ForEach(item => cmdS.Parameters.Add(item));
//                        retval = Convert.ToInt32(cmdS.ExecuteScalar());

//                    }
//                }


//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//            return retval;
//        }

//        public override int ExecuteScalar(string query, List<DbParameter> parametrs)
//        {

//            int retval = 0;
//            try
//            {

//                using (DB2Connection newconnection = new DB2Connection(DbConnectionString))
//                {
//                    if (newconnection.State != ConnectionState.Open)
//                        newconnection.Open();

//                    using (DB2Command cmdS = new DB2Command(query, newconnection))
//                    {
//                        parametrs.ForEach(item => cmdS.Parameters.Add(item));
//                        retval = cmdS.ExecuteNonQuery();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//            return retval;
//        }

//        public override object ExecuteScalar(string query)
//        {
//            int retval = 0;
//            try
//            {

//                using (DB2Connection newconnection = new DB2Connection(DbConnectionString))
//                {
//                    if (newconnection.State != ConnectionState.Open)
//                        newconnection.Open();

//                    using (DB2Command cmdS = new DB2Command(query, newconnection))
//                    {

//                        retval = cmdS.ExecuteNonQuery();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//            return retval;
//        }
//        public override string db_old_query(Model _object)
//        {
//            StringBuilder sqlStr = new StringBuilder();
//            StringBuilder valuesStr = new StringBuilder();

//            Type objectType = _object.GetType();


//            sqlStr.Append($"INSERT INTO \"{TableAttribute.GetName(objectType)}\" (");
//            valuesStr.Append(" Values(");

//            int oId = _object.OID;
//            sqlStr.Append($"\"{"OID"}\",\"{"CreateDate"}\"");

//            valuesStr.Append($"{oId},CAST('{DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss")}'AS TIMESTAMP)");
//            int count = 0;
//            foreach (PropertyInfo propertyInfo in objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(propertyInfo => (ColumnAttribute.GetColumnAttribute(propertyInfo) != null || AssociationAttribute.GetName(propertyInfo) != null) & !string.IsNullOrEmpty(TypeName(propertyInfo))))
//            {
//                sqlStr.Append(",");
//                valuesStr.Append(",");


//                sqlStr.Append(propertyInfo.PropertyType.BaseType == typeof (Model)
//                    ? $"\"{propertyInfo.Name}_ID\""
//                    : $"\"{propertyInfo.Name}\"");

//                if (propertyInfo.PropertyType == typeof(string))
//                    valuesStr.Append(propertyInfo.GetValue(_object, null) == null
//                        ? "''"
//                        : $"'{propertyInfo.GetValue(_object, null).ToString().Replace("'", "`")}'");
//                else if (propertyInfo.PropertyType == typeof(DateTime))
//                    valuesStr.Append($"CAST('{propertyInfo.GetValue(_object, null)}' AS TIMESTAMP)");
//                else if (propertyInfo.PropertyType.BaseType == typeof(Model))
//                {
//                    int referencedId = ((Model)propertyInfo.GetValue(_object, null)) == null ? 0 : ((Model)propertyInfo.GetValue(_object, null)).OID;
//                    if (referencedId == -1)
//                        ((Model)propertyInfo.GetValue(_object, null)).Save(this);
//                    valuesStr.Append(referencedId == 0
//                        ? "null"
//                        : $"{((Model)propertyInfo.GetValue(_object, null)).OID}");
//                }
//                else if (propertyInfo.PropertyType == typeof(bool) || propertyInfo.PropertyType == typeof(Boolean))
//                    valuesStr.Append($"'{((bool)propertyInfo.GetValue(_object, null) == true ? 1 : 0)}'");
//                else if (propertyInfo.PropertyType.IsEnum)
//                    valuesStr.Append($"{Convert.ToInt32(propertyInfo.GetValue(_object, null))}");
//                else
//                    valuesStr.Append($"{propertyInfo.GetValue(_object, null)}");
//                count++;
//            }
//            if (count > 0)
//            {
//                sqlStr.Append(") ");
//                valuesStr.Append(")");
//                sqlStr.Append(valuesStr);
//            }
//            return sqlStr.ToString();
//        }
//        public override string ToDbProviderString(Type targetType, object value)
//        {
//            System.Diagnostics.Trace.Assert(value != null, "Value is cant be null !!!");
//            if (targetType == typeof(bool))
//            {
//                if (Convert.ToBoolean(value))
//                    return "1"; //DbProviderBase.TRUE;
//                else
//                    return "0";// DbProviderBase.FALSE;
//            }
//            if (targetType == typeof(DateTime))
//            {
              
//                return $"CAST('{((DateTime) value).ToString("d")}' AS TIMESTAMP)";
//            }
//            if (targetType == typeof(string) || targetType == typeof(Guid))
//                return $"'{value}'";

//            if (targetType.IsEnum)
//            {
//                if (!(value is Enum))
//                    throw new InvalidOperationException(
//                        $"Value must be Enum. Bu value type is: {value.GetType().FullName}");

//                return (Convert.ToInt32(value)).ToString();
//            }

//            if (targetType == typeof(decimal))
//                return (Convert.ToDecimal(value)).ToString(new CultureInfo("en-US"));
//            return value.ToString();
//        }
//        #region LoadData

//        public override T LoadObject<T>(Criteria criteria)
//        {
//            StringBuilder builder = new StringBuilder();
//            builder.Append(GetSelectQuery(typeof(T)));
//            T _object = null;
//            string criteriasql = criteria.PopulateSQLCriteria(typeof(T), this);
//            builder.Append(!string.IsNullOrEmpty(criteriasql) ? $" WHERE {criteriasql} WITH ur"
//                : " WITH ur");
//            using (SqlConnection newconnection = new SqlConnection(DbConnectionString))
//            {
//                if (newconnection.State != ConnectionState.Open)
//                    newconnection.Open();
//                string query = builder.ToString();
//                query = query.Replace("WHERE ()", "");
//                using (SqlCommand cmdS = new SqlCommand(query, newconnection))
//                {
//                    using (SqlDataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
//                    {
//                        if (reader.Read())
//                        {
//                            var i = 0;
//                            _object = new T();
//                            _object.Populatereader(reader, i);



//                        }


//                    }
//                }
//            }
//            return _object;
//        }
//        public override ModelCollection<TBaseObject> LoadObjects<TBaseObject>(Criteria criteria)
//        {
//            StringBuilder builder = new StringBuilder();

//            ModelCollection<TBaseObject> liste = new ModelCollection<TBaseObject>();
//            builder.Append(GetSelectQuery(typeof(TBaseObject)));
//            string criteriasql = criteria.PopulateSQLCriteria(typeof(TBaseObject), this);
//            builder.Append(!string.IsNullOrEmpty(criteriasql)
//                ? $" WHERE {criteriasql} WITH ur"
//                : " WITH ur");
//            using (DB2Connection newconnection = new DB2Connection(DbConnectionString))
//            {
//                if (newconnection.State != ConnectionState.Open)
//                    newconnection.Open();
//                string query = builder.ToString();
//                query = query.Replace("WHERE ()", "");
//                using (DB2Command cmdS = new DB2Command(query, newconnection))
//                {



//                    using (DB2DataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
//                    {


//                        while (reader.Read())
//                        {
//                            int i = 0;
//                            TBaseObject _object = new TBaseObject();
//                            _object.Populatereader(reader, i);
//                            liste.Add(_object);


//                        }


//                    }
//                }
//            }
//            return liste;
//        }

//        public override ModelCollection<TBaseObject> LoadObjects<TBaseObject>(Criteria criteria, Order orders)
//        {
//            StringBuilder grupuFealdName = new StringBuilder();
         
//            if (orders.Count != 0)
//            {
//                if (orders.Count == 1)
//                {
//                    TBaseObject obj = (TBaseObject)Activator.CreateInstance(typeof(TBaseObject));
//                    PropertyInfo info = obj.GetType().GetProperty(orders[0]);
//                    grupuFealdName.AppendFormat(
//                        info.PropertyType.IsSubclassOf(typeof(Model)) ? "{0}_ID" : "{0}", orders[0]);
//                }
//                else
//                {

//                    for (int i = 0; i < orders.Count; i++)
//                    {
//                        TBaseObject obj = (TBaseObject)Activator.CreateInstance(typeof(TBaseObject));
//                        PropertyInfo info = obj.GetType().GetProperty(orders[i]);
//                        if (info.PropertyType.IsSubclassOf(typeof(Model)))
//                        {
//                            grupuFealdName.AppendFormat((i + 1) != orders.Count ? "{0}_ID," : "{0}_ID",
//                                orders[i]);
//                        }
//                        else
//                        {
//                            grupuFealdName.AppendFormat((i + 1) != orders.Count ? "{0}," : "{0}", orders[i]);
//                        }

//                    }
//                }
//            }

//            StringBuilder builder = new StringBuilder();
//            ModelCollection<TBaseObject> lst = new ModelCollection<TBaseObject>();
//            builder.Append(GetSelectQuery(typeof(TBaseObject)));
//            string criteriasql = criteria.PopulateSQLCriteria(typeof(TBaseObject), this);
//            if (!string.IsNullOrEmpty(criteriasql))
//                builder.Append($" WHERE {criteriasql}");
//            builder.AppendFormat(" ORDER BY {0} {1}", grupuFealdName, orders.IsDesc ? "DESC" : "ASC");
//            using (DB2Connection newconnection = new DB2Connection(DbConnectionString))
//            {
//                if (newconnection.State != ConnectionState.Open)
//                    newconnection.Open();
//                string query = builder.ToString();
//                query = query.Replace("WHERE ()", "");
//                using (DB2Command cmdS = new DB2Command(query, newconnection))
//                {
//                    using (DB2DataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
//                    {
//                        while (reader.Read())
//                        {
//                            int i = 0;
//                            TBaseObject returned = new TBaseObject();
//                            returned.Populatereader(reader, i);
//                            lst.Add(returned);
//                        }



//                    }
//                }
//            }

//            return lst;
//        }
//        public override string GetSelectQuery(Type modeltype)
//        {
//            List<string> tables = new List<string>();
//            PropertyInfo[] infos = modeltype.GetProperties(BindingFlags.Public | BindingFlags.Instance);
//            Array.Sort(infos, new DeclarationOrderComparator());
//            StringBuilder builder = new StringBuilder();
//            StringBuilder buiderJoins = new StringBuilder();
//            string baseTableName = TableAttribute.GetName(modeltype);
//            builder.AppendFormat("SELECT {0}.OID,", baseTableName);
//            builder.AppendFormat("{0}.CreateDate,", baseTableName);
//            builder.AppendFormat("{0}.Rowversion,", baseTableName);
//            builder.AppendFormat("{0}.UpdateDate,", baseTableName);
//            builder.AppendFormat("{0}.CreateUserId,", baseTableName);
//            builder.AppendFormat("{0}.UpdateUserId,", baseTableName);
//            tables.Add(baseTableName);
//            int i = 0;
//            Array.ForEach(infos, info =>
//            {
//                int k = i;
//                if ((ColumnAttribute.GetColumnAttribute(info) != null || AssociationAttribute.GetName(info) != null) & !string.IsNullOrEmpty(TypeName(info)))
//                    if (info.PropertyType.IsSubclassOf(typeof(Model)))
//                    {
//                        string tempTableName = TableAttribute.GetName(info.PropertyType);

//                        if (tables.Contains(tempTableName))
//                        {
//                            builder.AppendFormat(PopulatequeryString(info.PropertyType, $"{tempTableName}_{i}", k));
//                            buiderJoins.AppendFormat(" LEFT OUTER JOIN {4}.{0} AS {3} ON {1}.{2}={3}.OID", tempTableName, baseTableName,
//                                $"{info.Name}_ID", string.Format("{0}_{1}", tempTableName, i), Shema);
//                        }
//                        else
//                        {
//                            builder.AppendFormat(PopulatequeryString(info.PropertyType, tempTableName, k));
//                            buiderJoins.AppendFormat(" LEFT OUTER JOIN {3}.{0} ON {1}.{2}={0}.OID", tempTableName, baseTableName, string.Format("{0}_ID", info.Name), Shema);
//                        }
//                        tables.Add(tempTableName);
//                    }
//                    else
//                        builder.AppendFormat("{0}.{1},", baseTableName, info.Name);
//                i = i + 1;
//            });
//            builder.Replace(",", "", builder.Length - 1, 1);
//            builder.AppendFormat(" FROM {1}.{0}", baseTableName, Shema);
//            if (buiderJoins.Length != 0)
//                buiderJoins.Replace(",", "", buiderJoins.Length - 1, 1);
//            builder.Append(buiderJoins);
//            return builder.ToString();
//        }

//        public override string PopulatequeryString(Type modelType, string tableName, int i)
//        {
//            StringBuilder builder = new StringBuilder();
//            builder.AppendFormat("{0}.OID AS {0}_{1},", tableName, i++);
//            builder.AppendFormat("{0}.CreateDate  AS {0}_{1},", tableName, i++);
//            builder.AppendFormat("{0}.Rowversion  AS {0}_{1},", tableName, i++);
//            builder.AppendFormat("{0}.UpdateDate  AS {0}_{1},", tableName, i++);
//            builder.AppendFormat("{0}.CreateUserId  AS {0}_{1},", tableName, i++);
//            builder.AppendFormat("{0}.UpdateUserId  AS {0}_{1},", tableName, i++);
//            PropertyInfo[] infos = modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
//            Array.Sort(infos, new DeclarationOrderComparator());
//            Array.ForEach(infos, info =>
//            {

//                if ((ColumnAttribute.GetColumnAttribute(info) != null || AssociationAttribute.GetName(info) != null) & !string.IsNullOrEmpty(TypeName(info)))
//                    builder.AppendFormat("{0}.{1} AS {0}_{2} ,", tableName, ColumnAttribute.GetName(info), i++);

//            });
//            return builder.ToString();
//        }
//        public override List<T> LoadRaportBaseObjects<T>(string sql)
//        {
//            List<T> lst = new List<T>();
//            using (DB2Connection newconnection = new DB2Connection(DbConnectionString))
//            {
//                if (newconnection.State != ConnectionState.Open)
//                    newconnection.Open();
//                using (DB2Command cmdS = new DB2Command(sql, newconnection))
//                {
//                    using (DB2DataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
//                    {


//                        while (reader.Read())
//                        {
//                            T _object = new T();
//                            _object.PopulateReader(reader);
//                            lst.Add(_object);
//                        }
//                    }
//                }
//            }
//            return lst;
//        }

//        public override T LoadRaportBaseObje<T>(string sql)
//        {
//            T _object = null;
//            using (DB2Connection newconnection = new DB2Connection(DbConnectionString))
//            {
//                if (newconnection.State != ConnectionState.Open)
//                    newconnection.Open();
//                using (DB2Command cmdS = new DB2Command(sql, newconnection))
//                {
//                    using (DB2DataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
//                    {


//                        while (reader.Read())
//                        {
//                            _object = new T();
//                            _object.PopulateReader(reader);

//                        }
//                    }
//                }
//            }
//            return _object;
//        }
//        public override string GetMinValue(Type baseObjectType, string coloumName)
//        {
//            StringBuilder builder = new StringBuilder();
//            builder.Append($"SELECT MIN({coloumName}) from {Shema}.{TableAttribute.GetName(baseObjectType)} ");
//            DataTable returnedTable = ExecuteDataTable(builder.ToString());
//            if (returnedTable != null)
//                return $"{returnedTable.Rows[0][0]}";
//            else
//                return string.Empty;
//        }

//        public override string GetMinValue(Type baseObjectType, string coloumName, Criteria criteria)
//        {
//            StringBuilder builder = new StringBuilder();
//            builder.Append($"SELECT MIN({coloumName}) from {Shema}.{TableAttribute.GetName(baseObjectType)} ");
//            string criteriasql = criteria.PopulateSQLCriteria(baseObjectType, this);
//            if (!string.IsNullOrEmpty(criteriasql))
//                builder.Append($"WHERE {criteriasql}");
//            DataTable returnedTable = ExecuteDataTable(builder.ToString());
//            if (returnedTable != null)
//                return $"{returnedTable.Rows[0][0]}";
//            else
//                return string.Empty;
//        }
//        public override string GetMaxValue(Type baseObjectType, string coloumName)
//        {
//            StringBuilder builder = new StringBuilder();
//            builder.Append($"SELECT MAX(\"{coloumName}\") from \"{Shema}\".\"{TableAttribute.GetName(baseObjectType)}\" ");
//            DataTable returnedTable = ExecuteDataTable(builder.ToString());
//            return returnedTable != null ? $"{returnedTable.Rows[0][0]}" : string.Empty;
//        }

//        public override string GetMaxValue(Type baseObjectType, string coloumName, Criteria criteria)
//        {
//            StringBuilder builder = new StringBuilder();
//            builder.Append(
//                $"SELECT MAX(\"{coloumName}\") from \"{Shema}\".\"{TableAttribute.GetName(baseObjectType)}\" ");
//            string criteriasql = criteria.PopulateSQLCriteria(baseObjectType, this);
//            if (!string.IsNullOrEmpty(criteriasql))
//                builder.Append($"WHERE {criteriasql}");
//            DataTable returnedTable = ExecuteDataTable(builder.ToString());
//            if (returnedTable != null)
//                return $"{returnedTable.Rows[0][0]}";
//            else
//                return string.Empty;
//        }
//        #endregion
//        #region CreateTable

//        public override void CreateSchema(Type objectType)
//        {
//            if (objectType == null)
//                throw new UygulamaHatasiException("Oluşturlamak istenen Nesne NULL olamaz");
//            if (objectType.IsSubclassOf(typeof(Model)))
//            {
//                AddTable(TableAttribute.GetName(objectType), objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance), TableAttribute.GetDescription(objectType));
//                DelletAllIndex(TableAttribute.GetName(objectType), Shema);
//                bool isUnique = false;
//                Hashtable table = IndexedAttribute.GetIndexedAttributes(objectType);
//                foreach (int keys in table.Keys)
//                {
//                    List<IndexedAttribute> atributes = table[keys] as List<IndexedAttribute>;
//                    List<string> listesi = new List<string>();
//                    if (atributes != null)
//                        foreach (IndexedAttribute indexedAtri in atributes)
//                        {
//                            listesi.Add(!indexedAtri.Property.PropertyType.IsSubclassOf(typeof(Model))
//                                ? indexedAtri.Property.Name
//                                : $"{indexedAtri.Property.Name}_ID");
//                            isUnique = indexedAtri.IsUnique;
//                        }
//                    AddIndex(TableAttribute.GetName(objectType), listesi, isUnique);
//                }
//            }
//            else if (objectType.IsEnum)
//            {
//                AddEnumTable($"KC_ENUM_{objectType.Name.ToUpper(new CultureInfo("en-EN"))}", objectType, objectType.GetFields(BindingFlags.Static | BindingFlags.Public));
//            }
//            else
//            {
//                throw new Exception($"Type: '{objectType}' is not BaseObject class");
//            }
//        }

//        public override void AddTable(string tableName, PropertyInfo[] propertyInfos, string description)
//        {
//            try
//            {


//                DataTable table = null;

//                if (IsTableAvailable(tableName, Shema))
//                {
//                    table = GetMetaData(tableName);

//                }
//                else
//                {
//                    CreateTable(tableName, description);

//                }
//                foreach (PropertyInfo propertyInfo in propertyInfos)
//                {
//                    try
//                    {




//                        if (ColumnAttribute.GetColumnAttribute(propertyInfo) != null || AssociationAttribute.GetAssociationAttribute(propertyInfo) != null)
//                        {
//                            string columnName = ColumnAttribute.GetColumnAttribute(propertyInfo) != null ? ColumnAttribute.GetName(propertyInfo) : AssociationAttribute.GetName(propertyInfo);
//                            if (table != null && IsFieldAvaible(table, columnName))
//                            {

//                                if (IsFieldUpdateRequired(table, propertyInfo))
//                                    UpdateField(tableName, Shema, ColumnAttribute.GetName(propertyInfo), TypeName(propertyInfo));
//                                else
//                                {
//                                    if (propertyInfo.PropertyType.IsSubclassOf(typeof(Model)))
//                                    {

//                                        AddForeignKey(tableName, propertyInfo);

//                                    }
//                                }

//                            }
//                            else if (TypeName(propertyInfo) != null)
//                            {
//                                AddField(tableName, columnName, TypeName(propertyInfo), ColumnAttribute.GetIsCanBeNull(propertyInfo));
//                                AddForeignKey(tableName, propertyInfo);
//                            }
//                        }

//                    }
//                    catch (Exception ex)
//                    {


//                    }
//                }
//            }
//            catch (Exception ex)
//            {


//            }
//        }

//        public bool IsTableAvailable(string tablename)
//        {
//            string sql =$"select count(*)  from sysibm.systables WHERE  NAME='{tablename.ToUpper(new CultureInfo("en-EN"))}' and type = 'T' ";
//            DataTable table = ExecuteDataTable(sql);
//            int adet = Convert.ToInt32(table.Rows[0][0]);
//            return adet > 0;

//        }

//        public override DataTable GetMetaData(string tablename)
//        {
//            string sql = string.Format("Select distinct(name) AS name, ColType as type, Length size from Sysibm.syscolumns where tbname = '{1}' AND TBCREATOR='{0}'", Shema, tablename);
//            var table = ExecuteDataTable(sql);
//            return table;
//        }

//        public override void CreateTable(string tablename, string description)
//        {
//            string sql =
//                $@"CREATE TABLE {Shema}.{tablename
//                    } (
//  OID INTEGER NOT NULL PRIMARY KEY UNIQUE GENERATED BY DEFAULT AS IDENTITY (START WITH 1, INCREMENT BY 1, CACHE 2  ),
//  CreateDate TIMESTAMP(4),Rowversion INTEGER,UpdateDate TIMESTAMP(4),CreateUserId INTEGER,UpdateUserId INTEGER
//);";

//            ExecuteScalar(sql);

//        }

//        public override void UpdateField(string tableName, string schemaname, string fieldName, string type)
//        {

//            string sql = $@"ALTER TABLE {Shema}.{tableName}  ALTER COLUMN {fieldName} SET DATA TYPE {type}";
//            ExecuteScalar(sql);
//        }

//        public override void AddForeignKey(string tableName, PropertyInfo propertyInfo)
//        {
//            int forenkeyint = 0;
//            if (propertyInfo.PropertyType.IsSubclassOf(typeof(Model)))
//            {


//                string columnName = ColumnAttribute.GetColumnAttribute(propertyInfo) != null ? ColumnAttribute.GetName(propertyInfo) : AssociationAttribute.GetName(propertyInfo);
//                string _SS = string.Empty;
//                _SS = string.Format(@"select substr(tabname,1,20) table_name,substr(constname,1,20) 
//fk_name,substr(REFTABNAME,1,12) parent_table,substr(refkeyname,1,20) 
//pk_orig_table,fk_colnames from syscat.references where tabname = 
//'{0}' and fk_colnames=' {1}' and TABSCHEMA='{2}'", tableName, columnName, Shema);
//                DataTable reader = ExecuteDataTable(_SS);
//                for (int i = 0; i < reader.Rows.Count; i++)
//                {
//                    forenkeyint++;
//                }
//                if (forenkeyint == 0)
//                {

//                    string fkeyname = $"fk_{GetSequencerNumber("F_KEY")}";
//                    AddForeignKey(fkeyname, Shema,TableAttribute.GetName(propertyInfo.PropertyType), "OID", tableName, columnName);
//                }
//            }
//        }

//        public override void AddForeignKey(string fkeyName, string schemaName, string tableName, string masterFieldName, string detailTableName,
//            string detailFieldName)
//        {
//            string sql = string.Format(@"ALTER TABLE {5}.{0}
//  ADD CONSTRAINT {1}
//  FOREIGN KEY ({2})
//  REFERENCES {5}.{3}({4})
//  ON DELETE RESTRICT
//  ON UPDATE RESTRICT ", detailTableName, fkeyName, detailFieldName, tableName, masterFieldName, schemaName);

//            ExecuteScalar(sql);
//        }

//        public override string GetSequencerNumber(string keyname)
//        {
//            string returnedValu = string.Empty;
//            string sql =
//                string.Format(@"UPDATE 
//  {1}.SEQUENCER 
//SET 
 
//  VALUE = (SELECT MAX(VALUE)+1 FROM   {1}.SEQUENCER  WHERE NAME='{0}')
 
// WHERE NAME='{0}';
// SELECT MAX(VALUE)+1 FROM   {1}.SEQUENCER WHERE NAME='{0}';
//", keyname, Shema);
//            using (var newconnection = new DB2Connection(DbConnectionString))
//            {
//                if (newconnection.State != ConnectionState.Open) newconnection.Open();
//                using (var command = new DB2Command(sql, newconnection))
//                {

//                    using (var datareader = command.ExecuteReader(CommandBehavior.CloseConnection))
//                    {
//                        if (datareader.Read())
//                        {
//                            var data = datareader.GetValue(0);
//                            returnedValu = data.ToString();
//                        }
//                    }

//                }
//            }
//            return returnedValu;
//        }

//        public override void InsertField(string tableName, string schemaname, string fieldName, string type, bool canBeNull)
//        {
//            string sql = string.Format(@"ALTER TABLE {3}.{0}
//  ADD COLUMN {1} {2} ;", tableName, fieldName, type, Shema);
//            ExecuteScalar(sql);
//        }

//        public override void DelletAllIndex(string tablename, string schemaName)
//        {
//            StringBuilder bulder = new StringBuilder();
//            string tempIndexName = string.Empty;

//            bulder.AppendFormat(@" SELECT INDNAME, DEFINER, TABSCHEMA, TABNAME, 
//COLNAMES, COLCOUNT, UNIQUERULE, INDEXTYPE 
//FROM SYSCAT.INDEXES
//WHERE TABSCHEMA='{0}' AND UNIQUERULE<>'P' AND
//TABNAME = '{1}'", Shema, tablename);
//            var indextable = ExecuteDataTable(bulder.ToString());
//            for (int i = 0; i < indextable.Rows.Count; i++)
//            {
//                string indexColumnName = $"{indextable.Rows[i][$"{"INDNAME"}"]}";
//                if (!tempIndexName.Equals(indexColumnName))
//                {
//                    ExecuteScalar($"DROP INDEX {Shema}.{indexColumnName}");
//                    tempIndexName = indexColumnName;
//                }
//            }

//        }

//        public override void AddIndex(string tableName, List<string> grupIndexList, bool isUnique)
//        {

//            StringBuilder builder = new StringBuilder();
//            StringBuilder builderString = new StringBuilder();
//            for (int i = 0; i < grupIndexList.Count; i++)
//            {
//                builder.AppendFormat("{0}", grupIndexList[i]);
//                if (i != (grupIndexList.Count - 1))
//                    builder.Append(",");
//            }
//            string indexName = string.Format("INDX_{0}", GetSequencerNumber("INDX"));
//            builderString.AppendFormat(
//                isUnique
//                    ? @"CREATE UNIQUE INDEX {0}.{1}  ON {0}.{2}  ({3})  MINPCTUSED 50  PAGE SPLIT SYMMETRIC  COMPRESS NO"
//                    : @"CREATE INDEX {0}.{1}  ON {0}.{2}  ({3})  MINPCTUSED 50  PAGE SPLIT SYMMETRIC  COMPRESS NO",
//                Shema, indexName, tableName, builder);
//            ExecuteScalar(builderString.ToString());
//        }

//        public override void AddField(string tableName, string fieldName, string type, bool canBeNull)
//        {
//            InsertField(tableName, Shema, fieldName, type, canBeNull);
//        }

//        public override void AddEnumTable(string tableName, Type objectType, FieldInfo[] fieldInfos)
//        {
//            if (!IsTableAvailable(tableName, "dbo"))
//            {
//                string sql = String.Format("CREATE TABLE \"{0}\".\"{1}\" (\"NAME\" {2} NOT NULL, \"VALUE\" {3} NOT NULL,\"DISPLAYNAME\" {2})",Shema, tableName, GetTypeName(typeof(string), 100), GetTypeName(typeof(int), 100));

//                ExecuteScalar(sql);
//                Array.ForEach(fieldInfos, fInfo =>
//                {
//                    string enumName = fInfo.Name;
//                    string enumCaption = DisplayEnumNameAttribute.GetName(fInfo);
//                    int enumValue = Convert.ToInt32(Enum.Parse(objectType, enumName));
//                    sql = string.Format($"INSERT INTO {Shema}.{tableName} VALUES('{{0}}',{{1}},'{{2}}')", enumName, enumValue, enumCaption);
//                    ExecuteScalar(sql);
//                });
//            }
//            else
//            {

//                string ifIsAvaibleField = $"Delete  from {Shema}.{tableName}";
//                ExecuteScalar(ifIsAvaibleField);
//                Array.ForEach(fieldInfos, fInfo =>
//                {
//                    string enumName1 = fInfo.Name;
//                    string enumCaption1 = DisplayEnumNameAttribute.GetName(fInfo);
//                    int enumValue1 = Convert.ToInt32(Enum.Parse(objectType, enumName1));
//                    string sql1 = string.Format($"INSERT INTO {Shema}.{tableName} VALUES('{{0}}',{{1}},'{{2}}')", enumName1, enumValue1, enumCaption1);
//                    ExecuteScalar(sql1);
//                });


//            }
//        }

//        #endregion
   }
}
