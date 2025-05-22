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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace dersProgramı
{
    public partial class Derslikler : Form
    {
        public string OldName { get; set; }
        public Derslikler()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm();
            mainForm.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DerslikEkle derslikEkle = new DerslikEkle();
            derslikEkle.Show();
            this.Hide();
        }

        private void Derslikler_Load(object sender, EventArgs e)
        {
            VerileriListele();
        }
        private void VerileriListele()
        {
            string connectionString = "Server=.;Database=DersProgDb;Trusted_Connection=True;";
            string query = "SELECT Name,Capacity FROM Classes";

            listBox1.Items.Clear();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listBox1.Items.Add(reader["Name"].ToString()+ "-" + reader["Capacity"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir Derslik seçin!");
                return;
            }
            string selectedItem = listBox1.SelectedItem.ToString();
            string[] parts = selectedItem.Split('-'); // "-" ile ayır

            textBox3.Text = parts[0].Trim(); // İlk kısım Name
            OldName = parts[0].Trim();
            textBox1.Text = parts[1].Trim(); // İkinci kısım Capacity

            button2.Visible = true;
            textBox1.Visible = true;
            textBox3.Visible = true;
            label1.Visible = true;
            label2.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir Derslik seçin!");
                return;
            }

            string eskiAd = OldName;
            string yeniAd = textBox3.Text.Trim();

            string yeniCapacity = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(yeniAd)&& string.IsNullOrEmpty(yeniCapacity))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun!");
                return;
            }

            string connectionString = "Server=.;Database=DersProgDb;Trusted_Connection=True;"; 
            string updateQuery = "UPDATE Classes SET Name = @YeniAd, Capacity = @YeniCapacity WHERE Name = @EskiAd";


            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@YeniAd", yeniAd);
                        command.Parameters.AddWithValue("@YeniCapacity", yeniCapacity);
                        command.Parameters.AddWithValue("@EskiAd", eskiAd);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Güncelleme başarılı!");
                            VerileriListele(); // Listeyi yenile

                            button2.Visible = false;
                            textBox1.Visible = false;
                            textBox3.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show("Güncelleme başarısız!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir derslik seçiniz.");
                return;
            }

            // Formu kapat ve geri dön
            this.Close();
            MainForm mainForm = new MainForm();
            // Seçilen derslik sakla
            MainForm.SelectedDerslik = listBox1.SelectedItem.ToString();
            mainForm.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir Derslik seçin!");
                return;
            }

            string selectedItem = listBox1.SelectedItem.ToString();
            string[] parts = selectedItem.Split('-'); // "-" ile ayır

            string name = parts[0].Trim(); // İlk kısım Name

            string connectionString = "Server=.;Database=DersProgDb;Trusted_Connection=True;";
            string updateQuery = "DELETE FROM Classes WHERE Name = @Name";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Silme başarılı!");
                            VerileriListele(); // Listeyi yenile
                        }
                        else
                        {
                            MessageBox.Show("Silme başarısız!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata"+ ex.Message);
            }
        }
    }
}
