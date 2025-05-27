using LibrarieModele;
using NivelStocareDate;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Windows.Forms;

namespace InterfataUtilizator_WindowsForms
{
    public class AutentificareClient : Form
    {
        private TextBox txtNume, txtEmail, txtTelefon, txtCNP, txtParola, txtConfirmaParola;
        private Button btnContinua;
        private AdministrareClienti_FisierText adminClienti;

        public AutentificareClient()
        {
            this.Text = "Înregistrare Client Nou";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(700, 500);
            this.BackColor = ColorTranslator.FromHtml("#e3f2fd");

            string numeFisierClienti = "clienti.txt";
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string caleCompletaFisierClienti = Path.Combine(locatieFisierSolutie, numeFisierClienti);
            adminClienti = new AdministrareClienti_FisierText(caleCompletaFisierClienti);

            ConfigurareFormular();
        }

        private void ConfigurareFormular()
        {
            int xLabel = 150;
            int xTextBox = 250;
            int yStart = 50;
            int spacingY = 40;

            // Etichete și textboxuri
            this.Controls.AddRange(new Control[]
            {
                new Label() { Text = "Nume:", Location = new Point(xLabel, yStart), Width = 80, TextAlign = ContentAlignment.MiddleRight },
                txtNume = new TextBox() { Location = new Point(xTextBox, yStart), Width = 200 },

                new Label() { Text = "Email:", Location = new Point(xLabel, yStart + spacingY), Width = 80, TextAlign = ContentAlignment.MiddleRight },
                txtEmail = new TextBox() { Location = new Point(xTextBox, yStart + spacingY), Width = 200 },

                new Label() { Text = "Telefon:", Location = new Point(xLabel, yStart + spacingY * 2), Width = 80, TextAlign = ContentAlignment.MiddleRight },
                txtTelefon = new TextBox() { Location = new Point(xTextBox, yStart + spacingY * 2), Width = 200 },

                new Label() { Text = "CNP:", Location = new Point(xLabel, yStart + spacingY * 3), Width = 80, TextAlign = ContentAlignment.MiddleRight },
                txtCNP = new TextBox() { Location = new Point(xTextBox, yStart + spacingY * 3), Width = 200 },

                new Label() { Text = "Parolă:", Location = new Point(xLabel, yStart + spacingY * 4), Width = 80, TextAlign = ContentAlignment.MiddleRight },
                txtParola = new TextBox() { Location = new Point(xTextBox, yStart + spacingY * 4), Width = 200, UseSystemPasswordChar = true },

                new Label() { Text = "Confirmă:", Location = new Point(xLabel, yStart + spacingY * 5), Width = 80, TextAlign = ContentAlignment.MiddleRight },
                txtConfirmaParola = new TextBox() { Location = new Point(xTextBox, yStart + spacingY * 5), Width = 200, UseSystemPasswordChar = true }
            });

            // Buton
            btnContinua = new Button()
            {
                Text = "Înregistrare",
                Location = new Point(xTextBox, yStart + spacingY * 6 + 10),
                Width = 200,
                Height = 35,
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnContinua.Click += BtnContinua_Click;
            this.Controls.Add(btnContinua);

            // Buton înapoi
            Button btnBack = new Button()
            {
                Text = "Înapoi",
                Location = new Point(xTextBox, yStart + spacingY * 7 + 20),
                Width = 200,
                Height = 35,
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBack.Click += (s, e) => 
            {
                ClientLoginOptions options = new ClientLoginOptions();
                options.Show();
                this.Close();
            };
            this.Controls.Add(btnBack);
        }

        private void BtnContinua_Click(object sender, EventArgs e)
        {
            string nume = txtNume.Text.Trim();
            string email = txtEmail.Text.Trim();
            string telefon = txtTelefon.Text.Trim();
            string cnp = txtCNP.Text.Trim();
            string parola = txtParola.Text;
            string confirmaParola = txtConfirmaParola.Text;

            if (string.IsNullOrWhiteSpace(nume) || string.IsNullOrWhiteSpace(email) ||
                telefon.Length != 10 || !telefon.All(char.IsDigit) ||
                cnp.Length != 13 || !cnp.All(char.IsDigit) || !IsValidEmail(email))
            {
                MessageBox.Show("Date invalide. Asigură-te că:\n- CNP: 13 cifre\n- Telefon: 10 cifre\n- Email corect.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(parola) || parola.Length < 6)
            {
                MessageBox.Show("Parola trebuie să aibă minim 6 caractere.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (parola != confirmaParola)
            {
                MessageBox.Show("Parolele nu coincid.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var clienti = adminClienti.GetClienti();
            Client clientExistent = clienti.FirstOrDefault(c => c.CNP == cnp || c.email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (clientExistent != null)
            {
                MessageBox.Show("Există deja un cont cu acest CNP sau email.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idNou = clienti.Any() ? clienti.Max(c => c.IdClient) + 1 : 1;
            var clientNou = new Client(idNou, nume, email, cnp, telefon, parola);
            adminClienti.AddClient(clientNou);

            MessageBox.Show("Cont creat cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Deschizi dashboard-ul pentru client
            DashboardClient dashClient = new DashboardClient(clientNou);
            dashClient.Show();
            this.Hide();
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
