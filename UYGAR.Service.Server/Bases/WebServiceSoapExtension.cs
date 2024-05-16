using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Web.Services.Protocols;
using System.Xml;

namespace UYGAR.Service.Server.Bases
{
    public class WebServiceSoapExtension : SoapExtension
    {


        Stream _oldStream;
        Stream _newStream;

        public WebServiceSoapExtension()
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
                        SoapException soapException = message.Exception;
                        String serializedException;
                        if (soapException.InnerException is ISerializable)
                        {

                            serializedException = HandleSerializableWebServiceException((ISerializable)soapException.InnerException);
                        }
                        else
                        {
                            serializedException = HandleWebServiceException(message);
                        }
                        _newStream.Position = 0;
                        TextReader tr = new StreamReader(_newStream);
                        String s = tr.ReadToEnd();
                        s = s.Replace("<detail />", serializedException);
                        _newStream = new MemoryStream();
                        TextWriter tw = new StreamWriter(_newStream);
                        tw.Write(s);
                        tw.Flush();
                    }
                    _newStream.Position = 0;
                    CopyStream(_newStream, _oldStream);
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    CopyStream(_oldStream, _newStream);
                    _newStream.Position = 0;
                    break;
                case SoapMessageStage.BeforeSerialize:
                    break;
            }
        }

        public override Stream ChainStream(Stream stream)
        {
            _oldStream = stream;
            _newStream = new MemoryStream();
            return _newStream;
        }



        private String HandleWebServiceException(SoapMessage sm)
        {
            // _blnLogToUI = False
            // HandleException(sm.Exception)

            XmlDocument doc = new XmlDocument();
            XmlNode detailNode = doc.CreateNode(XmlNodeType.Element, SoapException.DetailElementName.Name, SoapException.DetailElementName.Namespace);

            XmlNode typeNode = doc.CreateNode(XmlNodeType.Element, "ExceptionType", SoapException.DetailElementName.Namespace);
            typeNode.InnerText = "";
            detailNode.AppendChild(typeNode);

            XmlNode messageNode = doc.CreateNode(XmlNodeType.Element, "ExceptionMessage", SoapException.DetailElementName.Namespace);
            if (sm.Exception.InnerException != null)
                messageNode.InnerText = sm.Exception.InnerException.Message;
            else
                messageNode.InnerText = sm.Exception.Message;
            detailNode.AppendChild(messageNode);

            XmlNode infoNode = doc.CreateNode(XmlNodeType.Element, "ExceptionInfo", SoapException.DetailElementName.Namespace);
            infoNode.InnerText = "";
            detailNode.AppendChild(infoNode);

            return detailNode.OuterXml;
        }
        private String HandleSerializableWebServiceException(ISerializable serializableException)
        {
            String serializedException = SerializeException(serializableException);
            XmlDocument doc = new XmlDocument();
            XmlNode detailNode = doc.CreateNode(XmlNodeType.Element, SoapException.DetailElementName.Name, SoapException.DetailElementName.Namespace);

            XmlNode typeNode = doc.CreateNode(XmlNodeType.Element, "ExceptionType", SoapException.DetailElementName.Namespace);
            typeNode.InnerText = "";
            detailNode.AppendChild(typeNode);

            XmlNode messageNode = doc.CreateNode(XmlNodeType.Element, "ExceptionMessage", SoapException.DetailElementName.Namespace);
            messageNode.InnerText = serializedException;
            detailNode.AppendChild(messageNode);

            XmlNode infoNode = doc.CreateNode(XmlNodeType.Element, "ExceptionInfo", SoapException.DetailElementName.Namespace);
            infoNode.InnerText = "";
            detailNode.AppendChild(infoNode);
            return detailNode.OuterXml;

        }

        private String SerializeException(ISerializable exception)
        {
            SoapFormatter formatter = new SoapFormatter();
            Stream stream = new MemoryStream();
            formatter.Serialize(stream, exception);
            stream.Position = 0;
            TextReader tr = new StreamReader(stream);
            return tr.ReadToEnd();

        }
        private void CopyStream(Stream fromStream, Stream toStream)
        {
            StreamReader sr = new StreamReader(fromStream);
            StreamWriter sw = new StreamWriter(toStream);
            sw.Write(sr.ReadToEnd());
            sw.Flush();

        }
    }
}