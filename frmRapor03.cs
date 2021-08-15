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
    public partial class frmRapor03 : Form
    {
        public frmRapor03()
        {
            InitializeComponent();
        }
        public static int grupYetki;

        private void frmRapor03_Load(object sender, EventArgs e)
        {
            grupYetki = Convert.ToInt32(frmGiris.gonderilecekyetli);
            if (grupYetki != 0)
            {
                btnAktar.Visible = true;
            }
            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(2019, 1, 1);
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
                dataGridView1.DataSource = null;
                SqlConnection sqlConn = new SqlConnection();

                sqlConn.ConnectionString = frmGiris.Database;
                sqlConn.Open();

                SqlCommand sqCom = new SqlCommand();
                sqCom.Connection = sqlConn;
                sqCom.CommandText = @"SELECT Tbl1.ArizaID, Tbl1.Tarih, TblN.Isimler
, Tbl2.Vardiya, CONVERT(VARCHAR(5),Tbl1.BaslamaSaati,108) AS Baslama
, CONVERT(VARCHAR(5),Tbl1.BitisSaati,108) AS Bitis
FROM bakArizalar Tbl1 LEFT JOIN bakVardiyalar Tbl2 ON Tbl1.Vardiya = Tbl2.VardiyaID
LEFT JOIN(SELECT DISTINCT o.ArizaID, Isimler = STUFF((SELECT ', ' + b.PersonelAdi
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
WHERE (Tbl1.Vardiya = 1
AND CAST(Tbl1.BaslamaSaati as time) not between '08:00' and '15:59'
OR Tbl1.Vardiya = 1 AND CAST(Tbl1.BitisSaati as time)
not between '08:00' and '16:00' OR Tbl1.Vardiya = 2
AND CAST(Tbl1.BaslamaSaati as time) not between '16:00'
and '23:59' OR Tbl1.Vardiya = 2 AND CAST(Tbl1.BitisSaati as time)
not between '16:00' and '23:59' OR Tbl1.Vardiya = 3
AND CAST(Tbl1.BaslamaSaati as time) not between '00:00'
and '08:00' OR Tbl1.Vardiya = 3 AND CAST(Tbl1.BitisSaati as time)
not between '00:00' and '08:00' OR Tbl1.Vardiya = 4
AND CAST(Tbl1.BaslamaSaati as time) not between '08:00' and '17:00'
OR Tbl1.Vardiya = 4 AND CAST(Tbl1.BitisSaati as time)
not between '08:00' and '17:00')
AND Tbl1.Tarih BETWEEN convert(datetime, '" + dateTimePicker1.Value + "', 105) AND convert(datetime, '" + dateTimePicker2.Value + "', 105) ORDER BY Tbl1.ArizaID";
                sqCom.ExecuteNonQuery();
                DataTable dtProd = new DataTable();
                SqlDataAdapter sqDa = new SqlDataAdapter();
                sqDa.SelectCommand = sqCom;
                sqlConn.Close();
                sqDa.Fill(dtProd);
                dataGridView1.DataSource = dtProd;
                btnResize();
               

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

        private void btnAktar_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Documents(*.xls)|*.xls";
            sfd.FileName = "VArdiya ile Müdahale Saatleri Uymayanlar Raporu.xls";
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
