using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UYGAR.Data
{
    public class TaxPayerWeb
    {
        private int _userId = 0;
        public string IpAdres { get; set; }
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
        public string MacAdress { get; set; }
        public string ComputareName { get; set; }
        public string KullaniciAdi { get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public int FirmaId { get; set; }
  
        public string SicilNo { get; set; }
        public string FromName { get; set; }
        public string FormCation { get; set; }
   
    }
}
