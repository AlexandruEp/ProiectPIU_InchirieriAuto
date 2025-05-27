using LibrarieModele;
using NivelStocareDate;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace InterfataUtilizator_WindowsForms
{
    public class AutentificareClientExistent : Form
    {
        private TextBox txtEmail, txtParola;
        private Button btnAutentificare;
        private Button btnBack;
        private AdministrareClienti_FisierText adminClienti;

        public AutentificareClientExistent()
        {
            this.Text = "Autentificare Client";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(500, 300);
            this.BackColor = ColorTranslator.FromHtml("#e3f2fd");

            string numeFisierClienti = "clienti.txt";
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string caleFisier = Path.Combine(locatieFisierSolutie, numeFisierClienti);
            adminClienti = new AdministrareClienti_FisierText(caleFisier);

            ConfigureazaComponente();
        }

        private void ConfigureazaComponente()
        {
            // Calculăm centrul formularului pentru TextBox-uri și butoane
            int formCenterX = this.ClientSize.Width / 2;
            int textBoxWidth = 200;
            int xTextBox = formCenterX - textBoxWidth / 2;
            int xLabel = xTextBox - 65; // Redus spațiul dintre label și TextBox
            int yStart = 50;
            int spacingY = 40;

            Label lblEmail = new Label() 
            { 
                Text = "Email:", 
                Location = new Point(xLabel, yStart), 
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft
            };

            txtEmail = new TextBox() 
            { 
                Location = new Point(xTextBox, yStart), 
                Width = textBoxWidth 
            };

            Label lblParola = new Label() 
            { 
                Text = "Parolă:", 
                Location = new Point(xLabel, yStart + spacingY), 
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft
            };

            txtParola = new TextBox() 
            { 
                Location = new Point(xTextBox, yStart + spacingY), 
                Width = textBoxWidth,
                UseSystemPasswordChar = true
            };

            btnAutentificare = new Button()
            {
                Text = "Autentificare",
                Location = new Point(xTextBox, yStart + spacingY * 2),
                Size = new Size(textBoxWidth, 35),
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAutentificare.Click += BtnAutentificare_Click;

            btnBack = new Button()
            {
                Text = "Înapoi",
                Location = new Point(xTextBox, yStart + spacingY * 3),
                Width = textBoxWidth,
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

            this.Controls.AddRange(new Control[] { lblEmail, txtEmail, lblParola, txtParola, btnAutentificare, btnBack });
        }

        private void BtnAutentificare_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string parola = txtParola.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(parola))
            {
                MessageBox.Show("Completează atât emailul cât și parola.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var clienti = adminClienti.GetClienti();
            var clientGasit = clienti.FirstOrDefault(c => 
                c.email.Equals(email, StringComparison.OrdinalIgnoreCase) && 
                c.parola == parola);

            if (clientGasit != null)
            {
                DashboardClient dash = new DashboardClient(clientGasit);
                dash.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Email sau parolă incorectă.", "Autentificare eșuată", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
