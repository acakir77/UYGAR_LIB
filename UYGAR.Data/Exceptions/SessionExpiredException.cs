using System;
using System.Collections.Generic;
using System.Text;

namespace UYGAR.Exceptions
{
    [global::System.Serializable]
    public class SessionExpiredException : KYSException
    {
        public SessionExpiredException()
            : base(KYSExceptionId.SESSION_EXPIRED)
        {
        }
        public SessionExpiredException(Exception inner)
            : base(KYSExceptionId.SESSION_EXPIRED, inner)
        {
        }
        protected SessionExpiredException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }

    }
}
