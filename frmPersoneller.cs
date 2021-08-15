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
    public partial class frmPersoneller : Form
    {
        public frmPersoneller()
        {
            InitializeComponent();
        }
        public string kullanicibilgi;

        private void frmPersoneller_Load(object sender, EventArgs e)
        {
            kullanicibilgi = frmGiris.kullanici_bilgi;
            verListeler();

        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (btnKaydet.Text=="KAYDET" && txtPersonelAdi.Text != "" && cmbBolum.SelectedIndex != -1)
            {
                SqlConnection sqlConn = new SqlConnection();
                sqlConn.ConnectionString = frmGiris.Database;
                sqlConn.Open();
                SqlCommand sqcom = new SqlCommand();
                sqcom.Connection = sqlConn;
                sqcom.CommandText = "INSERT INTO bakPersoneller (PersonelAdi,Tur, Pasif, Kullanici) VALUES (@PersonelAdi, @Tur, @Pasif, @Kullanici)";

                sqcom.Parameters.Add("@PersonelAdi", SqlDbType.NVarChar);
                sqcom.Parameters["@PersonelAdi"].Value = txtPersonelAdi.Text;
                
                sqcom.Parameters.Add("@Tur", SqlDbType.Int);
                sqcom.Parameters["@Tur"].Value = Convert.ToInt32(cmbBolum.SelectedValue);

                sqcom.Parameters.Add("@Pasif", SqlDbType.Bit);
                sqcom.Parameters["@Pasif"].Value = chkPasifMi.Checked;

                sqcom.Parameters.Add("@Kullanici", SqlDbType.NVarChar);
                sqcom.Parameters["@Kullanici"].Value = kullanicibilgi;

                sqcom.ExecuteNonQuery();
                sqlConn.Close();
                verListeler();
                Personelid.Text = "";
                txtPersonelAdi.Text = "";
                cmbBolum.SelectedIndex = -1;
                chkPasifMi.Checked = false;
                MessageBox.Show("Yeni kayıt eklendi.");

            }
            else if(btnKaydet.Text == "GÜNCELLE")
            {
                if (btnKaydet.Text== "GÜNCELLE" && txtPersonelAdi.Text != "" && cmbBolum.SelectedIndex != -1)
                {
                    if (!String.IsNullOrEmpty(Personelid.Text))
                    {
                        SqlConnection sqlConn = new SqlConnection();
                        sqlConn.ConnectionString = frmGiris.Database;
                        sqlConn.Open();
                        
                        SqlCommand sqCom = new SqlCommand();
                        sqCom.Connection = sqlConn;

                        sqCom.CommandText = "UPDATE bakPersoneller SET PersonelAdi = @PersonelAdi, Tur = @Tur, Pasif= @Pasif, Kullanici = @Kullanici WHERE PersonelID= " + Personelid.Text;
                        
                        sqCom.Parameters.Add("@PersonelAdi", SqlDbType.NVarChar);
                        sqCom.Parameters["@PersonelAdi"].Value = txtPersonelAdi.Text;

                        sqCom.Parameters.Add("@Tur", SqlDbType.Int);
                        sqCom.Parameters["@Tur"].Value = Convert.ToInt32(cmbBolum.SelectedValue);

                        sqCom.Parameters.Add("@Pasif",SqlDbType.Bit);
                        sqCom.Parameters["@Pasif"].Value = chkPasifMi.Checked;

                        sqCom.Parameters.Add("@Kullanici", SqlDbType.NVarChar);
                        sqCom.Parameters["@Kullanici"].Value = kullanicibilgi;

                        sqCom.ExecuteNonQuery();
                        sqlConn.Close();

                        MessageBox.Show("Kayıt Güncellendi");

                    }
                    verListeler();
                    Personelid.Text = "";
                    txtPersonelAdi.Text = "";
                    cmbBolum.SelectedIndex = -1;
                    chkPasifMi.Checked = false;
                    btnKaydet.Enabled = true;
                    btnKaydet.Text = "KAYDET";

                }
                else
                {
                    MessageBox.Show("Lütfen bilgileri tam doldurun");

                }
            }
        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            verListeler();

            Personelid.Text = "";
            txtPersonelAdi.Text = "";
            cmbBolum.SelectedIndex = -1;
            chkPasifMi.Checked = false;

            btnKaydet.Enabled = true;
            btnKaydet.Text = "KAYDET";

        }

        private void verListeler()
        {
            SqlConnection sqlCon = new SqlConnection();
            sqlCon.ConnectionString = frmGiris.Database;
            sqlCon.Open();

            SqlCommand sqCom = new SqlCommand();
            sqCom.Connection = sqlCon;

            sqCom.CommandText = "SELECT Tbl1.PersonelID,Tbl1.PersonelAdi, Tbl2.Turu, Tbl1.Pasif FROM bakPersoneller Tbl1 LEFT JOIN bakTur Tbl2 ON Tbl1.Tur=Tbl2.TurID ORDER BY Tbl1.Pasif,Tbl2.Turu, Tbl1.PersonelAdi ";
            sqCom.ExecuteNonQuery();

            DataTable dtProd = new DataTable();
            SqlDataAdapter sqDA = new SqlDataAdapter();
            sqDA.SelectCommand = sqCom;
            sqlCon.Close();
            sqDA.Fill(dtProd);

            dataGridView1.DataSource = dtProd;
            SqlConnection conn = new SqlConnection(frmGiris.Database);
            conn.Open();
            SqlCommand sc = new SqlCommand("SELECT * FROM bakTur WHERE TurID<>3",conn);

            SqlDataReader reader;
            reader = sc.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("TurID", typeof(string));
            dt.Columns.Add("Turu", typeof(string));
            dt.Load(reader);

            cmbBolum.ValueMember = "TurID";
            cmbBolum.DisplayMember = "Turu";
            cmbBolum.DataSource = dt;
            cmbBolum.SelectedIndex = -1;

            conn.Close();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Personelid.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                if (dataGridView1.Rows.Count>0)
                {
                    SqlConnection sqlConn = new SqlConnection();
                    sqlConn.ConnectionString = frmGiris.Database;
                    sqlConn.Open();
                    
                    SqlCommand sqCom = new SqlCommand();
                    sqCom.Connection = sqlConn;
                    
                    sqCom.CommandText = "SELECT Tbl1.PersonelID, tbl1.PersonelAdi, Tbl2.Turu, Tbl1.Pasif FROM bakPersoneller Tbl1 LEFT JOIN bakTur Tbl2 ON Tbl1.Tur = Tbl2.TurID WHERE PersonelID =" + Personelid.Text ;

                    sqCom.ExecuteNonQuery();
                    DataTable dtProd = new DataTable();
                    SqlDataAdapter sqDa = new SqlDataAdapter();
                    sqDa.SelectCommand = sqCom;
                    sqlConn.Close();
                    sqDa.Fill(dtProd);

                    txtPersonelAdi.Text = dtProd.Rows[0]["PersonelAdi"].ToString();
                    cmbBolum.Text = dtProd.Rows[0]["Turu"].ToString();
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
