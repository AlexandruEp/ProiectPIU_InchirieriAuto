using LibrarieModele;
using NivelStocareDate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace InterfataUtilizator_WindowsForms
{
    public class FormInchiriereMasina : Form
    {
        private DateTimePicker dtpStart, dtpEnd;
        private Label lblTotal;
        private Button btnInchiriaza, btnAnuleaza;

        private readonly Masina masina;
        private readonly Client client;
        private readonly AdministrareInchirieri_FisierText adminInchirieri;

        public FormInchiriereMasina(Masina masina, Client client)
        {
            this.masina = masina;
            this.client = client;

            this.Text = "Închiriere Mașină";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(450, 300);
            this.BackColor = ColorTranslator.FromHtml("#e3f2fd");

            string caleFisier = Path.Combine(
                Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
                ConfigurationManager.AppSettings["NumeFisierInchirieri"]
            );
            adminInchirieri = new AdministrareInchirieri_FisierText(caleFisier);

            InitializeForm();
        }

        private void InitializeForm()
        {
            Label lblStart = new Label
            {
                Text = "Dată început:",
                Location = new Point(30, 30),
                Width = 120
            };
            dtpStart = new DateTimePicker
            {
                Location = new Point(160, 30),
                Format = DateTimePickerFormat.Short
            };

            Label lblEnd = new Label
            {
                Text = "Dată sfârșit:",
                Location = new Point(30, 70),
                Width = 120
            };
            dtpEnd = new DateTimePicker
            {
                Location = new Point(160, 70),
                Format = DateTimePickerFormat.Short
            };
            dtpEnd.ValueChanged += CalculeazaTotal;
            dtpStart.ValueChanged += CalculeazaTotal;

            lblTotal = new Label
            {
                Text = "Total: 0 lei",
                Location = new Point(160, 110),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnInchiriaza = new Button
            {
                Text = "Închiriază",
                Location = new Point(80, 160),
                Size = new Size(120, 35),
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnInchiriaza.Click += BtnInchiriaza_Click;

            btnAnuleaza = new Button
            {
                Text = "Anulează",
                Location = new Point(220, 160),
                Size = new Size(120, 35),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAnuleaza.Click += (s, e) => this.Close();

            this.Controls.Add(lblStart);
            this.Controls.Add(dtpStart);
            this.Controls.Add(lblEnd);
            this.Controls.Add(dtpEnd);
            this.Controls.Add(lblTotal);
            this.Controls.Add(btnInchiriaza);
            this.Controls.Add(btnAnuleaza);

            CalculeazaTotal(null, null);
        }

        private void CalculeazaTotal(object sender, EventArgs e)
        {
            int nrZile = (int)(dtpEnd.Value.Date - dtpStart.Value.Date).TotalDays + 1;
            if (nrZile <= 0) nrZile = 0;
            double total = nrZile * masina.Pret;
            lblTotal.Text = $"Total: {total} lei";
        }

        private void BtnInchiriaza_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtpEnd.Value.Date < dtpStart.Value.Date)
                {
                    MessageBox.Show("Data de sfârșit nu poate fi anterioară celei de început!", "Atenție", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int nrZile = (int)(dtpEnd.Value.Date - dtpStart.Value.Date).TotalDays + 1;
                double total = nrZile * masina.Pret;

                var result = MessageBox.Show(
                    $"Confirmați închirierea mașinii {masina.Marca} {masina.Model}\n" +
                    $"Perioada: {dtpStart.Value.Date:dd.MM.yyyy} - {dtpEnd.Value.Date:dd.MM.yyyy}\n" +
                    $"Total de plată: {total} lei", 
                    "Confirmare închiriere",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Inchiriere inchiriere = new Inchiriere
                    {
                        IdMasina = masina.IdMasina,
                        IdClient = client.IdClient,
                        DataStart = dtpStart.Value.Date,
                        DataEnd = dtpEnd.Value.Date,
                        PretTotal = total
                    };

                    adminInchirieri.AdaugaInchiriere(inchiriere);
                    masina.Disponibil = false;

                    string numeFisierMasini = ConfigurationManager.AppSettings["NumeFisierMasini"];
                    string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                    string caleFisierMasini = Path.Combine(locatieFisierSolutie, numeFisierMasini);
                    var adminMasini = new AdministrareMasini_FisierText(caleFisierMasini);

                    List<Masina> masini = adminMasini.GetMasini();
                    int index = masini.FindIndex(m => m.IdMasina == masina.IdMasina);
                    if (index != -1)
                    {
                        masini[index] = masina;
                        adminMasini.SalveazaMasini(masini);
                    }

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("A apărut o eroare la procesarea închirierii. Vă rugăm să încercați din nou.",
                    "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
