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
    public partial class Form1 : Form
    {
        public static int sonders { get; set; }
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Register register = new Register();
            register.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userName = textBox1.Text;
            string password = textBox2.Text;

            string connectionString = "Server=.;Database=DersProgDb;Trusted_Connection=True;";

            // SQL sorgusu
            string query = "SELECT COUNT(*) FROM users WHERE Username = @UserName AND Password = @Password";

            // Veritabanına bağlan
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parametreleri ekle
                        command.Parameters.AddWithValue("@UserName", userName);
                        command.Parameters.AddWithValue("@Password", password);

                        // Sorguyu çalıştır ve sonuç al
                        int result = (int)command.ExecuteScalar();

                        if (result > 0)
                        {
                            // Giriş başarılı
                            MessageBox.Show("Giriş başarılı!");
                            // Ana sayfayı aç veya başka bir işlem yap
                            MainForm mainForm = new MainForm();
                            mainForm.Show();
                            this.Hide();
                        }
                        else
                        {
                            // Giriş başarısız
                            MessageBox.Show("Kullanıcı adı veya şifre hatalı.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata: {ex.Message}");
                }
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
