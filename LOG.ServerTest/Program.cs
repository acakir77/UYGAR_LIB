using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UYGAR.Log.Client;
using UYGAR.Log.Server;
using UYGAR.Log.Shared;

namespace LOG.ServerTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //DTOExeptionModel model = new DTOExeptionModel();
            //model.UserId = 0;
            //model.ExecptionString = "TEST";
            //model.ExecptionTrace = "";
            //model.Time = DateTime.Now;
            //model.ClientIPAddress = "";
            //model.ClientMacAddress = "";
            //model.ClientMachineName = "";
            //model.FormFullName = "";
            //model.FormCaption = "";
            //LogClient.PushExeptionLog(model);
            LogServer.GetQueryLog();
       
            Console.ReadLine();
        }
    }
}
