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
    public partial class frmRapor05 : Form
    {
        public frmRapor05()
        {
            InitializeComponent();
        }
        public static int grupYetki;
        private void frmRapor05_Load(object sender, EventArgs e)
        {
            grupYetki = Convert.ToInt32(frmGiris.gonderilecekyetli);
            if (grupYetki != 0)
            {
                btnAktar.Visible = true;
            }
            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            dateTimePicker1.Value = firstDayOfMonth;
            date = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            dateTimePicker2.Value = date;
        }

        private void btnRaporla_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value <= dateTimePicker2.Value)
            {
                veriListele();

            }
            else
            {
                MessageBox.Show("Bitiş Tarihi Başlangıç Tarihinden Küçük Olamaz");
            }
        }
        public void veriListele()
        {

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                dataGridView1.DataSource = null;
                SqlConnection sqlConn = new SqlConnection();

        sqlConn.ConnectionString = frmGiris.Database;
                sqlConn.Open();

                SqlCommand sqCom = new SqlCommand();
        sqCom.Connection = sqlConn;

                SqlDataAdapter da = new SqlDataAdapter();
        sqCom.CommandText = @"SELECT Tbl4.ArizaKodu
, Tbl7.ArizaKodu AS [Ariza Kodu Açıklaması]
, [ARIZA(Durus Adet)] = COUNT(CASE WHEN Tbl1.Islem = 1 THEN Tbl1.ParcaDurus END)
, SUM(case when Tbl1.Islem = 1 then Tbl1.ParcaDurus else 0 end) as [ARIZA(dk.)]
, [FAALİYET(Durus Adet)] = COUNT(CASE WHEN Tbl1.Islem = 2 THEN Tbl1.ParcaDurus END)
, SUM(case when Tbl1.Islem = 2 then Tbl1.ParcaDurus else 0 end) as [FAALIYET(dk.)]
, (COUNT(CASE WHEN Tbl1.Islem = 1 THEN Tbl1.ParcaDurus END) + COUNT(CASE WHEN Tbl1.Islem = 2
THEN Tbl1.ParcaDurus END)) as [TOPLAM(Duruş Adet)]
, (SUM(case when Tbl1.Islem = 1 then Tbl1.ParcaDurus else 0 end) + SUM(case when Tbl1.Islem = 2
then Tbl1.ParcaDurus else 0 end)) as [TOPLAM Duruş(dk.)]
, CONCAT((((SUM(case when Tbl1.Islem = 1 then Tbl1.ParcaDurus
else 0 end) + SUM(case when Tbl1.Islem = 2 then Tbl1.ParcaDurus
else 0 end))) * 1) / 60 , ' saat ' , ((SUM(case when Tbl1.Islem = 1 then Tbl1.ParcaDurus
else 0 end) + SUM(case when Tbl1.Islem = 2
then Tbl1.ParcaDurus else 0 end))) % 60 , ' dk.')  as [TOPLAM Duruş Saat, Dakika]
FROM bakArizalar Tbl1 LEFT JOIN bakTur Tbl3 ON Tbl1.Tur = Tbl3.TurID
LEFT JOIN(SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS ArizaKodu
FROM (SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(ArizaKodu
, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM bakArizalar) t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) Tbl4
ON Tbl1.ArizaID = Tbl4.ArizaID
LEFT JOIN bakIslem Tbl6 ON Tbl1.Islem = Tbl6.IslemID
LEFT JOIN bakArizaKodlari Tbl7 ON Tbl4.ArizaKodu = Tbl7.ArizaKoduID
WHERE Tbl1.Tarih BETWEEN convert(datetime, '" + dateTimePicker1.Value + "', 105) AND convert(datetime, '" + dateTimePicker2.Value + "', 105) GROUP BY Tbl4.ArizaKodu, Tbl7.ArizaKodu HAVING(SUM(case when Tbl1.Islem = 1 then Tbl1.ParcaDurus else 0 end) > 0) OR(SUM(case when Tbl1.Islem = 2 then Tbl1.ParcaDurus else 0 end) > 0) ORDER BY Tbl7.ArizaKodu ASC";
         
                
                sqCom.ExecuteNonQuery();
                DataTable dtProd = new DataTable();
        
                SqlDataAdapter sqDa = new SqlDataAdapter();
        
                sqDa.SelectCommand = sqCom;
                sqlConn.Close();
                sqDa.Fill(dtProd);
                dataGridView1.DataSource = dtProd;
                btnResize();
              
                dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            catch
            {
            }
        }
        private void btnResize()
        {
            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        }
       

        private void btnAktar_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Documents(*.xls)|*.xls";
            sfd.FileName = "PersonelDEtaylı Arıza ve Faaliyet Süreleri.xls";
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

    }
}
