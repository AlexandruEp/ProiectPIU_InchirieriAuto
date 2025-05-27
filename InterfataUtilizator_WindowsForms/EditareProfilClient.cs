using LibrarieModele;
using NivelStocareDate;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms; 

namespace InterfataUtilizator_WindowsForms
{
    public class EditareProfilClient : Form
    {
        private TextBox txtNume, txtEmail, txtTelefon, txtCNP;
        private Button btnSalveaza;
        private Client client;
        private AdministrareClienti_FisierText adminClienti;

        public EditareProfilClient(string cnp)
        {
            this.Text = "Editează Profil Client";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(500, 300);
            this.BackColor = ColorTranslator.FromHtml("#e3f2fd");

            string numeFisierClienti = "clienti.txt";
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string caleFisier = Path.Combine(locatieFisierSolutie, numeFisierClienti);
            adminClienti = new AdministrareClienti_FisierText(caleFisier);

            client = adminClienti.GetClienti().FirstOrDefault(c => c.CNP == cnp);

            if (client == null)
            {
                MessageBox.Show("Clientul nu a fost găsit!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            InitializeComponent();
            PrecompletareDate();
        }

        private void InitializeComponent()
        {
            int xLabel = 100;
            int xTextBox = 200;
            int yStart = 30;
            int spacingY = 40;

            Label lblNume = new Label() { Text = "Nume:", Location = new Point(xLabel, yStart), Width = 80 };
            txtNume = new TextBox() { Location = new Point(xTextBox, yStart), Width = 200 };

            Label lblEmail = new Label() { Text = "Email:", Location = new Point(xLabel, yStart + spacingY), Width = 80 };
            txtEmail = new TextBox() { Location = new Point(xTextBox, yStart + spacingY), Width = 200 };

            Label lblTelefon = new Label() { Text = "Telefon:", Location = new Point(xLabel, yStart + spacingY * 2), Width = 80 };
            txtTelefon = new TextBox() { Location = new Point(xTextBox, yStart + spacingY * 2), Width = 200 };

            Label lblCNP = new Label() { Text = "CNP:", Location = new Point(xLabel, yStart + spacingY * 3), Width = 80 };
            txtCNP = new TextBox() { Location = new Point(xTextBox, yStart + spacingY * 3), Width = 200, ReadOnly = true };

            Label lblParola = new Label() { Text = "Parolă nouă:", Location = new Point(xLabel, yStart + spacingY * 4), Width = 80 };
            TextBox txtParola = new TextBox() { Location = new Point(xTextBox, yStart + spacingY * 4), Width = 200, UseSystemPasswordChar = true };

            Label lblConfirmaParola = new Label() { Text = "Confirmă:", Location = new Point(xLabel, yStart + spacingY * 5), Width = 80 };
            TextBox txtConfirmaParola = new TextBox() { Location = new Point(xTextBox, yStart + spacingY * 5), Width = 200, UseSystemPasswordChar = true };

            btnSalveaza = new Button()
            {
                Text = "Salvează",
                Location = new Point(xTextBox, yStart + spacingY * 6),
                Size = new Size(200, 35),
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSalveaza.Click += (s, e) =>
            {
                // Validare date
                if (string.IsNullOrWhiteSpace(txtNume.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) ||
                    txtTelefon.Text.Length != 10 || !txtTelefon.Text.All(char.IsDigit))
                {
                    MessageBox.Show("Date invalide. Asigură-te că:\n- Numele și emailul sunt completate\n- Telefonul conține exact 10 cifre", 
                        "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validare parolă dacă a fost introdusă
                if (!string.IsNullOrWhiteSpace(txtParola.Text))
                {
                    if (txtParola.Text.Length < 6)
                    {
                        MessageBox.Show("Parola trebuie să aibă minim 6 caractere.", 
                            "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (txtParola.Text != txtConfirmaParola.Text)
                    {
                        MessageBox.Show("Parolele nu coincid.", 
                            "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    client.parola = txtParola.Text;
                }

                client.nume = txtNume.Text.Trim();
                client.email = txtEmail.Text.Trim();
                client.telefon = txtTelefon.Text.Trim();

                var clienti = adminClienti.GetClienti();
                var index = clienti.FindIndex(c => c.IdClient == client.IdClient);

                if (index != -1)
                {
                    clienti[index] = client;
                    adminClienti.SalveazaClienti(clienti);
                    MessageBox.Show("Profil actualizat cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            };

            this.Controls.AddRange(new Control[] 
            { 
                lblNume, txtNume,
                lblEmail, txtEmail,
                lblTelefon, txtTelefon,
                lblCNP, txtCNP,
                lblParola, txtParola,
                lblConfirmaParola, txtConfirmaParola,
                btnSalveaza 
            });
        }

        private void PrecompletareDate()
        {
            txtNume.Text = client.nume;
            txtEmail.Text = client.email;
            txtTelefon.Text = client.telefon;
            txtCNP.Text = client.CNP;
        }
    }
}
