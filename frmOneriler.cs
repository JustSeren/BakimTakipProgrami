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
    public partial class frmOneriler : Form
    {
        public frmOneriler()
        {
            InitializeComponent();
        }

        private void frmOneriler_Load(object sender, EventArgs e)
        {
            veriListele();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (btnKaydet.Text== "KAYDET" && cmbBolum.SelectedIndex != -1 && cmbArizaTipi.SelectedIndex != -1 && cmbPersonel.SelectedIndex != -1 && cmbVardiya.SelectedIndex != -1 && txtOneri.Text != "" )
            {
                SqlConnection sqlConn = new SqlConnection();
                sqlConn.ConnectionString = frmGiris.Database;
                sqlConn.Open();

                SqlCommand sqCom = new SqlCommand();
                sqCom.Connection = sqlConn;
                sqCom.CommandText = "INSERT INTO bakOneriler (Tarih, Bolum, Tur, Personel, Vardiya, Oneri, Pasif) VALUES (@Tarih, @Bolum, @Tur, @Personel, @Vardiya, @Oneri, @Pasif)";

                sqCom.Parameters.Add("@Tarih", SqlDbType.SmallDateTime);
                sqCom.Parameters["@Tarih"].Value = lblTarih.Text;
                
                sqCom.Parameters.Add("@Bolum", SqlDbType.Int);
                sqCom.Parameters["@Bolum"].Value = Convert.ToInt32(cmbBolum.SelectedValue);

                sqCom.Parameters.Add("@Tur", SqlDbType.Int);
                sqCom.Parameters["@Tur"].Value = Convert.ToInt32(cmbArizaTipi.SelectedValue);

                sqCom.Parameters.Add("@Personel", SqlDbType.Int);
                sqCom.Parameters["@Personel"].Value = Convert.ToInt32(cmbPersonel.SelectedValue);

                sqCom.Parameters.Add("@Vardiya", SqlDbType.Int);
                sqCom.Parameters["@Vardiya"].Value = Convert.ToInt32(cmbVardiya.SelectedValue);

                sqCom.Parameters.Add("@Oneri", SqlDbType.NVarChar);
                sqCom.Parameters["@Oneri"].Value = txtOneri.Text;

                sqCom.Parameters.Add("@Pasif", SqlDbType.Bit);
                sqCom.Parameters["@Pasif"].Value = false;

                sqCom.ExecuteNonQuery();
                sqlConn.Close();

                veriListele();
                cmbBolum.SelectedIndex = -1;
                cmbArizaTipi.SelectedIndex = -1;
                cmbPersonel.SelectedIndex = -1;
                cmbVardiya.SelectedIndex = -1;
                txtOneri.Text = null;
                MessageBox.Show("Yeni Kayıt Eklendi.");

            }
            else if (btnKaydet.Text=="GÜNCELLE")
            {
                if (btnKaydet.Text == "GÜNCELLE" && cmbBolum.SelectedIndex != -1 && cmbArizaTipi.SelectedIndex != -1 && cmbPersonel.SelectedIndex != -1 && cmbVardiya.SelectedIndex != -1 && txtOneri.Text != "")
                {
                    if (!String.IsNullOrEmpty(lblOneriID.Text))
                    {
                        SqlConnection sqlConn = new SqlConnection();
                        sqlConn.ConnectionString = frmGiris.Database;
                        sqlConn.Open();

                        SqlCommand sqCom = new SqlCommand();
                        sqCom.Connection = sqlConn;
                        sqCom.CommandText = "UPDATE bakOneriler SET Tarih =m @Tarih , Bolum = @Bolum, Tur = @Tur, Personel = @Personel, Vardiya = @VArdiya, Oneri = @Oneri, pasif = @Pasif WHERE OneriID=" + lblOneriID.Text;

                        sqCom.Parameters.Add("@Tarih", SqlDbType.SmallDateTime);
                        sqCom.Parameters["@Tarih"].Value = lblTarih.Text;

                        sqCom.Parameters.Add("@Bolum", SqlDbType.Int);
                        sqCom.Parameters["@Bolum"].Value = Convert.ToInt32(cmbBolum.SelectedValue);

                        sqCom.Parameters.Add("@Tur", SqlDbType.Int);
                        sqCom.Parameters["@Tur"].Value = Convert.ToInt32(cmbArizaTipi.SelectedValue);

                        sqCom.Parameters.Add("@Personel", SqlDbType.Int);
                        sqCom.Parameters["@Personel"].Value = Convert.ToInt32(cmbPersonel.SelectedValue);

                        sqCom.Parameters.Add("@Vardiya", SqlDbType.Int);
                        sqCom.Parameters["@Vardiya"].Value = Convert.ToInt32(cmbVardiya.SelectedValue);

                        sqCom.Parameters.Add("@Oneri", SqlDbType.NVarChar);
                        sqCom.Parameters["@Oneri"].Value = txtOneri.Text;

                        sqCom.Parameters.Add("@Pasif", SqlDbType.Bit);
                        sqCom.Parameters["@Pasif"].Value = false;
                       
                        sqCom.ExecuteNonQuery();
                        sqlConn.Close();

                        
                        MessageBox.Show("Kayıt Güncellendi.");
                    }
                    veriListele();
                    btnKaydet.Text = "KAYDET";


                }
                else
                {
                    MessageBox.Show("Lütfen Bilgileri Tam Doldurunuz.");
                }
            }
        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            veriListele();
            lblOneriID.Text = null;
            cmbBolum.SelectedIndex = -1;
            cmbArizaTipi.SelectedIndex = -1;
            cmbPersonel.SelectedIndex = -1;
            cmbVardiya.SelectedIndex = -1;
            txtOneri.Text = null;
            btnKaydet.Enabled = true;
            btnKaydet.Text = "KAYDET";

        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Seçili Olan Veri Silinecek\n Eminseniz Evet Butonuna Basınız.","DİKKAT!",MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                SqlConnection sqlConn = new SqlConnection();
                sqlConn.ConnectionString = frmGiris.Database;
                sqlConn.Open();
                
                SqlCommand sqCom = new SqlCommand();
                sqCom.Connection = sqlConn;

                sqCom.CommandText = "DELETE FROM bakOneriler WHERE OneriID =" + lblOneriID.Text;
                sqCom.ExecuteNonQuery();
                sqlConn.Close();
                MessageBox.Show("Veri Silindi.");
                veriListele();

            }
            else if (dialogResult == DialogResult.No)
            {

            }
        }

        private void veriListele()
        {
            SqlConnection sqlConn = new SqlConnection();
            sqlConn.ConnectionString = frmGiris.Database;
            sqlConn.Open();

            SqlCommand sqCom = new SqlCommand();
            sqCom.Connection = sqlConn;
            sqCom.CommandText = @"SELECT Tbl1.OneriID, Tbl1.Tarih, Tbl2.Bolum, Tbl3.Turu, Tbl4.PersonelAdi, Tbl5.Vardiya, Tbl1.Oneri 
              FROM  bakOneriler Tbl1 
              LEFT JOIN bakBolumler Tbl2 ON Tbl1.Bolum= Tbl2.BolumID
              LEFT JOIN bakTur Tbl3 ON Tbl1.Tur = Tbl3.TurID
              LEFT JOIN bakPersoneller Tbl4 ON Tbl1.Personel = Tbl4.PersonelID
              LEFT JOIN bakVardiyalar Tbl5 ON Tbl1.Vardiya = Tbl5.VardiyaID";
            sqCom.ExecuteNonQuery();
          
            DataTable dtProd = new DataTable();
            SqlDataAdapter sqDa = new SqlDataAdapter();
            sqDa.SelectCommand = sqCom;
            sqlConn.Close();
            sqDa.Fill(dtProd);

            dataGridView1.DataSource = dtProd;
            lblTarih.Text = DateTime.Now.ToString();
            //----
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
            
            //----
            SqlConnection conn2 = new SqlConnection(frmGiris.Database);
            conn2.Open();
            SqlCommand sc2 = new SqlCommand("SELECT * FROM bakTur",conn2);
            SqlDataReader reader2;
            reader2 = sc2.ExecuteReader();

            DataTable dt2 = new DataTable();
            dt2.Columns.Add("TurID", typeof(string));
            dt2.Columns.Add("Tur", typeof(string));
            dt2.Load(reader2);
            cmbArizaTipi.ValueMember = "TurID";
            cmbArizaTipi.DisplayMember = "Turu";
            cmbArizaTipi.DataSource = dt2;
            cmbArizaTipi.SelectedIndex = -1;
            conn2.Close();
            //---

            SqlConnection conn3 = new SqlConnection(frmGiris.Database);
            conn3.Open();
            SqlCommand sc3 = new SqlCommand("SELECT * FROM bakVardiyalar",conn3);
            SqlDataReader reader3;
            reader3 = sc3.ExecuteReader();

            DataTable dt3 = new DataTable();
            dt3.Columns.Add("VardiyaID", typeof(string));
            dt3.Columns.Add("Vardiya", typeof(string));
            dt3.Load(reader3);
            cmbVardiya.ValueMember = "VardiyaID";
            cmbVardiya.DisplayMember = "Vardiya";
            cmbVardiya.DataSource = dt3;
            cmbVardiya.SelectedIndex = -1;
            conn3.Close();
            //---

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                //bu dongu ıle verilere bakıyoruz ve veriler kadar sutunları genişletiyoruz.
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                int withCol = dataGridView1.Columns[i].Width;
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dataGridView1.Columns[i].Width = withCol;

            }

        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblOneriID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                if (dataGridView1.Rows.Count > 0)
                {
                    SqlConnection conn = new SqlConnection(frmGiris.Database);
                    conn.Open();
                    SqlCommand sc = new SqlCommand("SELECT * FROM bakPersoneller", conn);
                    SqlDataReader reader;
                    reader = sc.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("PersonelID", typeof(string));
                    dt.Columns.Add("PersonelAdi", typeof(string));
                    dt.Load(reader);

                    cmbPersonel.ValueMember = "PersonelID";
                    cmbPersonel.DisplayMember = "PersonelAdi";
                    cmbPersonel.DataSource = dt;
                    cmbPersonel.SelectedIndex = -1;

                    conn.Close();
                    //----
                    SqlConnection sqlConn = new SqlConnection();
                    sqlConn.ConnectionString = frmGiris.Database;
                    sqlConn.Open();

                    SqlCommand sqCom = new SqlCommand();
                    sqCom.Connection = sqlConn;
                    sqCom.CommandText = @"SELECT Tbl1.OneriID, Tbl1.Tarih, Tbl2.Bolum, Tbl3.Turu, Tbl4.PersonelAdi, Tbl5.Vardiya, Tbl1.Oneri 
              FROM  bakOneriler Tbl1 
              LEFT JOIN bakBolumler Tbl2 ON Tbl1.Bolum= Tbl2.BolumID
              LEFT JOIN bakTur Tbl3 ON Tbl1.Tur = Tbl3.TurID
              LEFT JOIN bakPersoneller Tbl4 ON Tbl1.Personel = Tbl4.PersonelID
              LEFT JOIN bakVardiyalar Tbl5 ON Vardiya = Tbl5.VardiyaID";

                    sqCom.ExecuteNonQuery();
                    DataTable dtProd = new DataTable();
                    SqlDataAdapter sqDa = new SqlDataAdapter();
                    sqDa.SelectCommand = sqCom;
                    sqlConn.Close();
                    sqDa.Fill(dtProd);

                    lblTarih.Text = dtProd.Rows[0]["Tarih"].ToString();
                    cmbBolum.Text = dtProd.Rows[0]["Bolum"].ToString();
                    cmbArizaTipi.Text = dtProd.Rows[0]["ArzaTipi"].ToString();
                    cmbPersonel.Text = dtProd.Rows[0]["Personel"].ToString();
                    cmbVardiya.Text = dtProd.Rows[0]["Vardiya"].ToString();
                    txtOneri.Text = dtProd.Rows[0]["Oneri"].ToString();

                    btnKaydet.Enabled = true;
                    btnKaydet.Text = "GÜNCELLE";
                    btnSil.Enabled = true;
                }
            }
            catch 
            {
            }
        }

        private void loadData(string kisim)
        {
            SqlConnection conn = new SqlConnection(frmGiris.Database);
            conn.Open();
            SqlCommand sc = new SqlCommand("SELECT * FROM bakPersoneller WHERE Pasif = 0 AND Tur=" + kisim + "ORDER BY PersonelAdi ASC", conn);

            SqlDataReader reader;
            reader = sc.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Columns.Add("PersonelID", typeof(string));
            dt.Columns.Add("PersonelAdi", typeof(string));
            dt.Load(reader);

            cmbPersonel.ValueMember = "PersonelID";
            cmbPersonel.DisplayMember = "PersonelAdi";
            cmbPersonel.DataSource = dt;
            cmbPersonel.SelectedIndex = -1;
            conn.Close();


        }

        private void arizaTipiYukle()
        {
            switch (cmbArizaTipi.Text)
            {
                case "ELEKTRİK":
                    loadData("1");
                    break;
                case "MEKANİK":
                    loadData("2");
                    break;

                default:
                    break;
            }
        }

        private void cmbArizaTipi_TextChanged(object sender, EventArgs e)
        {
            arizaTipiYukle();

        }

        
    }
}
