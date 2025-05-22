using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dersProgramı
{
    public partial class GunEkle : Form
    {
        public GunEkle()
        {
            InitializeComponent();
        }

        private void GunEkle_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm();
            mainForm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Seçili olan checkbox'ların metinlerini tutacak bir liste
            List<string> selectedTexts = new List<string>();

            // Checkbox'lar üzerinde döngü
            foreach (Control control in this.Controls)
            {
                // Eğer kontrol bir Checkbox ise ve seçiliyse
                if (control is CheckBox checkBox && checkBox.Checked)
                {
                    selectedTexts.Add(checkBox.Text); // Seçili olan checkbox'ın metnini ekle
                }
            }

            // Sonuçları ekranda göster
            if (selectedTexts.Count > 0)
            {
                string gunler = "";
                foreach (var item in selectedTexts)
                {
                    gunler += item + "-";
                }
                
                MainForm mainForm = new MainForm();
                MainForm.SelectedGunler = gunler;
                mainForm.Show();
                this.Close();
            }
            else
            {
                 MessageBox.Show("Lütfen Gün Seçin");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
