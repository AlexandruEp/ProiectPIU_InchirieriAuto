using LibrarieModele;
using NivelStocareDate;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Net.Mail;

namespace InterfataUtilizator_WindowsForms
{
    public partial class AdaugareClienti : Form
    {
        private TextBox txtNume, txtEmail, txtTelefon, txtCNP;
        private Button btnAdaugaClient;
        private Button btnBack;
        private AdministrareClienti_FisierText adminClienti;

        public AdaugareClienti()
        {
            InitializeComponent();
            this.Text = "Adăugare Client";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(700, 500);

            // Inițializare adminClienti
            string numeFisierClienti = "clienti.txt";
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string caleCompletaFisierClienti = Path.Combine(locatieFisierSolutie, numeFisierClienti);
            adminClienti = new AdministrareClienti_FisierText(caleCompletaFisierClienti);

            ConfigurareInputForm();
        }

        private void ConfigurareInputForm()
        {
            // Etichete și TextBox-uri
            int xLabel = 150;
            int xTextBox = 250;
            int yStart = 50;
            int spacingY = 50;

            // Nume
            Label lblNume = new Label() { Text = "Nume:", Location = new Point(xLabel, yStart), Width = 80, TextAlign = ContentAlignment.MiddleRight };
            txtNume = new TextBox() { Location = new Point(xTextBox, yStart), Width = 200 };

            // Email
            Label lblEmail = new Label() { Text = "Email:", Location = new Point(xLabel, yStart + spacingY), Width = 80, TextAlign = ContentAlignment.MiddleRight };
            txtEmail = new TextBox() { Location = new Point(xTextBox, yStart + spacingY), Width = 200 };

            // Telefon
            Label lblTelefon = new Label() { Text = "Telefon:", Location = new Point(xLabel, yStart + spacingY * 2), Width = 80, TextAlign = ContentAlignment.MiddleRight };
            txtTelefon = new TextBox() { Location = new Point(xTextBox, yStart + spacingY * 2), Width = 200 };

            // CNP
            Label lblCNP = new Label() { Text = "CNP:", Location = new Point(xLabel, yStart + spacingY * 3), Width = 80, TextAlign = ContentAlignment.MiddleRight };
            txtCNP = new TextBox() { Location = new Point(xTextBox, yStart + spacingY * 3), Width = 200 };

            // Buton
            btnAdaugaClient = new Button()
            {
                Text = "Adaugă Client",
                Location = new Point(xTextBox, yStart + spacingY * 4 + 10),
                Width = 200,
                Height = 35,
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnAdaugaClient.Click += BtnAdaugaClient_Click;

            // Adaugă controalele în formular
            this.Controls.Add(lblNume);
            this.Controls.Add(txtNume);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblTelefon);
            this.Controls.Add(txtTelefon);
            this.Controls.Add(lblCNP);
            this.Controls.Add(txtCNP);
            this.Controls.Add(btnAdaugaClient);


            btnBack = new Button()
            {
                Text = "Înapoi",
                Location = new Point(xTextBox + 65, yStart + spacingY * 5),
                Size = new Size(80, 30),
                BackColor = Color.MediumAquamarine,
                FlatStyle = FlatStyle.Flat
            };
            btnBack.Click += BtnBack_Click;
            this.Controls.Add(btnBack);
        }


        private void BtnAdaugaClient_Click(object sender, EventArgs e)
        {

            string nume = txtNume.Text.Trim();
            string email = txtEmail.Text.Trim();
            string telefon = txtTelefon.Text.Trim();
            string cnp = txtCNP.Text.Trim();

            if (string.IsNullOrWhiteSpace(nume) || 
                string.IsNullOrWhiteSpace(email) ||
                telefon.Length != 10 || 
                cnp.Length != 13 ||
                !telefon.All(char.IsDigit) || 
                !cnp.All(char.IsDigit) ||
                !IsValidEmail(email))
            {
                MessageBox.Show("Toate câmpurile trebuie completate corect:\n- Telefon: 10 cifre\n- CNP: 13 cifre", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var clientiExistenti = adminClienti.GetClienti();
            int idNou = clientiExistenti.Any() ? clientiExistenti.Max(c => c.IdClient) + 1 : 1;

            Client clientNou = new Client(idNou, nume, email, cnp, telefon);
            adminClienti.AddClient(clientNou);

            MessageBox.Show("Client adăugat cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtNume.Clear();
            txtEmail.Clear();
            txtTelefon.Clear();
            txtCNP.Clear();
            txtNume.Focus();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private bool IsValidEmail(string email)
        {
            try
            {
                var mail = new MailAddress(email);
                return mail.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}

