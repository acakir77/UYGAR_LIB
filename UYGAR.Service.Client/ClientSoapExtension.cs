using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Web.Services.Protocols;
using UYGAR.Exceptions;


namespace UYGAR.Service.Client
{
    public class ClientSoapExtension : SoapExtension
    {


        public ClientSoapExtension()
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

        public override void ProcessMessage(SoapMessage message){
            switch (message.Stage)
            {
                case SoapMessageStage.AfterDeserialize:
                    if (message.Exception != null)
                    {
                        throw DeserializeException(message.Exception.Detail.InnerText);
                    }
                    break;
                case SoapMessageStage.AfterSerialize:
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    break;
                case SoapMessageStage.BeforeSerialize:

                    break;
            }
        }
        public override Stream ChainStream(Stream stream)
        {
            return stream;
        }

        private Exception DeserializeException(string str)
        {

            var formatter = new SoapFormatter();
            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(str)))
            {
                try
                {
                    var exception = (Exception)formatter.Deserialize(stream);
                    return exception;
                }
                catch
                {

                    return new KYSException(str);
                }
            }

        }
    }
}
