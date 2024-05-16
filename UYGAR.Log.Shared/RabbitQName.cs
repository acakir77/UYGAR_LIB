using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UYGAR.Log.Shared
{
    [Flags]
    public enum RabbitQName : int
    {
        SELECT_Q = 0,
        INSERT_Q = 1,
        DELETE_Q = 2,
        UPDATE_Q = 4,
        EXCEPTION_Q = 8,
        GETALL_LOG_Q = 16
    }
}
