using System.ComponentModel;
using System.Web.Services.Protocols;

namespace UYGAR.Service.Client
{
    [System.Web.Services.WebServiceBindingAttribute(Name = "ServicesSoap", Namespace = "http://kasotomotiv.com/")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [ToolboxItem(false)]
    public class ClientSideWebserviceBase : SoapHttpClientProtocol
    {
        public WebServiceHeader WebServiceHand { get; set; }

        //public  string Url
        //{
        //    get { return base.Url; }
        //    set { base.Url = value; }
        //}


        protected  object[] GetResult(string methodName, object[] parameters)
        {



            object[] result = base.Invoke(methodName, parameters);

            return result;



        }
    }
}
