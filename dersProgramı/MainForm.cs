using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace dersProgramı
{
    public partial class MainForm : Form
    {
        public static string SelectedHoca { get; set; }
        public static string SelectedBolum { get; set; }
        public static string SelectedDerslik { get; set; }
        public static string SelectedDers { get; set; }
        public static string SelectedGunler { get; set; }

        public static List<string> savedData = new List<string>();
        private string connectionString = "Server=.;Database=DersProgDb;Trusted_Connection=True;";

        public MainForm()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Dersler dersler = new Dersler();
            dersler.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hocalar hocalar = new Hocalar();
            hocalar.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Derslikler derslik = new Derslikler();
            derslik.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bolumler bolumler = new Bolumler();
            bolumler.Show();
            this.Hide();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            label1.Text = $"Seçilen Hoca: {SelectedHoca}";
            label2.Text = $"Seçilen Bölümler: {SelectedBolum}";
            label3.Text = $"Seçilen Derslik: {SelectedDerslik}";
            label4.Text = $"Ders atannmayacak Günler: {SelectedGunler}";
            label5.Text = $"Seçilen Ders: {SelectedDers}";
            listBox1.Items.Clear();
            listBox1.Items.AddRange(savedData.ToArray());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SelectedHoca) && !string.IsNullOrEmpty(SelectedBolum) &&
                !string.IsNullOrEmpty(SelectedDerslik) && !string.IsNullOrEmpty(SelectedDers))
            {
                string kayit = $"{SelectedHoca}, {SelectedBolum}, {SelectedDerslik}, {SelectedDers}";
                listBox1.Items.Add(kayit);
                savedData.Add(kayit);

                SelectedHoca = null;
                SelectedBolum = null;
                SelectedDerslik = null;
                SelectedDers = null;

                label1.Text = $"Seçilen Hoca: {SelectedHoca}";
                label2.Text = $"Seçilen Bölümler: {SelectedBolum}";
                label3.Text = $"Seçilen Derslik: {SelectedDerslik}";
                label5.Text = $"Seçilen Ders: {SelectedDers}";

                MessageBox.Show("Seçilen veriler başarıyla kaydedildi.");
            }
            else
            {
                MessageBox.Show("Lütfen tüm alanları seçtiğinizden emin olun.");
            }
        }

        public void GetHocalarVeDerslerByBolum(string bolumAdi)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string bolumQuery = "SELECT Id FROM Departmans WHERE Name = @BolumAdi";
                    SqlCommand bolumCmd = new SqlCommand(bolumQuery, connection);
                    bolumCmd.Parameters.AddWithValue("@BolumAdi", bolumAdi);

                    object bolumIdObj = bolumCmd.ExecuteScalar();
                    if (bolumIdObj == null)
                    {
                        MessageBox.Show("Bölüm bulunamadı.");
                        return;
                    }
                    int bolumId = Convert.ToInt32(bolumIdObj);

                    string hocalarQuery = "SELECT Id, Username FROM Users WHERE Id IN (SELECT UserId FROM Departmans WHERE Id = @BolumId)";
                    SqlCommand hocalarCmd = new SqlCommand(hocalarQuery, connection);
                    hocalarCmd.Parameters.AddWithValue("@BolumId", bolumId);

                    List<int> hocaIds = new List<int>();
                    Dictionary<int, string> hocaAdlari = new Dictionary<int, string>();

                    using (SqlDataReader reader = hocalarCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int hocaId = reader.GetInt32(0);
                            string hocaAdi = reader.GetString(1);
                            hocaIds.Add(hocaId);
                            hocaAdlari[hocaId] = hocaAdi;
                        }
                    }

                    foreach (int hocaId in hocaIds)
                    {
                        string dersQuery = "SELECT Name FROM Subjects WHERE UserId = @UserId";
                        SqlCommand dersCmd = new SqlCommand(dersQuery, connection);
                        dersCmd.Parameters.AddWithValue("@UserId", hocaId);

                        List<string> dersler = new List<string>();
                        using (SqlDataReader dersReader = dersCmd.ExecuteReader())
                        {
                            while (dersReader.Read())
                            {
                                dersler.Add(dersReader.GetString(0));
                            }
                        }

                        // Derslik bilgisini al
                        string derslikQuery = "SELECT Name FROM Classes WHERE UserId = @UserId";
                        SqlCommand derslikCmd = new SqlCommand(derslikQuery, connection);
                        derslikCmd.Parameters.AddWithValue("@UserId", hocaId);

                        string derslikAdi = "(Derslik Seçilmedi)";
                        object derslikObj = derslikCmd.ExecuteScalar();
                        if (derslikObj != null)
                        {
                            derslikAdi = derslikObj.ToString();
                        }

                        // Verileri listele
                        foreach (string dersAdi in dersler)
                        {
                            string kayit = $"{hocaAdlari[hocaId]}, {bolumAdi}, {derslikAdi}, {dersAdi}";
                            savedData.Add(kayit);
                            listBox1.Items.Add(kayit);
                        }
                    }

                    MessageBox.Show("Bölüme ait hoca, ders ve derslik bilgileri başarıyla yüklendi.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veri alınırken hata oluştu: " + ex.Message);
                }
            }
        }

        private int GetAktsForDers(string dersAdi)
        {
            int akts = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT AKTS FROM subjects WHERE Name = @DersAdi";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DersAdi", dersAdi.Split(',')[3].Trim());

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        akts = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("AKTS bilgisi alınırken hata oluştu: " + ex.Message);
                }
            }
            return akts;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedGunler))
            {
                MessageBox.Show("Lütfen önce günleri seçin.");
                return;
            }

            List<string> tumGunler = new List<string> { "PAZARTESI", "SALI", "CARSAMBA", "PERSEMBE", "CUMA" };
            List<string> seciliGunler = new List<string>(SelectedGunler.Split('-'));

            List<string> kullanilacakGunler = tumGunler.Except(seciliGunler).ToList();

            if (kullanilacakGunler.Count == 0)
            {
                MessageBox.Show("Tüm günler seçili, ders atanamaz!");
                return;
            }

            // Güncellenmiş ders saatleri (12:00-13:00 tam 1 saat öğlen arası)
            List<string> dersSaatleri = new List<string>
{
    "09:00 - 09:50", "10:00 - 10:50", "11:00 - 11:50",
    "12:00 - 13:00", // 1 saat tam öğlen arası
    "13:00 - 13:50", "14:00 - 14:50", "15:00 - 15:50",
    "16:00 - 16:50", "17:00 - 17:50", "18:00 - 18:50"
};

            string pdfDosyasi = @"C:\Users\NESA\Desktop\dersProgramı.pdf";

            Dictionary<string, Dictionary<string, string>> dersProgrami = new Dictionary<string, Dictionary<string, string>>();
            foreach (var gun in tumGunler)
            {
                dersProgrami[gun] = new Dictionary<string, string>();
            }

            List<string> dersler = new List<string>();
            foreach (var item in listBox1.Items)
            {
                dersler.Add(item.ToString());
            }

            // Önce 6 AKTS'lik dersleri yerleştir
            var altiAktsDersler = dersler.Where(d => GetAktsForDers(d) == 6).ToList();
            var digerDersler = dersler.Except(altiAktsDersler).ToList();

            Random random = new Random();

            // 6 AKTS'lik dersleri yerleştir
            foreach (string ders in altiAktsDersler)
            {
                if (kullanilacakGunler.Count < 2)
                {
                    MessageBox.Show("6 AKTS'lik dersler için yeterli gün yok!");
                    continue;
                }

                string gun1 = kullanilacakGunler[random.Next(kullanilacakGunler.Count)];
                string gun2 = kullanilacakGunler.Where(g => g != gun1).FirstOrDefault();

                if (gun2 == null)
                {
                    MessageBox.Show("6 AKTS'lik dersler için yeterli gün yok!");
                    continue;
                }

                // İlk gün için 4 saatlik blok bul (öğlen arasını atlayarak)
                bool ilkDersYerlesti = false;
                for (int i = 0; i < dersSaatleri.Count - 3; i++)
                {
                    // Öğlen arasına denk geliyorsa atla
                    if (dersSaatleri[i] == "12:00 - 13:00" ||
                        dersSaatleri[i + 1] == "12:00 - 13:00" ||
                        dersSaatleri[i + 2] == "12:00 - 13:00" ||
                        dersSaatleri[i + 3] == "12:00 - 13:00")
                        continue;

                    if (!dersProgrami[gun1].ContainsKey(dersSaatleri[i]) &&
                        !dersProgrami[gun1].ContainsKey(dersSaatleri[i + 1]) &&
                        !dersProgrami[gun1].ContainsKey(dersSaatleri[i + 2]) &&
                        !dersProgrami[gun1].ContainsKey(dersSaatleri[i + 3]))
                    {
                        dersProgrami[gun1][dersSaatleri[i]] = ders;
                        dersProgrami[gun1][dersSaatleri[i + 1]] = ders;
                        dersProgrami[gun1][dersSaatleri[i + 2]] = ders;
                        dersProgrami[gun1][dersSaatleri[i + 3]] = ders;
                        ilkDersYerlesti = true;
                        break;
                    }
                }

                if (!ilkDersYerlesti)
                {
                    MessageBox.Show($"{gun1} gününde 4 saatlik uygun blok bulunamadı!");
                    continue;
                }

                // İkinci gün için 2 saatlik blok bul
                bool ikinciDersYerlesti = false;
                for (int i = 0; i < dersSaatleri.Count - 1; i++)
                {
                    // Öğlen arasına denk geliyorsa atla
                    if (dersSaatleri[i] == "12:00 - 13:00" ||
                        dersSaatleri[i + 1] == "12:00 - 13:00")
                        continue;

                    if (!dersProgrami[gun2].ContainsKey(dersSaatleri[i]) &&
                        !dersProgrami[gun2].ContainsKey(dersSaatleri[i + 1]))
                    {
                        dersProgrami[gun2][dersSaatleri[i]] = ders;
                        dersProgrami[gun2][dersSaatleri[i + 1]] = ders;
                        ikinciDersYerlesti = true;
                        break;
                    }
                }

                if (!ikinciDersYerlesti)
                {
                    MessageBox.Show($"{gun2} gününde 2 saatlik uygun blok bulunamadı!");
                    // İlk yerleştirilen dersleri temizle
                    for (int i = 0; i < dersSaatleri.Count; i++)
                    {
                        if (dersProgrami[gun1].ContainsKey(dersSaatleri[i]) &&
                            dersProgrami[gun1][dersSaatleri[i]] == ders)
                        {
                            dersProgrami[gun1].Remove(dersSaatleri[i]);
                        }
                    }
                    continue;
                }
            }

            // Diğer dersleri yerleştir (2 ve 4 AKTS'lik)
            foreach (string ders in digerDersler)
            {
                int akts = GetAktsForDers(ders);
                if (akts == 0) continue;

                int requiredSlots = akts / 2;
                int placedSlots = 0;

                while (placedSlots < requiredSlots && kullanilacakGunler.Count > 0)
                {
                    string randomGun = kullanilacakGunler[random.Next(kullanilacakGunler.Count)];

                    for (int i = 0; i < dersSaatleri.Count - (akts == 4 ? 3 : 1); i++)
                    {
                        // Öğlen arası kontrolü
                        bool oglenArasiVar = false;
                        for (int j = 0; j <= (akts == 4 ? 3 : 1); j++)
                        {
                            if (dersSaatleri[i + j] == "12:00 - 13:00")
                            {
                                oglenArasiVar = true;
                                break;
                            }
                        }
                        if (oglenArasiVar) continue;

                        if (akts == 4 && i < dersSaatleri.Count - 3)
                        {
                            if (!dersProgrami[randomGun].ContainsKey(dersSaatleri[i]) &&
                                !dersProgrami[randomGun].ContainsKey(dersSaatleri[i + 1]) &&
                                !dersProgrami[randomGun].ContainsKey(dersSaatleri[i + 2]) &&
                                !dersProgrami[randomGun].ContainsKey(dersSaatleri[i + 3]))
                            {
                                dersProgrami[randomGun][dersSaatleri[i]] = ders;
                                dersProgrami[randomGun][dersSaatleri[i + 1]] = ders;
                                dersProgrami[randomGun][dersSaatleri[i + 2]] = ders;
                                dersProgrami[randomGun][dersSaatleri[i + 3]] = ders;
                                placedSlots += 2;
                                break;
                            }
                        }
                        else if (akts == 2)
                        {
                            if (!dersProgrami[randomGun].ContainsKey(dersSaatleri[i]) &&
                                !dersProgrami[randomGun].ContainsKey(dersSaatleri[i + 1]))
                            {
                                dersProgrami[randomGun][dersSaatleri[i]] = ders;
                                dersProgrami[randomGun][dersSaatleri[i + 1]] = ders;
                                placedSlots += 1;
                                break;
                            }
                        }
                    }
                }
            }

            // PDF oluşturma
            using (var writer = new PdfWriter(pdfDosyasi))
            using (var pdf = new PdfDocument(writer))
            {
                var document = new Document(pdf);

                Table tablo = new Table(tumGunler.Count + 1);
                tablo.AddCell("Saatler");
                foreach (var gun in tumGunler)
                {
                    tablo.AddCell(gun);
                }

                foreach (var saat in dersSaatleri)
                {
                    tablo.AddCell(saat);
                    foreach (var gun in tumGunler)
                    {
                        if (saat == "12:00 - 13:00")
                        {
                            tablo.AddCell("OGLEN ARASI");
                        }
                        else if (dersProgrami[gun].ContainsKey(saat))
                        {
                            tablo.AddCell(dersProgrami[gun][saat]);
                        }
                        else
                        {
                            tablo.AddCell("");
                        }
                    }
                }

                document.Add(tablo);
                document.Close();
                MessageBox.Show("dersProgramı.pdf başarıyla oluşturuldu.");
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            GunEkle gunEkle = new GunEkle();
            gunEkle.Show();
            this.Hide();
        }
    }
}
