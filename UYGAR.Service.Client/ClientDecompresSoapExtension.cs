using System;
using System.IO;
using System.IO.Compression;
using System.Web.Services.Protocols;

namespace UYGAR.Service.Client
{
    public class ClientDecompresSoapExtension : SoapExtension
    {


        private Stream networkStream;
        private Stream newStream;

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

        public override void ProcessMessage(System.Web.Services.Protocols.SoapMessage message)
        {
            switch (message.Stage)
            {
                case SoapMessageStage.AfterDeserialize:
                    break;
                case SoapMessageStage.AfterSerialize:
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

        private void AfterSerialize(System.Web.Services.Protocols.SoapMessage message)
        {
            newStream.Position = 0;
            Copy(newStream, networkStream);
        }

        private void BeforeDeserialize(System.Web.Services.Protocols.SoapMessage message)
        {
            Decompress(networkStream, newStream);
            newStream.Position = 0;
        }

        private void Copy(Stream fromStream, Stream toStream)
        {
            using (StreamReader reader = new StreamReader(fromStream))
            {
                using (StreamWriter writer = new StreamWriter(toStream))
                {
                    writer.WriteLine(reader.ReadToEnd());
                    writer.Flush();
                }
            }
        }

        private void Decompress(Stream fromStream, Stream toStream)
        {

            using (StreamReader reader = new StreamReader(fromStream))
            {

                StreamWriter writer = new StreamWriter(toStream);
                String kysWebServiceXmldocumentData = reader.ReadToEnd();
                KYSWebServiceXmlDocument xmlDoc = new KYSWebServiceXmlCompressedDocument();
                xmlDoc.LoadXml(kysWebServiceXmldocumentData);
                writer.WriteLine(xmlDoc.GetData());
                writer.Flush();
            }
        }
     



    }
}
