using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BAkimTAkipProgramii
{
    public partial class frmRapor09 : Form
    {
        public frmRapor09()
        {
            InitializeComponent();
        }
        public DataTable dtTablom;
        public DataTable dtPerTablom;
        public DataTable dtPerTablom2;
        public DataTable dtArizaTablom;
        public DataTable dtArzTablom;
        public DataTable dtBolumTablom;
        public DataTable dtTurTablom;
        public DataTable dtVardiyalarTablom;
        public DataTable dtIslemTablom;
        public DataTable dtMakinelerTablom;

        public bool ilk = false;
        public bool ilk2 = false;
        public bool ilk3 = false;

        public string sb1;
        public string sb3;

        public bool comboAcık = false;
        public string sec;

        private void frmRapor09_Load(object sender, EventArgs e)
        {
            comboAcık = true;
            this.StartPosition = FormStartPosition.Manual;
            this.Dock = DockStyle.Fill;
            ilk = true;
            ilk2 = true;
            ListeleBuAy();

            this.ActiveControl = dataGridView1;
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            cmbCozuldu.SelectedIndex = 0;
            cmbTuru.SelectedIndex = -1;
            cmbBolum.SelectedIndex = -1;
            cmbMakine.SelectedIndex = -1;
            cmbArizaTipi.SelectedIndex = -1;
            cmbArizaKodu.SelectedIndex = -1;
            cmbPersonel.SelectedIndex = -1;
            cmbVardiya.SelectedIndex = -1;

            nmSureBas.Value = 0;
            nmSureBit.Value = 1000;

            txtAra.Text = "";
            txtArızaIDAra.Text = "";

            ListeleBuAy();

        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (dateTimePicker1.Value <= dateTimePicker2.Value)
            {
                dataGridView1.DataSource = null;
                string DataBase100 = frmGiris.Database;

                string query100 = @"SELECT Tbl1.ArizaID, Tbl1.Tarih, Tbl6.IslemTuru, Tbl3.Turu, Tbl2.Bolum
, Tbl8.MakineAdi, TblA.ArizaKodlari, Tbl1.Yapilanlar, TblN.Isimler, Tbl5.Vardiya
, CONVERT(VARCHAR(5),Tbl1.BaslamaSaati,108) AS BaslamaSaati
, CONVERT(VARCHAR(5),Tbl1.BitisSaati,108) AS BitisSaati
, (SELECT CONVERT(varchar(5), (DATEADD(MINUTE
, (SELECT DATEDIFF(MINUTE, Tbl1.BaslamaSaati, Tbl1.BitisSaati))
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
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS Personel
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(Personel
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS a
INNER JOIN[dbo].[bakPersoneller] AS b ON a.Personel = b.PersonelID
WHERE a.ArizaID = o.ArizaID FOR XML PATH, TYPE).value(N'.[1]', N'varchar(max)'), 1, 2, '')
FROM(SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS Personel
FROM ( SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(Personel
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS o) AS TblN
ON Tbl1.ArizaID = TblN.ArizaID LEFT JOIN (SELECT DISTINCT o.ArizaID
, ArizaKodlari = STUFF((SELECT ', ' + b.ArizaKodu
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS ArizaKodu
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(ArizaKodu
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS a
INNER JOIN[dbo].[bakArizaKodlari] AS b ON a.ArizaKodu = b.ArizaKoduID
WHERE a.ArizaID = o.ArizaID FOR XML PATH, TYPE).value(N'.[1]', N'varchar(max)'), 1, 2, '')
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS ArizaKodu
FROM ( SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(ArizaKodu
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM  bakArizalar )t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS o) AS TblA
ON Tbl1.ArizaID = TblA.ArizaID
WHERE Tbl1.Tarih BETWEEN convert(datetime, '" + dateTimePicker1.Value + "', 105)AND convert(datetime, '" + dateTimePicker2.Value + "', 105) AND Tbl6.IslemID LIKE '%" + cmbTuru.SelectedValue + "%' AND Tbl2.BolumID LIKE '%" + cmbBolum.SelectedValue + "%' AND Tbl8.MakineID LIKE '%" + cmbMakine.SelectedValue + "%' AND Tbl3.TurID LIKE '%" + cmbArizaTipi.SelectedValue + "%' AND Tbl1.ArizaKodu LIKE '%" + cmbArizaKodu.SelectedValue + "%' AND Tbl1.Personel LIKE '%" + cmbPersonel.SelectedValue + "%' AND Tbl5.VardiyaID LIKE '%" + cmbVardiya.SelectedValue + "%' AND Tbl1.ArizaID LIKE '%" + txtArızaIDAra.Text + "%' AND Tbl1.ToplamSure BETWEEN '" + nmSureBas.Value + "' AND '" + nmSureBit.Value + "' AND Tbl1.Cozuldu LIKE '%" + cmbCozuldu.Tag + "%' AND Tbl1.Yapilanlar LIKE '%" + txtAra.Text + "%'  ORDER BY Tbl1.ArizaID";
                DataTable dataTable100 = new DataTable();
                SqlDataAdapter dAdapter100 = new SqlDataAdapter(query100, DataBase100);
                dAdapter100.Fill(dataTable100);
                if (dataTable100.Rows.Count > 0 )
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = dataTable100;

                    dtTablom = null;
                    dtTablom = dataTable100;
                    RowsColor();
                    btnResize();
                }
                else
                {
                    MessageBox.Show("Aradığının Veri Bulunamadı.");
                }
                this.ActiveControl = dataGridView1;
                
            }
            else
            {
                MessageBox.Show("Bitiş Tarihi Başlangıç Tarihinden Küçük Olamaz");
            }
            Cursor.Current = Cursors.Default;
        }

        private void çözülmeyenTümKayıtlarıGösterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dun = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy 00:00:00");
            string dun2 = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy 23:59:59");
            string bugun = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            string bugun2 = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            veriListele(@"SELECT Tbl1.ArizaID, Tbl1.Tarih, Tbl6.IslemTuru, Tbl3.Turu, Tbl2.Bolum
, Tbl8.MakineAdi, TblA.ArizaKodlari, Tbl1.Yapilanlar, TblN.Isimler, Tbl5.Vardiya
, CONVERT(VARCHAR(5),Tbl1.BaslamaSaati,108) AS BaslamaSaati
, CONVERT(VARCHAR(5),Tbl1.BitisSaati,108) AS BitisSaati
, (SELECT CONVERT(varchar(5), (DATEADD(MINUTE, (SELECT DATEDIFF(MINUTE, Tbl1.BaslamaSaati, Tbl1.BitisSaati))
, (SELECT DATEDIFF(HOUR, Tbl1.BaslamaSaati, Tbl1.BitisSaati )))), 108)) AS ToplamSure
,Tbl1.Durus, Tbl1.ArizaKodu, Tbl1.Personel, Tbl1.Cozuldu, Tbl1.FormVar
FROM bakArizalar Tbl1
LEFT JOIN bakBolumler Tbl2 ON Tbl1.Bolum = Tbl2.BolumID
LEFT JOIN bakMakineler Tbl8 ON Tbl1.Makine = Tbl8.MakineID
LEFT JOIN bakTur Tbl3 ON Tbl1.Tur = Tbl3.TurID
LEFT JOIN bakPersoneller Tbl4 ON Tbl1.Personel = Tbl4.PersonelID
LEFT JOIN bakVardiyalar Tbl5 ON Tbl1.Vardiya = Tbl5.VardiyaID
LEFT JOIN bakIslem Tbl6 ON Tbl1.Islem = Tbl6.IslemID
LEFT JOIN bakArizaKodlari Tbl7 ON Tbl1.ArizaKodu = Tbl7.ArizaKoduID
LEFT JOIN(SELECT DISTINCT o.ArizaID
, Isimler = STUFF((SELECT ', ' + b.PersonelAdi
FROM(SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS Personel
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(Personel
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar)t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS a
INNER JOIN[dbo].[bakPersoneller] AS b ON a.Personel = b.PersonelID
WHERE a.ArizaID = o.ArizaID FOR XML PATH, TYPE).value(N'.[1]', N'varchar(max)'), 1, 2, '')
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS Personel
FROM ( SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(Personel
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS o) AS TblN
ON Tbl1.ArizaID = TblN.ArizaID
LEFT JOIN (SELECT DISTINCT o.ArizaID
, ArizaKodlari = STUFF((SELECT ', ' + b.ArizaKodu
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS ArizaKodu
FROM ( SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(ArizaKodu
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM  bakArizalar )t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS a
INNER JOIN[dbo].[bakArizaKodlari] AS b ON a.ArizaKodu = b.ArizaKoduID
WHERE a.ArizaID = o.ArizaID FOR XML PATH, TYPE).value(N'.[1]', N'varchar(max)'), 1, 2, '')
FROM(SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS ArizaKodu
FROM ( SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(ArizaKodu
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM  bakArizalar )t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS o) AS TblA
ON Tbl1.ArizaID = TblA.ArizaID
WHERE Tbl1.Cozuldu=0 ORDER BY Tbl1.ArizaID");



        }

        private void dünVeBügünüGösterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listeleDunBugun();


        }

        private void bugünüGösterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dun = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy 00:00:00");
            string dun2 = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy 23:59:59");
            string bugun = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            string bugun2 = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            veriListele(@"SELECT Tbl1.ArizaID, Tbl1.Tarih, Tbl6.IslemTuru, Tbl3.Turu, Tbl2.Bolum
, Tbl8.MakineAdi, TblA.ArizaKodlari, Tbl1.Yapilanlar, TblN.Isimler, Tbl5.Vardiya
, CONVERT(VARCHAR(5),Tbl1.BaslamaSaati,108) AS BaslamaSaati
, CONVERT(VARCHAR(5),Tbl1.BitisSaati,108) AS BitisSaati
, (SELECT CONVERT(varchar(5), (DATEADD(MINUTE
, (SELECT DATEDIFF(MINUTE, Tbl1.BaslamaSaati, Tbl1.BitisSaati))
, (SELECT DATEDIFF(HOUR, Tbl1.BaslamaSaati, Tbl1.BitisSaati )))), 108)) AS ToplamSure
, Tbl1.Durus, Tbl1.ArizaKodu, Tbl1.Personel, Tbl1.Cozuldu, Tbl1.FormVar
FROM bakArizalar Tbl1
LEFT JOIN bakBolumler Tbl2 ON Tbl1.Bolum = Tbl2.BolumID
LEFT JOIN bakMakineler Tbl8 ON Tbl1.Makine = Tbl8.MakineID
LEFT JOIN bakTur Tbl3 ON Tbl1.Tur = Tbl3.TurID
LEFT JOIN bakPersoneller Tbl4 ON Tbl1.Personel = Tbl4.PersonelID
LEFT JOIN bakVardiyalar Tbl5 ON Tbl1.Vardiya = Tbl5.VardiyaID
LEFT JOIN bakIslem Tbl6 ON Tbl1.Islem = Tbl6.IslemID
LEFT JOIN bakArizaKodlari Tbl7 ON Tbl1.ArizaKodu = Tbl7.ArizaKoduID
LEFT JOIN(SELECT DISTINCT o.ArizaID
, Isimler = STUFF((SELECT ', ' + b.PersonelAdi
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS Personel
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(Personel
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS a
INNER JOIN[dbo].[bakPersoneller] AS b ON a.Personel = b.PersonelID
WHERE a.ArizaID = o.ArizaID FOR XML PATH, TYPE).value(N'.[1]', N'varchar(max)'), 1, 2, '')
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS Personel
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(Personel
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS o) AS TblN
ON Tbl1.ArizaID = TblN.ArizaID
LEFT JOIN (SELECT DISTINCT o.ArizaID
, ArizaKodlari = STUFF((SELECT ', ' + b.ArizaKodu
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS ArizaKodu
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(ArizaKodu
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS a
INNER JOIN[dbo].[bakArizaKodlari] AS b
ON a.ArizaKodu = b.ArizaKoduID
WHERE a.ArizaID = o.ArizaID FOR XML PATH, TYPE).value(N'.[1]', N'varchar(max)'), 1, 2, '')
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS ArizaKodu
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(ArizaKodu
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS o) AS TblA
ON Tbl1.ArizaID = TblA.ArizaID
WHERE Tbl1.Tarih BETWEEN convert(datetime, '" + bugun + "', 105) AND convert(datetime, '" + bugun2 + "', 105) ORDER BY Tbl1.ArizaID");

        }

        private void dünüGösterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dun = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy 00:00:00");
            string dun2 = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy 23:59:59");
            string bugun = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            string bugun2 = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            veriListele(@"SELECT Tbl1.ArizaID, Tbl1.Tarih, Tbl6.IslemTuru, Tbl3.Turu, Tbl2.Bolum
, Tbl8.MakineAdi, TblA.ArizaKodlari, Tbl1.Yapilanlar, TblN.Isimler, Tbl5.Vardiya
, CONVERT(VARCHAR(5),Tbl1.BaslamaSaati,108) AS BaslamaSaati
, CONVERT(VARCHAR(5),Tbl1.BitisSaati,108) AS BitisSaati
, (SELECT CONVERT(varchar(5), (DATEADD(MINUTE
, (SELECT DATEDIFF(MINUTE, Tbl1.BaslamaSaati, Tbl1.BitisSaati))
, (SELECT DATEDIFF(HOUR, Tbl1.BaslamaSaati, Tbl1.BitisSaati )))), 108)) AS ToplamSure
, Tbl1.Durus, Tbl1.ArizaKodu, Tbl1.Personel, Tbl1.Cozuldu, Tbl1.FormVar
FROM bakArizalar Tbl1
LEFT JOIN bakBolumler Tbl2 ON Tbl1.Bolum = Tbl2.BolumID
LEFT JOIN bakMakineler Tbl8 ON Tbl1.Makine = Tbl8.MakineID
LEFT JOIN bakTur Tbl3 ON Tbl1.Tur = Tbl3.TurID
LEFT JOIN bakPersoneller Tbl4 ON Tbl1.Personel = Tbl4.PersonelID
LEFT JOIN bakVardiyalar Tbl5 ON Tbl1.Vardiya = Tbl5.VardiyaID
LEFT JOIN bakIslem Tbl6 ON Tbl1.Islem = Tbl6.IslemID
LEFT JOIN bakArizaKodlari Tbl7 ON Tbl1.ArizaKodu = Tbl7.ArizaKoduID
LEFT JOIN(SELECT DISTINCT o.ArizaID
, Isimler = STUFF((SELECT ', ' + b.PersonelAdi
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS Personel
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(Personel
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS a
INNER JOIN[dbo].[bakPersoneller] AS b ON a.Personel = b.PersonelID
WHERE a.ArizaID = o.ArizaID FOR XML PATH, TYPE).value(N'.[1]', N'varchar(max)'), 1, 2, '')
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS Personel
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(Personel
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS o) AS TblN
ON Tbl1.ArizaID = TblN.ArizaID
LEFT JOIN (SELECT DISTINCT o.ArizaID
, ArizaKodlari = STUFF((SELECT ', ' + b.ArizaKodu
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS ArizaKodu
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(ArizaKodu
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS a
INNER JOIN[dbo].[bakArizaKodlari] AS b
ON a.ArizaKodu = b.ArizaKoduID
WHERE a.ArizaID = o.ArizaID FOR XML PATH, TYPE).value(N'.[1]', N'varchar(max)'), 1, 2, '')
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS ArizaKodu
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(ArizaKodu
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS o) AS TblA
ON Tbl1.ArizaID = TblA.ArizaID
WHERE Tbl1.Tarih BETWEEN convert(datetime, '" + dun + "', 105) AND convert(datetime, '" + dun2 + "', 105) ORDER BY Tbl1.ArizaID");
        }

        private void buAykiKayıtlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListeleBuAy();
        }

        private void eXCELEGÖNDERToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Documents(*.xls)|*.xls";
            sfd.FileName = "Arıza ve Faaliyet Raporu.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                toCsV(dataGridView1, sfd.FileName);
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Excel Dosyası oluşturuldu");

            }
        }
        private void toCsV(DataGridView dGV, string fileName)
        {
            string stOutPut = "";
            string sHeaders = "";
            for (int j = 0; j < dGV.Columns.Count; j++)
                sHeaders = sHeaders.ToString() + Convert.ToString(dGV.Columns[j].HeaderText) + "\t";

            stOutPut += sHeaders + "\r\n";

            for (int i = 0; i < dGV.RowCount; i++)
            {
                string stLine = "";
                for (int j = 0; j < dGV.Rows[i].Cells.Count; j++)
                    stLine = stLine.ToString() + Convert.ToString(dGV.Rows[i].Cells[j].Value) + "\t";
               
                stOutPut += stLine + "\r\n";

            }
            Encoding utf16 = Encoding.GetEncoding(1254);
            byte[] outPut = utf16.GetBytes(stOutPut);
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(outPut, 0, outPut.Length);
            bw.Flush();
            bw.Close();
            fs.Close();
        }
        private void lblArizaTipi_TextChanged(object sender, EventArgs e)
        {
            if (ilk3 == true)
            {
                dtPerTablom2 = null;
                dtPerTablom2 = PersonelYukle2();
                dtArizaTablom = null;
                dtArizaTablom = ArizaYukle();
                {
                    ilk3 = false;
                }
            }
            switch (lblArizaTipi.Text)
            {
                case "ELEKTRİK":
                    LoadData5("1");
                    break;
                case "MEKANİK":
                    LoadData5("2");
                    break;
                default:
                    break;
            }
        }

        private void cmbBolum_TextChanged(object sender, EventArgs e)
        {
            if (ilk2 == true)
            {
                dtMakinelerTablom = null;
                dtMakinelerTablom = MakineYukle();
                
                ilk2 = false;

            }
            if (cmbBolum.SelectedValue != null)
            {
                LoadData4(cmbBolum.SelectedValue.ToString());
            }
        }

        private void cmbArizaTipi_TextChanged(object sender, EventArgs e)
        {
            if (ilk == true)
            {
                dtPerTablom = null;
                dtPerTablom = PersonelYukle();
                dtArizaTablom = null;
                dtArizaTablom = ArizaYukle();


                ilk = false;

            }
            switch (cmbArizaTipi.Text)
            {
                case "ELEKTRİK":
                    LoadData3("1");
                    break;

                case "MEKANİK":
                    LoadData3("2");
                    break;

                default:
                    break;
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int a = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            try
            {
                lblArizaID.Text = a.ToString();
            }
            catch 
            {

            }

            DataTable dtProd = new DataTable();
            try 
            {
                dtProd = tableListele(Convert.ToInt32(a.ToString()));
                if (dataGridView1.Rows.Count > 0 && lblArizaID.Text != "")
                {
                    sb1 = null;
                    sb3 = null;

                    string tarihKisa = dtProd.Rows[0]["Tarih"].ToString();
                    lblTarih.Text = tarihKisa.Substring(0, 16);
                    lblTuru.Text = dtProd.Rows[0]["IslemTuru"].ToString();
                    lblBolum.Text = dtProd.Rows[0]["Bolum"].ToString();
                    lblMakine.Text = dtProd.Rows[0]["MakineAdi"].ToString();
                    lblArizaTipi.Text = dtProd.Rows[0]["Turu"].ToString();
                    sb1 = dtProd.Rows[0]["Personel"].ToString();
                    sb3 = dtProd.Rows[0]["ArizaKodu"].ToString();
                    lblVardiya.Text = dtProd.Rows[0]["Vardiya"].ToString();
                    lblDurus.Text = dtProd.Rows[0]["Durus"].ToString();
                    lblYapilanlar.Text = dtProd.Rows[0]["Yapilanlar"].ToString();
                    chkCozuldu.Checked = Convert.ToBoolean(dtProd.Rows[0]["Cozuldu"]);

                    string sure = Convert.ToString(Convert.ToDateTime(dtProd.Rows[0]["BitisSaati"]) - Convert.ToDateTime(dtProd.Rows[0]["BitisSaati"]));
                    lblSure.Text = sure.Substring(0, 5);
                    lblBaslamaSaati.Text = Convert.ToString(dtProd.Rows[0]["BaslamaSaati"]);
                    lblBitisSaati.Text = Convert.ToString(dtProd.Rows[0]["BitisSaati"]);

                    personelSeciliOlmasin();
                    arizaKodlariSeciliOlmasin();
                    personelSec();
                    arizaKoduSec();

                }
            }
            catch
            {

            }
        }

        private void cmbCozuldu_TextChanged(object sender, EventArgs e)
        {
            switch (cmbCozuldu.Text)
            {
                case "HEPSİ":
                    cmbCozuldu.Tag = "";
                    break;
              
                case "CÖZÜLDÜ":
                    cmbCozuldu.Tag = -1;
                    break;
              
                case "ÇÖZÜLMEDİ":
                    cmbCozuldu.Tag = 0;
                    break;

                default:
                    break;
            }
        }

        private void frmRapor09_Shown(object sender, EventArgs e)
        {
            RowsColor();

        }

        private void frmRapor09_Activated(object sender, EventArgs e)
        {
            if (ilk2 == true)
            {
                dtBolumTablom = null;
                dtBolumTablom = BolumYukle();
                dtTurTablom = null;
                dtTurTablom = TurYukle();
                dtVardiyalarTablom = null;
                dtVardiyalarTablom = VardiyalarYukle();
                dtIslemTablom = null;
                dtIslemTablom = IslemYukle();
                dtMakinelerTablom = null;
                dtMakinelerTablom = MakineYukle();

                comboBoxYukle();

                cmbCozuldu.SelectedIndex = 0;
                cmbTuru.SelectedIndex = -1;
                cmbBolum.SelectedIndex = -1;
                cmbMakine.SelectedIndex = -1;
                cmbArizaTipi.SelectedIndex = -1;
                cmbArizaKodu.SelectedIndex = -1;
                cmbPersonel.SelectedIndex = -1;
                cmbVardiya.SelectedIndex = -1;

                nmSureBas.Value = 0;
                nmSureBit.Value = 1000;
                txtAra.Text = "";

                ilk2 = false;

            }
        }

        private void txtArızaIDAra_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            RowsColor();

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
            cmbVardiya.DataSource = dtVardiyalarTablom;
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

        public DataTable PersonelYukle()
        {
            SqlConnection conn = new SqlConnection(frmGiris.Database);
            conn.Open();

            SqlCommand sc = new SqlCommand("SELECT * FROM bakPersoneller Where Pasif = 0 ORDER BY PersonelAdi ASC", conn);
            SqlDataReader reader;

            reader = sc.ExecuteReader();
            DataTable dt6 = new DataTable();

            dt6.Columns.Add("PersonelID", typeof(string));
            dt6.Columns.Add("PersonelAdi", typeof(string));
            dt6.Load(reader);
            conn.Close();
            return dt6;


        }

        public DataTable PersonelYukle2()
        {
            SqlConnection conn = new SqlConnection(frmGiris.Database);
            conn.Open();
            SqlCommand sc = new SqlCommand("SELECT * FROM bakPersoneller WHERE Pasif = 0 ORDER BY PersonelAdi ASC ", conn);
            SqlDataReader reader;

            reader = sc.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("PersonelID", typeof(string));
            dt.Columns.Add("PersonelAdi", typeof(string));

            dt.Load(reader);
            conn.Close();
            return dt;
        }
        public DataTable ArizaYukle()
        {
            SqlConnection conn = new SqlConnection(frmGiris.Database);
            conn.Open();
            SqlCommand sc = new SqlCommand("SELECT * FROM bakArizaKodlari ORDER BY ArizaKodu ASC ", conn);

            SqlDataReader reader;

            reader = sc.ExecuteReader();
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("ArizaKoduID", typeof(string));
            dt1.Columns.Add("ArizaKodu", typeof(string));

            dt1.Load(reader);
            conn.Close();
            return dt1;
        }

        public DataTable BolumYukle()
        {

            SqlConnection conn = new SqlConnection(frmGiris.Database);
            conn.Open();
            SqlCommand sc = new SqlCommand("SELECT * FROM bakBolumler ORDER BY Bolum ASC ", conn);

            SqlDataReader reader;

            reader = sc.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("BolumID", typeof(string));
            dt2.Columns.Add("Bolum", typeof(string));

            dt2.Load(reader);
            conn.Close();
            return dt2;
        }
        public DataTable TurYukle()
        {
            SqlConnection conn2 = new SqlConnection(frmGiris.Database);
            conn2.Open();
            SqlCommand sc = new SqlCommand("SELECT * FROM bakTur WHERE TurId !=3 ", conn2);

            SqlDataReader reader2;

            reader2 = sc.ExecuteReader();
            DataTable dt3 = new DataTable();
            dt3.Columns.Add("TurID", typeof(string));
            dt3.Columns.Add("Turu", typeof(string));

            dt3.Load(reader2);
            conn2.Close();
            return dt3;
        }
        public DataTable VardiyalarYukle()
        {
            SqlConnection conn3 = new SqlConnection(frmGiris.Database);
            conn3.Open();
            SqlCommand sc = new SqlCommand("SELECT * FROM bakVardiyalar", conn3);

            SqlDataReader reader3;

            reader3 = sc.ExecuteReader();
            DataTable dt4 = new DataTable();
            dt4.Columns.Add("VardiyaID", typeof(string));
            dt4.Columns.Add("Vardiya", typeof(string));

            dt4.Load(reader3);
            conn3.Close();
            return dt4;
        }

        public DataTable IslemYukle()
        {
            SqlConnection conn4 = new SqlConnection(frmGiris.Database);
            conn4.Open();
            SqlCommand sc = new SqlCommand("SELECT * FROM bakIslem WHERE IslemID=1 OR IslemID=2 ", conn4);

            SqlDataReader reader4;

            reader4 = sc.ExecuteReader();
            DataTable dt5 = new DataTable();
            dt5.Columns.Add("IslemID", typeof(string));
            dt5.Columns.Add("Tur", typeof(string));

            dt5.Load(reader4);
            conn4.Close();
            return dt5;
        }
        public DataTable MakineYukle()
        {
            SqlConnection conn4 = new SqlConnection(frmGiris.Database);
            conn4.Open();
            SqlCommand sc4 = new SqlCommand("SELECT * FROM bakMakineler", conn4);

            SqlDataReader reader4;

            reader4 = sc4.ExecuteReader();
            DataTable dt6 = new DataTable();
            dt6.Columns.Add("MakineID", typeof(string));
            dt6.Columns.Add("MakineAdi", typeof(string));

            dt6.Load(reader4);
            conn4.Close();
            return dt6;
        }

        public DataTable tableListele(int ArizaNo)
        {
            DataTable dtProd = new DataTable();
            dtProd = dtTablom.Select("ArizaID=" + ArizaNo).CopyToDataTable();
            return dtProd;
        }

        public DataTable tableListele2()
        {
            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            dateTimePicker1.Value = firstDayOfMonth;
            date = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            dateTimePicker1.Value = date;
            SqlConnection sqlConn = new SqlConnection();
            sqlConn.ConnectionString = frmGiris.Database;

            SqlCommand sqCom = new SqlCommand();
            sqCom.Connection = sqlConn;
            sqCom.CommandText = @"SELECT Tbl1.ArizaID, Tbl1.Tarih, Tbl6.IslemTuru, Tbl3.Turu
, Tbl2.Bolum, Tbl8.MakineAdi, TblA.ArizaKodlari, Tbl1.Yapilanlar, TblN.Isimler, Tbl5.Vardiya
, CONVERT(VARCHAR(5),Tbl1.BaslamaSaati,108) AS BaslamaSaati
, CONVERT(VARCHAR(5),Tbl1.BitisSaati,108) AS BitisSaati
, (SELECT CONVERT(varchar(5), (DATEADD(MINUTE, (SELECT DATEDIFF(MINUTE, Tbl1.BaslamaSaati, Tbl1.BitisSaati))
, (SELECT DATEDIFF(HOUR, Tbl1.BaslamaSaati, Tbl1.BitisSaati )))), 108)) AS ToplamSure
,Tbl1.Durus, Tbl1.ArizaKodu, Tbl1.Personel, Tbl1.Cozuldu, Tbl1.FormVar
FROM bakArizalar Tbl1
LEFT JOIN bakBolumler Tbl2 ON Tbl1.Bolum = Tbl2.BolumID
LEFT JOIN bakMakineler Tbl8 ON Tbl1.Makine = Tbl8.MakineID
LEFT JOIN bakTur Tbl3 ON Tbl1.Tur = Tbl3.TurID
LEFT JOIN bakPersoneller Tbl4 ON Tbl1.Personel = Tbl4.PersonelID
LEFT JOIN bakVardiyalar Tbl5 ON Tbl1.Vardiya = Tbl5.VardiyaID
LEFT JOIN bakIslem Tbl6 ON Tbl1.Islem = Tbl6.IslemID
LEFT JOIN bakArizaKodlari Tbl7 ON Tbl1.ArizaKodu = Tbl7.ArizaKoduID
LEFT JOIN(SELECT DISTINCT o.ArizaID
, Isimler = STUFF((SELECT ', ' + b.PersonelAdi
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS Personel
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(Personel
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar )t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS a
INNER JOIN[dbo].[bakPersoneller] AS b ON a.Personel = b.PersonelID
WHERE a.ArizaID = o.ArizaID FOR XML PATH, TYPE).value(N'.[1]', N'varchar(max)'), 1, 2, '')
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS Personel
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(Personel, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS o) AS TblN
ON Tbl1.ArizaID = TblN.ArizaID
LEFT JOIN (SELECT DISTINCT o.ArizaID
, ArizaKodlari = STUFF((SELECT ', ' + b.ArizaKodu
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS ArizaKodu
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(ArizaKodu
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS a
INNER JOIN[dbo].[bakArizaKodlari] AS b ON a.ArizaKodu = b.ArizaKoduID
WHERE a.ArizaID = o.ArizaID FOR XML PATH, TYPE).value(N'.[1]', N'varchar(max)'), 1, 2, '')
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS ArizaKodu
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(ArizaKodu
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM  bakArizalar )t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS o) AS TblA
ON Tbl1.ArizaID = TblA.ArizaID 
WHERE Tbl1.Tarih BETWEEN convert(datetime, '" + dateTimePicker1.Value + "', 105) AND convert(datetime, '" + dateTimePicker2.Value + "', 105) ORDER BY Tbl1.ArizaID";
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dtProd2 = new DataTable();
            da.Fill(dtProd2);
            return dtProd2;
        }
        private void veriListele(string sorgu)
        {
            Cursor.Current = Cursors.WaitCursor;
            string dun = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy 00:00:00");
            string dun2 = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy 23:59:59");
            string bugun = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            string bugun2 = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
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

            lblTarih.Text = DateTime.Now.ToString();

            RowsColor();
            btnResize();

            Cursor.Current = Cursors.Default;

        }
        private void comboboxYukle2()
        { 
            cmbBolum.SelectedIndex = -1;
            cmbBolum.ValueMember = "BolumID";
            cmbBolum.DisplayMember = "Bolum";
            cmbBolum.DataSource = dtBolumTablom;
            
            cmbArizaTipi.SelectedIndex = -1;
            cmbArizaTipi.ValueMember = "TurID";
            cmbArizaTipi.DisplayMember = "Turu";
            cmbArizaTipi.DataSource = dtTurTablom;
          
            cmbVardiya.SelectedIndex = -1;
            cmbVardiya.ValueMember = "VardiyaID";
            cmbVardiya.DisplayMember = "Vardiya";
            cmbVardiya.DataSource = dtVardiyalarTablom;

            cmbTuru.SelectedIndex = -1;
            cmbTuru.ValueMember = "IslemID";
            cmbTuru.DisplayMember = "IslemTuru";
            cmbTuru.DataSource = dtIslemTablom;

            cmbMakine.SelectedIndex = -1;
            cmbMakine.ValueMember = "MakineID";
            cmbMakine.DisplayMember = "IslemTuru";
            cmbMakine.DataSource = dtMakinelerTablom;

            cmbArizaKodu.SelectedIndex = -1;
            cmbArizaKodu.ValueMember = "ArizaKoduID";
            cmbArizaKodu.DisplayMember = "ArizaKodu";
            cmbArizaKodu.DataSource = dtArizaTablom;

            cmbPersonel.SelectedIndex = -1;
            cmbPersonel.ValueMember = "PersonelID";
            cmbPersonel.DisplayMember = "PersonelAdi";
            cmbPersonel.DataSource = dtPerTablom;

            cmbCozuldu.SelectedIndex = 0;
            
        }
        private void veriListeleHepsi()
        {
            string dun = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy 00:00:00");
            string dun2 = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy 23:59:59");
            string bugun = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            string bugun2 = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            veriListele(@"SELECT Tbl1.ArizaID, Tbl1.Tarih, Tbl6.IslemTuru, Tbl3.Turu, Tbl2.Bolum
, Tbl8.MakineAdi, TblA.ArizaKodlari, Tbl1.Yapilanlar, TblN.Isimler, Tbl5.Vardiya
, CONVERT(VARCHAR(5),Tbl1.BaslamaSaati,108) AS BaslamaSaati
, CONVERT(VARCHAR(5),Tbl1.BitisSaati,108) AS BitisSaati
, (SELECT CONVERT(varchar(5), (DATEADD(MINUTE, (SELECT DATEDIFF(MINUTE, Tbl1.BaslamaSaati, Tbl1.BitisSaati))
, (SELECT DATEDIFF(HOUR, Tbl1.BaslamaSaati, Tbl1.BitisSaati )))), 108)) AS ToplamSure
, Tbl1.Durus, Tbl1.ArizaKodu, Tbl1.Personel, Tbl1.Cozuldu, Tbl1.FormVar
FROM bakArizalar Tbl1
LEFT JOIN bakBolumler Tbl2 ON Tbl1.Bolum = Tbl2.BolumID
LEFT JOIN bakMakineler Tbl8 ON Tbl1.Makine = Tbl8.MakineID
LEFT JOIN bakTur Tbl3 ON Tbl1.Tur = Tbl3.TurID
LEFT JOIN bakPersoneller Tbl4 ON Tbl1.Personel = Tbl4.PersonelID
LEFT JOIN bakVardiyalar Tbl5 ON Tbl1.Vardiya = Tbl5.VardiyaID
LEFT JOIN bakIslem Tbl6 ON Tbl1.Islem = Tbl6.IslemID
LEFT JOIN bakArizaKodlari Tbl7 ON Tbl1.ArizaKodu = Tbl7.ArizaKoduID
LEFT JOIN(SELECT DISTINCT o.ArizaID
, Isimler = STUFF((SELECT ', ' + b.PersonelAdi
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS Personel
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(Personel
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS a
INNER JOIN[dbo].[bakPersoneller] AS b ON a.Personel = b.PersonelID
WHERE a.ArizaID = o.ArizaID FOR XML PATH, TYPE).value(N'.[1]', N'varchar(max)'), 1, 2, '')
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS Personel
FROM ( SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(Personel
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS o) AS TblN ON Tbl1.ArizaID = TblN.ArizaID
LEFT JOIN (SELECT DISTINCT o.ArizaID
, ArizaKodlari = STUFF((SELECT ', ' + b.ArizaKodu
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS ArizaKodu
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(ArizaKodu
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS a
INNER JOIN[dbo].[bakArizaKodlari] AS b ON a.ArizaKodu = b.ArizaKoduID
WHERE a.ArizaID = o.ArizaID FOR XML PATH, TYPE).value(N'.[1]', N'varchar(max)'), 1, 2, '')
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS ArizaKodu
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(ArizaKodu
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS o) AS TblA ON Tbl1.ArizaID = TblA.ArizaID WHERE YEAR(Tbl1.Tarih)='" + sec + "' ORDER BY Tbl1.ArizaID ");

        }

        private void listeleDunBugun()
        {
            string dun = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy 00:00:00");
            string dun2 = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy 23:59:59");
            string bugun = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            string bugun2 = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            veriListele(@"SELECT Tbl1.ArizaID, Tbl1.Tarih, Tbl6.IslemTuru, Tbl3.Turu, Tbl2.Bolum
, Tbl8.MakineAdi, TblA.ArizaKodlari, Tbl1.Yapilanlar, TblN.Isimler, Tbl5.Vardiya
, CONVERT(VARCHAR(5),Tbl1.BaslamaSaati,108) AS BaslamaSaati
, CONVERT(VARCHAR(5),Tbl1.BitisSaati,108) AS BitisSaati
, (SELECT CONVERT(varchar(5), (DATEADD(MINUTE
, (SELECT DATEDIFF(MINUTE, Tbl1.BaslamaSaati, Tbl1.BitisSaati))
, (SELECT DATEDIFF(HOUR, Tbl1.BaslamaSaati, Tbl1.BitisSaati )))), 108)) AS ToplamSure
, Tbl1.Durus, Tbl1.ArizaKodu, Tbl1.Personel, Tbl1.Cozuldu, Tbl1.FormVar
FROM bakArizalar Tbl1
LEFT JOIN bakBolumler Tbl2 ON Tbl1.Bolum = Tbl2.BolumID
LEFT JOIN bakMakineler Tbl8 ON Tbl1.Makine = Tbl8.MakineID
LEFT JOIN bakTur Tbl3 ON Tbl1.Tur = Tbl3.TurID
LEFT JOIN bakPersoneller Tbl4 ON Tbl1.Personel = Tbl4.PersonelID
LEFT JOIN bakVardiyalar Tbl5 ON Tbl1.Vardiya = Tbl5.VardiyaID
LEFT JOIN bakIslem Tbl6 ON Tbl1.Islem = Tbl6.IslemID
LEFT JOIN bakArizaKodlari Tbl7 ON Tbl1.ArizaKodu = Tbl7.ArizaKoduID
LEFT JOIN(SELECT DISTINCT o.ArizaID
, Isimler = STUFF((SELECT ', ' + b.PersonelAdi
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS Personel
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(Personel
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS a
INNER JOIN[dbo].[bakPersoneller] AS b ON a.Personel = b.PersonelID
WHERE a.ArizaID = o.ArizaID FOR XML PATH, TYPE).value(N'.[1]', N'varchar(max)'), 1, 2, '')
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS Personel
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(Personel
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS o) AS TblN
ON Tbl1.ArizaID = TblN.ArizaID
LEFT JOIN (SELECT DISTINCT o.ArizaID
, ArizaKodlari = STUFF((SELECT ', ' + b.ArizaKodu
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS ArizaKodu
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(ArizaKodu
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS a
INNER JOIN[dbo].[bakArizaKodlari] AS b
ON a.ArizaKodu = b.ArizaKoduID
WHERE a.ArizaID = o.ArizaID FOR XML PATH, TYPE).value(N'.[1]', N'varchar(max)'), 1, 2, '')
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS ArizaKodu
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(ArizaKodu
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS o) AS TblA
ON Tbl1.ArizaID = TblA.ArizaID
WHERE Tbl1.Tarih BETWEEN convert(datetime, '" + dun + "', 105) AND convert(datetime, '" + bugun2 + "', 105) ORDER BY Tbl1.ArizaID");
        }

        private void ListeleBuAy()
        {
            DateTime date = DateTime.Now;
            var firsDAyOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDAyOfMonth = firsDAyOfMonth.AddMonths(1).AddMonths(-1);
            dateTimePicker1.Value = firsDAyOfMonth;
            date = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            dateTimePicker2.Value = date;
            veriListele(@"SELECT Tbl1.ArizaID, Tbl1.Tarih, Tbl6.IslemTuru, Tbl3.Turu, Tbl2.Bolum
, Tbl8.MakineAdi, TblA.ArizaKodlari, Tbl1.Yapilanlar, TblN.Isimler, Tbl5.Vardiya
, CONVERT(VARCHAR(5),Tbl1.BaslamaSaati,108) AS BaslamaSaati
, CONVERT(VARCHAR(5),Tbl1.BitisSaati,108) AS BitisSaati
, (SELECT CONVERT(varchar(5), (DATEADD(MINUTE
, (SELECT DATEDIFF(MINUTE, Tbl1.BaslamaSaati, Tbl1.BitisSaati))
, (SELECT DATEDIFF(HOUR, Tbl1.BaslamaSaati, Tbl1.BitisSaati )))), 108)) AS ToplamSure
, Tbl1.Durus, Tbl1.ArizaKodu, Tbl1.Personel, Tbl1.Cozuldu, Tbl1.FormVar
FROM bakArizalar Tbl1
LEFT JOIN bakBolumler Tbl2 ON Tbl1.Bolum = Tbl2.BolumID
LEFT JOIN bakMakineler Tbl8 ON Tbl1.Makine = Tbl8.MakineID
LEFT JOIN bakTur Tbl3 ON Tbl1.Tur = Tbl3.TurID
LEFT JOIN bakPersoneller Tbl4 ON Tbl1.Personel = Tbl4.PersonelID
LEFT JOIN bakVardiyalar Tbl5 ON Tbl1.Vardiya = Tbl5.VardiyaID
LEFT JOIN bakIslem Tbl6 ON Tbl1.Islem = Tbl6.IslemID
LEFT JOIN bakArizaKodlari Tbl7 ON Tbl1.ArizaKodu = Tbl7.ArizaKoduID
LEFT JOIN(SELECT DISTINCT o.ArizaID
, Isimler = STUFF((SELECT ', ' + b.PersonelAdi
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS Personel
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(Personel
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS a
INNER JOIN[dbo].[bakPersoneller] AS b ON a.Personel = b.PersonelID
WHERE a.ArizaID = o.ArizaID FOR XML PATH, TYPE).value(N'.[1]', N'varchar(max)'), 1, 2, '')
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS Personel
FROM ( SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(Personel
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS o) AS TblN
ON Tbl1.ArizaID = TblN.ArizaID
LEFT JOIN (SELECT DISTINCT o.ArizaID
, ArizaKodlari = STUFF((SELECT ', ' + b.ArizaKodu
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS ArizaKodu
FROM ( SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(ArizaKodu
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS a
INNER JOIN[dbo].[bakArizaKodlari] AS b ON a.ArizaKodu = b.ArizaKoduID
WHERE a.ArizaID = o.ArizaID FOR XML PATH, TYPE).value(N'.[1]', N'varchar(max)'), 1, 2, '')
FROM (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS ArizaKodu
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(ArizaKodu, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM  bakArizalar )t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) AS o) AS TblA
ON Tbl1.ArizaID = TblA.ArizaID
WHERE Tbl1.Tarih BETWEEN convert(datetime, '" + dateTimePicker1.Value + "', 105) AND convert(datetime, '" + dateTimePicker2.Value + "', 105) ORDER BY Tbl1.ArizaID");
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
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Blue;
                }
            }
        }

        private void btnResize()
        {
            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void sonKayıtlarıUsteGetir()
        {
            int jumpToRow = dataGridView1.Rows.Count - 1;
            if (dataGridView1.Rows.Count >= jumpToRow && jumpToRow >= 1)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = jumpToRow;
            }
        }

        private void LoadData(string kisim)
        {
            DataTable dtProd= new DataTable();
            dtProd = dtPerTablom.Select("Tur=" + kisim).CopyToDataTable();

            DataTable dtProd2 = new DataTable();
            dtProd2 = dtArizaTablom.Select("Tur=" + kisim).CopyToDataTable();
        }
        private void LoadData2(string bolum)
        {
            DataTable dtProd = new DataTable();
            dtProd = dtMakinelerTablom.Select("BolumID=" + bolum).CopyToDataTable();

        }

        private void LoadData3(string kisim)
        {
            DataTable dtProd = new DataTable();
            dtProd = dtPerTablom.Select("Tur=" + kisim).CopyToDataTable();
           
            
            cmbPersonel.ValueMember = "PersonelID";
            cmbPersonel.DisplayMember = "PersonelAdi";
            cmbPersonel.DataSource = dtProd;
            cmbPersonel.SelectedIndex = -1;

            DataRow[] foundRows = dtArizaTablom.Select("Tur= '3' OR Tur=" + kisim, "ArizaKodu ASC");
            DataTable dt = foundRows.CopyToDataTable();

            cmbArizaKodu.DataSource = dt;

            cmbArizaKodu.ValueMember = "ArizaKoduID";
            cmbArizaKodu.DisplayMember = "ArizaKodu";
            cmbArizaKodu.SelectedIndex = -1;
        }
        private void LoadData4(string bolumNo)
        {

            DataTable dtProd = new DataTable();
           // dtProd = dtMakinelerTablom.Select("MakineID= " + bolumNo).CopyToDataTable();

            cmbMakine.ValueMember = "MakineID";
            cmbMakine.DisplayMember = "MakineAdi";
            cmbMakine.DataSource = dtProd;
            cmbMakine.SelectedIndex = -1;


        }
        public void personelSeciliOlmasin()
        {
            foreach (int indexChecked in lstPersonel.CheckedIndices)
            {
                lstPersonel.SetItemChecked(indexChecked, false);

            }
            lstPersonel.ClearSelected();
        }

        public void arizaKodlariSeciliOlmasin()
        {
            foreach (int indexChecked in lstArizaKodlari.CheckedIndices)
            {
                lstArizaKodlari.SetItemChecked(indexChecked, false);
            }
            lstArizaKodlari.ClearSelected();
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
                    dt2 = dtPerTablom2.Select("PersonelID=" + item.ToString()).CopyToDataTable();

                    int index = lstPersonel.FindString(dt2.Rows[0][1].ToString());
                    if (index < 0)
                    {
                        MessageBox.Show("Item Bulunamdı");
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
                        MessageBox.Show("Item Bulunamdı");
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

        private void LoadData5(string kisim)
        {
            DataTable dtProd = new DataTable();
            dtProd = dtPerTablom.Select("Tur=" + kisim).CopyToDataTable();

            DataTable dtP = new DataTable();
            dtP = dtProd;
          
            lstPersonel.DataSource = dtP;
            lstPersonel.ValueMember = "PersonelID";
            lstPersonel.ValueMember = "PersonelAdi";
           
            lstPersonel.ClearSelected();

            DataRow[] foundRows = dtArizaTablom.Select("Tur= '3' OR Tur=" + kisim, "ArizaKodu ASC");
            DataTable dt = foundRows.CopyToDataTable();

            lstArizaKodlari.DataSource = dt;
            lstArizaKodlari.ValueMember = "ArizaKoduID";
            lstArizaKodlari.ValueMember = "ArizaKodu";
            lstArizaKodlari.ClearSelected();


        }

    }
}
