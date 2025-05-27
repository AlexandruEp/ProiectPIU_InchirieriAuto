using LibrarieModele;
using NivelStocareDate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace InterfataUtilizator_WindowsForms
{
    public class MasiniInchiriateClient : Form
    {
        private FlowLayoutPanel panelMasini;
        private readonly Client clientCurent;
        private readonly AdministrareInchirieri_FisierText adminInchirieri;
        private readonly AdministrareMasini_FisierText adminMasini;
        private Button btnBack;

        public MasiniInchiriateClient(Client client)
        {
            this.clientCurent = client;
            string locatie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            adminInchirieri = new AdministrareInchirieri_FisierText(Path.Combine(locatie, ConfigurationManager.AppSettings["NumeFisierInchirieri"]));
            adminMasini = new AdministrareMasini_FisierText(Path.Combine(locatie, ConfigurationManager.AppSettings["NumeFisierMasini"]));

            this.Text = "Mașinile Mele Închiriate";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ColorTranslator.FromHtml("#e3f2fd");

            InitializeComponents();
            AfiseazaInchirieri();
        }

        private void InitializeComponents()
        {
            // Panel pentru mașini
            panelMasini = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoScroll = true,
                Height = this.ClientSize.Height - 80,
                Padding = new Padding(10)
            };

            // Buton înapoi
            btnBack = new Button
            {
                Text = "Înapoi",
                Width = 200,
                Height = 35,
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point((this.ClientSize.Width - 200) / 2, this.ClientSize.Height - 50),
                Anchor = AnchorStyles.Bottom
            };
            btnBack.Click += (s, e) => this.Close();

            Controls.Add(panelMasini);
            Controls.Add(btnBack);
        }

        private void AfiseazaInchirieri()
        {
            try
            {
                panelMasini.Controls.Clear();

                var toateInchirierile = adminInchirieri.GetInchirieri();
                var aleMele = toateInchirierile.Where(i => i.IdClient == clientCurent.IdClient).ToList();
                var toateMasinile = adminMasini.GetMasini();

                if (!aleMele.Any())
                {
                    Label lblGol = new Label
                    {
                        Text = "Nu aveți mașini închiriate momentan.",
                        AutoSize = true,
                        Font = new Font("Segoe UI", 12),
                        ForeColor = Color.Gray,
                        Location = new Point(
                            (panelMasini.Width - 250) / 2,
                            panelMasini.Height / 3
                        )
                    };
                    panelMasini.Controls.Add(lblGol);
                    return;
                }

                foreach (var inchiriere in aleMele)
                {
                    Masina masina = toateMasinile.FirstOrDefault(m => m.IdMasina == inchiriere.IdMasina);
                    if (masina == null) continue;

                    Panel card = CreateMasinaCard(masina, inchiriere);
                    panelMasini.Controls.Add(card);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcarea închirierilor: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Panel CreateMasinaCard(Masina masina, Inchiriere inchiriere)
        {
            Panel card = new Panel
            {
                Width = 280,
                Height = 320,
                Margin = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Încărcare imagine cu eliberare corectă a resurselor
            PictureBox pic = new PictureBox
            {
                Width = 260,
                Height = 150,
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(10, 10)
            };

            if (File.Exists(masina.ImagePath))
            {
                try
                {
                    using (var stream = new MemoryStream(File.ReadAllBytes(masina.ImagePath)))
                    {
                        pic.Image = Image.FromStream(stream);
                    }
                }
                catch
                {
                    pic.Image = null;
                }
            }

            Label lbl = new Label
            {
                Text = $"{masina.Marca} {masina.Model}\n{masina.Combustibil}, {masina.Transmisie}\n" +
                       $"Perioadă: {inchiriere.DataStart:dd.MM.yyyy} - {inchiriere.DataEnd:dd.MM.yyyy}\n" +
                       $"Total: {inchiriere.PretTotal:F2} lei\n" +
                       $"Status: {(inchiriere.DataReturnare.HasValue ? $"Returnată la {inchiriere.DataReturnare.Value:dd.MM.yyyy}" : "Activă")}",
                Location = new Point(10, 170),
                Size = new Size(260, 100),
                Font = new Font("Segoe UI", 9),
                TextAlign = ContentAlignment.TopLeft
            };

            // Adăugăm butonul de returnare doar dacă închirierea este activă
            if (!inchiriere.DataReturnare.HasValue)
            {
                Button btnReturneaza = new Button
                {
                    Text = "Returnează",
                    Width = 120,
                    Height = 30,
                    Location = new Point(80, 280),
                    BackColor = Color.IndianRed,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };

                btnReturneaza.Click += (s, e) => ReturneazaMasina(masina, inchiriere);
                card.Controls.Add(btnReturneaza);
            }

            card.Controls.Add(pic);
            card.Controls.Add(lbl);

            return card;
        }

        private void ReturneazaMasina(Masina masina, Inchiriere inchiriere)
        {
            try
            {
                var confirm = MessageBox.Show($"Sigur vrei să returnezi {masina.Marca} {masina.Model}?",
                    "Confirmare returnare", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    var toateInchirierile = adminInchirieri.GetInchirieri();
                    var inchiriereActuala = toateInchirierile.FirstOrDefault(i => 
                        i.IdClient == clientCurent.IdClient &&
                        i.IdMasina == masina.IdMasina &&
                        i.DataStart == inchiriere.DataStart &&
                        i.DataEnd == inchiriere.DataEnd);

                    if (inchiriereActuala != null)
                    {
                        // Setăm data reală de returnare
                        inchiriereActuala.DataReturnare = DateTime.Now.Date;
                        
                        // Actualizăm lista de închirieri
                        var inchirieriActualizate = toateInchirierile.Where(i => 
                            !(i.IdClient == clientCurent.IdClient &&
                              i.IdMasina == masina.IdMasina &&
                              i.DataStart == inchiriere.DataStart &&
                              i.DataEnd == inchiriere.DataEnd)).ToList();
                        
                        inchirieriActualizate.Add(inchiriereActuala);
                        adminInchirieri.AddInchiriere(inchirieriActualizate);

                        // Actualizare status mașină
                        masina.Disponibil = true;
                        var masiniActualizate = adminMasini.GetMasini();
                        var index = masiniActualizate.FindIndex(m => m.IdMasina == masina.IdMasina);
                        if (index != -1)
                        {
                            masiniActualizate[index] = masina;
                            adminMasini.SalveazaMasini(masiniActualizate);
                        }

                        MessageBox.Show("Mașina a fost returnată cu succes.", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        AfiseazaInchirieri();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la returnarea mașinii: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            // Eliberăm resursele imaginilor
            foreach (Control control in panelMasini.Controls)
            {
                if (control is Panel panel)
                {
                    foreach (Control panelControl in panel.Controls)
                    {
                        if (panelControl is PictureBox pic && pic.Image != null)
                        {
                            pic.Image.Dispose();
                            pic.Image = null;
                        }
                    }
                }
            }
        }
    }
}
