using System;
using System.Collections.Generic;
using System.Text;
using UYGAR.Data;
using UYGAR.Log.Client;
using UYGAR.Log.Shared;

namespace UYGAR.Exceptions
{
    [Serializable]
    public class KYSException : ApplicationException
    {


        public KYSException()
        {
            DTOExeptionModel model = new DTOExeptionModel();
            model.UserId = TaxPayerDescendant.UserId;
            model.ExecptionString = this.Message;          
            model.ExecptionTrace = this.StackTrace;
            model.Time = DateTime.Now;
            model.ClientIPAddress = TaxPayerDescendant.IpAdres;
            model.ClientMacAddress = TaxPayerDescendant.MacAdress;
            model.ClientMachineName = TaxPayerDescendant.ComputareName;
            model.FormFullName = TaxPayerDescendant.FormName;
            model.FormCaption = TaxPayerDescendant.FormCapion;

            LogClient.PushExeptionLog(model);
        }
        public KYSException(string message)
            : base(message)
        {
            DTOExeptionModel model = new DTOExeptionModel();
            model.UserId = TaxPayerDescendant.UserId;
            model.ExecptionString = this.Message;
            model.ExecptionTrace = this.StackTrace;
            model.Time = DateTime.Now;
            model.ClientIPAddress = TaxPayerDescendant.IpAdres;
            model.ClientMacAddress = TaxPayerDescendant.MacAdress;
            model.ClientMachineName = TaxPayerDescendant.ComputareName;
            model.FormFullName = TaxPayerDescendant.FormName;
            model.FormCaption = TaxPayerDescendant.FormCapion;
            LogClient.PushExeptionLog(model);

        }
        public KYSException(string message, Exception inner) : base(message, inner)
        {
            DTOExeptionModel model = new DTOExeptionModel();
            model.UserId = TaxPayerDescendant.UserId;
            model.ExecptionString = this.Message;
            model.ExecptionTrace = this.StackTrace;
            model.Time = DateTime.Now;
            model.ClientIPAddress = TaxPayerDescendant.IpAdres;
            model.ClientMacAddress = TaxPayerDescendant.MacAdress;
            model.ClientMachineName = TaxPayerDescendant.ComputareName;
            model.FormFullName = TaxPayerDescendant.FormName;
            model.FormCaption = TaxPayerDescendant.FormCapion;
            LogClient.PushExeptionLog(model);
        }
        protected KYSException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            String s = info.AssemblyName;
            DTOExeptionModel model = new DTOExeptionModel();
            model.UserId = TaxPayerDescendant.UserId;
            model.ExecptionString = this.Message;
            model.ExecptionTrace = this.StackTrace;
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
