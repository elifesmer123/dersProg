using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace dersProgramı
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            InitializeDatabase(); // 💡 Veritabanını ilk çalıştırmada kur

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        /// <summary>
        /// Veritabanı yoksa oluşturur ve script dosyasını çalıştırır.
        /// </summary>
        static void InitializeDatabase()
        {
            string dbName = "DersProgDb";
            string masterConnectionString = "Server=.;Database=master;Trusted_Connection=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(masterConnectionString))
                {
                    connection.Open();

                    // Veritabanı mevcut mu?
                    string checkDbQuery = $"SELECT db_id('{dbName}')";
                    SqlCommand checkDbCmd = new SqlCommand(checkDbQuery, connection);
                    object result = checkDbCmd.ExecuteScalar();

                    if (result == DBNull.Value || result == null)
                    {
                        // Script dosyasını oku
                        string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DersProgDb.sql");

                        if (!File.Exists(scriptPath))
                        {
                            MessageBox.Show("Veritabanı script dosyası bulunamadı: " + scriptPath);
                            return;
                        }

                        string script = File.ReadAllText(scriptPath);

                        // "GO" komutlarıyla ayır
                        string[] commands = script.Split(
                            new[] { "GO", "Go", "go" },
                            StringSplitOptions.RemoveEmptyEntries);

                        foreach (string commandText in commands)
                        {
                            if (!string.IsNullOrWhiteSpace(commandText))
                            {
                                using (SqlCommand cmd = new SqlCommand(commandText, connection))
                                {
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        MessageBox.Show("Veritabanı başarıyla oluşturuldu.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı oluşturulurken hata oluştu:\n" + ex.Message);
            }
        }
    }
}

