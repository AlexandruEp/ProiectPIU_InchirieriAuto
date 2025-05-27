using LibrarieModele;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace InterfataUtilizator_WindowsForms
{
    public class DashboardClient : Form
    {
        private Button btnVeziMasini;
        private Button btnLogout;
        private Button btnEditareProfil;
        private Button btnMasiniInchiriate;

        private readonly Client client;

        public DashboardClient(Client client)
        {
            this.client = client;

            this.Text = "Dashboard Client";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(500, 400);
            this.BackColor = ColorTranslator.FromHtml("#e3f2fd");

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            TableLayoutPanel table = new TableLayoutPanel
            {
                RowCount = 4,
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 50, 0, 30)
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            for (int i = 0; i < 4; i++)
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));

            btnVeziMasini = new Button()
            {
                Text = "Vezi Mașini Disponibile",
                Width = 250,
                Height = 50,
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnVeziMasini.Click += BtnVeziMasini_Click;

            btnMasiniInchiriate = new Button()
            {
                Text = "Vezi Mașinile Închiriate",
                Width = 250,
                Height = 50,
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnMasiniInchiriate.Click += BtnMasiniInchiriate_Click;

            btnEditareProfil = new Button()
            {
                Text = "Editează Profilul",
                Width = 250,
                Height = 50,
                BackColor = Color.Orange,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnEditareProfil.Click += BtnEditareProfil_Click;

            btnLogout = new Button()
            {
                Text = "Logout",
                Width = 250,
                Height = 50,
                BackColor = Color.LightCoral,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLogout.Click += BtnLogout_Click;

            table.Controls.Add(WrapInPanel(btnVeziMasini), 0, 0);
            table.Controls.Add(WrapInPanel(btnMasiniInchiriate), 0, 1);
            table.Controls.Add(WrapInPanel(btnEditareProfil), 0, 2);
            table.Controls.Add(WrapInPanel(btnLogout), 0, 3);

            this.Controls.Add(table);
        }

        private Panel WrapInPanel(Control control)
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

        private void BtnVeziMasini_Click(object sender, EventArgs e)
        {
            var formMasini = new VizualizareMasiniClient(client);
            formMasini.ShowDialog();
        }

        private void BtnMasiniInchiriate_Click(object sender, EventArgs e)
        {
            var formMasiniInchiriate = new MasiniInchiriateClient(client);
            formMasiniInchiriate.ShowDialog();
        }

        private void BtnEditareProfil_Click(object sender, EventArgs e)
        {
            var formEditare = new EditareProfilClient(client.CNP);
            formEditare.ShowDialog();
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            new Login().Show();
            this.Close();
        }
    }
}
