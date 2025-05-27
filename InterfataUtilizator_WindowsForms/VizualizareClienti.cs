using LibrarieModele;
using NivelStocareDate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

public partial class VizualizareClienti : Form
{
    private DataGridView dataGridViewClienti;
    private AdministrareClienti_FisierText adminClienti;
    private List<Client> listaClienti;

    private TextBox txtCautare;
    private ComboBox cmbCriteriu;
    private Button btnCauta, btnReset, BtnBack;

    private Panel searchPanel;

    public VizualizareClienti()
    {
        this.Text = "Lista Clienți";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Size = new Size(1000, 600);
        this.MinimumSize = new Size(800, 500);
        this.BackColor = ColorTranslator.FromHtml("#e3f2fd");

        string numeFisierClienti = ConfigurationManager.AppSettings["NumeFisierClienti"];
        string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        string caleCompletaFisierClienti = Path.Combine(locatieFisierSolutie, numeFisierClienti);
        adminClienti = new AdministrareClienti_FisierText(caleCompletaFisierClienti);

        // Main layout
        TableLayoutPanel mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            ColumnCount = 1,
            Padding = new Padding(20)
        };

        // Configure row styles
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 80)); // DataGridView
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // Search panel
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // Back button

        ConfigurareDataGridView();
        ConfigureSearchControls();
        ConfigureBackButton();

        // Add controls to layout
        mainLayout.Controls.Add(dataGridViewClienti, 0, 0);
        Panel searchWrapper = new Panel { Dock = DockStyle.Fill };
        searchWrapper.Controls.Add(searchPanel);
        mainLayout.Controls.Add(searchWrapper, 0, 1);
        mainLayout.Controls.Add(BtnBack, 0, 2);

        this.Controls.Add(mainLayout);
        AfiseazaTotiClientii();
    }

    private void ConfigurareDataGridView()
    {
        dataGridViewClienti = new DataGridView
        {
            Dock = DockStyle.Fill,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            MultiSelect = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                Alignment = DataGridViewContentAlignment.MiddleCenter
            },
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 10),
                SelectionBackColor = Color.FromArgb(52, 152, 219),
                SelectionForeColor = Color.White
            },
            BackgroundColor = Color.White,
            GridColor = Color.LightGray,
            BorderStyle = BorderStyle.None,
            RowHeadersVisible = false
        };
    }

    private void AfiseazaTotiClientii()
    {
        listaClienti = adminClienti.GetClienti();

        var clientiValizi = listaClienti
            .Where(c => c != null && !string.IsNullOrWhiteSpace(c.nume))
            .ToList();

        dataGridViewClienti.DataSource = clientiValizi;

        dataGridViewClienti.Columns["IdClient"].HeaderText = "ID";
        dataGridViewClienti.Columns["nume"].HeaderText = "Nume";
        dataGridViewClienti.Columns["email"].HeaderText = "Email";
        dataGridViewClienti.Columns["telefon"].HeaderText = "Telefon";
        dataGridViewClienti.Columns["CNP"].HeaderText = "CNP";
    }

    private void ConfigureSearchControls()
    {
        // Create a panel for search controls
        searchPanel = new Panel
        {
            Dock = DockStyle.Fill,
            Height = 50,
            BackColor = Color.FromArgb(245, 245, 245)
        };

        // Create TableLayoutPanel for better alignment
        TableLayoutPanel searchControls = new TableLayoutPanel
        {
            ColumnCount = 5,
            Dock = DockStyle.Fill,
            Padding = new Padding(10, 5, 10, 5)
        };

        // Set column styles
        searchControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15)); // Label
        searchControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25)); // ComboBox
        searchControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35)); // TextBox
        searchControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5f)); // Search button
        searchControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5f)); // Reset button

        // Create and configure controls
        Label lblCautare = new Label()
        {
            Text = "Caută după:",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleRight
        };

        cmbCriteriu = new ComboBox()
        {
            Dock = DockStyle.Fill,
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = new Font("Segoe UI", 9F)
        };
        cmbCriteriu.Items.AddRange(new string[] { "Nume", "Email", "CNP", "Telefon" });
        cmbCriteriu.SelectedIndex = 0;

        txtCautare = new TextBox()
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9F)
        };

        btnCauta = new Button()
        {
            Text = "Caută",
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(41, 128, 185),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnCauta.Click += BtnCauta_Click;

        btnReset = new Button()
        {
            Text = "Resetează",
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnReset.Click += (s, e) =>
        {
            txtCautare.Clear();
            AfiseazaTotiClientii();
        };

        // Add controls to TableLayoutPanel
        searchControls.Controls.Add(lblCautare, 0, 0);
        searchControls.Controls.Add(cmbCriteriu, 1, 0);
        searchControls.Controls.Add(txtCautare, 2, 0);
        searchControls.Controls.Add(btnCauta, 3, 0);
        searchControls.Controls.Add(btnReset, 4, 0);

        // Add TableLayoutPanel to search panel
        searchPanel.Controls.Add(searchControls);
    }

    private void BtnCauta_Click(object sender, EventArgs e)
    {
        string valoare = txtCautare.Text.Trim().ToLower();
        string criteriu = cmbCriteriu.SelectedItem.ToString();

        if (string.IsNullOrWhiteSpace(valoare))
        {
            MessageBox.Show("Introduceți o valoare pentru căutare.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var clientiFiltrati = listaClienti.Where(c =>
        {
            switch (criteriu)
            {
                case "Nume":
                    return c.nume.ToLower().Contains(valoare);
                case "Email":
                    return c.email.ToLower().Contains(valoare);
                case "CNP":
                    return c.CNP.ToLower().Contains(valoare);
                case "Telefon":
                    return c.telefon.ToLower().Contains(valoare);
                default:
                    return false;
            }
        }).ToList();

        dataGridViewClienti.DataSource = clientiFiltrati;
    }

    private void ConfigureBackButton()
    {
        BtnBack = new Button()
        {
            Text = "Înapoi",
            Width = 120,
            Height = 35,
            BackColor = Color.FromArgb(46, 204, 113),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Anchor = AnchorStyles.None
        };
        BtnBack.Click += BtnBack_Click;
    }

    private void BtnBack_Click(object sender, EventArgs e)
    {
        this.Close();
    }
}
