using LibrarieModele;
using NivelStocareDate;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace InterfataUtilizator_WindowsForms
{
    public class EditareMasina : Form
    {
        private ComboBox cmbMarca, cmbCuloare, cmbCombustibil, cmbTransmisie;
        private TextBox txtModel, txtAnFabricatie, txtNrUsi, txtPret, txtImagine;
        private Button btnSalveaza, btnSelecteazaImagine;
        private Masina masina;
        private AdministrareMasini_FisierText adminMasini;

        public EditareMasina(int idMasina)
        {
            this.Text = "Editează Mașina";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(600, 550);
            this.BackColor = ColorTranslator.FromHtml("#e3f2fd");

            string fisierMasini = "masini.txt";
            string locatie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string cale = Path.Combine(locatie, fisierMasini);
            adminMasini = new AdministrareMasini_FisierText(cale);

            masina = adminMasini.GetMasini().FirstOrDefault(m => m.IdMasina == idMasina);
            if (masina == null)
            {
                MessageBox.Show("Mașina nu a fost găsită!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            InitializeComponent();
            PrecompletareDate();
        }

        private void InitializeComponent()
        {
            int xLabel = 100, xInput = 250, yStart = 30, spacingY = 40;

            // Marca
            Label lblMarca = new Label() { Text = "Marca:", Location = new Point(xLabel, yStart), Width = 100 };
            cmbMarca = new ComboBox() { Location = new Point(xInput, yStart), Width = 200 };
            cmbMarca.Items.AddRange(Enum.GetNames(typeof(MarcaMasina)));

            // Model
            Label lblModel = new Label() { Text = "Model:", Location = new Point(xLabel, yStart + spacingY), Width = 100 };
            txtModel = new TextBox() { Location = new Point(xInput, yStart + spacingY), Width = 200 };

            // Combustibil
            Label lblCombustibil = new Label() { Text = "Combustibil:", Location = new Point(xLabel, yStart + 2 * spacingY), Width = 100 };
            cmbCombustibil = new ComboBox() { Location = new Point(xInput, yStart + 2 * spacingY), Width = 200 };
            cmbCombustibil.Items.AddRange(Enum.GetNames(typeof(Tip_combustibil)));

            // Transmisie
            Label lblTransmisie = new Label() { Text = "Transmisie:", Location = new Point(xLabel, yStart + 3 * spacingY), Width = 100 };
            cmbTransmisie = new ComboBox() { Location = new Point(xInput, yStart + 3 * spacingY), Width = 200 };
            cmbTransmisie.Items.AddRange(Enum.GetNames(typeof(TipTransmisie)));

            // An fabricatie
            Label lblAn = new Label() { Text = "An fabricație:", Location = new Point(xLabel, yStart + 4 * spacingY), Width = 100 };
            txtAnFabricatie = new TextBox() { Location = new Point(xInput, yStart + 4 * spacingY), Width = 200 };

            // Culoare
            Label lblCuloare = new Label() { Text = "Culoare:", Location = new Point(xLabel, yStart + 5 * spacingY), Width = 100 };
            cmbCuloare = new ComboBox() { Location = new Point(xInput, yStart + 5 * spacingY), Width = 200 };
            cmbCuloare.Items.AddRange(Enum.GetNames(typeof(Culoare_masina)));

            // Nr usi
            Label lblNrUsi = new Label() { Text = "Nr. uși:", Location = new Point(xLabel, yStart + 6 * spacingY), Width = 100 };
            txtNrUsi = new TextBox() { Location = new Point(xInput, yStart + 6 * spacingY), Width = 200 };

            // Pret
            Label lblPret = new Label() { Text = "Preț (lei/zi):", Location = new Point(xLabel, yStart + 7 * spacingY), Width = 100 };
            txtPret = new TextBox() { Location = new Point(xInput, yStart + 7 * spacingY), Width = 200 };

            // Imagine
            Label lblImagine = new Label() { Text = "Imagine:", Location = new Point(xLabel, yStart + 8 * spacingY), Width = 100 };
            txtImagine = new TextBox() { Location = new Point(xInput, yStart + 8 * spacingY), Width = 200 };
            btnSelecteazaImagine = new Button()
            {
                Text = "Selectează...",
                Location = new Point(xInput + 210, yStart + 8 * spacingY),
                Width = 100
            };
            btnSelecteazaImagine.Click += BtnSelecteazaImagine_Click;

            // Buton Salvează
            btnSalveaza = new Button()
            {
                Text = "Salvează",
                Location = new Point(xInput, yStart + 9 * spacingY),
                Size = new Size(200, 35),
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSalveaza.Click += BtnSalveaza_Click;

            Controls.AddRange(new Control[] {
                lblMarca, cmbMarca, lblModel, txtModel, lblCombustibil, cmbCombustibil,
                lblTransmisie, cmbTransmisie, lblAn, txtAnFabricatie, lblCuloare, cmbCuloare,
                lblNrUsi, txtNrUsi, lblPret, txtPret, lblImagine, txtImagine, btnSelecteazaImagine,
                btnSalveaza
            });
        }

        private void PrecompletareDate()
        {
            cmbMarca.SelectedItem = masina.Marca.ToString();
            txtModel.Text = masina.Model;
            cmbCombustibil.SelectedItem = masina.Combustibil.ToString();
            cmbTransmisie.SelectedItem = masina.Transmisie.ToString();
            txtAnFabricatie.Text = masina.AnFabricatie.ToString();
            cmbCuloare.SelectedItem = masina.Culoare.ToString();
            txtNrUsi.Text = masina.NrUsi.ToString();
            txtPret.Text = masina.Pret.ToString();
            txtImagine.Text = masina.ImagePath;
        }

        private void BtnSelecteazaImagine_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Imagini (*.png;*.jpg)|*.png;*.jpg";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtImagine.Text = dlg.FileName;
                }
            }
        }

        private void BtnSalveaza_Click(object sender, EventArgs e)
        {
            try
            {
                masina.Marca = (MarcaMasina)Enum.Parse(typeof(MarcaMasina), cmbMarca.SelectedItem.ToString());
                masina.Model = txtModel.Text.Trim();
                masina.Combustibil = (Tip_combustibil)Enum.Parse(typeof(Tip_combustibil), cmbCombustibil.SelectedItem.ToString());
                masina.Transmisie = (TipTransmisie)Enum.Parse(typeof(TipTransmisie), cmbTransmisie.SelectedItem.ToString());
                masina.AnFabricatie = int.Parse(txtAnFabricatie.Text.Trim());
                masina.Culoare = (Culoare_masina)Enum.Parse(typeof(Culoare_masina), cmbCuloare.SelectedItem.ToString());
                masina.NrUsi = int.Parse(txtNrUsi.Text.Trim());
                masina.Pret = double.Parse(txtPret.Text.Trim());
                masina.ImagePath = txtImagine.Text.Trim();

                var masini = adminMasini.GetMasini();
                int index = masini.FindIndex(m => m.IdMasina == masina.IdMasina);

                if (index != -1)
                {
                    masini[index] = masina;
                    adminMasini.SalveazaMasini(masini);
                    MessageBox.Show("Mașina a fost actualizată!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
