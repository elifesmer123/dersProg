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
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();

            string userName = textBox1.Text;
            string password = textBox2.Text;

            if (userName != "ekremesrefkilinc" && userName != "mustafacatak" && userName != "zelihakaramanokay" && userName != "fatmafeyzakaya" && userName != "samiulukus" && userName != "emrahbeydilli" && userName != "deryaagcadag" && userName != "fatihkarabacak" && userName != "hasanhuseyinumutlu" && userName != "ozgurduden" && userName != "aysetürkmen" && userName != "hasankebapci" && userName != "aliikanir" && userName != "burcugokolgun" && userName != "oguzhanerdogan")
            {
                MessageBox.Show("Böyle bir hoca bulunmamaktadır");
                return;
            }


            string connectionString = "Server=.;Database=DersProgDb;Trusted_Connection=True;";

            string query = "INSERT INTO users (Username, Password) VALUES (@UserName, @Password)";

            // SQL bağlantısını aç
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

                        // Sorguyu çalıştır
                        int result = command.ExecuteNonQuery();

                        // Kullanıcıya bilgi ver
                        if (result > 0)
                        {
                            MessageBox.Show("Kayıt başarılı!");
                            login.Show();
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
    }
}
