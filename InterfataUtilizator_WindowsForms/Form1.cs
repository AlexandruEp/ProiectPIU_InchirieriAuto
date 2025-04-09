using LibrarieModele;
using NivelStocareDate;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

public partial class Form1 : Form
{
    private DataGridView dataGridViewClienti;
    private TextBox txtNume, txtEmail, txtTelefon, txtCNP;
    private Button btnAdaugaClient;
    private AdministrareClienti_FisierText adminClienti;

    public Form1()
    {
        this.Text = "Lista Clienți";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Size = new Size(800, 550);

        string numeFisierClienti = ConfigurationManager.AppSettings["NumeFisierClienti"];
        string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        string caleCompletaFisierClienti = Path.Combine(locatieFisierSolutie, numeFisierClienti);
        adminClienti = new AdministrareClienti_FisierText(caleCompletaFisierClienti);

        ConfigurareDataGridView();
        ConfigurareInputForm();
        this.Controls.Add(dataGridViewClienti);

        AfiseazaTotiClientii();
        AjusteazaInaltimeGrid();
    }

    private void ConfigurareDataGridView()
    {
        dataGridViewClienti = new DataGridView
        {
            Location = new Point(20, 20),
            Width = this.ClientSize.Width - 40,
            Height = 300,
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            MultiSelect = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.LightSlateGray,
                ForeColor = Color.White,
                Alignment = DataGridViewContentAlignment.MiddleCenter
            },
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 10),
                SelectionBackColor = Color.LightBlue,
                SelectionForeColor = Color.Black
            },
            BackgroundColor = Color.White,
            GridColor = Color.LightGray
        };
    }

    private void ConfigurareInputForm()
    {
        // Etichete și TextBox-uri
        Label lblNume = new Label() { Text = "Nume:", Location = new Point(20, 340), Width = 50 };
        txtNume = new TextBox() { Location = new Point(80, 340), Width = 150 };

        Label lblEmail = new Label() { Text = "Email:", Location = new Point(250, 340), Width = 50 };
        txtEmail = new TextBox() { Location = new Point(310, 340), Width = 150 };

        Label lblTelefon = new Label() { Text = "Telefon:", Location = new Point(20, 380), Width = 60 };
        txtTelefon = new TextBox() { Location = new Point(80, 380), Width = 150 };

        Label lblCNP = new Label() { Text = "CNP:", Location = new Point(250, 380), Width = 50 };
        txtCNP = new TextBox() { Location = new Point(310, 380), Width = 150 };

        btnAdaugaClient = new Button()
        {
            Text = "Adaugă Client",
            Location = new Point(500, 360),
            Width = 150,
            Height = 35,
            BackColor = Color.MediumSeaGreen,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };

        btnAdaugaClient.Click += BtnAdaugaClient_Click;

        // Adaugă controalele în formular
        this.Controls.Add(lblNume);
        this.Controls.Add(txtNume);
        this.Controls.Add(lblEmail);
        this.Controls.Add(txtEmail);
        this.Controls.Add(lblTelefon);
        this.Controls.Add(txtTelefon);
        this.Controls.Add(lblCNP);
        this.Controls.Add(txtCNP);
        this.Controls.Add(btnAdaugaClient);
    }

    private void BtnAdaugaClient_Click(object sender, System.EventArgs e)
    {
        string nume = txtNume.Text.Trim();
        string email = txtEmail.Text.Trim();
        string telefon = txtTelefon.Text.Trim();
        string cnp = txtCNP.Text.Trim();

        if (string.IsNullOrWhiteSpace(nume) || string.IsNullOrWhiteSpace(email) ||
            telefon.Length != 10 || cnp.Length != 13)
        {
            MessageBox.Show("Toate câmpurile trebuie completate corect:\n- Telefon: 10 cifre\n- CNP: 13 cifre", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var clientiExistenti = adminClienti.GetClienti();
        int idNou = clientiExistenti.Any() ? clientiExistenti.Max(c => c.IdClient) + 1 : 1;

        Client clientNou = new Client(idNou, nume, email, cnp, telefon);
        adminClienti.AddClient(clientNou);

        MessageBox.Show("Client adăugat cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);

        txtNume.Clear(); txtEmail.Clear(); txtTelefon.Clear(); txtCNP.Clear();

        AfiseazaTotiClientii();
        AjusteazaInaltimeGrid();
    }

    private void AfiseazaTotiClientii()
    {
        List<Client> clienti = adminClienti.GetClienti();

        var clientiValizi = clienti
            .Where(c => c != null && !string.IsNullOrWhiteSpace(c.nume))
            .ToList();

        dataGridViewClienti.DataSource = clientiValizi;
    }

    private void AjusteazaInaltimeGrid()
    {
        int nrRanduri = dataGridViewClienti.RowCount;
        int inaltimeRand = dataGridViewClienti.RowTemplate.Height;
        int inaltimeHeader = dataGridViewClienti.ColumnHeadersHeight;

        dataGridViewClienti.Height = (nrRanduri * inaltimeRand) + inaltimeHeader + 5;
    }
}
