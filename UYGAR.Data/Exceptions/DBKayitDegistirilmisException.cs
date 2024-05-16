using System;
using System.Collections.Generic;
using System.Text;

namespace UYGAR.Exceptions
{
    [global::System.Serializable]
    public class DbKayitDegistirilmisException : UygulamaHatasiException
    {
        public DbKayitDegistirilmisException()
            : base(KYSExceptionId.KAYIT_DEGISTIRILMIS)
        {
        }
        protected DbKayitDegistirilmisException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }

    }
}
