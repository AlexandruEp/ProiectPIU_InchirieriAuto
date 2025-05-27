using System;
using System.Drawing;
using System.Windows.Forms;

namespace InterfataUtilizator_WindowsForms
{
    public class Dashboard : Form
    {
        private Button btnVizualizareClienti;
        private Button btnAdaugaMasina;
        private Button btnVizualizareMasini;
        private Button btnIstoricInchirieri;
        private Button btnLogout;

        public Dashboard()
        {
            InitializeDashboard();
        }

        private void InitializeDashboard()
        {
            this.Text = "Dashboard - Admin";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(400, 600);
            this.BackColor = ColorTranslator.FromHtml("#e3f2fd");

            // Layout principal
            TableLayoutPanel table = new TableLayoutPanel
            {
                RowCount = 5,
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 50, 0, 30)
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            for (int i = 0; i < 5; i++)
            {
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            }

            // Butoane
            btnVizualizareClienti = CreateStyledButton("Vizualizează Clienți", Color.FromArgb(41, 128, 185), BtnVizualizareClienti_Click);
            btnAdaugaMasina = CreateStyledButton("Adaugă Mașină", Color.FromArgb(46, 204, 113), BtnAdaugaMasina_Click);
            btnVizualizareMasini = CreateStyledButton("Vizualizează Mașini", Color.FromArgb(52, 152, 219), BtnVizualizareMasini_Click);
            btnIstoricInchirieri = CreateStyledButton("Istoric Închirieri", Color.FromArgb(155, 89, 182), BtnIstoricInchirieri_Click);
            btnLogout = CreateStyledButton("Logout", Color.FromArgb(231, 76, 60), BtnLogout_Click);

            // Adăugare în layout
            table.Controls.Add(WrapInPanel(btnVizualizareClienti), 0, 0);
            table.Controls.Add(WrapInPanel(btnAdaugaMasina), 0, 1);
            table.Controls.Add(WrapInPanel(btnVizualizareMasini), 0, 2);
            table.Controls.Add(WrapInPanel(btnIstoricInchirieri), 0, 3);
            table.Controls.Add(WrapInPanel(btnLogout), 0, 4);

            this.Controls.Add(table);
        }

        private Button CreateStyledButton(string text, Color backColor, EventHandler onClick)
        {
            Button button = new Button
            {
                Text = text,
                Width = 250,
                Height = 50,
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.None,
                Margin = new Padding(10)
            };

            button.Click += onClick; // Attach the event handler here
            return button;
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

        private void BtnVizualizareClienti_Click(object sender, EventArgs e)
        {
            VizualizareClienti formVizualizare = new VizualizareClienti();
            formVizualizare.ShowDialog();
        }

        private void BtnAdaugaMasina_Click(object sender, EventArgs e)
        {
            Adaugare_Masini formAdaugareMasina = new Adaugare_Masini();
            formAdaugareMasina.ShowDialog();
        }

        private void BtnVizualizareMasini_Click(object sender, EventArgs e)
        {
            VizualizareMasiniAdmin formVizualizareMasini = new VizualizareMasiniAdmin();
            formVizualizareMasini.ShowDialog();
        }

        private void BtnIstoricInchirieri_Click(object sender, EventArgs e)
        {
            IstoricInchirieri formIstoric = new IstoricInchirieri();
            formIstoric.ShowDialog();
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }
}
