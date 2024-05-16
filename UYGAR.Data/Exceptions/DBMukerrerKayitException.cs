using System;
using System.Collections.Generic;
using System.Text;

namespace UYGAR.Exceptions
{
    [global::System.Serializable]
    public class DBMukerrerKayitException : UygulamaHatasiException
    {
        public DBMukerrerKayitException()
            : base(KYSExceptionId.KAYIT_MUKERRER)
        {
        }
        public DBMukerrerKayitException(Exception innerexeption)
            : base(KYSExceptionId.KAYIT_MUKERRER,innerexeption)
        {
        }
        protected DBMukerrerKayitException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }

    }
}
