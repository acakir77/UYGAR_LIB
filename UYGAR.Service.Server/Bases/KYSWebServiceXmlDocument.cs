using System;
using System.Xml;

namespace UYGAR.Service.Server.Bases
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
        private string _messageVersion;
        protected string MessageVersion
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _messageVersion; }
            [System.Diagnostics.DebuggerStepThrough]
            set
            {
                _messageVersion = value;
                UpdateVersion();
            }
        }

        public virtual String GetData()
        {
            var documentElement = this.DocumentElement;
            if (documentElement != null)
            {
                XmlElement xmlElement = documentElement[DATA_NODE_NAME];
                if (xmlElement != null) return xmlElement.InnerText;
            }
            return string.Empty;
        }

        public virtual void SetData(String data)
        {
            XmlElement dataElement = this.CreateElement(DATA_NODE_NAME);
            dataElement.InnerText = data;
            var element = this.DocumentElement;
            if (element != null) element.AppendChild(dataElement);
        }

        private void UpdateVersion()
        {
            var element = this.DocumentElement;
            if (element != null) element.SetAttribute("MessageVersion", this.MessageVersion);
        }

        private void Initialize()
        {
            this.LoadXml(@"<?xml version='1.0' encoding='utf-8'?>
                             <KYSWebServiceMessage />");

            this.MessageVersion = "1.0";

        }
    }
}