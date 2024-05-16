namespace TEST
{
    partial class FrmKullaniciGuncelle
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmKullaniciGuncelle));
            this.kULLANICILARBindingSource = new UYGAR.UI.WIN.Base.UygarBindingSource(this.components);
            this.adiUygarTextEdit = new UYGAR.UI.WIN.Base.Componenets.UygarTextEdit();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.kullaniciAdiUygarTextEdit = new UYGAR.UI.WIN.Base.Componenets.UygarTextEdit();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).BeginInit();
            this.layoutControlBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kULLANICILARBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.adiUygarTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kullaniciAdiUygarTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControlBase
            // 
            this.layoutControlBase.Controls.Add(this.kullaniciAdiUygarTextEdit);
            this.layoutControlBase.Controls.Add(this.adiUygarTextEdit);
            this.layoutControlBase.Size = new System.Drawing.Size(874, 493);
            // 
            // Root
            // 
            this.Root.AppearanceItemCaption.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(162)));
            this.Root.AppearanceItemCaption.Options.UseFont = true;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItem3});
            this.Root.Size = new System.Drawing.Size(874, 493);
            // 
            // kULLANICILARBindingSource
            // 
            this.kULLANICILARBindingSource.DataSource = typeof(UYGAR.Roles.KULLANICILAR);
            // 
            // adiUygarTextEdit
            // 
            this.adiUygarTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.kULLANICILARBindingSource, "Adi", true));
            this.adiUygarTextEdit.FontOtomatik = UYGAR.Roles.EvetHayir.Evet;
            this.adiUygarTextEdit.Isclear = true;
            this.adiUygarTextEdit.Location = new System.Drawing.Point(78, 2);
            this.adiUygarTextEdit.Name = "adiUygarTextEdit";
            this.adiUygarTextEdit.Size = new System.Drawing.Size(794, 20);
            this.adiUygarTextEdit.StyleController = this.layoutControlBase;
            this.adiUygarTextEdit.TabIndex = 5;
            this.adiUygarTextEdit.YetkiNesneId = 0;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.adiUygarTextEdit;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(874, 24);
            this.layoutControlItem2.Text = "Adi:";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(64, 14);
            // 
            // kullaniciAdiUygarTextEdit
            // 
            this.kullaniciAdiUygarTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.kULLANICILARBindingSource, "KullaniciAdi", true));
            this.kullaniciAdiUygarTextEdit.FontOtomatik = UYGAR.Roles.EvetHayir.Evet;
            this.kullaniciAdiUygarTextEdit.Isclear = true;
            this.kullaniciAdiUygarTextEdit.Location = new System.Drawing.Point(78, 26);
            this.kullaniciAdiUygarTextEdit.Name = "kullaniciAdiUygarTextEdit";
            this.kullaniciAdiUygarTextEdit.Size = new System.Drawing.Size(794, 20);
            this.kullaniciAdiUygarTextEdit.StyleController = this.layoutControlBase;
            this.kullaniciAdiUygarTextEdit.TabIndex = 6;
            this.kullaniciAdiUygarTextEdit.YetkiNesneId = 0;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.kullaniciAdiUygarTextEdit;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(874, 469);
            this.layoutControlItem3.Text = "Kullanici Adi:";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(64, 14);
            // 
            // FrmKullaniciGuncelle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(874, 524);
            this.IconOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("FrmKullaniciGuncelle.IconOptions.SvgImage")));
            this.Name = "FrmKullaniciGuncelle";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).EndInit();
            this.layoutControlBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kULLANICILARBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.adiUygarTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kullaniciAdiUygarTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UYGAR.UI.WIN.Base.UygarBindingSource kULLANICILARBindingSource;
        private UYGAR.UI.WIN.Base.Componenets.UygarTextEdit adiUygarTextEdit;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private UYGAR.UI.WIN.Base.Componenets.UygarTextEdit kullaniciAdiUygarTextEdit;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
    }
}
