using System;
using System.Data.Common;
using System.Text;
using UYGAR.Data.Attributes;
using UYGAR.Data.Base;

namespace UYGAR.Data.Utilyti
{
    public class Log
    {
        #region Private Fields
        private static DbConnectionBase m_connection;


        private static DateTime m_time = DateTime.Now;
        private static LogType m_logType = LogType.Error;
        private static string m_message;
        private static string m_logData;
        #endregion

        #region Public Fields

        [Column]
        public static DateTime Time
        {
            get { return Log.m_time; }
            set { Log.m_time = value; }
        }

        [Column]
        public static LogType LogType
        {
            get { return Log.m_logType; }
            set { Log.m_logType = value; }
        }

        [Column]
        public static string Message
        {
            get { return Log.m_message; }
            set { Log.m_message = value; }
        }

        [Column]
        public static string LogData
        {
            get { return Log.m_logData; }
            set { Log.m_logData = value; }
        }

        public static DbConnectionBase Connection
        {
            get { return Log.m_connection; }
            set { Log.m_connection = value; }
        }

        #endregion

        #region Methods
        public static void InitDatabase(DbConnectionBase connection)
        {
            if (connection == null)
                return;
            //if (!connection.IsTableAvaible("LogData"))
            //{
            //    connection.AddTable("LogData");
            //    connection.AddField("LogData", "Time", "DateTime");
            //    connection.AddField("LogData", "LogType", "int");
            //    connection.AddField("LogData", "Message", "varchar(200)");
            //    connection.AddField("LogData", "LogData", "text");
            //}
        }

        public static void Write(DbConnectionBase connection, LogType logType, string message, string logdata)
        {
            if (connection == null)
                return;

            Log.Connection = connection;
            lock (Log.Connection)
            {
                InitDatabase(connection);

                System.Text.StringBuilder builder = new StringBuilder();
                builder.Append("insert into ");
                builder.Append("LogData");
                builder.Append(" (Time, LogType, Message, logdata) values ('");
                builder.Append(DateTime.Now);
                builder.Append("', '");
                builder.Append((int)logType);
                builder.Append("', '");
                builder.Append(message);
                builder.Append("', '");
                builder.Append(logdata);
                builder.Append("') ");
               // connection.ExecuteScalarInsert(builder.ToString());

            }
        }
        #endregion
    }

}
