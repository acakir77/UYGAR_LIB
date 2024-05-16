using System;

namespace UYGAR.Exceptions
{
    [global::System.Serializable]
    public class UygulamaHatasiException : KYSException
    {
        public UygulamaHatasiException()
            : base()
        {
        }

        public UygulamaHatasiException(string message)
            : base(message)
        {
        }

        public UygulamaHatasiException(string message, Exception inner)
            : base(message, inner)
        {
        }
        protected UygulamaHatasiException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            
        }

    }
}
