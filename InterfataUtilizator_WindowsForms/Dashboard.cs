using System;
using System.Drawing;
using System.Windows.Forms;

namespace InterfataUtilizator_WindowsForms
{
    public partial class Dashboard : Form
    {
        private Button btnAdaugaClient;
        private Button btnVizualizareClienti;

        public Dashboard()
        {
            InitializeComponent();
            InitializeDashboard();
        }

        private void InitializeDashboard()
        {
            this.Text = "Dashboard - Închirieri Mașini";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(400, 300);
            this.BackColor = Color.White;

            btnAdaugaClient = new Button
            {
                Text = "Adaugă Client",
                Location = new Point(100, 60),
                Size = new Size(200, 40),
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAdaugaClient.Click += BtnAdaugaClient_Click;

            btnVizualizareClienti = new Button
            {
                Text = "Vizualizează Clienți",
                Location = new Point(100, 120),
                Size = new Size(200, 40),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnVizualizareClienti.Click += BtnVizualizareClienti_Click;

            this.Controls.Add(btnAdaugaClient);
            this.Controls.Add(btnVizualizareClienti);
        }

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
    }
}
