
namespace BAkimTAkipProgramii
{
    partial class frmIşPlanlari
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkYapildi = new System.Windows.Forms.CheckBox();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.btnYeni = new System.Windows.Forms.Button();
            this.txtIsDurumu = new System.Windows.Forms.TextBox();
            this.txtIsTanim = new System.Windows.Forms.TextBox();
            this.txtTakip = new System.Windows.Forms.TextBox();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.formNo = new System.Windows.Forms.NumericUpDown();
            this.cmbBolum = new System.Windows.Forms.ComboBox();
            this.cmbIslemTur = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.cmbIslemTuru = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTamamlanmaTarihi = new System.Windows.Forms.Label();
            this.lblIsID = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formNo)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1480, 375);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkYapildi);
            this.groupBox1.Controls.Add(this.btnKaydet);
            this.groupBox1.Controls.Add(this.btnYeni);
            this.groupBox1.Controls.Add(this.txtIsDurumu);
            this.groupBox1.Controls.Add(this.txtIsTanim);
            this.groupBox1.Controls.Add(this.txtTakip);
            this.groupBox1.Controls.Add(this.dateTimePicker2);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.formNo);
            this.groupBox1.Controls.Add(this.cmbBolum);
            this.groupBox1.Controls.Add(this.cmbIslemTur);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.cmbIslemTuru);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lblTamamlanmaTarihi);
            this.groupBox1.Controls.Add(this.lblIsID);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1474, 366);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // chkYapildi
            // 
            this.chkYapildi.AutoSize = true;
            this.chkYapildi.Location = new System.Drawing.Point(378, 283);
            this.chkYapildi.Name = "chkYapildi";
            this.chkYapildi.Size = new System.Drawing.Size(72, 21);
            this.chkYapildi.TabIndex = 6;
            this.chkYapildi.Text = "Yapıldı";
            this.chkYapildi.UseVisualStyleBackColor = true;
            // 
            // btnKaydet
            // 
            this.btnKaydet.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnKaydet.Location = new System.Drawing.Point(299, 327);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(209, 33);
            this.btnKaydet.TabIndex = 5;
            this.btnKaydet.Text = "KAYDET";
            this.btnKaydet.UseVisualStyleBackColor = false;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // btnYeni
            // 
            this.btnYeni.BackColor = System.Drawing.Color.PowderBlue;
            this.btnYeni.Location = new System.Drawing.Point(34, 327);
            this.btnYeni.Name = "btnYeni";
            this.btnYeni.Size = new System.Drawing.Size(208, 33);
            this.btnYeni.TabIndex = 5;
            this.btnYeni.Text = "YENİ";
            this.btnYeni.UseVisualStyleBackColor = false;
            this.btnYeni.Click += new System.EventHandler(this.btnYeni_Click);
            // 
            // txtIsDurumu
            // 
            this.txtIsDurumu.Location = new System.Drawing.Point(1241, 73);
            this.txtIsDurumu.Multiline = true;
            this.txtIsDurumu.Name = "txtIsDurumu";
            this.txtIsDurumu.Size = new System.Drawing.Size(164, 293);
            this.txtIsDurumu.TabIndex = 4;
            // 
            // txtIsTanim
            // 
            this.txtIsTanim.Location = new System.Drawing.Point(916, 73);
            this.txtIsTanim.Multiline = true;
            this.txtIsTanim.Name = "txtIsTanim";
            this.txtIsTanim.Size = new System.Drawing.Size(164, 293);
            this.txtIsTanim.TabIndex = 4;
            // 
            // txtTakip
            // 
            this.txtTakip.Location = new System.Drawing.Point(611, 74);
            this.txtTakip.Multiline = true;
            this.txtTakip.Name = "txtTakip";
            this.txtTakip.Size = new System.Drawing.Size(164, 290);
            this.txtTakip.TabIndex = 4;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(308, 247);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(200, 22);
            this.dateTimePicker2.TabIndex = 3;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(308, 204);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 22);
            this.dateTimePicker1.TabIndex = 3;
            // 
            // formNo
            // 
            this.formNo.Location = new System.Drawing.Point(308, 156);
            this.formNo.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.formNo.Name = "formNo";
            this.formNo.Size = new System.Drawing.Size(144, 22);
            this.formNo.TabIndex = 2;
            // 
            // cmbBolum
            // 
            this.cmbBolum.FormattingEnabled = true;
            this.cmbBolum.Location = new System.Drawing.Point(308, 111);
            this.cmbBolum.Name = "cmbBolum";
            this.cmbBolum.Size = new System.Drawing.Size(144, 24);
            this.cmbBolum.TabIndex = 1;
            // 
            // cmbIslemTur
            // 
            this.cmbIslemTur.FormattingEnabled = true;
            this.cmbIslemTur.Location = new System.Drawing.Point(308, 70);
            this.cmbIslemTur.Name = "cmbIslemTur";
            this.cmbIslemTur.Size = new System.Drawing.Size(144, 24);
            this.cmbIslemTur.TabIndex = 1;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(308, 111);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(144, 24);
            this.comboBox1.TabIndex = 1;
            // 
            // cmbIslemTuru
            // 
            this.cmbIslemTuru.FormattingEnabled = true;
            this.cmbIslemTuru.Location = new System.Drawing.Point(308, 70);
            this.cmbIslemTuru.Name = "cmbIslemTuru";
            this.cmbIslemTuru.Size = new System.Drawing.Size(144, 24);
            this.cmbIslemTuru.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(31, 288);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(156, 17);
            this.label7.TabIndex = 0;
            this.label7.Text = "TAMAMLANMA TARİHİ:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 247);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 17);
            this.label6.TabIndex = 0;
            this.label6.Text = "İSTENEN TARİH:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(31, 204);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "TALEP TARİHİ:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 162);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "FORM NO:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "BÖLÜM:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "TÜRÜ";
            // 
            // lblTamamlanmaTarihi
            // 
            this.lblTamamlanmaTarihi.AutoSize = true;
            this.lblTamamlanmaTarihi.Location = new System.Drawing.Point(305, 288);
            this.lblTamamlanmaTarihi.Name = "lblTamamlanmaTarihi";
            this.lblTamamlanmaTarihi.Size = new System.Drawing.Size(23, 17);
            this.lblTamamlanmaTarihi.TabIndex = 0;
            this.lblTamamlanmaTarihi.Text = "---";
            // 
            // lblIsID
            // 
            this.lblIsID.AutoSize = true;
            this.lblIsID.Location = new System.Drawing.Point(305, 25);
            this.lblIsID.Name = "lblIsID";
            this.lblIsID.Size = new System.Drawing.Size(23, 17);
            this.lblIsID.TabIndex = 0;
            this.lblIsID.Text = "---";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1238, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(73, 17);
            this.label10.TabIndex = 0;
            this.label10.Text = "DURUMU:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(913, 25);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 17);
            this.label9.TabIndex = 0;
            this.label9.Text = "İŞ TANIMI:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(608, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(120, 17);
            this.label8.TabIndex = 0;
            this.label8.Text = "TAKİP EDENLER:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "İŞ ID: ";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 375);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1480, 290);
            this.panel2.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1480, 290);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // frmIşPlanlari
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1480, 665);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frmIşPlanlari";
            this.Text = "İŞ PLANLARI";
            this.Load += new System.EventHandler(this.frmIşPlanlari_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formNo)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.NumericUpDown formNo;
        private System.Windows.Forms.ComboBox cmbBolum;
        private System.Windows.Forms.ComboBox cmbIslemTur;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox cmbIslemTuru;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblIsID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Button btnYeni;
        private System.Windows.Forms.TextBox txtIsDurumu;
        private System.Windows.Forms.TextBox txtIsTanim;
        private System.Windows.Forms.TextBox txtTakip;
        private System.Windows.Forms.Label lblTamamlanmaTarihi;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.CheckBox chkYapildi;
    }
}