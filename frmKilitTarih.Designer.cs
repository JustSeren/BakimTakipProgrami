
namespace BAkimTAkipProgramii
{
    partial class frmKilitTarih
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
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.btnKilit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.dateTimePicker1.Location = new System.Drawing.Point(55, 56);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(322, 30);
            this.dateTimePicker1.TabIndex = 0;
            // 
            // btnKilit
            // 
            this.btnKilit.Location = new System.Drawing.Point(55, 146);
            this.btnKilit.Name = "btnKilit";
            this.btnKilit.Size = new System.Drawing.Size(322, 52);
            this.btnKilit.TabIndex = 1;
            this.btnKilit.Text = "TARİHE KADAR KİLİTLE";
            this.btnKilit.UseVisualStyleBackColor = true;
            this.btnKilit.Click += new System.EventHandler(this.btnKilit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnKilit);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Location = new System.Drawing.Point(12, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(426, 274);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // frmKilitTarih
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 300);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmKilitTarih";
            this.Text = "Tarih Kilitle";
            this.Load += new System.EventHandler(this.frmKilitTarih_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button btnKilit;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}