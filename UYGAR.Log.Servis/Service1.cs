using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using UYGAR.Log.Server;

namespace UYGAR.Log.Servis
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                LogServer.GetQueryLog();
                LogServer.ExeptionLog();
            }
            catch (Exception ex)
            {

                EventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
            }

        }

        protected override void OnStop()
        {
        }
    }
}
