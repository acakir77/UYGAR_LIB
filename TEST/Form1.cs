using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UYGAR.Data.Attributes;
using UYGAR.Data.Base;
using UYGAR.Data.Connection;
using UYGAR.Data.Connections;
using UYGAR.Roles;
using UYGAR.Roles.Islemler;
using UYGAR.Roles.Web;
using UYGAR.Roles.YetkiIslemleri;
using UYGAR.UI.WIN.Klavye;

namespace TEST
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //LoginSesionInformation.Loaded = true;
            //DbConnectionBase conn = new DbConnectionMySql();
            //conn.CreateSchema(typeof(DbAction));
            InitializeComponent();

        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            //IsYeriTanim yeri=new IsYeriTanim();
            ////yeri.CreateTable();
            //yeri.Adi = "TEST123132";
            //yeri.Kod = "ÖÇĞÜQAZüişlçöğ5489/*-/%'%%*";
            //yeri.Save();
            //DbConnectionPostgreSql P = new DbConnectionPostgreSql();
            //var tenim = P.LoadObject<IsYeriTanim>(4);
            //P.CreateSchema(typeof(IsYeriTanim));

            //XtraForm1 frm=new XtraForm1();
            //frm.IslemYetkisi= yetki;
            //frm.Text = yetki.Islem.IslemMenuAdi;
            //frm.ShowDialog();

        }

        private void uygarWinGrid1_Click(object sender, EventArgs e)
        {

        }

        private void textEdit1_Click(object sender, EventArgs e)
        {
            //FrmGenelKlavye k = new FrmGenelKlavye();
            //k.Deger = textEdit1.EditValue;
            //k.StartPosition = FormStartPosition.CenterParent;
            //if (k.ShowDialog() == DialogResult.OK)
            //{
            //    textEdit1.EditValue = k.Deger;
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DbConnectionPostgreSql my = new DbConnectionPostgreSql();
            AppDomain currentDomain = AppDomain.CurrentDomain;
            Assembly[] assemblies = currentDomain.GetAssemblies();
         
            Array.ForEach(assemblies, assembly =>
            {
                Type[] assemblyTypes = assembly.GetTypes();
                Array.ForEach(assemblyTypes, atype =>
                {

                    if (atype.IsEnum)
                    {
                        try
                        {
                            my.CreateSchema(atype);
                        }
                        catch (Exception)
                        {


                        }
                    }
                    if (atype.IsSubclassOf(typeof(Model)) && !atype.IsAbstract)
                    {
                        try
                        {
                            my.CreateSchema(atype);
                        }
                        catch (Exception)
                        {


                        }
                       

                        //ConnSql.CreateSchema(lis.Type);
                        //var model = Activator.CreateInstance(lis.Type) as Model;
                        //if (model != null) model.CreateDefaultRecords();
                        ////liste.Add(atype);

                    }

                });
            });

      
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            throw new UYGAR.Exceptions.UygulamaHatasiException("TEST");
        }
    }
}
