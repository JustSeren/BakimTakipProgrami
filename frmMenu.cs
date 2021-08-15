using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BAkimTAkipProgramii
{
    public partial class frmMenu : Form
    {
        public frmMenu()
        {
            InitializeComponent();
        }
       public static int yetki;
        private void frmMenu_Load(object sender, EventArgs e)
        {
            this.Text = frmGiris.gonderilecekveri;
            yetki = Convert.ToInt32(frmGiris.gonderilecekyetli);
        }
        private void cıkısToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void basamaklaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void yatayOlarakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void dikeyOlarakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void tumPencereleriKucultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                frm.WindowState = FormWindowState.Minimized;
            }
        }

        private void hakkindaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpener<frmHakkında>.Open(this); 
        }

        private void personelEkleDegistirSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpener<frmPersoneller>.Open(this);
        }

        private void arizaKodlariEkleDegistirSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpener<frmArizaKodlari>.Open(this);
        }

        private void makinelerEkleDegistirSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpener<frmMakineler>.Open(this);
        }

        private void bolumEkleDegistirSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpener<frmBolumler>.Open(this);
        }

        private void sifreDegişikligiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpener<frmSifreDegisikligi>.Open(this);
        }

        private void tarihKilitleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpener<frmKilitTarih>.Open(this);
        }

        private void oNERILERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpener<frmOneriler>.Open(this);
        }

        private void dUYURULARToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpener<frmDuyurular>.Open(this);
        }

        private void iSPLANLARIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpener<frmIşPlanlari>.Open(this);
        }

        private void pERIYODIKBAKIMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpener<frmPeriyodikBakim>.Open(this);
        }

        private void bAKIMLARToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpener<frmBakim>.Open(this);

        }

        private void personelArizaVeFaliyetSureleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpener<frmRapor01>.Open(this);
        }

        private void personelDetayliArizaVeFaaliyetSureleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpener<frmRapor02>.Open(this);
        }

        private void vardiyaIleMudahaleSureleriUymayanlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpener<frmRapor03>.Open(this);
        }

        private void bolumOlarakDurusSureleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpener<frmRapor04>.Open(this);
        }

        private void arizaKodlarinaGoreDurusSureleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpener<frmRapor05>.Open(this);
        }

        private void bolumMakineArizaKodlarinaGoreDurusSuresiAdetleriRaporlariToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpener<frmRapor06>.Open(this);

        }

        private void arizaKodlarinaGoreMudahaleSuresiAdetleriRaporuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpener<frmRapor07>.Open(this);

        }

        private void bolumMakineArizaKodlarinaMudahaleSuresiAdetleriRaporuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpener<frmRapor08>.Open(this);
        }

        private void arizaVeFaaliyetRaporlariToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpener<frmRapor09>.Open(this);
        }

        private void periyodikBakimKayitFormuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpener<frmRapor10>.Open(this);
        }

        private void mekanikElektrikArizaKayitFormuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpener<frmRapor11>.Open(this);
        }
    }
    public static class FormOpener<T> where T : Form
    {
       public static void Open(Form mdiContainer)
        {
            foreach (Form SelectedFrm in mdiContainer.MdiChildren)
            {
                if (SelectedFrm is T)
                {
                    SelectedFrm.Activate();
                    return;
                }
            }
            T frm = (T)Activator.CreateInstance(typeof(T));
            frm.MdiParent = mdiContainer;
            frm.Show();
        }
    }
}
