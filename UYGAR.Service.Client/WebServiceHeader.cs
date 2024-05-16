using System.Web.Services.Protocols;
using UYGAR.Data;
using UYGAR.Roles;


namespace UYGAR.Service.Client
{
    public class WebServiceHeader : SoapHeader
    {
        public TaxPayerWeb SessionTaxPayerInfo { get; set; }

        public KULLANICILAR GirisYapanKullanici { get; set; }
        public WebServiceHeader()
        {
           

        }


    }
}
