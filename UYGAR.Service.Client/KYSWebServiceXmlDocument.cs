using System;
using System.Xml;

namespace UYGAR.Service.Client
{
    public class KYSWebServiceXmlDocument : XmlDocument
    {
        public const String DATA_NODE_NAME = "KPLWebServiceData";

        public KYSWebServiceXmlDocument()
            : base()
        {
            Initialize();
        }

        public KYSWebServiceXmlDocument(String data)
            : base()
        {
            Initialize();
            this.SetData(data);
        }
        private String messageVersion;
        protected String MessageVersion
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return messageVersion; }
            [System.Diagnostics.DebuggerStepThrough]
            set
            {
                messageVersion = value;
                UpdateVersion();
            }
        }

        public virtual String GetData()
        {
            XmlElement xmlElement = this.DocumentElement[DATA_NODE_NAME];
            return xmlElement.InnerText;
        }

        public virtual void SetData(String data)
        {
            XmlElement dataElement = this.CreateElement(DATA_NODE_NAME);
            dataElement.InnerText = data;
            this.DocumentElement.AppendChild(dataElement);
        }

        private void UpdateVersion()
        {
            this.DocumentElement.SetAttribute("MessageVersion", this.MessageVersion);
        }

        private void Initialize()
        {
            this.LoadXml(@"<?xml version='1.0' encoding='utf-8'?>
                             <KYSWebServiceMessage />");

            this.MessageVersion = "1.0";

        }

    }
}
