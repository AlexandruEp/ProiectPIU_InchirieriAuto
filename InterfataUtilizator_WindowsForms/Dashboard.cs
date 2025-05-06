using System;
using System.Drawing;
using System.Windows.Forms;

namespace InterfataUtilizator_WindowsForms
{
    public partial class Dashboard : Form
    {
        private Button btnAdaugaClient;
        private Button btnVizualizareClienti;
        private Button btnAdaugaMasina;
        private Button btnVizualizareMasini;

        public Dashboard()
        {
            InitializeComponent();
            InitializeDashboard();
        }

        private void InitializeDashboard()
        {
            this.Text = "Dashboard - Închirieri Mașini";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(400, 450);
            this.BackColor = Color.White;

            // Creează un TableLayoutPanel pentru centrare
            TableLayoutPanel table = new TableLayoutPanel
            {
                RowCount = 4,
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 30, 0, 30)
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            for (int i = 0; i < 4; i++)
            {
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            }

            // Inițializare butoane
            btnAdaugaClient = CreateStyledButton("Adaugă Client", Color.MediumSeaGreen, BtnAdaugaClient_Click);
            btnVizualizareClienti = CreateStyledButton("Vizualizează Clienți", Color.SteelBlue, BtnVizualizareClienti_Click);
            btnAdaugaMasina = CreateStyledButton("Adaugă Mașină", Color.Coral, BtnAdaugaMasina_Click);
            btnVizualizareMasini = CreateStyledButton("Vizualizează Mașini", Color.CadetBlue, BtnVizualizareMasini_Click);

            // Centrare: adăugăm panouri pentru fiecare buton
            table.Controls.Add(WrapInPanel(btnAdaugaClient), 0, 0);
            table.Controls.Add(WrapInPanel(btnVizualizareClienti), 0, 1);
            table.Controls.Add(WrapInPanel(btnAdaugaMasina), 0, 2);
            table.Controls.Add(WrapInPanel(btnVizualizareMasini), 0, 3);

            this.Controls.Add(table);
        }

        // Creează un buton stilizat
        private Button CreateStyledButton(string text, Color backColor, EventHandler onClick)
        {
            Button btn = new Button
            {
                Text = text,
                Width = 250,
                Height = 50,
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btn.Click += onClick;
            return btn;
        }

        // Centrează butonul într-un panou
        private Panel WrapInPanel(Control control)
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill
            };
            control.Anchor = AnchorStyles.None;
            control.Location = new Point((panel.Width - control.Width) / 2, (panel.Height - control.Height) / 2);
            panel.Controls.Add(control);
            panel.Resize += (s, e) =>
            {
                control.Location = new Point((panel.Width - control.Width) / 2, (panel.Height - control.Height) / 2);
            };
            return panel;
        }

        // Evenimente click
        private void BtnAdaugaClient_Click(object sender, EventArgs e)
        {
            AdaugareClienti formAdaugare = new AdaugareClienti();
            formAdaugare.ShowDialog();
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
            VizualizareMasini formVizualizareMasini = new VizualizareMasini();
            formVizualizareMasini.ShowDialog();
        }
    }
}
