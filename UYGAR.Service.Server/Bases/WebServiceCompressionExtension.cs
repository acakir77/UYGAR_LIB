using System;
using System.IO;
using System.Web.Services.Protocols;

namespace UYGAR.Service.Server.Bases
{
    public class WebServiceCompressionExtension : SoapExtension
    {
        Stream networkStream;
        Stream newStream;

        public WebServiceCompressionExtension()
        {

        }

        public override object GetInitializer(Type serviceType)
        {
            return null;
        }

        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return null;
        }

        public override void Initialize(object initializer)
        {
            return;
        }

        public override void ProcessMessage(SoapMessage message)
        {
            switch (message.Stage)
            {

                case SoapMessageStage.AfterDeserialize:
                    break;
                case SoapMessageStage.AfterSerialize:
                    if (message.Exception != null)
                    {
                        Exception ex = message.Exception;
                    }
                    AfterSerialize(message);
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    BeforeDeserialize(message);
                    break;
                case SoapMessageStage.BeforeSerialize:
                    break;
                default:
                    break;
            }
        }

        public override Stream ChainStream(Stream stream)
        {
            networkStream = stream;
            newStream = new MemoryStream();
            return newStream;
        }

        private void AfterSerialize(SoapMessage message)
        {
            this.newStream.Position = 0;
            Compress(this.newStream, this.networkStream);
        }

        private void BeforeDeserialize(SoapMessage message)
        {
            Copy(this.networkStream, this.newStream);
            this.newStream.Position = 0;
        }

        private void Copy(Stream fromStream, Stream toStream)
        {
            var reader = new StreamReader(fromStream);
            var writer = new StreamWriter(toStream);
            writer.WriteLine(reader.ReadToEnd());
            writer.Flush();
        }

        private void Compress(Stream fromStream, Stream toStream)
        {


            using (var reader = new StreamReader(fromStream))
            {

                var writer = new StreamWriter(toStream);
                String webServicePlainData = reader.ReadToEnd();
                KYSWebServiceXmlDocument xmlDoc = new KYSWebServiceXmlCompressedDocument();
                xmlDoc.SetData(webServicePlainData);
                writer.WriteLine(xmlDoc.OuterXml);
                writer.Flush();
            }

        }
    }
}