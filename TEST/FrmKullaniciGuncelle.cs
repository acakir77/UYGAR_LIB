using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using UYGAR.Data.Base;
using UYGAR.Roles;

namespace TEST
{
    public partial class FrmKullaniciGuncelle : UYGAR.UI.WIN.Base.Forms.FrmBaseSaveFrom
    {


        public FrmKullaniciGuncelle()
        {
            InitializeComponent();
        }
        private KULLANICILAR _user = null;
        public KULLANICILAR User
        {
            get => kULLANICILARBindingSource.GetData<KULLANICILAR>();

            set
            {
            
                kULLANICILARBindingSource.SetData(value); _user = value;
            }
        }
    }
}
