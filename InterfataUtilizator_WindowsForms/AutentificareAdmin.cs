using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfataUtilizator_WindowsForms
{
    public partial class AutentificareAdmin: Form
    {
        private TextBox txtUser, txtPass;
        private Button btnLogin;
        private Button btnBack;

        public AutentificareAdmin()
        {
            this.Text = "Autentificare Admin";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(500, 300);
            this.BackColor = ColorTranslator.FromHtml("#e3f2fd");

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

            Label lblUser = new Label() 
            { 
                Text = "Username:", 
                Location = new Point(xLabel, yStart), 
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            txtUser = new TextBox() 
            { 
                Location = new Point(xTextBox, yStart), 
                Width = textBoxWidth 
            };

            Label lblPassword = new Label() 
            { 
                Text = "Password:", 
                Location = new Point(xLabel, yStart + spacingY), 
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            txtPass = new TextBox() 
            { 
                Location = new Point(xTextBox, yStart + spacingY), 
                Width = textBoxWidth, 
                UseSystemPasswordChar = true 
            };

            btnLogin = new Button()
            {
                Text = "Login",
                Location = new Point(xTextBox, yStart + spacingY * 2),
                Width = textBoxWidth,
                Height = 35,
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLogin.Click += BtnLogin_Click;

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
                Login login = new Login();
                login.Show();
                this.Close();
            };

            this.Controls.AddRange(new Control[] { lblUser, txtUser, lblPassword, txtPass, btnLogin, btnBack });
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUser.Text.Trim();
            string password = txtPass.Text;
            if (username == "admin" && password == "admin123")
            {
                Dashboard dashboard = new Dashboard();
                dashboard.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Autentificare eșuată! Te rugăm să încerci din nou.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
