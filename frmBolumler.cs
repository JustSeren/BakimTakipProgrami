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
    public partial class frmBolumler : Form
    {
        public frmBolumler()
        {
            InitializeComponent();
        }

        private void frmBolumler_Load(object sender, EventArgs e)
        {
            veriListele();
        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            veriListele();
            lblBolumID.Text = "";
            txtBolumAdi.Text = "";
            chkPasifMi.Checked = false;

            btnKaydet.Enabled = true;
            btnKaydet.Text = "KAYDET";
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (btnKaydet.Text == "KAYDET" && txtBolumAdi.Text != "" )
            {
                SqlConnection sqlConn = new SqlConnection();
                sqlConn.ConnectionString = frmGiris.Database;

                sqlConn.Open();

                SqlCommand sqCom = new SqlCommand();
                sqCom.Connection = sqlConn;

                sqCom.CommandText = "INSERT INTO bakBolumler (Bolum, Pasif) VALUES (@Bolum, @Pasif) ";

                sqCom.Parameters.Add("@Bolum", SqlDbType.NVarChar);
                sqCom.Parameters["@Bolum"].Value = txtBolumAdi.Text;

                sqCom.Parameters.Add("@Pasif", SqlDbType.Bit);
                sqCom.Parameters["@Pasif"].Value = chkPasifMi.Checked;

                sqCom.ExecuteNonQuery();
                sqlConn.Close();

                veriListele();
                lblBolumID.Text = "";
                txtBolumAdi.Text = "";
                chkPasifMi.Checked = false;

                MessageBox.Show("Yeni Kayıt Eklendi");

            }
            else if (btnKaydet.Text == "GÜNCELLE")
            {
                if (btnKaydet.Text == "GÜNCELLE" && txtBolumAdi.Text != "")
                {
                    if (!String.IsNullOrEmpty(lblBolumID.Text))
                    {
                        SqlConnection sqlConn = new SqlConnection();
                        sqlConn.ConnectionString = frmGiris.Database;
                        sqlConn.Open();

                        SqlCommand sqCom = new SqlCommand();
                        sqCom.Connection = sqlConn;

                        sqCom.CommandText = "UPDATE bakBolumler SET Bolum = @Bolum, Pasif = @Pasif WHERE BolumID =" + lblBolumID.Text;

                        sqCom.Parameters.Add("@Bolum", SqlDbType.NVarChar);
                        sqCom.Parameters["@Bolum"].Value = txtBolumAdi.Text;

                        sqCom.Parameters.Add("@Pasif", SqlDbType.Bit);
                        sqCom.Parameters["@Pasif"].Value = chkPasifMi.Checked;

                        sqCom.ExecuteNonQuery();
                        sqlConn.Close();

                        MessageBox.Show("Kayıt Güncellendi");

                    }
                    veriListele(); 
                    lblBolumID.Text = "";
                    txtBolumAdi.Text = "";
                    chkPasifMi.Checked = false;

                    btnKaydet.Text = "KAYDET";

                }
                else
                {
                    MessageBox.Show("Tüm Bilgileri Doldurunuz.");
                }
            }
        }

        private void veriListele()
        {
            SqlConnection sqlConn = new SqlConnection();
            sqlConn.ConnectionString = frmGiris.Database;
            sqlConn.Open();

            SqlCommand sqCom = new SqlCommand();
            sqCom.Connection = sqlConn;

            sqCom.CommandText = "SELECT BolumID, Bolum, Pasif FROM bakBolumler";
            sqCom.ExecuteNonQuery();

            DataTable dtProd = new DataTable();
            SqlDataAdapter sqDa = new SqlDataAdapter();
            sqDa.SelectCommand = sqCom;
            sqlConn.Close();
            sqDa.Fill(dtProd);

            dataGridView1.DataSource = dtProd;

        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblBolumID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

                if (dataGridView1.Rows.Count > 0)
                {
                    SqlConnection sqlConn = new SqlConnection();
                    sqlConn.ConnectionString = frmGiris.Database;
                    sqlConn.Open();

                    SqlCommand sqCom = new SqlCommand();
                    sqCom.Connection = sqlConn;

                    sqCom.CommandText = "SELECT BolumID, Bolum, Pasif FROM bakBolumler WHERE BolumID=" + lblBolumID.Text;
                    sqCom.ExecuteNonQuery();

                    DataTable dtProd = new DataTable();
                    SqlDataAdapter sqDa = new SqlDataAdapter();
                    sqDa.SelectCommand = sqCom;
                    sqlConn.Close();
                    sqDa.Fill(dtProd);

                    txtBolumAdi.Text = dtProd.Rows[0]["@Bolum"].ToString();
                    chkPasifMi.Checked = Convert.ToBoolean(dtProd.Rows[0]["Pasif"]);
                    btnKaydet.Enabled = true;
                  
                   
                } 
                btnKaydet.Text = "GÜNCELLE";
            }
            catch 
            {
                
            }
        }
    }
}
