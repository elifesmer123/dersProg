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
using static dersProgramı.DerslikEkle;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace dersProgramı
{
    public partial class BolumEkle : Form
    {
        public int SelectedUserId { get; set; }
        public BolumEkle()
        {
            InitializeComponent();
        }
        private void BolumEkle_Load(object sender, EventArgs e)
        {
            // ComboBox'a veri doldur
            LoadUsers();
        }

        private void LoadUsers()
        {

            string connectionString = "Server=.;Database=DersProgDb;Trusted_Connection=True;";
            string query = "SELECT Id, Username FROM Users";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // ComboBox'a veri bağlama
                            while (reader.Read())
                            {
                                comboBox1.Items.Add(new ComboBoxItem
                                {
                                    Id = reader.GetInt32(0),
                                    Username = reader.GetString(1)
                                });
                            }
                        }
                    }
                }

                // Görüntülenecek alanı ayarla
                comboBox1.DisplayMember = "Username";
                comboBox1.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri yüklenirken hata oluştu: " + ex.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=.;Database=DersProgDb;Trusted_Connection=True;";

            string query = "INSERT INTO Departmans (UserId, Name) VALUES (@UserId, @Name)";

            if (comboBox1.SelectedItem is ComboBoxItem selectedItem)
            {
                this.SelectedUserId = selectedItem.Id;
            }

            int userId = this.SelectedUserId;
            string name = textBox1.Text;
            // SQL bağlantısını aç
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parametreleri ekle
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@Name", name);

                        // Sorguyu çalıştır
                        int result = command.ExecuteNonQuery();

                        // Kullanıcıya bilgi ver
                        if (result > 0)
                        {
                            MessageBox.Show("Kayıt başarılı!");
                            Bolumler bolumler = new Bolumler();
                            bolumler.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Kayıt başarısız.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata: {ex.Message}");
                }
            }
        }
        

        public class ComboBoxItem
        {
            public int Id { get; set; }
            public string Username { get; set; }

            public override string ToString()
            {
                return Username; // ComboBox'ta sadece Username görünecek
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm();
            mainForm.Show();
            this.Hide();
        }
    }
}
