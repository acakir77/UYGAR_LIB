using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UYGAR.Data.Criterias;
using UYGAR.Roles;
using UYGAR.UI.WIN.Base.Forms;

namespace TEST
{
    public partial class XtraForm1 : FrmBaseListFromGrid
    {
        public XtraForm1()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!DesignMode)
            {
                KayitlariListele();
            }
        }
        public override void KayitlariListele()
        {
            CriteriaGroup criteriaGroup = new CriteriaGroup();
            if (!string.IsNullOrEmpty(kullaniciAdiUygarTextEdit.Text))
                criteriaGroup.CriteriaList.Add(new CompareCriteria(KULLANICILAR.F_KULLANICIADI, CompareOperator.StartWith, kullaniciAdiUygarTextEdit.Text));
            if (!string.IsNullOrEmpty(adiUygarTextEdit.Text))
                criteriaGroup.CriteriaList.Add(new CompareCriteria(KULLANICILAR.F_ADI, CompareOperator.Like, adiUygarTextEdit.Text));
            if (!string.IsNullOrEmpty(soyadiUygarTextEdit.Text))
                criteriaGroup.CriteriaList.Add(new CompareCriteria(KULLANICILAR.F_SOYADI, CompareOperator.Like, soyadiUygarTextEdit.Text));

            kULLANICILARBindingSource.SetData(Conn.LoadObjects<KULLANICILAR>(criteriaGroup));
            base.KayitlariListele();
        }
        protected override void KayitGuncelle()
        {
            base.KayitGuncelle();
            FrmKullaniciGuncelle frm = new FrmKullaniciGuncelle();
            frm.User = SelectedData as KULLANICILAR;
            frm.EditType = UYGAR.UI.WIN.Base.EditType.Edit;
            frm.Model = SelectedData as KULLANICILAR;
            KayitGuncelle(frm);
        }
    }
}