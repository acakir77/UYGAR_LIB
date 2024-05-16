using Devart.Data.MySql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
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
    public class DbConnectionMySql : DbConnectionBase
    {


        public override string DbConnectionString => ConfigurationManager.AppSettings["mysqlConn"];
        public override string DbName => ConfigurationManager.AppSettings["mysqlDbName"];

        public DbConnectionMySql()
        {


            //Devart.Data.MySql.sqld sqlDependency = new SqlDependency();
            //SqlDependency.Start(DbConnectionString);
            //sqlDependency.OnChange += SqlDependency_OnChange;
        }



        public override void InsertObject(Model _object)
        {
            try
            {


                int Oid = 0;
                var sqlStr = new StringBuilder();
                var valuesStr = new StringBuilder();
                var parametrseListesi = new List<DbParameter>();
                var objectType = _object.GetType();


                sqlStr.Append(String.Format("INSERT INTO {0} (", TableAttribute.GetName(objectType)));
                valuesStr.Append(" Values(");
                sqlStr.Append(string.Format("{0} ,", "CreateDate"));
                valuesStr.Append(string.Format("@{0},", "CreateDate"));
                var paramCreatateDateTime = new MySqlParameter { DbType = DbType.DateTime, Value = DateTime.Now, ParameterName = string.Format("@{0}", "CreateDate") };
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
                var paramUpdateDateTime = new MySqlParameter { DbType = DbType.DateTime, Value = DateTime.Now, ParameterName = string.Format("@{0}", "UpdateDate") };
                parametrseListesi.Add(paramUpdateDateTime);
                sqlStr.Append(string.Format("{0} ,", "DeleteUserId"));
                valuesStr.Append(string.Format("@{0},", "DeleteUserId"));
                var paramDeleteUserId = new MySqlParameter { DbType = DbType.Int32, Value = 0, ParameterName = string.Format("@{0}", "DeleteUserId") };
                parametrseListesi.Add(paramDeleteUserId);
                sqlStr.Append(string.Format("{0} ,", "DeleteTime"));
                valuesStr.Append(string.Format("@{0},", "DeleteTime"));
                var paramDeleteTime = new MySqlParameter { DbType = DbType.DateTime, Value = new DateTime(1901, 1, 1), ParameterName = string.Format("@{0}", "DeleteTime") };
                parametrseListesi.Add(paramDeleteTime);
                sqlStr.Append(string.Format("{0}", "IsDeleted"));
                valuesStr.Append(string.Format("@{0}", "IsDeleted"));
                var paramIsDeleted = new MySqlParameter { DbType = DbType.Boolean, Value = false, ParameterName = string.Format("@{0}", "IsDeleted") };
                parametrseListesi.Add(paramIsDeleted);

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
                            var param = new MySqlParameter { ParameterName = paramnae, DbType = DbType.Int32 };
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
                            var param = new MySqlParameter { ParameterName = string.Format("@{0}", propertyInfo.Name), Value = GetPropertyInfoValue(propertyInfo, _object), DbType = GetValueType(propertyInfo) };
                            parametrseListesi.Add(param);
                        }
                        count++;
                    }
                });

                if (count > 0)
                {
                    sqlStr.Append(") ");
                    valuesStr.Append(");   SELECT LAST_INSERT_ID();");
                    sqlStr.Append(valuesStr);
                    Oid = ExecuteScalarInsert(sqlStr.ToString(), parametrseListesi);
                    _object.OID = Oid;
                    _object.Rowversion = 1;
                    _object.CreateUserId = TaxPayerDescendant.UserId;
                    _object.UpdateUserId = TaxPayerDescendant.UserId;
                    _object.CreateDate = DateTime.Now;
                    _object.UpdateDate = DateTime.Now;
                    InsertLog(DbActionType.Insert, 1, Oid, TableAttribute.GetName(objectType), objectType, sqlStr.ToString(), DateTime.Now, parametrseListesi);



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
                var paramOid = new MySqlParameter { DbType = DbType.Int32, Value = _object.OID, ParameterName = string.Format("@{0}", "OID") };
                parametrseListesi.Add(paramOid);
                sqlStr.Append(string.Format("{0} ,", "CreateDate"));
                valuesStr.Append(string.Format("@{0},", "CreateDate"));
                var paramCreatateDateTime = new MySqlParameter { DbType = DbType.DateTime, Value = _object.CreateDate, ParameterName = string.Format("@{0}", "CreateDate") };
                parametrseListesi.Add(paramCreatateDateTime);
                sqlStr.Append(string.Format("{0},", "CreateUserId"));
                valuesStr.Append(string.Format("@{0},", "CreateUserId"));
                var paramCreatateUserId = new MySqlParameter { DbType = DbType.Int32, Value = _object.CreateUserId, ParameterName = string.Format("@{0}", "CreateUserId") };
                parametrseListesi.Add(paramCreatateUserId);
                sqlStr.Append(string.Format("{0},", "UpdateUserId"));
                valuesStr.Append(string.Format("@{0},", "UpdateUserId"));
                var paramUpdateUserId = new MySqlParameter { DbType = DbType.Int32, Value = _object.UpdateUserId, ParameterName = string.Format("@{0}", "UpdateUserId") };
                parametrseListesi.Add(paramUpdateUserId);

                sqlStr.Append(string.Format("{0},", "Rowversion"));
                valuesStr.Append(string.Format("@{0},", "Rowversion"));
                var paramRowversion = new MySqlParameter { DbType = DbType.Int32, Value = _object.Rowversion, ParameterName = string.Format("@{0}", "Rowversion") };
                parametrseListesi.Add(paramRowversion);
                sqlStr.Append(string.Format("{0} ,", "UpdateDate"));
                valuesStr.Append(string.Format("@{0},", "UpdateDate"));
                var paramUpdateDateTime = new MySqlParameter { DbType = DbType.DateTime, Value = _object.UpdateDate, ParameterName = string.Format("@{0}", "UpdateDate") };
                parametrseListesi.Add(paramUpdateDateTime);
                sqlStr.Append(string.Format("{0} ,", "DeleteUserId"));
                valuesStr.Append(string.Format("@{0},", "DeleteUserId"));
                var paramDeleteUserId = new MySqlParameter { DbType = DbType.Int32, Value = 0, ParameterName = string.Format("@{0}", "DeleteUserId") };
                parametrseListesi.Add(paramDeleteUserId);
                sqlStr.Append(string.Format("{0} ,", "DeleteTime"));
                valuesStr.Append(string.Format("@{0},", "DeleteTime"));
                var paramDeleteTime = new MySqlParameter { DbType = DbType.DateTime, Value = new DateTime(1901, 1, 1), ParameterName = string.Format("@{0}", "DeleteTime") };
                parametrseListesi.Add(paramDeleteTime);
                sqlStr.Append(string.Format("{0} ", "IsDeleted"));
                valuesStr.Append(string.Format("@{0}", "IsDeleted"));
                var paramIsDeleted = new MySqlParameter { DbType = DbType.Boolean, Value = false, ParameterName = string.Format("@{0}", "IsDeleted") };
                parametrseListesi.Add(paramIsDeleted);



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
                            var param = new MySqlParameter { ParameterName = paramnae, DbType = DbType.Int32 };
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
                            var param = new MySqlParameter { ParameterName = string.Format("@{0}", propertyInfo.Name), Value = GetPropertyInfoValue(propertyInfo, _object), DbType = GetValueType(propertyInfo) };
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
                    InsertLog(DbActionType.Insert, 1, Oid, TableAttribute.GetName(objectType), objectType, sqlStr.ToString(), DateTime.Now, parametrseListesi);
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
                var paramCreatateUserId = new MySqlParameter { DbType = DbType.Int32, Value = TaxPayerDescendant.UserId, ParameterName = string.Format("@{0}", "UpdateUserId") };
                parametrseListesi.Add(paramCreatateUserId);
                _object.Rowversion = _object.Rowversion + 1;
                sqlStr.Append(string.Format("{0}={1}", "Rowversion", string.Format("@{0},", "Rowversion")));
                var paramRowversion = new MySqlParameter { DbType = DbType.Int32, Value = _object.Rowversion, ParameterName = string.Format("@{0}", "Rowversion") };
                parametrseListesi.Add(paramRowversion);
                sqlStr.Append(string.Format("{0}={1}", "UpdateDate", string.Format("@{0}", "UpdateDate")));
                var paramUpdateDateTime = new MySqlParameter { DbType = DbType.DateTime, Value = DateTime.Now, ParameterName = string.Format("@{0}", "UpdateDate") };
                parametrseListesi.Add(paramUpdateDateTime);
                var paramOid = new MySqlParameter { DbType = DbType.Int32, Value = _object.OID, ParameterName = string.Format("@{0}", "OID") };
                parametrseListesi.Add(paramOid);
                var paramNewRowversion = new MySqlParameter { DbType = DbType.Int32, Value = oldRowVersion, ParameterName = string.Format("@{0}", "NewRowversion") };
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
                            var param = new MySqlParameter { ParameterName = string.Format("@{0}", string.Format("{0}_ID", propertyInfo.Name)), DbType = DbType.Int32 };
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
                            var param = new MySqlParameter { ParameterName = string.Format("@{0}", propertyInfo.Name), Value = GetPropertyInfoValue(propertyInfo, _object), DbType = GetValueType(propertyInfo) };
                            parametrseListesi.Add(param);
                        }

                    }
                });
                sqlStr.Append("  Where OID = @OID AND Rowversion=@NewRowversion ");
                int isUpdatet = ExecuteScalar(sqlStr.ToString(), parametrseListesi);
                if (isUpdatet.Equals(0))
                    throw new DbKayitDegistirilmisException();

                InsertLog(DbActionType.Update, oldRowVersion, _object.OID, TableAttribute.GetName(objectType), objectType, sqlStr.ToString(), DateTime.Now, parametrseListesi);


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
            sqlStr.AppendFormat($@"SELECT T.TABLE_NAME,K.COLUMN_NAME FROM information_schema.REFERENTIAL_CONSTRAINTS T
 INNER JOIN  information_schema.KEY_COLUMN_USAGE K ON K.TABLE_NAME=T.TABLE_NAME AND K.CONSTRAINT_NAME=T.CONSTRAINT_NAME  
  WHERE T.REFERENCED_TABLE_NAME='{TableAttribute.GetName(objectType)}'  AND K.TABLE_SCHEMA='{DbName}'");
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
            var paramDeleteUserId = new MySqlParameter { DbType = DbType.Int32, Value = TaxPayerDescendant.UserId, ParameterName = string.Format("@{0}", "DeleteUserId") };
            parametrseListesi.Add(paramDeleteUserId);
            sqlStr.Append(string.Format("{0}={1}", "DeleteTime", string.Format("@{0},", "DeleteTime")));
            var paramDeleteTime = new MySqlParameter { DbType = DbType.DateTime, Value = DateTime.Now, ParameterName = string.Format("@{0}", "DeleteTime") };
            parametrseListesi.Add(paramDeleteTime);
            sqlStr.Append(string.Format("{0}={1}", "IsDeleted", string.Format("@{0}", "IsDeleted")));
            var paramIsDeleted = new MySqlParameter { DbType = DbType.Boolean, Value = true, ParameterName = string.Format("@{0}", "IsDeleted") };
            parametrseListesi.Add(paramIsDeleted);
            sqlStr.Append($@" Where OID ={_object.OID} AND Rowversion={_object.Rowversion} ");

            int isUpdatet = ExecuteScalar(sqlStr.ToString(), parametrseListesi);
            if (isUpdatet.Equals(0))
            {
                throw new DbKayitDegistirilmisException();
            }
            InsertLog(DbActionType.Delete, _object.Rowversion, _object.OID, TableAttribute.GetName(objectType), objectType, sqlStr.ToString(), DateTime.Now, new List<DbParameter>());

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
        public override int ExecuteScalarInsert(string query, List<DbParameter> parameters)
        {
            try
            {

                int retval;
                var connection = GetConnection();
                using (var cmdS = new MySqlCommand())
                {
                    //cmdS.FetchAll = true;
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

                var connection = GetConnection();



                using (var cmdS = new MySqlCommand())
                {
                    //cmdS.FetchAll = true;
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
        public override int ExecuteScalarParams(string query, List<QueryParameters> dbParameters)
        {
            try
            {

                int retval;
                var connection = GetConnection();

                using (var cmdS = new MySqlCommand())
                {
                    //cmdS.FetchAll = true;
                    foreach (var item in dbParameters)
                    {
                        MySqlParameter param = new MySqlParameter();
                        param.ParameterName = item.ParametrName;
                        param.Value = item.Value;
                        param.DbType = item.DbType;
                        cmdS.Parameters.Add(param);
                    }
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
        public override object ExecuteScalar(string query)
        {
            try
            {
                var connection = GetConnection();

                object returneddata = null;
                using (var cmdS = new MySqlCommand())
                {
                    //cmdS.FetchAll = true;
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
                return string.Format("CAST('{0}' AS DATE)",
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
                return string.Format("CAST('{0}' AS DATETIME)",
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
                builder.Append(string.Format(" AND {0}", criteriasql));

            var connection = GetConnection();
            //builder.Append("  LOCK IN SHARE MODE");
            string query = builder.ToString();
            query = query.Replace("WHERE ()", "");


            LastQuery = query;
            _object = null;
            using (MySqlCommand CmdS = new MySqlCommand(query, connection))
            {



                //cmdS.FetchAll = true;
                using (MySqlDataReader reader = CmdS.ExecuteReader(CommandBehavior.CloseConnection))
                {


                    if (reader.Read())
                    {
                        int i = 0;
                        _object = new T();
                        _object.Populatereader(reader, i);



                    }


                }

            }
            if (_object != null)
                InsertLog(DbActionType.Select, _object != null ? _object.Rowversion : 0, _object != null ? _object.OID : 0, TableAttribute.GetName(typeof(T)), typeof(T), query, DateTime.Now, new List<DbParameter>());
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
                builder.Append(string.Format(" AND {0}", criteriasql));
            builder.AppendFormat(" ORDER BY {0} {1}", grupuFealdName, orders.IsDesc ? "DESC" : "ASC");
            var connection = GetConnection();

            string query = builder.ToString();

            query = query.Replace("WHERE ()", "");
            LastQuery = query;
            _object = null;
            using (MySqlCommand CmdS = new MySqlCommand(query, connection))
            {



                //cmdS.FetchAll = true;
                using (MySqlDataReader reader = CmdS.ExecuteReader(CommandBehavior.CloseConnection))
                {


                    if (reader.Read())
                    {
                        int i = 0;
                        _object = new T();
                        _object.Populatereader(reader, i);



                    }


                }
            }
            if (_object != null)
                InsertLog(DbActionType.Select, _object != null ? _object.Rowversion : 0, _object != null ? _object.OID : 0, TableAttribute.GetName(typeof(T)), typeof(T), query, DateTime.Now, new List<DbParameter>());
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
                    builder.Append(string.Format(" AND {0}", criteriasql));
                var connection = GetConnection();
                string query = builder.ToString();

                query = query.Replace("WHERE ()", "");
                LastQuery = query;
                using (var cmdS = new MySqlCommand(query, connection))
                {
                    //cmdS.FetchAll = true;
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
                if (lst.Count > 0)
                    InsertLog(DbActionType.Select, 0, 0, TableAttribute.GetName(typeof(TBaseObject)), typeof(TBaseObject), query, DateTime.Now, new List<DbParameter>());
                connection.Close();
                connection.Dispose();

                return lst;
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
        public override ModelCollection<Model> LoadObjects(Type type, Criteria criteria)
        {
            try
            {
                var builder = new StringBuilder();
                var lst = new ModelCollection<Model>();
                builder.Append(GetSelectQuery(type));

                string criteriasql = criteria.PopulateSQLCriteria(type, this);
                if (!string.IsNullOrEmpty(criteriasql))
                    builder.Append(string.Format(" AND {0}", criteriasql));


                var connection = GetConnection();

                string query = builder.ToString();

                query = query.Replace("WHERE ()", "");
                LastQuery = query;
                using (var cmdS = new MySqlCommand(query, connection))
                {
                    //cmdS.FetchAll = true;
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
                if (lst.Count > 0)
                    InsertLog(DbActionType.Select, 0, 0, TableAttribute.GetName(type), type, query, DateTime.Now, new List<DbParameter>());
                connection.Close();
                connection.Dispose();

                return lst;
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
                var connection = GetConnection();
                DataTable _table = new DataTable();
                using (var CmdS = new MySqlCommand())
                {
                    //cmdS.FetchAll = true;
                    CmdS.CommandText = query;
                    CmdS.Connection = connection;

                    using (MySqlDataAdapter AdapterS = new MySqlDataAdapter(CmdS))
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
        public override DataTable ExecuteDataTable(string query, List<QueryParameters> dbParameters)
        {
            try
            {
                var connection = GetConnection();
                DataTable _table = new DataTable();
                using (var CmdS = new MySqlCommand())
                {
                    //cmdS.FetchAll = true;
                    foreach (var item in dbParameters)
                    {
                        MySqlParameter param = new MySqlParameter();
                        param.ParameterName = item.ParametrName;
                        param.Value = item.Value;
                        param.DbType = item.DbType;
                        CmdS.Parameters.Add(param);
                    }

                    CmdS.CommandText = query;
                    CmdS.Connection = connection;

                    using (MySqlDataAdapter AdapterS = new MySqlDataAdapter(CmdS))
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
                var connection = GetConnection();
                string query = builder.ToString();
                query = query.Replace("WHERE ()", "");
                using (MySqlCommand cmdS = new MySqlCommand(query, connection))
                {
                    //cmdS.FetchAll = true;
                    using (MySqlDataAdapter adapet = new MySqlDataAdapter(cmdS))
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
        public override DataSet LoadToDataSet(string query)
        {
            try
            {
                DataSet set = new DataSet();
                var connection = GetConnection();

                using (MySqlCommand cmdS = new MySqlCommand(query, connection))
                {
                    //cmdS.FetchAll = true;
                    using (MySqlDataAdapter adapet = new MySqlDataAdapter(cmdS))
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
        public override DataTable LoadToDataTable(string query)
        {
            try
            {

                var connection = GetConnection();
                DataTable table = new DataTable();
                using (MySqlCommand cmdS = new MySqlCommand(query, connection))
                {
                    //cmdS.FetchAll = true;
                    using (MySqlDataAdapter adapet = new MySqlDataAdapter(cmdS))
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
                builder.Append(string.Format(" AND {0}", criteriasql));
            builder.AppendFormat(" ORDER BY {0} {1}", grupuFealdName, orders.IsDesc ? "DESC" : "ASC");
            var connection = GetConnection();

            string query = builder.ToString();
            query = query.Replace("WHERE ()", "");
            LastQuery = query;
            using (var cmdS = new MySqlCommand(query, connection))
            {
                //cmdS.FetchAll = true;
                using (MySqlDataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
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
            if (lst.Count > 0)
                InsertLog(DbActionType.Select, 0, 0, TableAttribute.GetName(typeof(TBaseObject)), typeof(TBaseObject), query, DateTime.Now, new List<DbParameter>());
            connection.Close();
            connection.Dispose();

            return lst;
        }
        public override string GetMaxValue(Type baseObjectType, string coloumName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(String.Format("SELECT MAX({0}) from {1} WHERE IsDeleted=0 ", coloumName, TableAttribute.GetName(baseObjectType)));
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
                builder.Append(string.Format("  WHERE IsDeleted=0 AND {0}", criteriasql));
            DataTable returnedTable = ExecuteDataTable(builder.ToString());
            if (returnedTable != null)
                return string.Format("{0}", returnedTable.Rows[0][0]);
            else
                return string.Empty;
        }
        public override string GetMinValue(Type baseObjectType, string coloumName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(String.Format("SELECT MIN({0}) from {1}  WHERE IsDeleted=0 ", coloumName, TableAttribute.GetName(baseObjectType)));
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
                builder.Append(string.Format(" WHERE IsDeleted=0 AND {0}", criteriasql));
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
            builder.Append(string.Format("SELECT SUM({0}) from {1}  WHERE IsDeleted=0", coloumName, TableAttribute.GetName(baseObjectType)));
            var connection = GetConnection();
            using (MySqlCommand cmdS = new MySqlCommand(builder.ToString(), connection))
            {
                //cmdS.FetchAll = true;
                using (MySqlDataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
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
            builder.Append(String.Format("SELECT ISNULL(SUM({0}),0) from {1} ", coloumName, TableAttribute.GetName(baseObjectType)));
            string criteriasql = criteria.PopulateSQLCriteria(baseObjectType, this);
            if (!string.IsNullOrEmpty(criteriasql))
                builder.Append(string.Format("   WHERE IsDeleted=0  AND {0}", criteriasql));
            var connection = new MySqlConnection(DbConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            using (MySqlCommand cmdS = new MySqlCommand(builder.ToString(), connection))
            {
                //cmdS.FetchAll = true;
                using (MySqlDataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
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
            var connection = GetConnection();
            using (MySqlCommand cmdS = new MySqlCommand(sql, connection))
            {
                //cmdS.FetchAll = true;
                using (MySqlDataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
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
            InsertLog(DbActionType.Select, 0, 0, typeof(T).Name, typeof(T), sql, DateTime.Now, new List<DbParameter>());
            return obje;

        }

        public override List<T> LoadRaportBaseObjects<T>(string sql)
        {
            List<T> lst = new List<T>();
            var connection = GetConnection();
            using (MySqlCommand cmdS = new MySqlCommand(sql, connection))
            {
                //cmdS.FetchAll = true;
                using (MySqlDataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
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
            InsertLog(DbActionType.Select, 0, 0, typeof(T).Name, typeof(T), sql, DateTime.Now, new List<DbParameter>());
            return lst;
        }
        public override List<T> LoadRaportBaseObjects<T>(string sql, List<QueryParameters> dbParameters)
        {
            List<T> lst = new List<T>();
            var connection = GetConnection();
            List<DbParameter> dd = new List<DbParameter>();
            using (MySqlCommand cmdS = new MySqlCommand(sql, connection))
            {
                //cmdS.FetchAll = true;
                foreach (var item in dbParameters)
                {
                    MySqlParameter param = new MySqlParameter();
                    param.ParameterName = item.ParametrName;
                    param.Value = item.Value;
                    param.DbType = item.DbType;
                    dd.Add(param);
                    cmdS.Parameters.Add(param);
                }

                using (MySqlDataReader reader = cmdS.ExecuteReader(CommandBehavior.CloseConnection))
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

            InsertLog(DbActionType.Select, 0, 0, typeof(T).Name, typeof(T), sql, DateTime.Now, dd);
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
                return "BIGINT";

            if (type == typeof(byte) || type == typeof(Int16))
                return "INT(11)";

            if (type == typeof(Int32))
                return "INT(11)";

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
                return "datetime";

            if (type == typeof(Nullable<DateTime>))
                return "datetime";

            if (type == typeof(double))
                return "float";

            if (type == typeof(decimal))
                return "decimal(16, 8)";

            if (type == typeof(bool))
                return "bit";
            if (type == typeof(Guid))
                return "varchar(1000)";

            if (type.IsSubclassOf(typeof(Model)))
                return "INT(11)";

            if (type.IsEnum)
                return "INT(11)";
            if (type == typeof(byte[]))
                return "LONGBLOB";
            return null;
        }
        public override string GetSelectQuery(Type BaseEntitytype)
        {
            List<string> tables = new List<string>();
            PropertyInfo[] infos = BaseEntitytype.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Array.Sort(infos, new DeclarationOrderComparator());
            StringBuilder builder = new StringBuilder();
            StringBuilder buiderJoins = new StringBuilder();
            string baseTableName = TableAttribute.GetName(BaseEntitytype);
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
                            buiderJoins.AppendFormat(" LEFT OUTER JOIN {0} AS {3}   ON {1}.{2}={3}.OID AND {3}.IsDeleted=0 ", tempTableName, baseTableName,
                                $"{info.Name}_ID", $"{tempTableName}_{i}");
                        }
                        else
                        {
                            builder.AppendFormat(PopulatequeryString(info.PropertyType, tempTableName, k));
                            buiderJoins.AppendFormat(" LEFT OUTER JOIN {0}  ON {1}.{2}={0}.OID AND {0}.IsDeleted=0", tempTableName, baseTableName,
                                $"{info.Name}_ID");
                        }
                        tables.Add(tempTableName);
                    }
                    else
                        builder.AppendFormat("{0}.{1},", baseTableName, info.Name);
                i = i + 1;
            });
            builder.Replace(",", "", builder.Length - 1, 1);
            builder.AppendFormat(" FROM {0}   ", baseTableName);
            if (buiderJoins.Length != 0)
                buiderJoins.Replace(",", "", buiderJoins.Length - 1, 1);
            builder.Append(buiderJoins);
            builder.AppendFormat(" WHERE {0}.IsDeleted=0", baseTableName);
            return builder.ToString();
        }

        public override string PopulatequeryString(Type BaseEntityType, string tableName, int i)
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
            PropertyInfo[] infos = BaseEntityType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            Array.Sort(infos, new DeclarationOrderComparator()); Array.ForEach(infos, info =>
            {

                if ((ColumnAttribute.GetColumnAttribute(info) != null || AssociationAttribute.GetName(info) != null) & !string.IsNullOrEmpty(TypeName(info)))
                    builder.AppendFormat("{0}.{1} AS {0}_{2} ,", tableName, ColumnAttribute.GetName(info), i++);


            });
            return builder.ToString();
        }

        public override void CreateSchema(Type objectType)
        {
            if (objectType == null)
                throw new UygulamaHatasiException("Oluşturlamak istenen Nesne NULL olamaz");
            if (objectType.IsSubclassOf(typeof(Model)))
            {
                string temtableName = TableAttribute.GetName(objectType);
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
                                _Listesi.Add(string.Format("{0}", _IndexedAtri.Property.Name));
                            }
                            else
                                _Listesi.Add(_IndexedAtri.Property.Name);
                        }
                        else
                            _Listesi.Add(string.Format("{0}_ID", _IndexedAtri.Property.Name));
                        _IsUnique = _IndexedAtri.IsUnique;
                    }
                    if (_IsUnique)
                    {
                        _Listesi.Add("IsDeleted");
                        _Listesi.Add("DeleteTime");
                    }
                    AddIndex(TableAttribute.GetName(objectType), _Listesi, _IsUnique);
                }
                //StringBuilder builder = new StringBuilder();
                //builder.AppendLine($@"DROP    TABLE IF EXISTS T_{TableAttribute.GetName(objectType)};");
                //builder.AppendLine($@"CREATE  TABLE T_{TableAttribute.GetName(objectType)} LIKE {TableAttribute.GetName(objectType)};");
                ////builder.AppendLine($@"ALTER   TABLE T_{TableAttribute.GetName(objectType)} ENGINE=MEMORY;");
                //builder.AppendLine($@"INSERT  INTO T_{TableAttribute.GetName(objectType)} SELECT * FROM {TableAttribute.GetName(objectType)} WHERE  IsDeleted=0;");
                //ExecuteScalar(builder.ToString());
            }
            else if (objectType.IsEnum)
            {
                AddEnumTable(string.Format("ENUM_{0}", objectType.Name), objectType, objectType.GetFields(BindingFlags.Static | BindingFlags.Public));
            }
            else
            {
                throw new Exception(string.Format("Type: '{0}' is not BaseObject class", objectType.GetType().ToString()));
            }
        }
        public void DeleteallForeignKey(string tableName)
        {
            string sql = string.Format("select TABLE_NAME,COLUMN_NAME,CONSTRAINT_NAME,REFERENCED_TABLE_NAME,REFERENCED_COLUMN_NAME from information_schema.KEY_COLUMN_USAGE  where   TABLE_NAME = '{0}'  and referenced_column_name is not NULL AND CONSTRAINT_SCHEMA='{1}';", tableName, DbName);
            DataTable table = ExecuteDataTable(sql);

            for (int i = 0; i < table.Rows.Count; i++)
            {
                string dropsql = string.Format("ALTER TABLE {0} DROP FOREIGN KEY {1};", tableName, table.Rows[i][2]);
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

                }
                else
                {
                    CreateTable(tableName, DbName);

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

                                AddField(tableName, columnName, TypeName(propertyInfo), ColumnAttribute.GetIsCanBeNull(propertyInfo), GetAfterFieldName(tableName));
                                //AddForeignKey(tableName, propertyInfo);
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
            }
        }
        public override bool IsTableAvailable(string tablename, string schemaname)
        {
            string sql = string.Format("select * from information_schema.TABLES where TABLE_SCHEMA='{1}' AND TABLE_NAME='{0}'", tablename, schemaname);
            return ExecuteScalar(sql) == null ? false : true; ;
        }
        public override void AddField(string tableName, string fieldName, string type, bool canBeNull, string afterField)
        {
            InsertField(tableName, DbName, fieldName, type, canBeNull, GetAfterFieldName(tableName));
            //base.AddField(tableName, fieldName, type, canBeNull, afterField);
        }
        public override void CreateTable(string tablename, string description)
        {
            string sql = $"CREATE TABLE {tablename} (OID INT  AUTO_INCREMENT NOT NULL ,CreateDate datetime NOT NULL DEFAULT CURRENT_TIMESTAMP(),Rowversion      INT NOT NULL DEFAULT 1,UpdateDate datetime NOT NULL DEFAULT CURRENT_TIMESTAMP(),CreateUserId   INT NOT NULL DEFAULT 1,UpdateUserId   INT NOT NULL DEFAULT 1, DeleteUserId INT NOT NULL DEFAULT 0,DeleteTime DATETIME DEFAULT '1901-01-01 00:00:00',  IsDeleted BIT NOT NULL DEFAULT 0 ,PRIMARY KEY (OID))ENGINE = InnoDB,CHARACTER SET latin5,COLLATE latin5_turkish_ci, AUTO_INCREMENT = 1, AVG_ROW_LENGTH = 1, ROW_FORMAT = DYNAMIC;";
            ExecuteScalar(sql);
        }
        public override void UpdateField(string tableName, string schemaname, string fieldName, string type)
        {

            string sql = string.Format(@"ALTER TABLE {0} CHANGE {1} {1}  {2};", tableName, fieldName, type);
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
            string sql = string.Format("select COLUMN_NAME from information_schema.COLUMNS where TABLE_SCHEMA='{1}' AND TABLE_NAME='{0}' ORDER BY ORDINAL_POSITION DESC LIMIT 1", tableName, DbName);
            string returnvalue = string.Empty;
            var connection = GetConnection();
            using (var command = new MySqlCommand(sql, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
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
            string sql = string.Empty;
            sql = string.Format(canBeNull ? @"ALTER TABLE {0}   ADD {1} {2} DEFAULT NULL AFTER {3};" : @"ALTER TABLE {0}   ADD {1} {2}   AFTER {3};", tableName, fieldName, type, afterField);

            ExecuteScalar(sql);
        }
        public override void AddForeignKey(string tableName, PropertyInfo propertyInfo)
        {
            int forenkeyint = 0;
            if (propertyInfo.PropertyType.IsSubclassOf(typeof(Model)))
            {


                string columnName = ColumnAttribute.GetColumnAttribute(propertyInfo) != null ? ColumnAttribute.GetName(propertyInfo) : AssociationAttribute.GetName(propertyInfo);
                if (!string.IsNullOrEmpty(columnName))
                {
                    List<string> columns = new List<string>();
                    columns.Add(columnName);
                    var ss = new StringBuilder();
                    ss.AppendFormat("select TABLE_NAME,COLUMN_NAME,CONSTRAINT_NAME,REFERENCED_TABLE_NAME,REFERENCED_COLUMN_NAME from information_schema.KEY_COLUMN_USAGE  where TABLE_SCHEMA='{2}' AND   TABLE_NAME = '{0}' and COLUMN_NAME='{1}'and referenced_column_name is not NULL;", tableName, columnName, DbName);
                    DataTable reader = ExecuteDataTable(ss.ToString());
                    for (int i = 0; i < reader.Rows.Count; i++)
                    {
                        forenkeyint++;
                    }
                    if (forenkeyint == 0)
                    {

                        string fkeyname = string.Format("fk_{0}", GetSequencerNumber("F_KEY"));
                        AddForeignKey(fkeyname, DbName, TableAttribute.GetName(propertyInfo.PropertyType), "OID", tableName, columnName);
                        AddFKIndex(tableName, columns, false);
                    }
                }
            }
        }
        public override void AddForeignKey(string fkeyName, string schemaName, string tableName, string masterFieldName, string detailTableName, string detailFieldName)
        {



            string sql = string.Format(@"ALTER TABLE {0} ADD CONSTRAINT {1} FOREIGN KEY ({2}) REFERENCES {3} ({4}) ON UPDATE RESTRICT ON DELETE RESTRICT;", detailTableName, fkeyName, detailFieldName, tableName, masterFieldName);
            ExecuteScalar(sql);
        }
        public override string GetSequencerNumber(string keyname)
        {
            string returnedValu = string.Empty;
            string sql = $"set @a=(SELECT  MAX(CAST(VALUE +1 AS SIGNED))FROM {SequencerTablename} WHERE NAME='{keyname}');  UPDATE {SequencerTablename} SET  VALUE = @a WHERE NAME='{keyname}'; select @a;";
            var connection = GetConnection();
            //if (connection.State != ConnectionState.Open)
            //    connection.Open();
            using (var command = new MySqlCommand(sql, connection))
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
            string _SQl = string.Format("SELECT index_name FROM information_schema.statistics WHERE index_name != 'primary' and TABLE_SCHEMA='{1}' and table_name='{0}'  GROUP BY index_name ", tablename, DbName);
            _INDEX_TABLE = this.ExecuteDataTable(_SQl.ToString());
            for (int i = 0; i < _INDEX_TABLE.Rows.Count; i++)
            {
                string _IndexColumn_Name = string.Format("{0}", _INDEX_TABLE.Rows[i][0]);
                this.ExecuteScalar(string.Format("ALTER TABLE {0} DROP INDEX {1};", tablename, _IndexColumn_Name));
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
                string indexName = string.Format("INDX_{0}", this.GetSequencerNumber("INDX"));
                sql = string.Format(isUnique ? "ALTER TABLE {0} ADD UNIQUE INDEX {1} ( {2} ); " : "ALTER TABLE {0} ADD  INDEX {1} ( {2} );", tableName, indexName, _Builder);
                ExecuteScalar(sql);
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

            }

        }
        private void AddFKIndex(string tableName, List<string> grupIndexList, bool isUnique)
        {

            StringBuilder _Builder = new StringBuilder();
            string sql;

            for (int i = 0; i < grupIndexList.Count; i++)
            {
                _Builder.AppendFormat("{0}", grupIndexList[i]);
                if (i != (grupIndexList.Count - 1))
                    _Builder.Append(",");
            }
            string indexName = string.Format("FORK_INDX_{0}", this.GetSequencerNumber("INDX"));
            sql = string.Format(isUnique ? "ALTER TABLE {0} ADD UNIQUE INDEX {1} ( {2} ); " : "ALTER TABLE {0} ADD  INDEX {1} ( {2} );", tableName, indexName, _Builder);
            ExecuteScalar(sql);
            try
            {

            }
            catch (Exception ex)
            {


            }

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
        public MySqlConnection GetConnection()
        {


            MySqlConnection Connection = new MySqlConnection(DbConnectionString);
            if (Connection.State != ConnectionState.Open)
            {

                Connection.Open();
            }

            return Connection;
        }



    }
}
