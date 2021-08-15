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
    public partial class frmRapor02 : Form
    {
        public frmRapor02()
        {
            InitializeComponent();
        }
        // yetkilendirmelere göre işlem yapabilmek için değişken tanımladım.

        public static int grupYetki;

        private void frmRapor02_Load(object sender, EventArgs e)
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
            date = DateTime.Now.AddHours(23).AddMinutes(59).AddSeconds(59);
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
                dataGridView1.DataSource = null;
                SqlConnection sqlConn = new SqlConnection();

                sqlConn.ConnectionString = frmGiris.Database;
                sqlConn.Open();

                SqlCommand sqCom = new SqlCommand();
                sqCom.Connection = sqlConn;
                sqCom.CommandText = @"SELECT Tbl5.PersonelAdi
,CONCAT(((SUM(case when Tbl1.Islem = 1 then(Tbl1.ToplamSure) else 0 end) * 1) / 60), ' saat '
, ((SUM(case when Tbl1.Islem = 1 then(Tbl1.ToplamSure) else 0 end)) % 60), ' dk.') as [Ariza Saat, Dakika] 
,CONCAT(((SUM(case when Tbl1.Islem = 1 and FormVar = 0 
and Tbl1.Tarih>convert(datetime, '01.01.2019', 105) 
then (Tbl1.ToplamSure * -1) else 0 end) * 1) / 60), ' saat '
, ((SUM(case when Tbl1.Islem = 1  and FormVar = 0 
and Tbl1.Tarih>convert(datetime, '01.01.2019', 105) 
then(Tbl1.ToplamSure * -1) else 0 end)) % 60), ' dk.') as [Formsuz Arıza Saat, Dakika] 
,CONCAT(((SUM(case when Tbl1.Islem = 1 
then (Tbl1.ToplamSure) else 0 end) + SUM(case when Tbl1.Islem = 1 and FormVar = 0 
and Tbl1.Tarih>convert(datetime, '01.01.2019', 105) 
then (Tbl1.ToplamSure * -1) else 0 end)) * 1) / 60, ' saat ' 
, (SUM(case when Tbl1.Islem = 1 then (Tbl1.ToplamSure) 
else 0 end) + SUM(case when Tbl1.Islem = 1 and FormVar = 0 
and Tbl1.Tarih>convert(datetime, '01.01.2019', 105) 
then (Tbl1.ToplamSure * -1) else 0 end)) % 60, ' dk.') as [Geçerli Arıza Saat, Dakika] 
,CONCAT(((SUM(case when Tbl1.Islem = 2 then(Tbl1.ToplamSure) else 0 end) * 1) / 60), ' saat '
, ((SUM(case when Tbl1.Islem = 2 then(Tbl1.ToplamSure) 
else 0 end)) % 60), ' dk.') as [Faaliyet Saat, Dakika] 
,CONCAT(((SUM(case when Tbl1.Islem = 1 then(Tbl1.ToplamSure) 
else 0 end) + SUM(case when Tbl1.Islem = 1 and FormVar = 0 
and Tbl1.Tarih>convert(datetime, '01.01.2019', 105) 
then (Tbl1.ToplamSure * -1) else 0 end) + SUM(case when Tbl1.Islem = 2 
then (Tbl1.ToplamSure) else 0 end)) * 1) / 60, ' saat ' 
,(SUM(case when Tbl1.Islem = 1 then(Tbl1.ToplamSure) 
else 0 end) + SUM(case when Tbl1.Islem = 1 and FormVar = 0 
and Tbl1.Tarih>convert(datetime, '01.01.2019', 105) 
then (Tbl1.ToplamSure * -1) else 0 end) + SUM(case when Tbl1.Islem = 2 
then (Tbl1.ToplamSure) else 0 end)) % 60, ' dk.') as [TOPLAM Saat, Dakika]  
FROM bakArizalar Tbl1 LEFT JOIN bakTur Tbl3 ON Tbl1.Tur = Tbl3.TurID 
LEFT JOIN (SELECT ArizaID, LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS Personel 
FROM(SELECT ArizaID, CAST('<XMLRoot><RowData>' + REPLACE(Personel, '-', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x FROM  bakArizalar)t 
CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)) Tbl4 ON Tbl1.ArizaID = Tbl4.ArizaID 
LEFT JOIN bakPersoneller Tbl5 ON Tbl5.PersonelID = Tbl4.Personel 
LEFT JOIN bakIslem Tbl6 ON Tbl1.Islem = Tbl6.IslemID 
WHERE Tbl1.Tarih BETWEEN convert(datetime, '" + dateTimePicker1.Value + "', 105) " +
"AND convert(datetime, '" + dateTimePicker2.Value + "', 105) GROUP BY Tbl5.PersonelAdi " +
"ORDER BY Tbl5.PersonelAdi ASC";

                sqCom.ExecuteNonQuery();
                DataTable dtProd = new DataTable();
                SqlDataAdapter sqDa = new SqlDataAdapter();
                sqDa.SelectCommand = sqCom;
                sqlConn.Close();
                sqDa.Fill(dtProd);
                dataGridView1.DataSource = dtProd;
                btnResize();
                RowsColor();

                dataGridView1.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;


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
        public void RowsColor()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                double val = Convert.ToByte(dataGridView1.Rows[i].Cells[7].Value);
                if (val > 107)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;

                }
            }
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

        private void toCsV (DataGridView dGV, string fileName)
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

        private void btnOnceki_Click(object sender, EventArgs e)
        {
            //datetimepıcker 1 de saat başlangıcını gunun başını date tıme picker 2 de saat başlangıcını gunun sonu yapmak için oluşturudum.

            DateTime date = dateTimePicker1.Value;
            DateTime date2 = dateTimePicker2.Value;

            date = dateTimePicker1.Value.AddDays(-1).AddHours(00).AddMinutes(00).AddSeconds(00);
            dateTimePicker1.Value = date;

            date2 = date.AddHours(23).AddMinutes(59).AddSeconds(59);
            dateTimePicker2.Value = date2;
            veriListele();
        }

        private void btnSonraki_Click(object sender, EventArgs e)
        { //datetimepıcker 1 de saat başlangıcını gunun başını date tıme picker 2 de saat başlangıcını gunun sonu yapmak için oluşturudum.

            DateTime date = dateTimePicker1.Value;
            DateTime date2 = dateTimePicker2.Value;

            date = dateTimePicker1.Value.AddDays(1).AddHours(00).AddMinutes(00).AddSeconds(00);
            dateTimePicker1.Value = date;

            date2 = date.AddHours(23).AddMinutes(59).AddSeconds(59);
            dateTimePicker2.Value = date2;
            veriListele();
        }
    }
}
