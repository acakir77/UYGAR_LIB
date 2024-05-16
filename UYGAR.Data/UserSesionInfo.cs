

using System.Web;

namespace UYGAR.Data
{
    public class UserSesionInfo
    {

        public TaxPayerWeb TaxPayerdescendant { get; set; }
        private static UserSesionInfo sessionInfo;
        private UserSesionInfo()
        {
            TaxPayerdescendant = new TaxPayerWeb();
        }
        public UserSesionInfo(TaxPayerWeb identity)
        {
            sessionInfo = new UserSesionInfo { TaxPayerdescendant = HttpContext.Current.Session["TaxPayer"] as TaxPayerWeb };
        }

        public static UserSesionInfo Get()
        {
            sessionInfo = new UserSesionInfo { TaxPayerdescendant = HttpContext.Current.Session["TaxPayer"] as TaxPayerWeb };
            return sessionInfo;
        }



    }
}
