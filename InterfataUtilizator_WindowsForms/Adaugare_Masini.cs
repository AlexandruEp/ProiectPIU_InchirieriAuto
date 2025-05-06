using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using LibrarieModele;
using NivelStocareDate;

namespace InterfataUtilizator_WindowsForms
{
    public partial class Adaugare_Masini : Form
    {
        private TextBox txtAnFabricatie;
        private ComboBox cmbModel, cmbCuloare;
        private CheckBox chkBenzina, chkMotorina, chkElectric;
        private Button btnAdaugaMasina, btnBack;
        private AdministrareMasini_FisierText adminMasini;

        public Adaugare_Masini()
        {
            InitializeComponent();
            this.Text = "Adăugare Mașină";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(700, 550);

            string numeFisierMasini = "masini.txt";
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string caleCompletaFisierMasini = Path.Combine(locatieFisierSolutie, numeFisierMasini);
            adminMasini = new AdministrareMasini_FisierText(caleCompletaFisierMasini);

            ConfigurareInputForm();
        }

        private void ConfigurareInputForm()
        {
            int xLabel = 150;
            int xInput = 250;
            int yStart = 50;
            int spacing = 50;

            // Model
            Label lblModel = new Label() { Text = "Model:", Location = new Point(xLabel, yStart), Width = 80, TextAlign = ContentAlignment.MiddleRight };
            cmbModel = new ComboBox() { Location = new Point(xInput, yStart), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbModel.Items.AddRange(Enum.GetNames(typeof(Model_masina)));

            // Combustibil (cu CheckBox-uri)
            Label lblCombustibil = new Label() { Text = "Combustibil:", Location = new Point(xLabel, yStart + spacing), Width = 80, TextAlign = ContentAlignment.MiddleRight };
            chkBenzina = new CheckBox() { Text = "Benzină", Location = new Point(xInput, yStart + spacing) };
            chkMotorina = new CheckBox() { Text = "Motorină", Location = new Point(xInput, yStart + spacing + 25) };
            chkElectric = new CheckBox() { Text = "Electric", Location = new Point(xInput, yStart + spacing + 50) };

            chkBenzina.CheckedChanged += CombustibilCheckedChanged;
            chkMotorina.CheckedChanged += CombustibilCheckedChanged;
            chkElectric.CheckedChanged += CombustibilCheckedChanged;

            // An fabricație
            Label lblAnFabricatie = new Label() { Text = "An fabricație:", Location = new Point(xLabel, yStart + 2 * spacing + 30), Width = 80, TextAlign = ContentAlignment.MiddleRight };
            txtAnFabricatie = new TextBox() { Location = new Point(xInput, yStart + 2 * spacing + 30), Width = 200 };

            // Culoare
            Label lblCuloare = new Label() { Text = "Culoare:", Location = new Point(xLabel, yStart + 3 * spacing + 30), Width = 80, TextAlign = ContentAlignment.MiddleRight };
            cmbCuloare = new ComboBox() { Location = new Point(xInput, yStart + 3 * spacing + 30), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCuloare.Items.AddRange(Enum.GetNames(typeof(Culoare_masina)));

            // Buton Adaugă
            btnAdaugaMasina = new Button()
            {
                Text = "Adaugă Mașină",
                Location = new Point(250, yStart + 4 * spacing + 60),
                Width = 200,
                Height = 30
            };
            btnAdaugaMasina.Click += BtnAdaugaMasina_Click;

            // Buton Înapoi
            btnBack = new Button()
            {
                Text = "Înapoi",
                Location = new Point(250, yStart + 5 * spacing + 80),
                Size = new Size(200, 30),
                BackColor = Color.MediumAquamarine,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBack.Click += BtnBack_Click;

            // Adaugă în form
            this.Controls.Add(lblModel);
            this.Controls.Add(cmbModel);
            this.Controls.Add(lblCombustibil);
            this.Controls.Add(chkBenzina);
            this.Controls.Add(chkMotorina);
            this.Controls.Add(chkElectric);
            this.Controls.Add(lblAnFabricatie);
            this.Controls.Add(txtAnFabricatie);
            this.Controls.Add(lblCuloare);
            this.Controls.Add(cmbCuloare);
            this.Controls.Add(btnAdaugaMasina);
            this.Controls.Add(btnBack);
        }

        private void CombustibilCheckedChanged(object sender, EventArgs e)
        {
            if (sender == chkBenzina && chkBenzina.Checked)
            {
                chkMotorina.Checked = false;
                chkElectric.Checked = false;
            }
            else if (sender == chkMotorina && chkMotorina.Checked)
            {
                chkBenzina.Checked = false;
                chkElectric.Checked = false;
            }
            else if (sender == chkElectric && chkElectric.Checked)
            {
                chkBenzina.Checked = false;
                chkMotorina.Checked = false;
            }
        }

        private void BtnAdaugaMasina_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbModel.SelectedItem == null || cmbCuloare.SelectedItem == null)
                    throw new Exception("Toate câmpurile trebuie completate.");

                Model_masina model = (Model_masina)Enum.Parse(typeof(Model_masina), cmbModel.SelectedItem.ToString());
                Culoare_masina culoare = (Culoare_masina)Enum.Parse(typeof(Culoare_masina), cmbCuloare.SelectedItem.ToString());
                int anFabricatie = int.Parse(txtAnFabricatie.Text);

                Tip_combustibil combustibil;
                if (chkBenzina.Checked) combustibil = Tip_combustibil.Benzina;
                else if (chkMotorina.Checked) combustibil = Tip_combustibil.Diesel;
                else if (chkElectric.Checked) combustibil = Tip_combustibil.Electric;
                else throw new Exception("Selectează un tip de combustibil.");

                Masina masinaNoua = new Masina(0, model, combustibil, anFabricatie, culoare);
                adminMasini.AddMasina(masinaNoua);

                MessageBox.Show("Mașina a fost adăugată cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
