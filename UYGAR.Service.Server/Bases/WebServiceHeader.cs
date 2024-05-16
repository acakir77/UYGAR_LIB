using System.Web.Services.Protocols;
using UYGAR.Data;

namespace UYGAR.Service.Server.Bases
{
    public  class WebServiceHeader : SoapHeader
    {
        public TaxPayerWeb SessionTaxPayerInfo { get; set; }
        public WebServiceHeader()
        {
            SessionTaxPayerInfo = new TaxPayerWeb();

        }


    }
}