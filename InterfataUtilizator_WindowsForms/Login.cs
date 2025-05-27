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
    public partial class Login: Form
    {
        private Button btnAdmin;
        private Button btnClient;

        public Login()
        {
            this.Text = "Sistem Închirieri Auto";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ColorTranslator.FromHtml("#e3f2fd");
            ConfigureazaComponente();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            AutentificareAdmin autentificareAdmin = new AutentificareAdmin();
            autentificareAdmin.Show();
            this.Hide();
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            ClientLoginOptions clientLogin = new ClientLoginOptions();
            clientLogin.Show();
            this.Hide();
        }

        private void ConfigureazaComponente()
        {
            this.btnAdmin = new Button();
            this.btnClient = new Button();

            // Configurare buton Admin
            this.btnAdmin.Location = new Point(100, 50);
            this.btnAdmin.Name = "btnAdmin";
            this.btnAdmin.Size = new Size(200, 50);
            this.btnAdmin.Text = "Autentificare Admin";
            this.btnAdmin.BackColor = Color.FromArgb(52, 152, 219);
            this.btnAdmin.ForeColor = Color.White;
            this.btnAdmin.FlatStyle = FlatStyle.Flat;
            this.btnAdmin.Click += new EventHandler(this.btnAdmin_Click);

            // Configurare buton Client
            this.btnClient.Location = new Point(100, 150);
            this.btnClient.Name = "btnClient";
            this.btnClient.Size = new Size(200, 50);
            this.btnClient.Text = "Autentificare Client";
            this.btnClient.BackColor = Color.FromArgb(41, 128, 185);
            this.btnClient.ForeColor = Color.White;
            this.btnClient.FlatStyle = FlatStyle.Flat;
            this.btnClient.Click += new EventHandler(this.btnClient_Click);

            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(400, 300);
            this.Controls.Add(this.btnAdmin);
            this.Controls.Add(this.btnClient);
            this.Name = "Login";
        }
    }
}
