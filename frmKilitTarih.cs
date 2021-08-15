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
    public partial class frmKilitTarih : Form
    {
        public frmKilitTarih()
        {
            InitializeComponent();
        }

        private void frmKilitTarih_Load(object sender, EventArgs e)
        {
            SqlConnection sqlConn = new SqlConnection();
            sqlConn.ConnectionString = frmGiris.Database;
            sqlConn.Open();

            SqlCommand sqCom = new SqlCommand();
            sqCom.Connection = sqlConn;
            
            sqCom.CommandText = "SELECT AyarID, KilitTarihi FROM bakAyarlar WHERE AyarID = 1";
            sqCom.ExecuteNonQuery();

            DataTable dtProd = new DataTable();
            SqlDataAdapter sqDa = new SqlDataAdapter();
            sqDa.SelectCommand = sqCom;
            sqlConn.Close();
            sqDa.Fill(dtProd);
            dateTimePicker1.Value = Convert.ToDateTime(dtProd.Rows[0]["KilitTarihi"]);

        }

        private void btnKilit_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("Tarih Güncellenecek. Bu tarihten öncesine Güncelleme Yapılmayacak.\nEminseniz Evet Butonuna Basınız.", "DİKKAT!", MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                SqlConnection sqlConn = new SqlConnection();
                sqlConn.ConnectionString = frmGiris.Database;
                sqlConn.Open();

                SqlCommand sqCom = new SqlCommand();
                sqCom.Connection = sqlConn;
                sqCom.CommandText = "UPDATE bakAyarlar SET KilitTarihi = @KilitTarihi WHERE AyarID = 1";

                sqCom.Parameters.Add("@KiilitTarihi", SqlDbType.Date);
                sqCom.Parameters["@KilitTarihi"].Value = dateTimePicker1.Value;
                sqCom.ExecuteNonQuery();

                sqlConn.Close();
                MessageBox.Show("Kayıt Güncellendi.");


            }
            else if (dialogResult == DialogResult.No)
            {

            }

        }
    }
}
