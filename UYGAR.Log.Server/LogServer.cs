using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data;
using System.Text;
using System.Xml.Linq;
using UYGAR.Data.Base;
using UYGAR.Data.Connections;
using UYGAR.Log.Shared;
using Devart.Data.MySql;

namespace UYGAR.Log.Server
{
    public static class LogServer
    {
        private static IModel _model = null;

        public static IModel Model
        {
            get
            {

                if (_model == null)
                {
                    string rhost = ConfigurationManager.AppSettings["RHOST"];
                    string rport = ConfigurationManager.AppSettings["RPORT"];
                    string ruser = ConfigurationManager.AppSettings["RUSER"];
                    string rPasword = ConfigurationManager.AppSettings["RPAS"];
                    ConnectionFactory factory = new ConnectionFactory();
                    factory.HostName = rhost;
                    factory.Port = Convert.ToInt32(rport);
                    factory.UserName = ruser;
                    factory.Password = rPasword;
                    IConnection connection = factory.CreateConnection();
                    _model = connection.CreateModel();



                }

                return _model;
            }
        }
        public static void GetQueryLog()
        {

            Model.QueueDeclare(queue: "QueryLog",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: true,
                                 arguments: null);
            var consumer = new EventingBasicConsumer(Model);
            consumer.Received += Consumer_Received;
           
            Model.BasicConsume(queue: "QueryLog",
                                 autoAck: true,
                                 consumer: consumer);


        }
        public static void ExeptionLog()
        {

            Model.QueueDeclare(queue: "ExeptionLog",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: true,
                                 arguments: null);
            var consumerex = new EventingBasicConsumer(Model);
            consumerex.Received += Consumerex_Received;

            Model.BasicConsume(queue: "ExeptionLog",
                           autoAck: true,
                           consumer: consumerex);


        }

