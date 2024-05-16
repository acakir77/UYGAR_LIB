namespace TEST
{
    partial class Form1
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
            this.uygarXtraTab1 = new UYGAR.UI.WIN.UygarXtraTab();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.uygarXtraTab1)).BeginInit();
            this.uygarXtraTab1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uygarXtraTab1
            // 
            this.uygarXtraTab1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uygarXtraTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uygarXtraTab1.Location = new System.Drawing.Point(0, 0);
            this.uygarXtraTab1.Name = "uygarXtraTab1";
            this.uygarXtraTab1.SelectedTabPage = this.xtraTabPage1;
            this.uygarXtraTab1.Size = new System.Drawing.Size(800, 450);
            this.uygarXtraTab1.TabIndex = 0;
            this.uygarXtraTab1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.button2);
            this.xtraTabPage1.Controls.Add(this.button1);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(798, 425);
            this.xtraTabPage1.Text = "xtraTabPage1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(367, 211);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(798, 425);
            this.xtraTabPage2.Text = "xtraTabPage2";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(593, 197);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.uygarXtraTab1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.uygarXtraTab1)).EndInit();
            this.uygarXtraTab1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UYGAR.UI.WIN.UygarXtraTab uygarXtraTab1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

