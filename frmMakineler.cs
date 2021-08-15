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
    public partial class frmMakineler : Form
    {
        public frmMakineler()
        {
            InitializeComponent();
        }

        private void frmMakineler_Load(object sender, EventArgs e)
        {
            veriListele();
        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            veriListele();
            lblMakineID.Text = "";
            txtMakineAdi.Text = "";
            cmbBolum.SelectedIndex = -1;

            btnKaydet.Enabled = true;
            btnKaydet.Text = "KAYDET";
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (btnKaydet.Text == "KAYDET" && txtMakineAdi.Text != "" && cmbBolum.SelectedIndex !=-1)
            {
                SqlConnection sqlCon = new SqlConnection();
                sqlCon.ConnectionString = frmGiris.Database;

                sqlCon.Open();

                SqlCommand sqCom = new SqlCommand();
                sqCom.Connection = sqlCon;

                sqCom.CommandText = "INSERT INTO bakMakineler (MakineAdi,BolumID,Secenek) VALUES (@MakineAdi, @BolumID, @Secenek)";

                sqCom.Parameters.Add("@MakineAdi", SqlDbType.NVarChar);
                sqCom.Parameters["@MakineAdi"].Value = txtMakineAdi.Text;

                sqCom.Parameters.Add("@BolumID", SqlDbType.Int);
                sqCom.Parameters["@BolumID"].Value = Convert.ToInt32(cmbBolum.SelectedValue);

                sqCom.Parameters.Add("@Secenek", SqlDbType.Bit);
                sqCom.Parameters["@Secenek"].Value = chkSecenek.Checked;

                sqCom.ExecuteNonQuery();
                sqlCon.Close();

                veriListele();
                lblMakineID.Text = "";
                txtMakineAdi.Text = "";
                cmbBolum.SelectedIndex = -1;
                chkSecenek.Checked = false;
                MessageBox.Show("Yeni Kayıt EKlendi");

            }
            else if (btnKaydet.Text == "GÜNCELLE")
            {
                if (btnKaydet.Text == "GÜNCELLE" && txtMakineAdi.Text != "" && cmbBolum.SelectedIndex != -1)
                {
                    if (!String.IsNullOrEmpty(lblMakineID.Text))
                    {
                        SqlConnection sqlConn = new SqlConnection();
                        sqlConn.ConnectionString = frmGiris.Database;
                        sqlConn.Open();

                        SqlCommand sqCom = new SqlCommand();
                        sqCom.Connection = sqlConn;

                        sqCom.CommandText = "UPDATE bakMakineler SET MakineAdi = @MakineAdi, BolumID = @BolumID, Secenek =@Secenek WHERE MakineID=" + lblMakineID.Text;

                        sqCom.Parameters.Add("@MakineAdi", SqlDbType.NVarChar);
                        sqCom.Parameters["@MakineAdi"].Value = txtMakineAdi.Text;


                        sqCom.Parameters.Add("@BolumID", SqlDbType.Int);
                        sqCom.Parameters["@BolumID"].Value = Convert.ToInt32(cmbBolum.SelectedValue);

                        sqCom.Parameters.Add("@Secenek", SqlDbType.Bit);
                        sqCom.Parameters["@Secenek"].Value = chkSecenek.Checked;

                        sqCom.ExecuteNonQuery();
                        sqlConn.Close();

                        MessageBox.Show("Kayıt Güncellendi");

                    }
                    veriListele();
                    lblMakineID.Text = "";
                    txtMakineAdi.Text = "";
                    cmbBolum.SelectedIndex = -1;
                    chkSecenek.Checked = false;

                    btnKaydet.Text = "KAYDET";
                }
                else
                {
                    MessageBox.Show("Tüm bilgileri doldurun");

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

            sqCom.CommandText = "SELECT Tbl1.MakineID, Tbl1.MakineAdi, Tbl2.Bolum, Tbl1.Secenek FROM bakMakineler Tbl1 LEFT JOIN bakBolumler Tbl2 ON Tbl1.BolumID = Tbl2.BolumID";
            sqCom.ExecuteNonQuery();

            DataTable dtProd = new DataTable();
            SqlDataAdapter sqDa = new SqlDataAdapter();
            sqDa.SelectCommand = sqCom;
            sqlConn.Close();
            sqDa.Fill(dtProd);

            dataGridView1.DataSource = dtProd;

            SqlConnection conn = new SqlConnection(frmGiris.Database);
            conn.Open();
            SqlCommand sc = new SqlCommand("SELECT * FROM bakBolumler", conn);
            SqlDataReader reader;

            reader = sc.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("BolumID", typeof(string));
            dt.Columns.Add("Bolum", typeof(string));
            dt.Load(reader);
            cmbBolum.ValueMember = "BolumID";
            cmbBolum.DisplayMember = "Bolum";
            cmbBolum.DataSource = dt;
            cmbBolum.SelectedIndex = -1;

            conn.Close();


        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblMakineID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                if (dataGridView1.Rows.Count > 0 )
                {
                    SqlConnection sqlConn = new SqlConnection();
                    sqlConn.ConnectionString = frmGiris.Database;
                    sqlConn.Open();

                    SqlCommand sqCom = new SqlCommand();
                    sqCom.Connection = sqlConn;

                    sqCom.CommandText = "SELECT Tbl1.MakineID, Tbl1.MakineAdi, Tbl2.Bolum, Tbl1.Secenek FROM bakMakineler Tbl1 LEFT JOIN bakBolumler Tbl2 ON Tbl1.BolumID = Tbl2.BolumId WHERE MakineID=" + lblMakineID.Text;
                    sqCom.ExecuteNonQuery();

                    DataTable dtProd = new DataTable();
                    SqlDataAdapter sqDa = new SqlDataAdapter();
                    sqDa.SelectCommand = sqCom;
                    sqlConn.Close();
                    sqDa.Fill(dtProd);

                    txtMakineAdi.Text = dtProd.Rows[0]["MakineAdi"].ToString();
                    cmbBolum.Text = dtProd.Rows[0]["Bolum"].ToString();
                    chkSecenek.Checked = Convert.ToBoolean(dtProd.Rows[0]["Secenek"]);
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
