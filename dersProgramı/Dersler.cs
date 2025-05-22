using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace dersProgramı
{
    public partial class Dersler : Form
    {

        public string OldName { get; set; }
        public Dersler()
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
            DersEkle dersEkle = new DersEkle();
            dersEkle.Show();
            this.Hide();
        }
        private void VerileriListele()
        {
            string connectionString = "Server=.;Database=DersProgDb;Trusted_Connection=True;";
            string query = "SELECT Name FROM Subjects";

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
                            listBox1.Items.Add(reader["Name"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void Dersler_Load(object sender, EventArgs e)
        {
            VerileriListele();
        }
      
        private void button6_Click_1(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir Ders seçin!");
                return;
            }

            string name = listBox1.SelectedItem.ToString();

            string connectionString = "Server=.;Database=DersProgDb;Trusted_Connection=True;";
            string updateQuery = "DELETE FROM Subjects WHERE Name = @Name";

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
                MessageBox.Show("Hata" + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir Ders seçin!");
                return;
            }

            string eskiAd = OldName;
            string yeniAd = textBox3.Text.Trim();

            if (string.IsNullOrEmpty(yeniAd))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun!");
                return;
            }

            string connectionString = "Server=.;Database=DersProgDb;Trusted_Connection=True;";
            string updateQuery = "UPDATE Subjects SET Name = @YeniAd WHERE Name = @EskiAd";


            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@YeniAd", yeniAd);
                        command.Parameters.AddWithValue("@EskiAd", eskiAd);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Güncelleme başarılı!");
                            VerileriListele(); // Listeyi yenile

                            button2.Visible = false;
                           
                            textBox3.Visible = false;
                            label1.Visible = false;
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir Ders seçin!");
                return;
            }
            string selectedItem = listBox1.SelectedItem.ToString();

            textBox3.Text = selectedItem; // İlk kısım Name
            OldName = selectedItem;
           

            button2.Visible = true;       
            textBox3.Visible = true;
            label1.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir ders seçiniz.");
                return;
            }

            // Formu kapat ve geri dön
            this.Close();
            MainForm mainForm = new MainForm();
            // Seçilen derslik sakla
            MainForm.SelectedDers = listBox1.SelectedItem.ToString();
            mainForm.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm();
            mainForm.Show();
            this.Hide();
        }
    }
}
