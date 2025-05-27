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
        private ComboBox cmbMarca, cmbCuloare, cmbTransmisie;
        private TextBox txtModel, txtAnFabricatie, txtPret, txtImagePath;
        private NumericUpDown nudNrUsi;
        private CheckBox chkBenzina, chkMotorina, chkElectric;
        private Button btnAdaugaMasina, btnBack, btnSelecteazaImagine;
        private AdministrareMasini_FisierText adminMasini;

        public Adaugare_Masini()
        {
            InitializeComponent();
            this.Text = "Adăugare Mașină";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(800, 700);
            this.BackColor = ColorTranslator.FromHtml("#e3f2fd");
            this.MinimumSize = new Size(800, 700);

            string cale = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "masini.txt");
            adminMasini = new AdministrareMasini_FisierText(cale);

            ConfigurareInputForm();
        }

        private void ConfigurareInputForm()
        {
            // Calculăm poziția centrală pentru elementele formularului
            int formCenterX = this.ClientSize.Width / 2;
            int labelWidth = 120;
            int inputWidth = 250;
            int xLabel = formCenterX - (labelWidth + inputWidth) / 2;
            int xInput = xLabel + labelWidth + 10;
            int yStart = 40;
            int spacing = 50;

            // Marca
            Label lblMarca = new Label() { 
                Text = "Marca:", 
                Location = new Point(xLabel, yStart), 
                Width = labelWidth,
                TextAlign = ContentAlignment.MiddleRight
            };
            cmbMarca = new ComboBox() { 
                Location = new Point(xInput, yStart), 
                Width = inputWidth, 
                DropDownStyle = ComboBoxStyle.DropDownList 
            };
            cmbMarca.Items.AddRange(Enum.GetNames(typeof(MarcaMasina)));

            // Model
            Label lblModel = new Label() { 
                Text = "Model:", 
                Location = new Point(xLabel, yStart + spacing), 
                Width = labelWidth,
                TextAlign = ContentAlignment.MiddleRight
            };
            txtModel = new TextBox() { 
                Location = new Point(xInput, yStart + spacing), 
                Width = inputWidth 
            };

            // Combustibil
            Label lblCombustibil = new Label() { 
                Text = "Combustibil:", 
                Location = new Point(xLabel, yStart + 2 * spacing), 
                Width = labelWidth,
                TextAlign = ContentAlignment.MiddleRight
            };
            
            Panel panelCombustibil = new Panel() {
                Location = new Point(xInput, yStart + 2 * spacing),
                Width = inputWidth,
                Height = 80
            };
            
            chkBenzina = new CheckBox() { 
                Text = "Benzină", 
                Location = new Point(0, 0),
                AutoSize = true
            };
            chkMotorina = new CheckBox() { 
                Text = "Motorină", 
                Location = new Point(0, 25),
                AutoSize = true
            };
            chkElectric = new CheckBox() { 
                Text = "Electric", 
                Location = new Point(0, 50),
                AutoSize = true
            };
            
            panelCombustibil.Controls.AddRange(new Control[] { chkBenzina, chkMotorina, chkElectric });
            
            chkBenzina.CheckedChanged += CombustibilCheckedChanged;
            chkMotorina.CheckedChanged += CombustibilCheckedChanged;
            chkElectric.CheckedChanged += CombustibilCheckedChanged;

            // Transmisie
            Label lblTransmisie = new Label() { 
                Text = "Transmisie:", 
                Location = new Point(xLabel, yStart + 4 * spacing), 
                Width = labelWidth,
                TextAlign = ContentAlignment.MiddleRight
            };
            cmbTransmisie = new ComboBox() { 
                Location = new Point(xInput, yStart + 4 * spacing), 
                Width = inputWidth, 
                DropDownStyle = ComboBoxStyle.DropDownList 
            };
            cmbTransmisie.Items.AddRange(Enum.GetNames(typeof(TipTransmisie)));

            // An fabricație
            Label lblAn = new Label() { 
                Text = "An fabricație:", 
                Location = new Point(xLabel, yStart + 5 * spacing), 
                Width = labelWidth,
                TextAlign = ContentAlignment.MiddleRight
            };
            txtAnFabricatie = new TextBox() { 
                Location = new Point(xInput, yStart + 5 * spacing), 
                Width = inputWidth 
            };

            // Culoare
            Label lblCuloare = new Label() { 
                Text = "Culoare:", 
                Location = new Point(xLabel, yStart + 6 * spacing), 
                Width = labelWidth,
                TextAlign = ContentAlignment.MiddleRight
            };
            cmbCuloare = new ComboBox() { 
                Location = new Point(xInput, yStart + 6 * spacing), 
                Width = inputWidth, 
                DropDownStyle = ComboBoxStyle.DropDownList 
            };
            cmbCuloare.Items.AddRange(Enum.GetNames(typeof(Culoare_masina)));

            // Nr uși
            Label lblUsi = new Label() { 
                Text = "Nr. Uși:", 
                Location = new Point(xLabel, yStart + 7 * spacing), 
                Width = labelWidth,
                TextAlign = ContentAlignment.MiddleRight
            };
            nudNrUsi = new NumericUpDown() { 
                Location = new Point(xInput, yStart + 7 * spacing), 
                Width = inputWidth, 
                Minimum = 2, 
                Maximum = 5 
            };

            // Preț
            Label lblPret = new Label() { 
                Text = "Preț/zi (lei):", 
                Location = new Point(xLabel, yStart + 8 * spacing), 
                Width = labelWidth,
                TextAlign = ContentAlignment.MiddleRight
            };
            txtPret = new TextBox() { 
                Location = new Point(xInput, yStart + 8 * spacing), 
                Width = inputWidth 
            };

            // Imagine
            Label lblImagine = new Label() { 
                Text = "Imagine:", 
                Location = new Point(xLabel, yStart + 9 * spacing), 
                Width = labelWidth,
                TextAlign = ContentAlignment.MiddleRight
            };
            
            Panel panelImagine = new Panel() {
                Location = new Point(xInput, yStart + 9 * spacing),
                Width = inputWidth + 110,
                Height = 30
            };
            
            txtImagePath = new TextBox() { 
                Location = new Point(0, 0), 
                Width = inputWidth,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            btnSelecteazaImagine = new Button() { 
                Text = "Selectează...", 
                Location = new Point(inputWidth + 10, 0),
                Width = 100
            };
            
            panelImagine.Controls.AddRange(new Control[] { txtImagePath, btnSelecteazaImagine });
            
            btnSelecteazaImagine.Click += (s, e) =>
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Imagini (*.jpg;*.png)|*.jpg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                    txtImagePath.Text = ofd.FileName;
            };

            // Panel pentru butoane
            Panel panelButoane = new Panel() {
                Width = 450,
                Height = 35,
                Location = new Point((this.ClientSize.Width - 450) / 2, yStart + 11 * spacing)
            };

            // Buton Adăugare
            btnAdaugaMasina = new Button()
            {
                Text = "Adaugă Mașină",
                Width = 200,
                Height = 35,
                Location = new Point(0, 0),
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAdaugaMasina.Click += BtnAdaugaMasina_Click;

            // Buton Înapoi
            btnBack = new Button()
            {
                Text = "Înapoi",
                Width = 200,
                Height = 35,
                Location = new Point(250, 0),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBack.Click += (s, e) => this.Close();

            panelButoane.Controls.AddRange(new Control[] { btnAdaugaMasina, btnBack });

            // Adaugă toate controalele în form
            Controls.AddRange(new Control[]
            {
                lblMarca, cmbMarca,
                lblModel, txtModel,
                lblCombustibil, panelCombustibil,
                lblTransmisie, cmbTransmisie,
                lblAn, txtAnFabricatie,
                lblCuloare, cmbCuloare,
                lblUsi, nudNrUsi,
                lblPret, txtPret,
                lblImagine, panelImagine,
                panelButoane
            });
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
                // Validări minime
                if (cmbMarca.SelectedItem == null || string.IsNullOrWhiteSpace(txtModel.Text) ||
                    cmbCuloare.SelectedItem == null || cmbTransmisie.SelectedItem == null ||
                    string.IsNullOrWhiteSpace(txtAnFabricatie.Text) || string.IsNullOrWhiteSpace(txtPret.Text))
                    throw new Exception("Completează toate câmpurile obligatorii.");

                // Conversii
                var marca = (MarcaMasina)Enum.Parse(typeof(MarcaMasina), cmbMarca.SelectedItem.ToString());
                var model = txtModel.Text.Trim();
                var combustibil = chkBenzina.Checked ? Tip_combustibil.Benzina :
                                  chkMotorina.Checked ? Tip_combustibil.Diesel :
                                  chkElectric.Checked ? Tip_combustibil.Electric :
                                  throw new Exception("Selectează un tip de combustibil.");
                var transmisie = (TipTransmisie)Enum.Parse(typeof(TipTransmisie), cmbTransmisie.SelectedItem.ToString());
                int an = int.Parse(txtAnFabricatie.Text.Trim());
                var culoare = (Culoare_masina)Enum.Parse(typeof(Culoare_masina), cmbCuloare.SelectedItem.ToString());
                string imagine = txtImagePath.Text.Trim();
                int usi = (int)nudNrUsi.Value;
                double pret = double.Parse(txtPret.Text.Trim());

                Masina masina = new Masina(0, marca, model, combustibil, transmisie, an, culoare, imagine, usi, pret);
                adminMasini.AddMasina(masina);

                MessageBox.Show("Mașina a fost adăugată cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetForm()
        {
            cmbMarca.SelectedIndex = -1;
            txtModel.Clear();
            chkBenzina.Checked = chkMotorina.Checked = chkElectric.Checked = false;
            cmbTransmisie.SelectedIndex = -1;
            txtAnFabricatie.Clear();
            cmbCuloare.SelectedIndex = -1;
            nudNrUsi.Value = 2;
            txtPret.Clear();
            txtImagePath.Clear();
        }
    }
}
