using System;
using System.Collections.Generic;
using System.Text;

namespace UYGAR.Exceptions
{
    [global::System.Serializable]
    public class GenelSistemHatasiException : KYSException
    {
        public GenelSistemHatasiException()
            : base(KYSExceptionId.GENEL_SISTEM_HATASI)
        {
          
        }
        public GenelSistemHatasiException(Exception inner)
            : base(KYSExceptionId.GENEL_SISTEM_HATASI, inner)
        {
         
        }
        protected GenelSistemHatasiException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            
        }

    }
}
