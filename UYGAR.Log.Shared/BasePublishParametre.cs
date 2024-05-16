using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UYGAR.Log.Shared
{
    public class BasePublishParametre : ILOGParametre
    {
        public int OID { get; set; }
        public DateTime CreateDate { get; set; }
        public int Rowversion { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }
        public int DeleteUserId { get; set; }
        public DateTime DeleteTime { get; set; }
        public bool IsDeleted { get; set; }
        public int DbActionType { get; set; }
        public int UserId { get; set; }
        public int ObjectOID { get; set; }
        public int ObjectRowVersion { get; set;   }
        public string ObjectTableName { get; set; }
        public string ObjectTypeFullName { get; set; }
        public string Query { get; set; }
        public int ObjectId { get; set; }
        public DateTime Time { get; set; }
        public string ClientIPAddress { get; set; }
        public string ClientMacAddress { get; set; }
        public string ClientMachineName { get; set; }
        public string FormFullName { get; set; }
        public string FormCaption { get; set; }
   
        public List<DTO_DB_Parameter> DbDtoParameter { get; set; }
    }
}
