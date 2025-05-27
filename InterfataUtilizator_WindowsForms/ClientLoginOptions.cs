using System;
using System.Drawing;
using System.Windows.Forms;

namespace InterfataUtilizator_WindowsForms
{
    public class ClientLoginOptions : Form
    {
        private Button btnContNou;
        private Button btnContExistent;
        private Button btnBack;

        public ClientLoginOptions()
        {
            this.Text = "Client - Opțiuni Autentificare";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(400, 350);
            this.BackColor = ColorTranslator.FromHtml("#e3f2fd");

            ConfigureazaComponente();
        }

        private void ConfigureazaComponente()
        {
            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1,
                Padding = new Padding(0, 50, 0, 30)
            };
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));

            btnContExistent = new Button()
            {
                Text = "Am deja cont",
                Width = 220,
                Height = 50,
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnContExistent.Click += BtnContExistent_Click;

            btnContNou = new Button()
            {
                Text = "Creează cont nou",
                Width = 220,
                Height = 50,
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnContNou.Click += BtnContNou_Click;

            btnBack = new Button()
            {
                Text = "Înapoi",
                Width = 220,
                Height = 50,
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

            table.Controls.Add(Wrap(btnContExistent), 0, 0);
            table.Controls.Add(Wrap(btnContNou), 0, 1);
            table.Controls.Add(Wrap(btnBack), 0, 2);

            this.Controls.Add(table);
        }

        private Panel Wrap(Control control)
        {
            Panel panel = new Panel { Dock = DockStyle.Fill };
            control.Anchor = AnchorStyles.None;
            panel.Controls.Add(control);
            panel.Resize += (s, e) =>
            {
                control.Location = new Point((panel.Width - control.Width) / 2, (panel.Height - control.Height) / 2);
            };
            return panel;
        }

        private void BtnContExistent_Click(object sender, EventArgs e)
        {
            AutentificareClientExistent form = new AutentificareClientExistent();
            form.Show();
            this.Hide();
        }

        private void BtnContNou_Click(object sender, EventArgs e)
        {
            AutentificareClient form = new AutentificareClient();
            form.Show();
            this.Hide();
        }
    }
}
