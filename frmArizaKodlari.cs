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
    public partial class frmArizaKodlari : Form
    {
        public frmArizaKodlari()
        {
            InitializeComponent();
        }

        private void frmArizaKodlari_Load(object sender, EventArgs e)
        {
            veriListele();

        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (btnKaydet.Text == "KAYDET" && txtArizaKodu.Text !="" && cmbBolum.SelectedIndex != -1)
            {
                SqlConnection sqlCon = new SqlConnection();
                sqlCon.ConnectionString = frmGiris.Database;

                sqlCon.Open();

                SqlCommand sqCom = new SqlCommand();
                sqCom.Connection = sqlCon;

                sqCom.CommandText = "INSERT INTO bakArizaKodlari (ArizaKodu,Tur,Pasif) VALUES (@ArizaKodu, @Tur, @Pasif) ";

                //Parametre tanımlama
                sqCom.Parameters.Add("@ArizaKodu", SqlDbType.NVarChar);
                sqCom.Parameters["@ArizaKodu"].Value = txtArizaKodu.Text;

                sqCom.Parameters.Add("@Tur", SqlDbType.Int);
                sqCom.Parameters["@Tur"].Value = Convert.ToInt32(cmbBolum.SelectedValue);

                sqCom.Parameters.Add("@Pasif", SqlDbType.Bit);
                sqCom.Parameters["@Pasif"].Value = chkPasifMi.Checked;

                sqCom.ExecuteNonQuery();
                sqlCon.Close();
                
                veriListele();
                ArizaKoduID.Text = "";
                txtArizaKodu.Text = "";
                cmbBolum.SelectedIndex = -1;
                chkPasifMi.Checked = false;
                MessageBox.Show("Yeni Kayıt eklendi");

            }
            else if (btnKaydet.Text == "GÜNCELLE")
            {
                if (btnKaydet.Text == "GÜNCELLE" && txtArizaKodu.Text != "" && cmbBolum.SelectedIndex != -1)
                {
                    if (!String.IsNullOrEmpty(ArizaKoduID.Text))
                    {
                        SqlConnection sqlConn = new SqlConnection();
                        sqlConn.ConnectionString = frmGiris.Database;
                        sqlConn.Open();

                        SqlCommand sqCom = new SqlCommand();
                        sqCom.Connection = sqlConn;

                        sqCom.CommandText = "UPDATE bakArizaKodlari SET ArizaKodu = @ArizaKodu, Tur = @Tur, Pasif = @Pasif WHERE ArizaKoduID =" + ArizaKoduID.Text;

                        sqCom.Parameters.Add("@ArizaKodu", SqlDbType.NVarChar);
                        sqCom.Parameters["@ArizaKodu"].Value = txtArizaKodu.Text;

                        sqCom.Parameters.Add("@Tur", SqlDbType.Int);
                        sqCom.Parameters["@Tur"].Value = Convert.ToInt32(cmbBolum.SelectedValue);

                        sqCom.Parameters.Add("@Pasif", SqlDbType.Bit);
                        sqCom.Parameters["@Pasif"].Value = chkPasifMi.Checked;

                        sqCom.ExecuteNonQuery();
                        sqlConn.Close();

                        MessageBox.Show("Kayıt Güncellendi");
                    }

                    veriListele();
                    ArizaKoduID.Text = "";
                    txtArizaKodu.Text = "";
                    cmbBolum.SelectedIndex = -1;
                    chkPasifMi.Checked = false;

                    btnKaydet.Text = "KAYDET";

                    
                }
                else
                {
                    MessageBox.Show("Tüm Bilgileri Doldurunuz.");
                }
            }

        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            veriListele();
            ArizaKoduID.Text = "";
            txtArizaKodu.Text = "";
            cmbBolum.SelectedIndex = -1;
            chkPasifMi.Checked = false;

            btnKaydet.Enabled = true;
            btnKaydet.Text = "KAYDET";

        }

        private void veriListele()
        {
            SqlConnection sqlConn = new SqlConnection();
            sqlConn.ConnectionString = frmGiris.Database;
            sqlConn.Open();

            SqlCommand sqCom = new SqlCommand();
            sqCom.Connection = sqlConn;
            

            sqCom.CommandText = "SELECT Tbl1.ArizaKoduID, Tbl1.ArizaKodu, Tbl2.Turu, Tbl1.Pasif FROM bakArizaKodlari Tbl1 LEFT JOIN bakTur Tbl2 ON Tbl1.Tur = Tbl2.TurID";
            sqCom.ExecuteNonQuery();

            DataTable dtProd = new DataTable();
            SqlDataAdapter sqDa = new SqlDataAdapter();
            sqDa.SelectCommand = sqCom;
            sqlConn.Close();
            sqDa.Fill(dtProd);

            dataGridView1.DataSource = dtProd; // veriyi aktarmıs olduk

            SqlConnection conn = new SqlConnection(frmGiris.Database);
            conn.Open();
            SqlCommand sc = new SqlCommand("SELECT * FROM bakTur", conn);
            SqlDataReader reader;

            reader = sc.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("TurID", typeof(string));
            dt.Columns.Add("Turu", typeof(string));
            dt.Load(reader);
            cmbBolum.ValueMember = "TurId";
            cmbBolum.DisplayMember = "Turu";
            cmbBolum.DataSource = dt;
            cmbBolum.SelectedIndex = -1;

            conn.Close();



        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ArizaKoduID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

                if (dataGridView1.Rows.Count > 0)
                {
                    SqlConnection sqlConn = new SqlConnection();
                    sqlConn.ConnectionString = frmGiris.Database;
                    sqlConn.Open();

                    SqlCommand sqCom = new SqlCommand();
                    sqCom.Connection = sqlConn;

                    sqCom.CommandText = "SELECT Tbl1.ArizaKoduId, Tbl1.ArizaKodu, Tbl2.Turu, Tbl1.Pasif FROM bakArizaKodlari Tbl1 LEFT JOIN bakTur Tbl2 ON Tbl1.Tur = Tbl2.TurID WHERE ArizaKoduID =" +ArizaKoduID.Text ;
                    sqCom.ExecuteNonQuery();

                    DataTable dtProd = new DataTable();
                    SqlDataAdapter sqDa = new SqlDataAdapter();
                    sqDa.SelectCommand = sqCom;
                    sqlConn.Close();
                    sqDa.Fill(dtProd);

                    txtArizaKodu.Text = dtProd.Rows[0]["ArizaKodu"].ToString();
                    cmbBolum.Text = dtProd.Rows[0]["Turu"].ToString();
                    chkPasifMi.Checked = Convert.ToBoolean(dtProd.Rows[0]["Pasif"]);
                    btnKaydet.Enabled = true;
                    btnKaydet.Text = "GÜNCELLE";



                }

            }
            catch
            {

            }
        }

       
    }
}
