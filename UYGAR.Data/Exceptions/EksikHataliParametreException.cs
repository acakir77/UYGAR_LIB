using System;
using System.Collections.Generic;
using System.Text;

namespace UYGAR.Exceptions
{
    [global::System.Serializable]
    public class EksikHataliParametreException : UygulamaHatasiException
    {
        private String parametre;
        public String Parametre
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return parametre; }
            [System.Diagnostics.DebuggerStepThrough]
            set { parametre = value; }
        }


        public EksikHataliParametreException()
            : base(KYSExceptionId.EKSIK_HATALI_BILGI)
        {

        }

        public EksikHataliParametreException(String parametre)
            : this()
        {
            this.parametre = parametre;
        }

        protected EksikHataliParametreException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            this.parametre = info.GetString("Parametre");
        }

        public override string Message
        {
            get
            {
                if (String.IsNullOrEmpty(parametre))
                {
                    return base.Message;
                }
                else
                {
                    StringBuilder sb = new StringBuilder(base.Message.Length + 20);
                    sb.Append(base.Message).Append(Environment.NewLine);
                    sb.Append("Parametre: ").Append(this.parametre).Append(Environment.NewLine);
                    return sb.ToString();
                }
            }
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            base.GetObjectData(info, context);
            info.AddValue("Parametre", this.parametre, typeof(string));
        }
    }
}
