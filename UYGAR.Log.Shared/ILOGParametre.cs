using System;
using System.Collections.Generic;
using System.Data.Common;

namespace UYGAR.Log.Shared
{
  
    public interface ILOGParametre
    {
        int OID { get; set; }

        DateTime CreateDate { get; set; }
        int Rowversion { get; set; }
        DateTime UpdateDate { get; set; }
        int CreateUserId { get; set; }
        int UpdateUserId { get; set; }
        int DeleteUserId { get; set; }
        DateTime DeleteTime { get; set; }
        bool IsDeleted { get; set; }
        int DbActionType { get; set; }
        int UserId { get; set; }
        int ObjectOID { get; set; }
        int ObjectRowVersion { get; set; }
        string ObjectTableName { get; set; }
        string ObjectTypeFullName { get; set; }
        string Query { get; set; }
        int ObjectId { get; set; }
        DateTime Time { get; set; }
        string ClientIPAddress { get; set; }
        string ClientMacAddress { get; set; }
        string ClientMachineName { get; set; }
        string FormFullName { get; set; }
        string FormCaption { get; set; }
 
        List<DTO_DB_Parameter> DbDtoParameter { get; set; }

    }
}