        private static void Consumerex_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);
            var hata = JsonConvert.DeserializeObject<DTOExeptionModel>(message);
            InsertLogExeption(hata);
        }

        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);
            var person = JsonConvert.DeserializeObject<BasePublishParametre>(message);
            InsertLog(person);


        }
        public static void InsertLogExeption(DTOExeptionModel p)
        {
            LogExeptionInsert(p);


        }
        public static void InsertLog(BasePublishParametre p)
        {
            LogInsert(p);



        }
        public static void LogInsert(BasePublishParametre p)
        {

            var sqlStr = new StringBuilder();
            var valuesStr = new StringBuilder();
            var parametrseListesi = new List<DbParameter>();

            sqlStr.Append($"INSERT INTO SYS_LogMaster (");
            valuesStr.Append(" Values(");
            sqlStr.Append(string.Format(" {0} ,", "CreateDate"));
            valuesStr.Append(string.Format("@{0},", "CreateDate"));
            var paramCreatateDateTime = new MySqlParameter { DbType = DbType.DateTime, Value = DateTime.Now, ParameterName = string.Format("@{0}", "CreateDate") };
            parametrseListesi.Add(paramCreatateDateTime);
            sqlStr.Append(string.Format("{0},", "CreateUserId"));
            valuesStr.Append(string.Format("@{0},", "CreateUserId"));
            var paramCreatateUserId = new MySqlParameter { DbType = DbType.Int32, Value = p.UserId, ParameterName = string.Format("@{0}", "CreateUserId") };
            parametrseListesi.Add(paramCreatateUserId);
            sqlStr.Append(string.Format("{0},", "UpdateUserId"));
            valuesStr.Append(string.Format("@{0},", "UpdateUserId"));
            var paramUpdateUserId = new MySqlParameter { DbType = DbType.Int32, Value = p.UserId, ParameterName = string.Format("@{0}", "UpdateUserId") };
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
            sqlStr.Append(string.Format("{0} ,", "IsDeleted"));
            valuesStr.Append(string.Format("@{0},", "IsDeleted"));
            var paramIsDeleted = new MySqlParameter { DbType = DbType.Boolean, Value = false, ParameterName = string.Format("@{0}", "IsDeleted") };
            parametrseListesi.Add(paramIsDeleted);
            sqlStr.Append(string.Format("{0},", "DbActionType"));
            valuesStr.Append(string.Format("@{0},", "DbActionType"));
            var paramDbActionType = new MySqlParameter { DbType = DbType.Int32, Value = Convert.ToInt32(p.DbActionType), ParameterName = string.Format("@{0}", "DbActionType") };
            parametrseListesi.Add(paramDbActionType);

            sqlStr.Append(string.Format("{0},", "UserId"));
            valuesStr.Append(string.Format("@{0},", "UserId"));
            var paramUserId = new MySqlParameter { DbType = DbType.Int32, Value = p.UserId, ParameterName = string.Format("@{0}", "UserId") };
            parametrseListesi.Add(paramUserId);

            sqlStr.Append(string.Format("{0},", "ObjectOID"));
            valuesStr.Append(string.Format("@{0},", "ObjectOID"));
            var paramObjectOID = new MySqlParameter { DbType = DbType.Int32, Value = p.ObjectOID, ParameterName = string.Format("@{0}", "ObjectOID") };
            parametrseListesi.Add(paramObjectOID);


            sqlStr.Append(string.Format("{0},", "ObjectRowVersion"));
            valuesStr.Append(string.Format("@{0},", "ObjectRowVersion"));
            var paramObjectRowVersion = new MySqlParameter { DbType = DbType.Int32, Value = p.ObjectRowVersion, ParameterName = string.Format("@{0}", "ObjectRowVersion") };
            parametrseListesi.Add(paramObjectRowVersion);

            sqlStr.Append(string.Format("{0},", "ObjectTableName"));
            valuesStr.Append(string.Format("@{0},", "ObjectTableName"));
            var paramObjectTableName = new MySqlParameter { DbType = DbType.String, Value = p.ObjectTableName, ParameterName = string.Format("@{0}", "ObjectTableName") };
            parametrseListesi.Add(paramObjectTableName);

            sqlStr.Append(string.Format("{0},", "ObjectTypeFullName"));
            valuesStr.Append(string.Format("@{0},", "ObjectTypeFullName"));
            var paramObjectTypeFullName = new MySqlParameter { DbType = DbType.String, Value = p.ObjectTypeFullName, ParameterName = string.Format("@{0}", "ObjectTypeFullName") };
            parametrseListesi.Add(paramObjectTypeFullName);

            sqlStr.Append(string.Format("{0},", "Query"));
            valuesStr.Append(string.Format("@{0},", "Query"));
            var paramQuery = new MySqlParameter { DbType = DbType.String, Value = p.Query, ParameterName = string.Format("@{0}", "Query") };
            parametrseListesi.Add(paramQuery);

            sqlStr.Append(string.Format("{0},", "Time"));
            valuesStr.Append(string.Format("@{0},", "Time"));
            var paramTime = new MySqlParameter { DbType = DbType.DateTime, Value = p.Time, ParameterName = string.Format("@{0}", "Time") };
            parametrseListesi.Add(paramTime);

            sqlStr.Append(string.Format("{0},", "ClientIPAddress"));
            valuesStr.Append(string.Format("@{0},", "ClientIPAddress"));
            var paramClientIPAddress = new MySqlParameter { DbType = DbType.String, Value = p.ClientIPAddress, ParameterName = string.Format("@{0}", "ClientIPAddress") };
            parametrseListesi.Add(paramClientIPAddress);


            sqlStr.Append(string.Format("{0},", "ClientMacAddress"));
            valuesStr.Append(string.Format("@{0},", "ClientMacAddress"));
            var paramClientMacAddress = new MySqlParameter { DbType = DbType.String, Value = p.ClientMacAddress, ParameterName = string.Format("@{0}", "ClientMacAddress") };
            parametrseListesi.Add(paramClientMacAddress);

            sqlStr.Append(string.Format("{0},", "ClientMachineName"));
            valuesStr.Append(string.Format("@{0},", "ClientMachineName"));
            var paramClientMachineName = new MySqlParameter { DbType = DbType.String, Value = p.ClientMachineName, ParameterName = string.Format("@{0}", "ClientMachineName") };
            parametrseListesi.Add(paramClientMachineName);

            sqlStr.Append(string.Format("{0},", "FormFullName"));
            valuesStr.Append(string.Format("@{0},", "FormFullName"));
            var paramFormFullName = new MySqlParameter { DbType = DbType.String, Value = p.FormFullName, ParameterName = string.Format("@{0}", "FormFullName") };
            parametrseListesi.Add(paramFormFullName);


            sqlStr.Append(string.Format("{0}", "FormCaption"));
            valuesStr.Append(string.Format("@{0}", "FormCaption"));
            var paramFormCaption = new MySqlParameter { DbType = DbType.String, Value = p.FormCaption, ParameterName = string.Format("@{0}", "FormCaption") };
            parametrseListesi.Add(paramFormCaption);
            sqlStr.Append(") ");
            valuesStr.Append(");   SELECT LAST_INSERT_ID();");
            sqlStr.Append(valuesStr);
            var oid = ExecuteScalarInsert(sqlStr.ToString(), parametrseListesi);
            foreach (var item in p.DbDtoParameter)
            {
                sqlStr = new StringBuilder();
                valuesStr = new StringBuilder();
                parametrseListesi = new List<DbParameter>();
                sqlStr.Append($"INSERT INTO SYS_LogDetail (");
                valuesStr.Append(" Values(");
                sqlStr.Append(string.Format("{0} ,", "CreateDate"));
                valuesStr.Append(string.Format("@{0},", "CreateDate"));
                var paramCreatateDateTimed = new MySqlParameter { DbType = DbType.DateTime, Value = p.Time, ParameterName = string.Format("@{0}", "CreateDate") };
                parametrseListesi.Add(paramCreatateDateTimed);
                sqlStr.Append(string.Format("{0},", "CreateUserId"));
                valuesStr.Append(string.Format("@{0},", "CreateUserId"));
                var paramCreatateUserIdd = new MySqlParameter { DbType = DbType.Int32, Value = p.UserId, ParameterName = string.Format("@{0}", "CreateUserId") };
                parametrseListesi.Add(paramCreatateUserIdd);
                sqlStr.Append(string.Format("{0},", "UpdateUserId"));
                valuesStr.Append(string.Format("@{0},", "UpdateUserId"));
                var paramUpdateUserIdd = new MySqlParameter { DbType = DbType.Int32, Value = p.UserId, ParameterName = string.Format("@{0}", "UpdateUserId") };
                parametrseListesi.Add(paramUpdateUserIdd);

                sqlStr.Append(string.Format("{0},", "Rowversion"));
                valuesStr.Append(string.Format("@{0},", "Rowversion"));
                var paramRowversiond = new MySqlParameter { DbType = DbType.Int32, Value = 1, ParameterName = string.Format("@{0}", "Rowversion") };
                parametrseListesi.Add(paramRowversiond);
                sqlStr.Append(string.Format("{0} ,", "UpdateDate"));
                valuesStr.Append(string.Format("@{0},", "UpdateDate"));
                var paramUpdateDateTimed = new MySqlParameter { DbType = DbType.DateTime, Value = p.Time, ParameterName = string.Format("@{0}", "UpdateDate") };
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
                var paramFieldValue = new MySqlParameter { DbType = DbType.String, Value = item.Value != null ? item.Value.ToString() : "", ParameterName = string.Format("@{0}", "FieldValue") };
                parametrseListesi.Add(paramFieldValue);

                sqlStr.Append(string.Format("{0} ", "FiledType"));
                valuesStr.Append(string.Format("@{0}", "FiledType"));
                var paramFiledType = new MySqlParameter { DbType = DbType.String, Value = item.DbType != null ? item.DbType.ToString() : "", ParameterName = string.Format("@{0}", "FiledType") };
                parametrseListesi.Add(paramFiledType);
                sqlStr.Append(") ");
                valuesStr.Append("); SELECT LAST_INSERT_ID();");
                sqlStr.Append(valuesStr);
                ExecuteScalarInsert(sqlStr.ToString(), parametrseListesi);
            }
        }
        public static void LogExeptionInsert(DTOExeptionModel p)
        {
            var sqlStr = new StringBuilder();
            var valuesStr = new StringBuilder();
            var parametrseListesi = new List<DbParameter>();

            sqlStr.Append($"INSERT INTO SYS_LogExeption (");
            valuesStr.Append(" Values(");
            sqlStr.Append(string.Format(" {0} ,", "CreateDate"));
            valuesStr.Append(string.Format("@{0},", "CreateDate"));
            var paramCreatateDateTime = new MySqlParameter { DbType = DbType.DateTime, Value = DateTime.Now, ParameterName = string.Format("@{0}", "CreateDate") };
            parametrseListesi.Add(paramCreatateDateTime);
            sqlStr.Append(string.Format("{0},", "CreateUserId"));
            valuesStr.Append(string.Format("@{0},", "CreateUserId"));
            var paramCreatateUserId = new MySqlParameter { DbType = DbType.Int32, Value = p.UserId, ParameterName = string.Format("@{0}", "CreateUserId") };
            parametrseListesi.Add(paramCreatateUserId);
            sqlStr.Append(string.Format("{0},", "UpdateUserId"));
            valuesStr.Append(string.Format("@{0},", "UpdateUserId"));
            var paramUpdateUserId = new MySqlParameter { DbType = DbType.Int32, Value = p.UserId, ParameterName = string.Format("@{0}", "UpdateUserId") };
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
            sqlStr.Append(string.Format("{0} ,", "IsDeleted"));
            valuesStr.Append(string.Format("@{0},", "IsDeleted"));
            var paramIsDeleted = new MySqlParameter { DbType = DbType.Boolean, Value = false, ParameterName = string.Format("@{0}", "IsDeleted") };
            parametrseListesi.Add(paramIsDeleted);

            sqlStr.Append(string.Format("{0},", "UserId"));
            valuesStr.Append(string.Format("@{0},", "UserId"));
            var paramUserId = new MySqlParameter { DbType = DbType.Int32, Value = p.UserId, ParameterName = string.Format("@{0}", "UserId") };
            parametrseListesi.Add(paramUserId);

            sqlStr.Append(string.Format("{0},", "ExecptionString"));
            valuesStr.Append(string.Format("@{0},", "ExecptionString"));
            var paramObjectOID = new MySqlParameter { DbType = DbType.String, Value = p.ExecptionString, ParameterName = string.Format("@{0}", "ExecptionString") };
            parametrseListesi.Add(paramObjectOID);


            sqlStr.Append(string.Format("{0},", "ExecptionTrace"));
            valuesStr.Append(string.Format("@{0},", "ExecptionTrace"));
            var paramObjectRowVersion = new MySqlParameter { DbType = DbType.String, Value = p.ExecptionTrace, ParameterName = string.Format("@{0}", "ExecptionTrace") };
            parametrseListesi.Add(paramObjectRowVersion);

            sqlStr.Append(string.Format("{0},", "Time"));
            valuesStr.Append(string.Format("@{0},", "Time"));
            var paramObjectTableName = new MySqlParameter { DbType = DbType.DateTime, Value = p.Time, ParameterName = string.Format("@{0}", "Time") };
            parametrseListesi.Add(paramObjectTableName);

            sqlStr.Append(string.Format("{0},", "ClientIPAddress"));
            valuesStr.Append(string.Format("@{0},", "ClientIPAddress"));
            var paramObjectTypeFullName = new MySqlParameter { DbType = DbType.String, Value = p.ClientIPAddress, ParameterName = string.Format("@{0}", "ClientIPAddress") };
            parametrseListesi.Add(paramObjectTypeFullName);

            sqlStr.Append(string.Format("{0},", "ClientMacAddress"));
            valuesStr.Append(string.Format("@{0},", "ClientMacAddress"));
            var paramQuery = new MySqlParameter { DbType = DbType.String, Value = p.ClientMacAddress, ParameterName = string.Format("@{0}", "ClientMacAddress") };
            parametrseListesi.Add(paramQuery);

            sqlStr.Append(string.Format("{0},", "ClientMachineName"));
            valuesStr.Append(string.Format("@{0},", "ClientMachineName"));
            var paramTime = new MySqlParameter { DbType = DbType.String, Value = p.ClientMachineName, ParameterName = string.Format("@{0}", "ClientMachineName") };
            parametrseListesi.Add(paramTime);

            sqlStr.Append(string.Format("{0},", "FormFullName"));
            valuesStr.Append(string.Format("@{0},", "FormFullName"));
            var paramClientIPAddress = new MySqlParameter { DbType = DbType.String, Value = p.FormFullName, ParameterName = string.Format("@{0}", "FormFullName") };
            parametrseListesi.Add(paramClientIPAddress);


            sqlStr.Append(string.Format("{0}", "FormCaption"));
            valuesStr.Append(string.Format("@{0}", "FormCaption"));
            var paramClientMacAddress = new MySqlParameter { DbType = DbType.String, Value = p.FormCaption, ParameterName = string.Format("@{0}", "FormCaption") };
            parametrseListesi.Add(paramClientMacAddress);


            sqlStr.Append(") ");
            valuesStr.Append(");   SELECT LAST_INSERT_ID();");
            sqlStr.Append(valuesStr);
            var oid = ExecuteScalarInsert(sqlStr.ToString(), parametrseListesi);
        }
        public static int ExecuteScalarInsert(string query, List<DbParameter> parameters)
        {
            try
            {

                int retval;
                var connection = new MySqlConnection(ConfigurationManager.AppSettings["mysqlConn"]);
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (var cmdS = new MySqlCommand())
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

    }
}
