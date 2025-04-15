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
    private Button BtnBack;

    public VizualizareClienti()
    {
        this.Text = "Lista Clienți";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Size = new Size(800, 550);

        string numeFisierClienti = ConfigurationManager.AppSettings["NumeFisierClienti"];
        string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        string caleCompletaFisierClienti = Path.Combine(locatieFisierSolutie, numeFisierClienti);
        adminClienti = new AdministrareClienti_FisierText(caleCompletaFisierClienti);

        ConfigurareDataGridView();
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

        BtnBack = new Button()
        {
            Text = "Înapoi",
            Location = new Point(250, 200),
            Width = 200,
            Height = 35,
            BackColor = Color.MediumAquamarine,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        BtnBack.Click += BtnBack_Click;
        this.Controls.Add(BtnBack);
         

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

    private void BtnBack_Click(object sender, EventArgs e)
    {
        this.Close();
    }
}
