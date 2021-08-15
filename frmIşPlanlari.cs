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
    public partial class frmIşPlanlari : Form
    {
        public frmIşPlanlari()
        {
            InitializeComponent();
        }


        private void frmIşPlanlari_Load(object sender, EventArgs e)
        { 
            
            bolumYukle();
            islemYukle();

            txtIsDurumu.ScrollBars = ScrollBars.Vertical;
            txtIsTanim.ScrollBars = ScrollBars.Vertical;
            this.ActiveControl = dataGridView1;
            kullaniciAdiSoyadi = frmGiris.kullanici_bilgi;
            
            veriListele();
           

        }

        public string kullaniciAdiSoyadi;
        private void btnYeni_Click(object sender, EventArgs e)
        {
            lblIsID.Text = null;
            cmbBolum.SelectedIndex = -1;
            cmbIslemTur.SelectedIndex = -1;
            formNo.Value = 0;
            txtTakip.Text = null;
            txtIsTanim.Text = null;
            txtIsDurumu.Text = null;
            chkYapildi.Checked = false;
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.Value = DateTime.Today;

            btnKaydet.Enabled = true;
            btnKaydet.Text = "KAYDET";

        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (btnKaydet.Text == "KAYDET" && cmbBolum.SelectedIndex != -1 && cmbIslemTur.SelectedIndex != -1 && txtIsDurumu.Text != "" && txtIsTanim.Text != "" && txtTakip.Text != "" )
            {
                SqlConnection sqlConn = new SqlConnection();
                sqlConn.ConnectionString = frmGiris.Database;
                sqlConn.Open();

                SqlCommand sqCom = new SqlCommand();
                sqCom.Connection = sqlConn;
                sqCom.CommandText = "INSERT INTO bakIsPlanı (IsTanimi, Durumu, Turu, Bolum, FormSiraNo, TalepTarihi, IstenenTarih, TamamlanmaTarihi, TakipEdenler, Yapildi, Kullanici, DegisiklikTuru, IslemTarihi) VALUES (@IsTanimi, @Durumu, @Turu, @Bolum, @FormSiraNo, @TalepTarihi, @IstenenTarih, @TamamlanmaTarihi, @TakipEdenler, @Yapildi, @Kullanici, @DegisiklikTuru, @IslemTarihi);  SELECT SCOPE_IDENTITY()";

                sqCom.Parameters.Add("@IsTanimi", SqlDbType.NVarChar);
                sqCom.Parameters["@IsTanimi"].Value = txtIsTanim.Text;

                sqCom.Parameters.Add("@Durumu", SqlDbType.NVarChar);
                sqCom.Parameters["@Durumu"].Value = txtIsDurumu.Text;

                sqCom.Parameters.Add("@Turu", SqlDbType.Int);
                sqCom.Parameters["@Turu"].Value = Convert.ToInt32(cmbIslemTur.SelectedValue);

                sqCom.Parameters.Add("@Bolum", SqlDbType.Int);
                sqCom.Parameters["@Bolum"].Value = Convert.ToInt32(cmbBolum.SelectedValue);

                sqCom.Parameters.Add("@FormSiraNo", SqlDbType.Int);
                sqCom.Parameters["@FormSiraNo"].Value = formNo.Value;

                sqCom.Parameters.Add("@TalepTarihi", SqlDbType.Date);
                sqCom.Parameters["@TalepTarihi"].Value = dateTimePicker1.Value;
                
                sqCom.Parameters.Add("@IstenenTarih", SqlDbType.Date);
                sqCom.Parameters["@IstenenTarih"].Value = dateTimePicker2.Value;

                sqCom.Parameters.Add("@Kullanici", SqlDbType.NVarChar);
                sqCom.Parameters["@Kullanici"].Value = kullaniciAdiSoyadi;

                sqCom.Parameters.Add("@DegisiklikTuru", SqlDbType.NVarChar);
                sqCom.Parameters["@DegisiklikTuru"].Value = "KAYIT";

                sqCom.Parameters.Add("@IslemTarihi", SqlDbType.SmallDateTime);
                sqCom.Parameters["@IslemTarihi"].Value = DateTime.Now.ToString();
                
                sqCom.Parameters.Add("@Yapildi", SqlDbType.Bit);
                sqCom.Parameters["@Yapildi"].Value = chkYapildi.Checked;

                if (chkYapildi.Checked == true)
                {
                    sqCom.Parameters.Add("@TamamlanmaTarihi", SqlDbType.Date);
                    sqCom.Parameters["@TamamlanmaTarihi"].Value = DateTime.Now.ToLongDateString();
                }
                else
                {
                    sqCom.Parameters.Add("@TamamlanmaTarihi", SqlDbType.Date);
                    sqCom.Parameters["@TamamlanmaTarihi"].Value = DBNull.Value;
        
                }
                sqCom.Parameters.Add("@TakipEdenler", SqlDbType.NVarChar);
                sqCom.Parameters["@TakipEdenler"].Value = txtTakip.Text;

                var order_id = sqCom.ExecuteNonQuery();

                lblIsID.Text = order_id.ToString();
                sqlConn.Close();

                cmbBolum.SelectedIndex = -1;
                cmbIslemTur.SelectedIndex = -1;
                formNo.Value = 0;
                txtIsTanim.Text = null;
                txtIsDurumu.Text = null;
                chkYapildi.Checked = false;
                
                MessageBox.Show(lblIsID.Text + " Nolu İş ID ile\nYeni Kayıt Eklendi");
                lblIsID.Text = null;
                veriListele();
            }
            else if(btnKaydet.Text == "GÜNCELLE")
            {
                if (btnKaydet.Text == "GÜNCELLE" &&  cmbBolum.SelectedIndex != -1 && cmbIslemTur.SelectedIndex != -1 && txtIsDurumu.Text != "" &&  txtIsTanim.Text != "" && txtTakip.Text != "")
                   
                {
                    DialogResult dialogResult = MessageBox.Show("Seçili Olan Veri Güncellenecek\n Eminseniz Evet Butonuna basınız","DİKKAT!", MessageBoxButtons.YesNo);
                   
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (!String.IsNullOrEmpty(lblIsID.Text))
                        {
                            SqlConnection sqlConn = new SqlConnection();
                            sqlConn.ConnectionString = frmGiris.Database;
                            sqlConn.Open();

                            SqlCommand sqCom = new SqlCommand();
                            sqCom.Connection = sqlConn;
                            sqCom.CommandText = "UPDATE bakIsPlanı SET IsTanimi = @IsTanimi, Durumu = @Durumu, Turu=@Turu, Bolum = @Bolum, FormSiraNo = @FormSiraNo, TalepTarihi= @TalepTarihi, IstenenTarih = @IstenenTarih, TamamlanmaTarihi= @TamamlanmaTarihi, TakipEdenler = @TakipEdenler, Yapildi = @Yapildi, Kullanici=@Kullanici, DegisiklikTuru =@DegisiklikTuru, IslemTarihi=@IslemTarihi WHERE IsID = " + lblIsID.Text;

                            sqCom.Parameters.Add("@IsTanimi", SqlDbType.NVarChar);
                            sqCom.Parameters["@IsTanimi"].Value = txtIsTanim.Text;

                            sqCom.Parameters.Add("@Durumu", SqlDbType.NVarChar);
                            sqCom.Parameters["@Durumu"].Value = txtIsDurumu.Text;

                            sqCom.Parameters.Add("@Turu", SqlDbType.Int);
                            sqCom.Parameters["@Turu"].Value = Convert.ToInt32(cmbIslemTur.SelectedValue);

                            sqCom.Parameters.Add("@Bolum", SqlDbType.Int);
                            sqCom.Parameters["@Bolum"].Value = Convert.ToInt32(cmbBolum.SelectedValue);

                            sqCom.Parameters.Add("@FormSiraNo", SqlDbType.Int);
                            sqCom.Parameters["@FormSiraNo"].Value = formNo.Value;

                            sqCom.Parameters.Add("@TalepTarihi", SqlDbType.Date);
                            sqCom.Parameters["@TalepTarihi"].Value = dateTimePicker1.Value;

                            sqCom.Parameters.Add("@IstenenTarih", SqlDbType.Date);
                            sqCom.Parameters["@IstenenTarih"].Value = dateTimePicker2.Value;

                            sqCom.Parameters.Add("@Kullanici", SqlDbType.NVarChar);
                            sqCom.Parameters["@Kullanici"].Value = kullaniciAdiSoyadi;

                            sqCom.Parameters.Add("@DegisiklikTuru", SqlDbType.NVarChar);
                            sqCom.Parameters["@DegisiklikTuru"].Value = "DÜZELTME";

                            sqCom.Parameters.Add("@IslemTarihi", SqlDbType.SmallDateTime);
                            sqCom.Parameters["@IslemTarihi"].Value = DateTime.Now.ToString();

                            sqCom.Parameters.Add("@Yapildi", SqlDbType.Bit);
                            sqCom.Parameters["@Yapildi"].Value = chkYapildi.Checked;

                            

                            if (chkYapildi.Checked == true)
                            {
                                sqCom.Parameters.Add("@TamamlanmaTarihi", SqlDbType.Date);
                                sqCom.Parameters["@TamamlanmaTarihi"].Value = DateTime.Now.ToLongDateString();
                            }
                            else
                            {
                                sqCom.Parameters.Add("@TamamlanmaTarihi", SqlDbType.Date);
                                sqCom.Parameters["@TamamlanmaTarihi"].Value = DBNull.Value;

                            }
                            sqCom.Parameters.Add("@TakipEdenler", SqlDbType.NVarChar);
                            sqCom.Parameters["@TakipEdenler"].Value = txtTakip.Text;

                            sqCom.ExecuteNonQuery();
                            sqlConn.Close();
                            MessageBox.Show(lblIsID.Text + "IsID nolu\nKayıt Güncelllendi");
                            
                            veriListele();
                            
                            cmbBolum.SelectedIndex = -1;
                            cmbBolum.SelectedIndex = -1;
                            formNo.Value = 0;
                           
                            txtIsDurumu.Text = null;
                             txtIsTanim.Text = null;
                            lblIsID.Text = null;
                            chkYapildi.Checked = false;
                        }
                        btnKaydet.Text = "KAYDET";
                        
                    }
                    else if (dialogResult == DialogResult.No)
                    {

                    }
                    this.ActiveControl = dataGridView1;

                }
                else
                {
                    MessageBox.Show("Lütfen Bilgileri Tam Doldurunuz.");
                }
            }
            else
            {
                MessageBox.Show("Lütfen Bilgileri Tam Doldurunuz.");
            }

        }


        public void bolumYukle()
        {
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

        public void islemYukle()
        {
            SqlConnection conn = new SqlConnection(frmGiris.Database);
            conn.Open();

            SqlCommand sc = new SqlCommand("SELECT * FROM bakIslem WHERE IslemID = 2 OR IslemID = 3",conn);

            SqlDataReader reader;
            reader = sc.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Columns.Add("IslemID", typeof(string));
            dt.Columns.Add("IslemTuru", typeof(string));
            dt.Load(reader);

            cmbIslemTur.ValueMember = "IslemID";
            cmbIslemTur.DisplayMember = "IslemTuru";
            cmbIslemTur.DataSource = dt;
            cmbIslemTur.SelectedIndex = -1;

            conn.Close();

        }
        private void veriListele()
        {
            SqlConnection sqlConn = new SqlConnection();
            sqlConn.ConnectionString = frmGiris.Database;

            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Open();

            }
            else
            {

                sqlConn.Open();
            }

            SqlCommand sqCom = new SqlCommand();
            sqCom.Connection = sqlConn;
            sqCom.CommandText = @"SELECT Tbl1.IsID, Tbl1.IsTanimi, Tbl1.Durumu, Tbl3.IslemTuru, Tbl2.Bolum, Tbl1.FormSiraNo, Tbl1.TalepTarihi, Tbl1.IstenenTarih, Tbl1.TamamlanmaTarihi, Tbl1.TakipEdenler,Tbl1.Yapildi
              FROM bakIsPlanı Tbl1 
              LEFT JOIN bakBolumler Tbl2 ON Tbl1.Bolum = Tbl2.BolumID
              LEFT JOIN bakIslem Tbl3 ON Tbl1.Turu = Tbl3.IslemID
              WHERE Tbl1.Yapildi = 0
              ORDER BY Tbl1.IstenenTarih, Tbl1.IsID";

            SqlDataAdapter da = new SqlDataAdapter(sqCom);
            DataTable dtProd = new DataTable();
            da.Fill(dtProd);
            
            dataGridView1.DataSource = dtProd;
            sqlConn.Close();
            this.ActiveControl = dataGridView1;

            
            
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            
            try
            {
                lblIsID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                
                if (dataGridView1.Rows.Count > 0)
                   
                {
                    btnKaydet.Text = "GÜNCELLE";

                    SqlConnection sqlConn = new SqlConnection();
                    sqlConn.ConnectionString = frmGiris.Database;
                    sqlConn.Open();

                    SqlCommand sqCom = new SqlCommand();
                    sqCom.Connection = sqlConn;

                    sqCom.CommandText = @"SELECT Tbl1.IsID, Tbl1.IsTanimi, Tbl1.Durumu, Tbl3.IslemTuru, Tbl2.Bolum, Tbl1.FormSiraNo, Tbl1.TalepTarihi, Tbl1.IstenenTarih,Tbl.TamamlanmaTarihi, Tbl1.TakipEdenler, Tbl1.Yapildi
                      FROM bakIsPlanı Tbl1
                      LEFT JOIN bakBolumler Tbl2 ON Tbl1.Bolum = Tbl2.BolumID
                      LEFT JOIN bakIslem Tbl3 ON Tbl1.Turu = Tbl3.IslemID
                      WHERE Tbl1.IsID = " + lblIsID.Text;
                    sqCom.ExecuteNonQuery();

                    DataTable dt = new DataTable();
                    SqlDataAdapter sqDA = new SqlDataAdapter();
                    sqDA.SelectCommand = sqCom;
                    sqlConn.Close();
                    sqDA.Fill(dt);
                    
                    cmbIslemTur.Text = dt.Rows[0]["IslemTuru"].ToString();
                    cmbBolum.Text = dt.Rows[0]["Bolum"].ToString();
                    formNo.Value = Convert.ToInt32(dt.Rows[0]["FormSiraNo"]);
                    dateTimePicker1.Value = Convert.ToDateTime(dt.Rows[0]["TalepTarihi"]);
                    dateTimePicker2.Value = Convert.ToDateTime(dt.Rows[0]["IstenenTarih"]);
                    txtTakip.Text = dt.Rows[0]["TakipEdenler"].ToString();
                    string tamamlanmatarih = dt.Rows[0]["TamamlanmaTarihi"].ToString();
                    
                    if (!String.IsNullOrEmpty(tamamlanmatarih))
                    {
                        lblTamamlanmaTarihi.Text = tamamlanmatarih.Substring(0, 10);
                    }
                   
                    txtIsTanim.Text = dt.Rows[0]["IsTanimi"].ToString();
                    txtIsDurumu.Text = dt.Rows[0]["Durumu"].ToString();
                    chkYapildi.Checked = Convert.ToBoolean(dt.Rows[0]["Yapildi"]);
                    txtTakip.Text = dt.Rows[0]["TakipEdenler"].ToString();

                    sqCom.ExecuteNonQuery();

                } 
            }
            catch 
            {
                
                
            }
        }

        
    }
}
