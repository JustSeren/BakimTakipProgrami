using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BAkimTAkipProgramii
{
    public partial class frmSifreDegisikligi : Form
    {
        public frmSifreDegisikligi()
        {
            InitializeComponent();
        }
        private void frmSifreDegisikligi_Load(object sender, EventArgs e)
        {
            lblKullaniciAdi.Text = frmGiris.kullanici_bilgi;

        }
        private void btnDegistir_Click(object sender, EventArgs e)
        {
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = frmGiris.Database;

            try
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                    cnn.Open();

                }
                else
                {
                    cnn.Open();
                }
                SqlParameter prm1 = new SqlParameter("@Prm1", lblKullaniciAdi.Text);
                SqlParameter prm2 = new SqlParameter("@Prm2", txtEskiSifre.Text);
                string sql = "";
                sql = "SELECT * FROM Kullanicilar WHERE KullaniciAdi = @Prm1 and KullaniciSifre = @Prm2";
                SqlCommand cmd = new SqlCommand(sql, cnn);

                cmd.Parameters.Add(prm1);
                cmd.Parameters.Add(prm2);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                cnn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (txtYeniSifre1.Text == txtYeniSifre2.Text && txtYeniSifre1.Text!="")
                    {
                        SqlConnection sqlConn = new SqlConnection();
                        sqlConn.ConnectionString = frmGiris.Database;
                        sqlConn.Open();

                        SqlCommand sqCom = new SqlCommand();
                        sqCom.Connection = sqlConn;
                        sqCom.CommandText = "UPDATE kullanicilar SET KullaniciSifre = @KullaniciSifre WHERE KullaniciID=" + frmGiris.gonderilecekveri2;

                        sqCom.Parameters.Add("@KullaniciSifre", SqlDbType.Int);
                        sqCom.Parameters["@KullaniciSifre"].Value = Convert.ToInt32(txtYeniSifre2.Text);
                        sqCom.ExecuteNonQuery();
                        sqlConn.Close();

                        MessageBox.Show("Şifreniz Değiştirildi");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Eski Şifre yanlış veya yeni şifre yazmadınız.");

                    }
                }

            }
            catch 
            {
                MessageBox.Show("Eski Şifre yanlış veya yeni şifre yazmadınız.");

            }

        }

        private void btnVazgeç_Click(object sender, EventArgs e)
        {
            this.Close();//direk şifre değişikliği iptal olacak. ve form kapanacak
        }
    }
}
