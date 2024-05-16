
using UYGAR.Roles;

namespace UYGAR.Service.Client
{
    public class ServiceAdapter
    {

        private static WebServiceHeader GirisYapanKullaniciBilgileriniGetir()
        {
            WebServiceHeader header = new WebServiceHeader { SessionTaxPayerInfo = LoginSesionInformation.GirisYapanKullaniciBilgileri, GirisYapanKullanici = LoginSesionInformation.GirisYapanKullanici };
            return header;
        }


        public static TService LoadService<TService>() where TService : ClientSideWebserviceBase, new()
        {
            using (var service = new TService { WebServiceHand = GirisYapanKullaniciBilgileriniGetir(), Timeout = int.MaxValue })
            {
                return service;
            }

        }


    }
}
