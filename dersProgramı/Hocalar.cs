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

namespace dersProgramı
{
    public partial class Hocalar : Form
    {
        public Hocalar()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm();
            mainForm.Show();
            this.Hide();
        }

        private void Hocalar_Load(object sender, EventArgs e)
        {
            VerileriListele();
        }

        private void VerileriListele()
        {
            string connectionString = "Server=.;Database=DersProgDb;Trusted_Connection=True;"; 
            string query = "SELECT Username FROM Users";

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
                            listBox1.Items.Add(reader["Username"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir hoca seçin!");
                return;
            }

            string eskiAd = listBox1.SelectedItem.ToString();
            string yeniAd = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(yeniAd))
            {
                MessageBox.Show("Lütfen yeni bir ad girin!");
                return;
            }

            string connectionString = "Server=.;Database=DersProgDb;Trusted_Connection=True;";
            string updateQuery = "UPDATE Users SET Username = @YeniAd WHERE Username = @EskiAd";

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
                            textBox1.Visible = false;
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
                MessageBox.Show("Lütfen bir hoca seçiniz.");
                return;
            }

            // Formu kapat ve geri dön
            this.Close();
            MainForm mainForm = new MainForm();
            // Seçilen hocayı sakla
            MainForm.SelectedHoca = listBox1.SelectedItem.ToString();
            mainForm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir hoca seçin!");
                return;
            }
            textBox1.Text = listBox1.SelectedItem.ToString();

            button2.Visible = true;
            textBox1.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {

            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir hoca seçin!");
                return;
            }

            string userName = listBox1.SelectedItem.ToString();

            string connectionString = "Server=.;Database=DersProgDb;Trusted_Connection=True;";
            string updateQuery = "DELETE FROM Users WHERE Username = @userName";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@userName", userName);

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
                MessageBox.Show("Hata: Bu hoca silinemez. Bağlantılı olduğu yerler var");
            }
        }
    }
}
