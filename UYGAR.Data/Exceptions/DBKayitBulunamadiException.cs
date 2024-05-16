using System;
using System.Collections.Generic;
using System.Text;



namespace UYGAR.Exceptions
{
    [global::System.Serializable]
    public class DBKayitBulunamadiException : UygulamaHatasiException
    {
        private String dac;
        public String DacAdi
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return dac; }
        }

        private String sorgu;
        public String Sorgu
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return sorgu; }
        }

        private object[] kriterler;
        public object[] KriterDegerleri
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return kriterler; }
        }


        public DBKayitBulunamadiException()
            : base(KYSExceptionId.KAYIT_BULUNAMADI)
        {
        }

        public DBKayitBulunamadiException(Type dacType, String sorgu, params object[] kriterler)
            : this(dacType.ToString(), sorgu, kriterler)
        {
        }

        public DBKayitBulunamadiException(String dacAdi, String sorgu, params object[] kriterler)
            : base(KYSExceptionId.KAYIT_BULUNAMADI)
        {
            this.dac = dacAdi;
            this.sorgu = sorgu;
            this.kriterler = kriterler;
        }

        protected DBKayitBulunamadiException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            this.dac = info.GetString("DacAdi");
            this.sorgu = info.GetString("Sorgu");
            this.kriterler = (object[])info.GetValue("KriterDegerleri", typeof(object[]));
        }

        public override string Message
        {
            get
            {
                if (kriterler != null && kriterler.Length > 0)
                {
                    StringBuilder sb = new StringBuilder(base.Message.Length + 100);
                    sb.Append(base.Message).Append(Environment.NewLine);
                    sb.Append("DAC: ").Append(this.dac).Append(Environment.NewLine);
                    sb.Append("Sorgu: ").Append(this.sorgu).Append(Environment.NewLine);
                    sb.Append("Kriterler: ");
                    foreach (object kriter in kriterler)
                    {
                        sb.Append(kriter.ToString()).Append(", ");
                    }
                    return sb.Remove(sb.Length - 2, 2).ToString();
                }
                else
                {
                    return base.Message;
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

            info.AddValue("DacAdi", this.dac, typeof(string));
            info.AddValue("Sorgu", this.sorgu, typeof(string));
            info.AddValue("KriterDegerleri", this.kriterler);
        }
    }
}
