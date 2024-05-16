using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace UYGAR.Data
{
    [Serializable]
    public static class TaxPayerDescendant
    {
        private static int _userId = 0;
        private static string _MacAdress = string.Empty;
        private static string _ComputareName = string.Empty;
        private static string _KullaniciAdi = string.Empty;
        private static string _Adi = string.Empty;
        private static string _Soyadi = string.Empty;
        public static string IpAdres { get; set; }
        private static string _formName = string.Empty;
        private static string _formCapion = string.Empty;
        private static int _firmaid =0;
 
        public static int UserId
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Session != null)
                    {
                        if (HttpContext.Current.Session["TaxPayer"] != null)
                            return ((TaxPayerWeb)HttpContext.Current.Session["TaxPayer"]).UserId;
                        else
                            return _userId;
                    }
                    else
                        return _userId;
                }
                else
                    return _userId;
            }

            set
            {
                _userId = value;
            }
        }
      



        public static string MacAdress
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Session != null)
                    {
                        if (HttpContext.Current.Session["TaxPayer"] != null)
                            return ((TaxPayerWeb)HttpContext.Current.Session["TaxPayer"]).MacAdress;
                        else
                            return _MacAdress;
                    }
                    else
                        return _MacAdress;
                }
                else
                    return _MacAdress;
            }

            set
            {
                _MacAdress = value;
            }
        }

        public static string ComputareName
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Session != null)
                    {
                        if (HttpContext.Current.Session["TaxPayer"] != null)
                            return ((TaxPayerWeb)HttpContext.Current.Session["TaxPayer"]).ComputareName;
                        return _ComputareName;
                    }
                    return _ComputareName;
                }
                else
                    return _ComputareName;

            }

            set
            {
                _ComputareName = value;
            }
        }

        public static string KullaniciAdi
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Session != null)
                    {
                        if (HttpContext.Current.Session["TaxPayer"] != null)
                            return ((TaxPayerWeb)HttpContext.Current.Session["TaxPayer"]).KullaniciAdi;
                        else
                            return _KullaniciAdi;
                    }
                    else
                        return _KullaniciAdi;
                }
                else
                    return _KullaniciAdi;

            }

            set
            {
                _KullaniciAdi = value;
            }
        }
        public static int Firmaid
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Session != null)
                    {
                        if (HttpContext.Current.Session["TaxPayer"] != null)
                            return ((TaxPayerWeb)HttpContext.Current.Session["TaxPayer"]).FirmaId;
                        else
                            return _firmaid;
                    }
                    else
                        return _firmaid;
                }
                else
                    return _firmaid;
            }

            set
            {
                _firmaid = value;
            }
        }

        public static string Adi
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Session != null)
                    {
                        if (HttpContext.Current.Session["TaxPayer"] != null)
                            return ((TaxPayerWeb)HttpContext.Current.Session["TaxPayer"]).Adi;
                        else
                            return _Adi;
                    }
                    else
                        return _Adi;
                }
                else
                    return _Adi;

            }

            set
            {
                _Adi = value;
            }
        }

        public static string Soyadi
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Session != null)
                    {
                        if (HttpContext.Current.Session["TaxPayer"] != null)
                            return ((TaxPayerWeb)HttpContext.Current.Session["TaxPayer"]).Soyadi;
                        else
                            return _Soyadi;
                    }
                    else
                        return _Soyadi;
                }
                else
                    return _Soyadi;

            }

            set
            {
                _Soyadi = value;
            }
        }

        public static string FormName
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Session != null)
                    {
                        if (HttpContext.Current.Session["TaxPayer"] != null)
                            return ((TaxPayerWeb)HttpContext.Current.Session["TaxPayer"]).FromName;
                        return _formName;
                    }
                    return _formName;
                }
                else
                    return _formName;

            }

            set
            {
                _formName = value;
            }
        }
        public static string FormCapion
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Session != null)
                    {
                        if (HttpContext.Current.Session["TaxPayer"] != null)
                            return ((TaxPayerWeb)HttpContext.Current.Session["TaxPayer"]).FormCation;
                        return _formCapion;
                    }
                    return _formCapion;
                }
                else
                    return _formCapion;

            }

            set
            {
                _formCapion = value;
            }
        }


    }
}
