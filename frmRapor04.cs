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
    public partial class frmRapor04 : Form
    {
        public frmRapor04()
        {
            InitializeComponent();
        }
        public static int grupYetki;
        private void frmRapor04_Load(object sender, EventArgs e)
        {
            grupYetki = Convert.ToInt32(frmGiris.gonderilecekyetli);
            if (grupYetki != 0)
            {
                btnAktar.Visible = true;
            }
            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime( date.Year, date.Month, 1);
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
                sqCom.CommandText = @"SELECT Tbl2.Bolum AS [Bölüm]
, SUM(Tbl1.Durus) AS[Duruş(dk.)]
, CONCAT(((SUM(Tbl1.Durus)) * 1) / 60, ' saat '
, (SUM(Tbl1.Durus)) % 60, ' dk.') as [Duruş Saat, Dakika]
FROM bakArizalar Tbl1
LEFT JOIN bakBolumler Tbl2 ON Tbl1.Bolum = Tbl2.BolumID 
WHERE Tbl1.Tarih BETWEEN convert(datetime, '" + dateTimePicker1.Value + "', 105) AND convert(datetime, '" + dateTimePicker2.Value + "', 105) GROUP BY Tbl2.Bolum ORDER BY Tbl2.Bolum";
                sqCom.ExecuteNonQuery();
                DataTable dtProd = new DataTable();
                SqlDataAdapter sqDa = new SqlDataAdapter();
                sqDa.SelectCommand = sqCom;
                sqlConn.Close();
                sqDa.Fill(dtProd);
                dataGridView1.DataSource = dtProd;
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    int WidyhCol = dataGridView1.Columns[i].Width;
                    dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dataGridView1.Columns[i].Width = WidyhCol;
                    dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    //veri girişine göre datagridview ayarlanıyor 


                }
                dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.Automatic;
                Cursor.Current = Cursors.Default;

            }
            catch
            {
            }
        }
        private void btnAktar_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Documents(*.xls)|*.xls";
            sfd.FileName = "Bölüm Olarak Duruş Süreleri.xls";
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
