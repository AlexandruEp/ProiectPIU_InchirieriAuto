using LibrarieModele;
using NivelStocareDate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace InterfataUtilizator_WindowsForms
{
    public partial class VizualizareMasini : Form
    {
        private DataGridView dataGridViewMasini;
        private AdministrareMasini_FisierText adminMasini;
        private Button BtnBack;

        public VizualizareMasini()
        {
            this.Text = "Lista Mașini";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(800, 550);
            this.MinimumSize = new Size(800, 550);
            this.Font = new Font("Segoe UI", 10F);
            this.BackColor = Color.White;

            string numeFisierMasini = ConfigurationManager.AppSettings["NumeFisierMasini"];
            string locatieFisierSolutie = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string caleCompletaFisierMasini = System.IO.Path.Combine(locatieFisierSolutie, numeFisierMasini);
            adminMasini = new AdministrareMasini_FisierText(caleCompletaFisierMasini);

            ConfigurareDataGridView();
            this.Controls.Add(dataGridViewMasini);

            AfiseazaToateMasinile();
            AjusteazaInaltimeGrid();
            ConfigureBackButton();
        }

        private void ConfigurareDataGridView()
        {
            dataGridViewMasini = new DataGridView
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
                BackgroundColor = Color.White,
                GridColor = Color.LightGray,
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
                }
            };
        }

        private void ConfigureBackButton()
        {
            BtnBack = new Button
            {
                Text = "Înapoi",
                Width = 200,
                Height = 35,
                BackColor = Color.MediumAquamarine,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            BtnBack.Click += BtnBack_Click;
            this.Controls.Add(BtnBack);

            // Poziționează după ce grid-ul este adăugat
            BtnBack.Location = new Point((this.ClientSize.Width - BtnBack.Width) / 2, dataGridViewMasini.Bottom + 30);
            BtnBack.Anchor = AnchorStyles.Top;
        }

        private void AfiseazaToateMasinile()
        {
            List<Masina> masini = adminMasini.GetMasini();
            var masiniSortate = masini.Select(m => new
            {
                ID = m.IdMasina,
                model = m.model.ToString(),
                combustibil = m.combustibil.ToString(),
                an_fabricatie = m.an_fabricatie,
                culoare = m.culoare.ToString().ToUpper()
            }).ToList();

           dataGridViewMasini.DataSource = masiniSortate;
           dataGridViewMasini.Columns["model"].HeaderText = "Model";
           dataGridViewMasini.Columns["combustibil"].HeaderText = "Combustibil";
           dataGridViewMasini.Columns["an_fabricatie"].HeaderText = "An fabricație";
           dataGridViewMasini.Columns["culoare"].HeaderText = "Culoare";
        }

        private void AjusteazaInaltimeGrid()
        {
            int nrRanduri = dataGridViewMasini.RowCount;
            int inaltimeRand = dataGridViewMasini.RowTemplate.Height;
            int inaltimeHeader = dataGridViewMasini.ColumnHeadersHeight;

            dataGridViewMasini.Height = (nrRanduri * inaltimeRand) + inaltimeHeader + 5;
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
