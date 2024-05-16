using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UYGAR.Log.Shared
{
    public class DTOExeptionModel
    {
        #region Constructor

        public DTOExeptionModel() { }
    
        public DTOExeptionModel(string message, string trace)
        {
            ExecptionString = message;
            ExecptionTrace = trace;
         
        }
        public int UserId { get; set; }
        #endregion
        /// <summary>
        /// Sistemde Herhangi Bir Problem Olduğunda Yani [ Try - Catch ] Blokları Arasında Kod [ Catch ] Bloğuna Düştüyse [ Exeption ] Bilgisinin [ Message ] Field Bilgisi.
        /// </summary>
        public string ExecptionString { get; set; }
        /// <summary>
        /// Sistemde Herhangi Bir Problem Olduğunda Yani [ Try - Catch ] Blokları Arasında Kod [ Catch ] Bloğuna Düştüyse [ Exeption ] Bilgisinin [ Trace ] Field Bilgisi.
        /// </summary>
        public string ExecptionTrace { get; set; }
        /// <summary>
        /// Sistemde Herhangi Bir Problem Olduğunda Yani [ Try - Catch ] Blokları Arasında Kod [ Catch ] Bloğuna Düştüyse [ Exeption ] Bilgisinin [ Source ] Field Bilgisi.
        /// </summary>
        public DateTime Time { get; set; }
        public string ClientIPAddress { get; set; }
        public string ClientMacAddress { get; set; }
        public string ClientMachineName { get; set; }
        public string FormFullName { get; set; }
        public string FormCaption { get; set; }
      
   
      
    }
}
