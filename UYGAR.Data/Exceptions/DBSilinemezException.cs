using System;
using System.Collections.Generic;
using System.Text;

namespace UYGAR.Exceptions
{
    [global::System.Serializable]
    public class DBSilinemezException : KYSException{
        public DBSilinemezException()
            : base(KYSExceptionId.KAYIT_SILINEMEZ)
        {

        }
        public DBSilinemezException(Exception innerExeption)
            : base(KYSExceptionId.KAYIT_SILINEMEZ, innerExeption)
        {

        }
        protected DBSilinemezException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }

    }
}
