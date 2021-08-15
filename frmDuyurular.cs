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
    public partial class frmDuyurular : Form
    {
        public frmDuyurular()
        {
            InitializeComponent();
        }

        private void frmDuyurular_Load(object sender, EventArgs e)
        {
            SqlConnection sqlConn = new SqlConnection();
            sqlConn.ConnectionString = frmGiris.Database;
            sqlConn.Open();

            SqlCommand sqCom = new SqlCommand();
            sqCom.Connection = sqlConn;
            sqCom.CommandText = "SELECT Duyuru,Yapilacak1,Yapilacak2,Yapilacak3,Yapilacak4,Yapilacak5,Yapilacak6,Yapilacak7,Yapilacak8,Yapilacak9,Yapilacak10 FROM bakDuyurular";
            
            sqCom.ExecuteNonQuery();
            
            DataTable dtProd = new DataTable();
            SqlDataAdapter sqDa = new SqlDataAdapter();
            sqDa.SelectCommand = sqCom;
            sqlConn.Close();
            sqDa.Fill(dtProd);
            txtSıfır.Text = dtProd.Rows[0]["Duyuru"].ToString();
            txtBir.Text= dtProd.Rows[0]["Yapilacak1"].ToString();
            txtIki.Text= dtProd.Rows[0]["Yapilacak2"].ToString();
            txtUc.Text= dtProd.Rows[0]["Yapilacak3"].ToString();
            txtDort.Text= dtProd.Rows[0]["Yapilacak4"].ToString();
            txtBes.Text= dtProd.Rows[0]["Yapilacak5"].ToString();
            txtAltı.Text= dtProd.Rows[0]["Yapilacak6"].ToString();
            txtYedi.Text= dtProd.Rows[0]["Yapilacak7"].ToString();
            txtSekiz.Text= dtProd.Rows[0]["Yapilacak8"].ToString();
            txtDokuz.Text= dtProd.Rows[0]["Yapilacak9"].ToString();
            txtOn.Text= dtProd.Rows[0]["Yapilacak10"].ToString();

            txtSıfır.Focus(); // bu form yuklendıgınde imlecin txtSıfıra odaklanmasını sağlıyor.

        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            SqlConnection sqlConn = new SqlConnection();
            sqlConn.ConnectionString = frmGiris.Database;
            sqlConn.Open();

            SqlCommand sqCom = new SqlCommand();
            sqCom.Connection = sqlConn;
            sqCom.CommandText = "UPDATE bakDuyurular SET Duyuru = @Duyuru, Yapilacak1 = @Yapilacak1, Yapilacak2 = @Yapilacak2, Yapilacak3 = @Yapilacak3, Yapilacak4 = @Yapilacak4, Yapilacak5 = @Yapilacak5, Yapilacak6 = @Yapilacak6, Yapilacak7 = @Yapilacak7, Yapilacak8 = @Yapilacak8, Yapilacak9 = @Yapilacak9, Yapilacak10 = @Yapilacak10 WHERE DuyuruID = 1 ";

            sqCom.Parameters.Add("@Duyuru", SqlDbType.NVarChar);
            sqCom.Parameters["@Duyuru"].Value = txtSıfır.Text;
          
            sqCom.Parameters.Add("@Yapilacak1", SqlDbType.NVarChar);
            sqCom.Parameters["@Yapilacak1"].Value = txtBir.Text;
           
            sqCom.Parameters.Add("@Yapilacak2", SqlDbType.NVarChar);
            sqCom.Parameters["@Yapilacak2"].Value = txtIki.Text;
            
            sqCom.Parameters.Add("@Yapilacak3", SqlDbType.NVarChar);
            sqCom.Parameters["@Yapilacak3"].Value = txtUc.Text;
            
            sqCom.Parameters.Add("@Yapilacak4", SqlDbType.NVarChar);
            sqCom.Parameters["@Yapilacak4"].Value = txtDort.Text;
            
            sqCom.Parameters.Add("@Yapilacak5", SqlDbType.NVarChar);
            sqCom.Parameters["@Yapilacak5"].Value = txtBes.Text;
            
            sqCom.Parameters.Add("@Yapilacak6", SqlDbType.NVarChar);
            sqCom.Parameters["@Yapilacak6"].Value = txtAltı.Text;
            
            sqCom.Parameters.Add("@Yapilacak7", SqlDbType.NVarChar);
            sqCom.Parameters["@Yapilacak7"].Value = txtYedi.Text;
            
            sqCom.Parameters.Add("@Yapilacak8", SqlDbType.NVarChar);
            sqCom.Parameters["@Yapilacak8"].Value = txtSekiz.Text;
            
            sqCom.Parameters.Add("@Yapilacak9", SqlDbType.NVarChar);
            sqCom.Parameters["@Yapilacak9"].Value = txtDokuz.Text;
            
            sqCom.Parameters.Add("@Yapilacak10", SqlDbType.NVarChar);
            sqCom.Parameters["@Yapilacak10"].Value = txtOn.Text;

            sqCom.ExecuteNonQuery();

            sqlConn.Close();
            MessageBox.Show("Duyuru Güncellendi");

            this.Close();

        }

        
    }
}
