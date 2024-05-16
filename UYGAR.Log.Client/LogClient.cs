using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Text;
using UYGAR.Log.Shared;

namespace UYGAR.Log.Client
{
    public static class LogClient
    {

        private static IModel _model = null;
        private static int _islog = 2;
        public static int Islog
        {
            get
            {
                try
                {
                    if (_islog == 2)
                        _islog = Convert.ToInt32(ConfigurationManager.AppSettings["ISLOG"]);
                }
                catch (Exception)
                {

                    
                }
          
                return _islog;
            }
        }
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



        public static void PushQueryLog(BasePublishParametre log)
        {
            try
            {
               
                if (Islog == 1)
                {
                    if (log != null)
                    {

                        Model.QueueDeclare(queue: "QueryLog",
                                             durable: false,
                                             exclusive: false,
                                             autoDelete: true,
                                             arguments: null);
                        string message = JsonConvert.SerializeObject(log);
                        var body = Encoding.UTF8.GetBytes(message);

                        Model.BasicPublish(exchange: "",
                                             routingKey: "QueryLog",
                                             basicProperties: null,
                                             body: body);

                    }
                }

            }
            catch (Exception )
            {


            }

        }
        public static void PushExeptionLog(DTOExeptionModel log)
        {
            try
            {
      
                if (Islog == 1)
                {
                    if (log != null)
                    {
                        Model.QueueDeclare(queue: "ExeptionLog",
                                             durable: false,
                                             exclusive: false,
                                             autoDelete: true,
                                             arguments: null);
                        string message = JsonConvert.SerializeObject(log);
                        var body = Encoding.UTF8.GetBytes(message);
                        Model.BasicPublish(exchange: "",
                                             routingKey: "ExeptionLog",
                                             basicProperties: null,
                                             body: body);

                    }
                }
            }
            catch (Exception)
            {


            }

        }

    }
}
