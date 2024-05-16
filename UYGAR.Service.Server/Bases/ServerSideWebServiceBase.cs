using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using UYGAR.Data;

namespace UYGAR.Service.Server.Bases
{
    [WebService(Namespace = "http://kasotomotiv.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class ServerSideWebServiceBase : System.Web.Services.WebService
    {
        private WebServiceHeader webServiceUserInfo;
        public ServerSideWebServiceBase()
        {
            //if (WebServiceUserInfo != null)
            //{
            //    if (WebServiceUserInfo.SessionTaxPayerInfo != null)
            //    {
            //        UserSesionInfo sessionInfo = new UserSesionInfo(WebServiceUserInfo.SessionTaxPayerInfo);
            //        HttpContext.Current.Items["WebServiceHanduserinfo"] = WebServiceUserInfo.SessionTaxPayerInfo;
            //        HttpContext.Current.Session["TaxPayer"] = WebServiceUserInfo.SessionTaxPayerInfo;
            //    }
            //}
        }


        public WebServiceHeader WebServiceUserInfo
        {

            get
            {
                return webServiceUserInfo;
            }
            set
            {

                webServiceUserInfo = value;
                if (webServiceUserInfo != null)
                {
                    if (webServiceUserInfo.SessionTaxPayerInfo != null)
                    {

                        UserSesionInfo sessionInfo = new UserSesionInfo(webServiceUserInfo.SessionTaxPayerInfo);
                        HttpContext.Current.Items["WebServiceHanduserinfo"] = webServiceUserInfo.SessionTaxPayerInfo;
                        HttpContext.Current.Session["TaxPayer"] = webServiceUserInfo.SessionTaxPayerInfo;

                    }



                }

            }
        }
    }
}
