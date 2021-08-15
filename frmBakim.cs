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
    public partial class frmBakim : Form
    {
        public frmBakim()
        {
            InitializeComponent();
        }

        public static int yetkilendirme;
        public string metin;
        public DataTable dtTablom;
        public DataTable dtPerTablom;
        public DataTable dtArzTablom;
        public DataTable dtArizaTablom;
        public DataTable dtBolumTablom;
        public DataTable dtTurTablom;
        public DataTable dtVardiyaTablom;
        public DataTable dtIslemTablom;
        public DataTable dtMakinelerTablom;
        public static int grupYetki;
        public bool ilk = false;
        public bool ilk2 = false;
        public bool ilk3 = false;
        public bool personelSecim = false;
        public bool arizaKoduSecim = false;
        public bool guncellemeyeUygun = false;
        public bool checkedKontrol = false;
        public string sb1;
        public string sb2;
        public string sb3;
        public string sb4;
        public string kullaniciAdiSoyadi;
        public DateTime KT;
        public bool comboAcık = false;
        


        private void frmBakim_Load(object sender, EventArgs e)
        {
            comboAcık = true;
            kullaniciAdiSoyadi = frmGiris.kullanici_bilgi;
            grupYetki = Convert.ToInt32(frmGiris.gonderilecekyetli);

            if (grupYetki != 0)
            {
                lstArizaKodlari.Visible = true;

            }

            ilk = true;
            ilk2 = true;

            duyuruGoruntule();
            listeleTumu();

            if (ilk2 == true )
            {
                dtBolumTablom = null;
                dtBolumTablom = bolumYukle();
               
                dtTurTablom = null;
                dtTurTablom = turYukle();

                dtVardiyaTablom = null;
                dtVardiyaTablom = vardiyalarYukle();

                dtIslemTablom = null;
                dtIslemTablom = islemYukle();

                dtMakinelerTablom = null;
                dtMakinelerTablom = makineYukle();
                comboBoxYukle();

                ilk2 = false;

            }
            txtYapilanlar.ScrollBars = ScrollBars.Vertical;
            this.ActiveControl = dataGridView1;


        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            guncelTarih();
            lblMudahaleSuresi.Text = "00:00";
            lblArizaID.Text = null;
            cmbBolum.SelectedIndex = -1;
            cmbArizaTipi.SelectedIndex = -1;
            cmbVardiya.SelectedIndex = -1;
            cmbTuru.SelectedIndex = -1;
            cmbMakine.SelectedIndex = -1;
            nmDurus.Value = 0;
            txtYapilanlar.Text = null;
            chkCozuldu.Checked = false;

            btnKaydet.Enabled = true;
            btnKaydet.Text = "KAYDET";

            sonKayitlariUsteGetir();
            duyuruGoruntule();
            personelSeciliOlmasın();
            arizaKodlariSeciliOlmasın();
            lstPersonel.Visible = false;
            lstArizaKodlari.Visible = false;

            dtBaslamaSaati.Value = Convert.ToDateTime("00:00");
            dtBitisSaati.Value = Convert.ToDateTime("00:00");

            //btnYeni.Visible = false;

        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            string sure = Convert.ToString(Convert.ToDateTime(dtBitisSaati.Text) - Convert.ToDateTime(dtBaslamaSaati.Text));
            lblMudahaleSuresi.Text = sure.Substring(0, 5);

            personelSecili();
            arizaKoduSecili();

            StringBuilder eksikler = new StringBuilder();
            if (cmbTuru.SelectedIndex == -1)
            {
                eksikler.Append("Türü Seçmediniz.");
                eksikler.Append(Environment.NewLine);

            }
            if (cmbBolum.SelectedIndex == -1)
            {
                eksikler.Append("Bölüm seçmediniz.");
                eksikler.Append(Environment.NewLine);

            }
            if (cmbMakine.SelectedIndex == -1)
            {
                eksikler.Append("Makine Seçmediniz");
                eksikler.Append(Environment.NewLine);
            }
            if (cmbArizaTipi.SelectedIndex == -1)
            {
                eksikler.Append("Arıza Tipi seçmediniz.");
                eksikler.Append(Environment.NewLine);
            }
            if (cmbVardiya.SelectedIndex == -1)
            {
                eksikler.Append("Vardiya seçmediniz");
                eksikler.Append(Environment.NewLine);
            }
            if (Convert.ToDateTime(dtBitisSaati.Text) == Convert.ToDateTime(dtBaslamaSaati.Text))
            {
                eksikler.Append("Başlangıç ve bitiş saati belirtmediniz.");
                eksikler.Append(Environment.NewLine);
            }
            if (Convert.ToDateTime(dtBitisSaati.Value) < Convert.ToDateTime(dtBaslamaSaati.Value))
            {
                eksikler.Append("Başlama saati bitiş saatinden büyük olamaz.");
                eksikler.Append(Environment.NewLine);
            }
            if (personelSecim == false)
            {
                eksikler.Append("Personel Secmediniz");
                eksikler.Append(Environment.NewLine);
            }
            if (arizaKoduSecim == false)
            {
                eksikler.Append("Arıza Kodu secmediniz");
                eksikler.Append(Environment.NewLine);
            }
            if (txtYapilanlar.Text == "")
            {
                eksikler.Append("Yapılanları boş bıraktınız");
                eksikler.Append(Environment.NewLine);
            }
            if (btnKaydet.Text == "KAYDET" && cmbBolum.SelectedIndex != -1 && cmbArizaTipi.SelectedIndex != -1 && cmbMakine.SelectedIndex != -1 && cmbVardiya.SelectedIndex != -1 && cmbTuru.SelectedIndex != -1 && lblMudahaleSuresi.Text != "00:00" && txtYapilanlar.Text != "" && personelSecim != false && arizaKoduSecim != false && (Convert.ToDateTime(dtBitisSaati.Value ) >  Convert.ToDateTime(dtBaslamaSaati.Value)))
            {
                guncelTarih();
                SqlConnection sqlConn = new SqlConnection();
                sqlConn.ConnectionString = frmGiris.Database;
                sqlConn.Open();

                SqlCommand sqCom = new SqlCommand();
                sqCom.Connection = sqlConn;
                sqCom.CommandText = "INSERT INTO bakArizalar (Tarih, Islem, Bolum, Makine, Tur, Vardiya, BaslamaSaati, BitisSaati, Durus, ArizaKodu, Yapilanlar, Cozuldu, FormVar, Personel, Kullanici ) VALUES(@Tarih, @Islem, @Bolum, @Makine, @Tur, @Vardiya, @BaslamaSaati, @BitisSaati, @Durus, @ArizaKodu, @Yapilanlar, @Cozuldu, @FormVar, @Personel, @Kullanici ); SELECT SCOPE_IDENTITY()";

                string time3 = lblTarih.Text.ToString();
                var result3 = Convert.ToDateTime(time3);
                string test3 = result3.ToString("HH:mm", System.Globalization.CultureInfo.CurrentCulture);

                var alt0 = Convert.ToDateTime("00:00");
                string alt = alt0.ToString("HH:mm", System.Globalization.CultureInfo.CurrentCulture);
                var ust0 = Convert.ToDateTime("00:50");
                string ust = ust0.ToString("HH:mm", System.Globalization.CultureInfo.CurrentCulture);

                if (Convert.ToDateTime(test3) >= Convert.ToDateTime(alt) && Convert.ToDateTime(test3) < Convert.ToDateTime(ust) && cmbVardiya.SelectedIndex==1) 
                {
                    lblTarih.Text = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy 23:58:00");
                }

                sqCom.Parameters.Add("@Tarih", SqlDbType.SmallDateTime);
                sqCom.Parameters["@Tarih"].Value = lblTarih.Text;

                sqCom.Parameters.Add("@Islem", SqlDbType.Int);
                sqCom.Parameters["@Islem"].Value = Convert.ToInt32(cmbTuru.SelectedValue);

                sqCom.Parameters.Add("@Bolum", SqlDbType.Int);
                sqCom.Parameters["@Bolum"].Value = Convert.ToInt32(cmbBolum.SelectedValue);

                sqCom.Parameters.Add("@Makine", SqlDbType.Int);
                sqCom.Parameters["@Makine"].Value = Convert.ToInt32(cmbMakine.SelectedValue);

                sqCom.Parameters.Add("@Tur", SqlDbType.Int);
                sqCom.Parameters["@Tur"].Value = Convert.ToInt32(cmbArizaTipi.SelectedValue);

                sqCom.Parameters.Add("@Vardiya", SqlDbType.Int);
                sqCom.Parameters["@Vardiya"].Value = Convert.ToInt32(cmbVardiya.SelectedValue);

                string time = dtBaslamaSaati.Value.ToString();
                var result = Convert.ToDateTime(time);
                string test = result.ToString("HH:mm", System.Globalization.CultureInfo.InvariantCulture);

                sqCom.Parameters.Add("@BaslamaSaati", SqlDbType.Time);
                sqCom.Parameters["@BaslamaSaati"].Value = test.ToString();

                string time2 = dtBitisSaati.Value.ToString();
                var result2 = Convert.ToDateTime(time2);
                string test2 = result2.ToString("HH:mm", System.Globalization.CultureInfo.InvariantCulture);

                sqCom.Parameters.Add("@BitisSaati", SqlDbType.Time);
                sqCom.Parameters["@BitisSaati"].Value = test2.ToString();

                sqCom.Parameters.Add("@Durus", SqlDbType.Int);
                sqCom.Parameters["@Durus"].Value = nmDurus.Value;

                if (grupYetki != 0)
                {
                    arizaKodlariIDGuncelle();
                    sqCom.Parameters.Add("@ArizaKodu", SqlDbType.NVarChar);
                    sqCom.Parameters["@ArizaKodu"].Value = sb4.ToString();
                }

                sqCom.Parameters.Add("@Yapilanlar", SqlDbType.NVarChar);
                sqCom.Parameters["@Yapilanlar"].Value = txtYapilanlar .Text;

                sqCom.Parameters.Add("@Cozuldu", SqlDbType.Bit);
                sqCom.Parameters["@Cozuldu"].Value = chkCozuldu.Checked;

                sqCom.Parameters.Add("@FormVar", SqlDbType.Bit);
                sqCom.Parameters["@FormVar"].Value = 0;

                personelIDGuncelle();
                sqCom.Parameters.Add("@Personel", SqlDbType.NVarChar);
                sqCom.Parameters["@Personel"].Value = sb2.ToString();

                sqCom.Parameters.Add("@Kullanici", SqlDbType.NVarChar);
                sqCom.Parameters["@Kullanici"].Value = kullaniciAdiSoyadi;

                kullaniciAdiSoyadi = frmGiris.kullanici_bilgi;

                var order_id = sqCom.ExecuteScalar();
                lblArizaID.Text = order_id.ToString();
                sqlConn.Close();

                cmbBolum.SelectedIndex = -1;
                cmbArizaTipi.SelectedIndex = -1;
                cmbVardiya.SelectedIndex = -1;
                cmbTuru.SelectedIndex = -1;
                cmbMakine.SelectedIndex = -1;
                nmDurus.Value = 0;
                txtYapilanlar.Text = null;
                chkCozuldu.Checked = false;

                MessageBox.Show(order_id.ToString() + " nolu arıza ıd ile\nyeni kayıt eklendi");

                lblArizaID.Text = null;
                personelSeciliOlmasın();
                arizaKodlariSeciliOlmasın();
                sonKayitlariUsteGetir();
                duyuruGoruntule();
                listeleTumu();
                lstPersonel.Visible = false;
                this.ActiveControl = dataGridView1;

            }
            else if (btnKaydet.Text == "GÜNCELLE")
            {
                string bugün = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
                if (Convert.ToDateTime(lblTarih.Text) > Convert.ToDateTime(bugün) || checkedKontrol == false)
                {
                    guncellemeyeUygun = true;
                }
                else
                {
                    guncellemeyeUygun = false;

                }
                if (grupYetki != 0)
                {
                    kilitTarihKontrol();
                    if (Convert.ToDateTime(lblTarih.Text) > Convert.ToDateTime(KT) || checkedKontrol == false)
                    {
                        guncellemeyeUygun = true;
                    }
                }
                personelSecili();
                if (btnKaydet.Text == "GÜNCELLE" && cmbBolum.SelectedIndex != -1 && cmbArizaTipi.SelectedIndex != -1 && cmbMakine.SelectedIndex != -1 && cmbVardiya.SelectedIndex != -1 && cmbTuru.SelectedIndex != -1 && lblMudahaleSuresi.Text != "00:00" && txtYapilanlar.Text != "" && personelSecim != false && arizaKoduSecim != false && guncellemeyeUygun != false &&  (Convert.ToDateTime(dtBitisSaati.Value) > Convert.ToDateTime(dtBaslamaSaati.Value)))
                {
                    DialogResult dialogResult = MessageBox.Show("Seçili olan veri guncellenecek\nEminseniz Evete basınız", "DİKKAT", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (!String.IsNullOrEmpty(lblArizaID.Text))
                        {
                            SqlConnection sqlConn = new SqlConnection();
                            sqlConn.ConnectionString = frmGiris.Database;
                            sqlConn.Open();

                            SqlCommand sqCom = new SqlCommand();
                            sqCom.Connection = sqlConn;
                            sqCom.CommandText = "UPDATE bakArizalar SET Islem = @Islem, Bolum = @Bolum, Makine = @Makine, Tur = @Tur, Vardiya = @Vardiya, BaslamaSaati=@BaslamSaati, BitisSaati = @BitisSaati, Durus = @Durus, ArizaKodu = @ArizaKodu, Yapilanlar = @Yapilanlar, Cozuldu = @Cozuldu, Personel = @Personel, Kullanici = @Kullanici WHERE ArizaID = " + lblArizaID.Text;

                            sqCom.Parameters.Add("@Islem", SqlDbType.Int);
                            sqCom.Parameters["@Islem"].Value = Convert.ToInt32(cmbTuru.SelectedValue);

                            sqCom.Parameters.Add("@Bolum", SqlDbType.Int);
                            sqCom.Parameters["@Bolum"].Value = Convert.ToInt32(cmbBolum.SelectedValue);


                            sqCom.Parameters.Add("@Makine", SqlDbType.Int);
                            sqCom.Parameters["@Makine"].Value = Convert.ToInt32(cmbMakine.SelectedValue);

                            sqCom.Parameters.Add("@Tur", SqlDbType.Int);
                            sqCom.Parameters["@Tur"].Value = Convert.ToInt32(cmbArizaTipi.SelectedValue);

                            sqCom.Parameters.Add("@Vardiya", SqlDbType.Int);
                            sqCom.Parameters["@Vardiya"].Value = Convert.ToInt32(cmbVardiya.SelectedValue);

                            string time = dtBaslamaSaati.Value.ToString();
                            var result = Convert.ToDateTime(time);
                            string test = result.ToString("HH:mm", System.Globalization.CultureInfo.InvariantCulture);

                            sqCom.Parameters.Add("@BaslamaSaati", SqlDbType.Time);
                            sqCom.Parameters["@BaslamaSaati"].Value = test.ToString();

                            string time2 = dtBitisSaati.Value.ToString();
                            var result2 = Convert.ToDateTime(time2);
                            string test2 = result2.ToString("HH:mm", System.Globalization.CultureInfo.InvariantCulture);

                            sqCom.Parameters.Add("@BitisSaati", SqlDbType.Time);
                            sqCom.Parameters["@BitisSaati"].Value = test2.ToString();

                            sqCom.Parameters.Add("@Durus", SqlDbType.Int);
                            sqCom.Parameters["@Durus"].Value = nmDurus.Value;

                            if (grupYetki != 0)
                            {
                                arizaKodlariIDGuncelle();
                                sqCom.Parameters.Add("@ArizaKodu", SqlDbType.NVarChar);
                                sqCom.Parameters["@ArizaKodu"].Value = sb4.ToString();
                            }
                            sqCom.Parameters.Add("@Yapilanlar", SqlDbType.NVarChar);
                            sqCom.Parameters["@Yapilanlar"].Value = txtYapilanlar.Text;

                            sqCom.Parameters.Add("@Cozuldu", SqlDbType.Bit);
                            sqCom.Parameters["@Cozuldu"].Value = chkCozuldu.Checked;

                            personelIDGuncelle();
                            sqCom.Parameters.Add("@Personel", SqlDbType.NVarChar);
                            sqCom.Parameters["@Personel"].Value = sb2.ToString();

                            sqCom.Parameters.Add("@Kullanici", SqlDbType.NVarChar);
                            sqCom.Parameters["@Kullanici"].Value = kullaniciAdiSoyadi;

                            sqCom.ExecuteNonQuery();
                            sqlConn.Close();

                            MessageBox.Show(lblArizaID.Text + " arizaID nolu\nKayıt Güncellendi");
                            cmbBolum.SelectedIndex = -1;
                            cmbArizaTipi.SelectedIndex = -1;
                            cmbVardiya.SelectedIndex = -1;
                            cmbTuru.SelectedIndex = -1;
                            cmbMakine.SelectedIndex = -1;
                           
                            nmDurus.Value = 0;
                            txtYapilanlar.Text = null;
                            lblArizaID.Text = null;
                            chkCozuldu.Checked = false;

                            duyuruGoruntule();
                            listeleTumu();

                        }
                        btnKaydet.Text = "KAYDET";
                    }

                    else if (dialogResult==DialogResult.No)
                    {

                    }
                    this.ActiveControl = dataGridView1;
                }
                else
                {
                    if (guncellemeyeUygun == false)
                    {
                        MessageBox.Show("Bugün, kilitli tarih ve çözülmemiş sorunlar değişikliy yapılmaz");
                    }
                    else
                    {
                        MessageBox.Show(eksikler.ToString());
                    }
                }
            }
            else
            {
                MessageBox.Show(eksikler.ToString());
            }
        }

        private void veriListele(string sorgu)
        {
            SqlConnection sqlConn = new SqlConnection();
            sqlConn.ConnectionString = frmGiris.Database;

            if (sqlConn.State != ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Open();
            }
            SqlCommand sqCom = new SqlCommand();
            sqCom.Connection = sqlConn;
            sqCom.CommandText = sorgu;
            
            SqlDataAdapter da = new SqlDataAdapter(sqCom);
            sqlConn.Close();

            DataTable dtProd = new DataTable();
            da.Fill(dtProd);
            dtTablom = null;
            dtTablom = dtProd;

            dataGridView1.DataSource = dtProd;
            RowsColor();
            btnResize();
        } 

        private void listeleTumu()
        {
            veriListele(@"SELECT Tbl1.ArizaID, Tbl1.Tarih, Tbl6.IslemTuru, Tbl3.Turu, Tbl2.Bolum, Tbl8.MakineAdi, TblA.ArizaKodlari, Tbl1.Yapilanlar, TblN.Isimler, Tbl5.Vardiya
                        , CONVERT(VARCHAR(5),Tbl1.BaslamaSaati,108) AS BaslamaSaati
                        , CONVERT(VARCHAR(5),Tbl1.BitisSaati,108) AS BitisSaati
                        , (SELECT CONVERT(varchar(5), (DATEADD(MINUTE, (SELECT DATEDIFF(MINUTE, Tbl1.BaslamaSaati, Tbl1.BitisSaati))
                        , (SELECT DATEDIFF(HOUR, Tbl1.BaslamaSaati, Tbl1.BitisSaati )))), 108)) AS ToplamSure
                        , Tbl1.Durus, Tbl1.ArizaKodu, Tbl1.Personel, Tbl1.Cozuldu, Tbl1.FormVar 
                        FROM bakArizalar Tbl1 LEFT JOIN bakBolumler Tbl2 ON Tbl1.Bolum = Tbl2.BolumID 
LEFT JOIN bakMakineler Tbl8 ON Tbl1.Makine = Tbl8.MakineID 
LEFT JOIN bakTur Tbl3 ON Tbl1.Tur = Tbl3.TurID 
LEFT JOIN bakPersoneller Tbl4 ON Tbl1.Personel = Tbl4.PersonelID 
LEFT JOIN bakVardiyalar Tbl5 ON Tbl1.Vardiya = Tbl5.VardiyaID 
LEFT JOIN bakIslem Tbl6 ON Tbl1.Islem = Tbl6.IslemID 
LEFT JOIN bakArizaKodlari Tbl7 ON Tbl1.ArizaKodu = Tbl7.ArizaKoduID 
LEFT JOIN(SELECT DISTINCT o.ArizaID
, Isimler = STUFF((SELECT ', ' + b.PersonelAdi 
FROM(SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS Personel 
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(Personel, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x FROM   bakArizalar )t 
CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS a 
INNER JOIN[dbo].[bakPersoneller] AS b ON a.Personel = b.PersonelID 
WHERE a.ArizaID = o.ArizaID FOR XML PATH, TYPE).value(N'.[1]', N'varchar(max)'), 1, 2, '') 
FROM(SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS Personel 
FROM ( SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(Personel, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x FROM   bakArizalar )t 
CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS o) AS TblN ON Tbl1.ArizaID = TblN.ArizaID 
LEFT JOIN (SELECT DISTINCT o.ArizaID, ArizaKodlari = STUFF((SELECT ', ' + b.ArizaKodu FROM(SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS ArizaKodu 
FROM ( SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(ArizaKodu, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x FROM  bakArizalar )t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS a 
INNER JOIN[dbo].[bakArizaKodlari] AS b ON a.ArizaKodu = b.ArizaKoduID 
WHERE a.ArizaID = o.ArizaID FOR XML PATH, TYPE).value(N'.[1]', N'varchar(max)'), 1, 2, '') 
FROM(SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS ArizaKodu 
FROM ( SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(ArizaKodu, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x FROM  bakArizalar )t 
CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS o) AS TblA ON Tbl1.ArizaID = TblA.ArizaID ORDER BY Tbl1.ArizaID");

        }
        public DataTable PersonelYukle()
        {
            using(SqlConnection conn = new SqlConnection(frmGiris.Database))
            {
                conn.Open();
                using(SqlCommand sc = new SqlCommand("SELECT * FROM bakPersoneller WHERE Pasif = 0 ORDER BY PersonelAdi ASC", conn))
                    //bakoersoneller tablosundaki tüm sütünları getir koşul olarak pasıf = 0 olanları yani çalışanlari (Pasif = 1 ise işten ayrılmış demek ) getir
                    //order by ile de getirdikleriini adan z ye sırala personel adına göre
                {
                    SqlDataReader reader;
                    reader = sc.ExecuteReader();
                    
                    DataTable dt = new DataTable();

                    dt.Columns.Add("PersonelID", typeof(string));
                    dt.Columns.Add("PersonelAdi", typeof(string));

                    dt.Load(reader);
                    conn.Close();

                    return dt;
                }

            }
        }

        public DataTable arizaYukle()
        {
            using (SqlConnection conn = new SqlConnection(frmGiris.Database))
            {
                conn.Open();
                using(SqlCommand sc = new SqlCommand("SELECT * FROM bakArizaKodlari WHERE Pasif = 0 ORDER BY ArizaKodu ASC", conn))
                //bakoArizaKodlari tablosundaki tüm sütünları * ile getiriyoruz. koşul olarak pasıf = 0 olanları yani çalışanlari (Pasif = 1 ise işten ayrılmış demek ) getir diyoruz
                //order by ile de getirdiklerini adan z ye sırala  Arıza Koduna göre
                {
                    SqlDataReader reader;
                    reader = sc.ExecuteReader();

                    DataTable dt = new DataTable();

                    dt.Columns.Add("ArizaKoduID", typeof(string));
                    dt.Columns.Add("ArizaKodu", typeof(string));

                    dt.Load(reader);
                    conn.Close();

                    return dt;
                }
            }
        }

        public DataTable bolumYukle()
        {
            using (SqlConnection conn = new SqlConnection(frmGiris.Database))
            {
                conn.Open();
                using (SqlCommand sc = new SqlCommand("SELECT BolumID, Bolum FROM bakBolumler WHERE Pasif = 0 ORDER BY Bolum", conn))
                {
                    SqlDataReader reader;
                    reader = sc.ExecuteReader();

                    DataTable dt = new DataTable();

                    dt.Columns.Add("BolumID", typeof(string));
                    dt.Columns.Add("Bolum", typeof(string));

                    dt.Load(reader);
                    conn.Close();

                    return dt;

                }    
                
            }
        }

        public DataTable yilYukle()
        {
            using (SqlConnection conn = new SqlConnection(frmGiris.Database))
            {
                conn.Open();
                using (SqlCommand sc = new SqlCommand("SELECT YilID, Yil FROM bakYillar ORDER BY Yil DSC", conn))
                {
                    SqlDataReader reader;
                    reader = sc.ExecuteReader();

                    DataTable dt = new DataTable();

                    dt.Columns.Add("YilID", typeof(string));
                    dt.Columns.Add("Yil", typeof(string));

                    dt.Load(reader);
                    conn.Close();

                    return dt;
                }
            }
        }
        public DataTable turYukle()
        {
            using (SqlConnection conn2 = new SqlConnection(frmGiris.Database))
            {
                conn2.Open();
                using (SqlCommand sc2 = new SqlCommand("SELECT * FROM bakTur WHERE TurID != 3 ", conn2)) //tür ıd si 3'e eşit olmayan tüm verileri getir demek
                {
                    SqlDataReader reader2;
                    reader2 = sc2.ExecuteReader();

                    DataTable dt2 = new DataTable();

                    dt2.Columns.Add("TurID", typeof(string));
                    dt2.Columns.Add("Turu", typeof(string));

                    dt2.Load(reader2);
                    conn2.Close();

                    return dt2;
                }
            }
        }
    
        public DataTable vardiyalarYukle()
        {
            using (SqlConnection conn3 = new SqlConnection(frmGiris.Database))
            {
                conn3.Open();
                using (SqlCommand sc3 = new SqlCommand("SELECT * FROM bakVardiyalar", conn3))
                {
                    SqlDataReader reader3;
                    reader3 = sc3.ExecuteReader();

                    DataTable dt3 = new DataTable();

                    dt3.Columns.Add("VardiyaID", typeof(string));
                    dt3.Columns.Add("Vardiya", typeof(string));

                    dt3.Load(reader3);
                    conn3.Close();

                    return dt3;
                }
            }
        }

        public DataTable islemYukle()
        {
            using (SqlConnection conn4 = new SqlConnection(frmGiris.Database))
            {
                conn4.Open();
                using (SqlCommand sc4 = new SqlCommand("SELECT * FROM bakIslem WHERE IslemID = 1 OR IslemID = 2 ", conn4)) //bakIslem tablosundan ıslemID sı 1 ya da 2 olanları alarak lıştele demek
                {
                    SqlDataReader reader4;
                    reader4 = sc4.ExecuteReader();

                    DataTable dt4 = new DataTable();
//////////////////////////////////////////////////////////////////////////////****Tur - IslemTuru yerıne yazdım 
                    dt4.Columns.Add("IslemID", typeof(string));
                    dt4.Columns.Add("Tur", typeof(string));

                    dt4.Load(reader4);
                    conn4.Close();

                    return dt4;

                }
            }

        }

        public DataTable makineYukle()
        {
            using (SqlConnection conn4 = new SqlConnection(frmGiris.Database))
            {
                conn4.Open();
                using (SqlCommand sc4 = new SqlCommand("SELECT * FROM bakMakineler", conn4))
                {
                    SqlDataReader reader4;
                    reader4 = sc4.ExecuteReader();

                    DataTable dt4 = new DataTable();

                    dt4.Columns.Add("MakineID", typeof(string));
                    dt4.Columns.Add("MakineAdi", typeof(string));

                    dt4.Load(reader4);
                    conn4.Close();

                    return dt4;

                }
            }
        }
        
        private void LoadData(string kisim)
        {
            lstPersonel.Visible = true;
            if (grupYetki != 0)
            {
                lstArizaKodlari.Visible = true;

            }
            else
            {

            }

            DataTable dtProd = new DataTable();
            dtProd = dtPerTablom.Select("Tur=" + kisim).CopyToDataTable(); // datatable içerisinde verdiğimiz kritere göre bir data table kopyalamış oluyoruz. 

            DataTable dtp = new DataTable();

            dtp = dtProd;
            lstPersonel.DataSource = dtp;
            lstPersonel.ValueMember = "PersonelID";
            lstPersonel.DisplayMember = "PersonelAdi";
            
            lstPersonel.ClearSelected();

            DataRow[] foundRows = dtArizaTablom.Select("Tur='3' Or Tur=" + kisim, "ArizaKodu ASC");

            DataTable dt = foundRows.CopyToDataTable();
            lstArizaKodlari.DataSource = dt;
            lstArizaKodlari.ValueMember = "ArizaKoduID";
            lstArizaKodlari.DisplayMember = "ArizaKodu";
            lstArizaKodlari.ClearSelected();


        }

        public DataTable tabloListele(int ArizaNo)
        {
            DataTable dtProd = new DataTable();
            dtProd = dtTablom.Select("ArizaID= " + ArizaNo).CopyToDataTable();
            return dtProd;

        }

        private void comboBoxYukle()
        {
            cmbBolum.ValueMember = "BolumID";
            cmbBolum.DisplayMember = "Bolum";
            cmbBolum.DataSource = dtBolumTablom;
            cmbBolum.SelectedIndex = -1;

            cmbArizaTipi.ValueMember = "TurID";
            cmbArizaTipi.DisplayMember = "Turu";
            cmbArizaTipi.DataSource = dtTurTablom;
            cmbArizaTipi.SelectedIndex = -1;

            cmbVardiya.ValueMember = "VardiyaID";
            cmbVardiya.DisplayMember = "Vardiya";
            cmbVardiya.DataSource = dtVardiyaTablom;
            cmbVardiya.SelectedIndex = -1;

            cmbTuru.ValueMember = "IslemID";
            cmbTuru.DisplayMember = "IslemTuru";
            cmbTuru.DataSource = dtIslemTablom;
            cmbTuru.SelectedIndex = -1;

            cmbMakine.ValueMember = "MakineID";
            cmbMakine.DisplayMember = "IslemTuru";
            cmbMakine.DataSource = dtMakinelerTablom;
            cmbMakine.SelectedIndex = -1;

        }

        public void RowsColor()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                byte val2 = Convert.ToByte(dataGridView1.Rows[i].Cells[17].Value);
                string val3 = Convert.ToString(dataGridView1.Rows[i].Cells[2].Value);

                if (val2 == 0 && val3 == "ARIZA")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Aqua;

                }
                byte val = Convert.ToByte(dataGridView1.Rows[i].Cells[16].Value);
                if (val == 0)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;

                }
                string val4 = Convert.ToString(dataGridView1.Rows[i].Cells[3].Value);
                if (val4 == "MEKANİK")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;
                }
            }
        }

        private void frmBakim_Shown(object sender, EventArgs e)
        {
            RowsColor();

        }

        private void btnResize()
        {
            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        }
        private void sonKayitlariUsteGetir()
        {
            int JumpToRow = dataGridView1.Rows.Count - 1;
            if (dataGridView1.Rows.Count >= JumpToRow && JumpToRow >= 1)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = JumpToRow;

            }
        }


        private void duyuruGoruntule()
        {
            SqlConnection sqlConn = new SqlConnection();
            sqlConn.ConnectionString = frmGiris.Database;
            sqlConn.Open();

            SqlCommand sqCom = new SqlCommand();
            sqCom.Connection = sqlConn;

            sqCom.CommandText = "SELECT Duyuru, Yapilacak1, Yapilacak2, Yapilacak3, Yapilacak4, Yapilacak5, Yapilacak6, Yapilacak7, Yapilacak8, Yapilacak9, Yapilacak10 FROM bakDuyurular";

            SqlDataAdapter da = new SqlDataAdapter(sqCom);
            DataTable dtProd = new DataTable();

            da.Fill(dtProd);

            string metin = dtProd.Rows[0]["Duyuru"].ToString();
            string metin2 = dtProd.Rows[0]["Yapilacak1"].ToString();
            string metin3 = dtProd.Rows[0]["Yapilacak2"].ToString();
            string metin4 = dtProd.Rows[0]["Yapilacak3"].ToString();
            string metin5 = dtProd.Rows[0]["Yapilacak4"].ToString();
            string metin6 = dtProd.Rows[0]["Yapilacak5"].ToString();
            string metin7 = dtProd.Rows[0]["Yapilacak6"].ToString();
            string metin8 = dtProd.Rows[0]["Yapilacak7"].ToString();
            string metin9 = dtProd.Rows[0]["Yapilacak8"].ToString();
            string metin10 = dtProd.Rows[0]["Yapilacak9"].ToString();
            string metin11 = dtProd.Rows[0]["Yapilacak10"].ToString();

            string text = "<MARQUEE DIRECTION=UP SCROLLAMOUNT=1><FONT COLOR='RED'><FONT FACA='ARİAL'>" + metin + "<br><br>" + metin2 + "<br><br>" + metin3 + "<br><br>" + metin4 + "<br><br>" + metin5 + "<br><br>" + metin6 + " <br><br>" + metin7 + "<br><br>" + metin8 + "<br><br>" + metin9 + "<br><br>" + metin10 + "<br><br>" + metin11 + "</MARQUEE>";

            browser.DocumentText = text;


        }

        private void guncelTarih()
        {
            SqlConnection sqlConn = new SqlConnection();
            sqlConn.ConnectionString = frmGiris.Database;
            sqlConn.Open();

            SqlCommand sqCom = new SqlCommand();
            sqCom.Connection = sqlConn;
            sqCom.CommandText = "SELECT GETDATE() AS tarih";
            
            SqlDataAdapter da = new SqlDataAdapter(sqCom);
            sqlConn.Close();
            
            DataTable dtProd = new DataTable();
            da.Fill(dtProd);
            string tarihKisa = dtProd.Rows[0]["Tarih"].ToString();


            lblTarih.Text = tarihKisa.Substring(0, 16);
            string bilgisayarTarihi = DateTime.Now.ToString();
            string dbTarih = lblTarih.Text;
            string dbt = dbTarih.Substring(0, 10);

            string pcTarih = bilgisayarTarihi;
            string pct = pcTarih.Substring(0, 10);

            if (dbt != pct )
            {
                MessageBox.Show("Bilgisayar tarihi yanlış. Düzeltiniz!");
            }

        }

        private void LoadDataIki(string bolumNo)
        {
            DataTable dtProd = new DataTable();
            dtProd = dtMakinelerTablom.Select("BolumID=" + bolumNo).CopyToDataTable();

            DataTable dtp = new DataTable();
            dtp = dtProd;
            
            cmbMakine.ValueMember = "MakineID";
            cmbMakine.DisplayMember = "MakineAdi";
            cmbMakine.DataSource = dtp;
            cmbMakine.SelectedIndex = -1;

        }

        public void personelSec()
        {
            try
            {
                char[] ayrac = { '-' };
                string[] kelimeler = sb1.Split(ayrac);
                foreach (var item in kelimeler)
                {
                    DataTable dt2 = new DataTable();
                    dt2 = dtPerTablom.Select("PersonelID=" + item.ToString()).CopyToDataTable();

                    int index = lstPersonel.FindString(dt2.Rows[0][1].ToString());
                    if (index < 0)
                    {
                       // MessageBox.Show("Bulunamadı");
                    }
                    else
                    {
                        lstPersonel.SetItemChecked(index, true);         
                    }

                }

            }
            catch 
            {
            }
        }
        public void arizaKoduSec()
        {
            try
            {
                char[] ayrac = { '-' };
                string[] kelimeler = sb3.Split(ayrac);
                foreach (var item in kelimeler)
                {
                    DataTable dt2 = new DataTable();
                    dt2 = dtArzTablom.Select("ArizaKoduID=" + item.ToString()).CopyToDataTable();

                    int index = lstArizaKodlari.FindString(dt2.Rows[0][1].ToString());
                    if (index < 0)
                    {
                        //MessageBox.Show("Bulunamadı");
                    }
                    else
                    {
                        lstArizaKodlari.SetItemChecked(index, true);
                    }

                }

            }
            catch
            {
            }
        }


        public void personelSecili()
        {
            if (lstPersonel.CheckedItems.Count < 1)
            {
                personelSecim = false;

            }
            else
            {
                personelSecim = true;
            }



        }

        public void arizaKoduSecili()
        {
            if (lstArizaKodlari.CheckedItems.Count < 1)
            {
                arizaKoduSecim = false;
            }
            else
            {
                arizaKoduSecim = true;
            }

        }

        public void personelSeciliOlmasın()
        {
            foreach (int indexChecked in lstPersonel.CheckedIndices)
            {
                lstPersonel.SetItemChecked(indexChecked, false);
            }
            lstPersonel.ClearSelected();

        }

        public void arizaKodlariSeciliOlmasın()
        {
            foreach (int indexChecked in lstArizaKodlari.CheckedIndices)
            {
                lstArizaKodlari.SetItemChecked(indexChecked, false);

            }
            lstArizaKodlari.ClearSelected();
        }


        public void personelIDGuncelle()
        {
            string aa = lblArizaID.Text;
            sb2 = null;
            foreach (object element in lstPersonel.CheckedItems)
            {
                DataRowView row = (DataRowView)element;
                sb2 += row[0].ToString();
                sb2 += "-";

            }
            sb2 = sb2.Substring(0, sb2.Length - 1);

        }

        public void arizaKodlariIDGuncelle()
        {
            string aa = lblArizaID.Text;
            sb4 = null;
            foreach (object element in lstArizaKodlari.CheckedItems)
            {
                DataRowView row = (DataRowView)element;

                sb4 += row[0].ToString();
                sb4 += "-";

            }
            sb4 = sb4.Substring(0, sb4.Length - 1);

        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            btnYeni.Visible = true;
            try
            {
                int a = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                lblArizaID.Text = a.ToString();
                
                DataTable dtProd = new DataTable();
                dtProd = tabloListele(Convert.ToInt32(a.ToString()));
                if (dataGridView1.Rows.Count > 0 && lblArizaID.Text != "")
                {
                    sb1 = null;
                    sb3 = null;
                    string tarihKisa = dtProd.Rows[0]["Tarih"].ToString();
                    lblTarih.Text = tarihKisa.Substring(0, 16);

                    cmbBolum.Text = dtProd.Rows[0]["Bolum"].ToString();
                    cmbMakine.Text = dtProd.Rows[0]["MakineAdi"].ToString();
                    cmbArizaTipi.Text = dtProd.Rows[0]["Tur"].ToString();

                    sb1 = dtProd.Rows[0]["Personel"].ToString();
                    sb3 = dtProd.Rows[0]["ArizaKodu"].ToString();
                    cmbVardiya.Text = dtProd.Rows[0]["Vardiya"].ToString();
                    cmbTuru.Text = dtProd.Rows[0]["IslemTuru"].ToString();
                    nmDurus.Value = Convert.ToInt32(dtProd.Rows[0]["Durus"]);
                    txtYapilanlar.Text = dtProd.Rows[0]["Yapilanlar"].ToString();

                    chkCozuldu.Checked = Convert.ToBoolean(dtProd.Rows[0]["Cozuldu"]);
                    checkedKontrol = chkCozuldu.Checked;
                    
                    string sure = Convert.ToString(Convert.ToDateTime(dtBitisSaati.Text) - Convert.ToDateTime(dtBaslamaSaati.Text));
                    lblMudahaleSuresi.Text = sure.Substring(0, 5);
                    dtBaslamaSaati.Text = Convert.ToString(dtProd.Rows[0]["BaslamaSaati"]);
                    dtBitisSaati.Text = Convert.ToString(dtProd.Rows[0]["BitisSaati"]);

                    personelSeciliOlmasın();
                    arizaKodlariSeciliOlmasın();
                    personelSec();
                    arizaKoduSec();
                    if (btnKaydet.Enabled== false)
                    {
                        btnKaydet.Enabled = true;
                    }
                    if (btnKaydet.Text == "KAYDET")
                    {
                        btnKaydet.Text = "GÜNCELLE";
                    }

                }
            }
            catch 
            {
            }
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            RowsColor();
        }

        private void dtBaslamaSaati_ValueChanged(object sender, EventArgs e)
        {
            string sure = Convert.ToString(Convert.ToDateTime(dtBitisSaati.Text) - Convert.ToDateTime(dtBaslamaSaati.Text));
            lblMudahaleSuresi.Text = sure.Substring(0, 5);

        }

        private void dtBitisSaati_ValueChanged(object sender, EventArgs e)
        {

            string sure = Convert.ToString(Convert.ToDateTime(dtBitisSaati.Text) - Convert.ToDateTime(dtBaslamaSaati.Text));
            lblMudahaleSuresi.Text = sure.Substring(0, 5);
        }

        private void cmbArizaTipi_TextChanged(object sender, EventArgs e)
        {
            if (ilk == true)
            {
                dtPerTablom = null;
                dtPerTablom = PersonelYukle();
                
                dtArizaTablom = null;
                dtArizaTablom = arizaYukle();

                dtArzTablom = null;
                dtArzTablom = arizaYukle();

                ilk = false;

            }
            switch (cmbArizaTipi.Text)
            {
                case "ELEKTRİK":
                    LoadData("1");
                    break;
                case "MEKANİK":
                    LoadData("2");
                    break;
                default:
                    break;
            }
        }

        private void cmbBolum_TextChanged(object sender, EventArgs e)
        {
            if (ilk3 == true)
            {
                dtMakinelerTablom = null;
                dtMakinelerTablom = makineYukle();

                ilk3 = false;


            }
            if (cmbBolum.SelectedValue != null)
            {
                LoadDataIki(cmbBolum.SelectedValue.ToString());

            }
        }

        private void txtYapilanlar_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar == (char)Keys.Enter;

        }

        private void kilitTarihKontrol()
        {
            SqlConnection sqlConn = new SqlConnection();
            sqlConn.ConnectionString = frmGiris.Database;
            sqlConn.Open();

            SqlCommand sqCom = new SqlCommand();
            sqCom.Connection = sqlConn;

            sqCom.CommandText = "SELECT KilitTarihi FROM bakAyarlar WHERE AyarID= 1";
            sqCom.ExecuteNonQuery();

            DataTable dtProd = new DataTable();
            SqlDataAdapter sqDa = new SqlDataAdapter();
            sqDa.SelectCommand = sqCom;
            sqlConn.Close();
            sqDa.Fill(dtProd);

            string tarihKisa = dtProd.Rows[0]["KilitTarihi"].ToString();
            string tarihKisaIki = tarihKisa.Substring(0, 10);

            KT = Convert.ToDateTime(tarihKisaIki);

              

        }
    }

}
