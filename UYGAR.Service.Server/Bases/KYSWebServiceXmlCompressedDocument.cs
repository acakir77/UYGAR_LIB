using System;
using UYGAR.Service.Server.Bases;

namespace UYGAR.Service.Server.Bases
{
    public class KYSWebServiceXmlCompressedDocument : KYSWebServiceXmlDocument
    {


        public KYSWebServiceXmlCompressedDocument()
            : base()
        {
            Initialize();
        }

        public KYSWebServiceXmlCompressedDocument(String data)
            : base(data)
        {
            Initialize();
        }

        private bool compressed;
        protected bool Compressed
        {
            get { return GetCompressed(); }
            set
            {
                compressed = value;
                UpdateCompressed(value);
            }
        }


        private void Initialize()
        {
            this.MessageVersion = "1.1";
        }

        private bool GetCompressed()
        {
            String val = this.DocumentElement.GetAttribute("Compressed");
            this.compressed = (val.Equals("true", StringComparison.OrdinalIgnoreCase));
            return this.compressed;
        }

        private void UpdateCompressed(bool val)
        {
            this.compressed = val;
            this.DocumentElement.SetAttribute("Compressed", this.compressed.ToString());
        }

        public override void SetData(string data)
        {
            this.Compressed = false;
            if (data.Length > 4096)
            {
                data = Compression.Compress(data);
                this.Compressed = true;
            }
            base.SetData(data);
        }

        public override string GetData()
        {
            String data = base.GetData();
            int datalen = data.Length;
            if (this.Compressed)
            {
                data = Compression.DeCompress(data);
            }
            return data;
        }
    }
}