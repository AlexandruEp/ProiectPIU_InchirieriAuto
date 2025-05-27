using LibrarieModele;
using NivelStocareDate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace InterfataUtilizator_WindowsForms
{
    public class IstoricInchirieri : Form
    {
        private DataGridView gridInchirieri;
        private Button btnBack;
        private ComboBox cmbFiltru;
        private TextBox txtCautare;
        private Button btnCauta;
        private Button btnReset;
        private readonly AdministrareInchirieri_FisierText adminInchirieri;
        private readonly AdministrareMasini_FisierText adminMasini;
        private readonly AdministrareClienti_FisierText adminClienti;

        public IstoricInchirieri()
        {
            this.Text = "Istoric Închirieri";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ColorTranslator.FromHtml("#e3f2fd");

            string locatie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            adminInchirieri = new AdministrareInchirieri_FisierText(Path.Combine(locatie, "inchirieri.txt"));
            adminMasini = new AdministrareMasini_FisierText(Path.Combine(locatie, ConfigurationManager.AppSettings["NumeFisierMasini"]));
            adminClienti = new AdministrareClienti_FisierText(Path.Combine(locatie, "Clienti.txt"));

            InitializeComponents();
            IncarcaDate();
        }

        private void InitializeComponents()
        {
            // Main layout
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1,
                Padding = new Padding(0)
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // Search panel
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Grid
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // Back button

            // Panel pentru controale de filtrare
            Panel filterPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 50,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            // TableLayout pentru controalele de căutare
            TableLayoutPanel searchControls = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                Padding = new Padding(10, 5, 10, 5)
            };

            // Set column styles for search controls
            searchControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25)); // ComboBox
            searchControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45)); // TextBox
            searchControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15)); // Search button
            searchControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15)); // Reset button

            cmbFiltru = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F)
            };
            cmbFiltru.Items.AddRange(new string[] { "Client", "Mașină", "Data început", "Data sfârșit" });
            cmbFiltru.SelectedIndex = 0;

            txtCautare = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F)
            };

            btnCauta = new Button
            {
                Text = "Caută",
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCauta.Click += BtnCauta_Click;

            btnReset = new Button
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
                IncarcaDate();
            };

            // Add controls to search panel
            searchControls.Controls.Add(cmbFiltru, 0, 0);
            searchControls.Controls.Add(txtCautare, 1, 0);
            searchControls.Controls.Add(btnCauta, 2, 0);
            searchControls.Controls.Add(btnReset, 3, 0);
            filterPanel.Controls.Add(searchControls);

            // Panel principal pentru DataGridView
            Panel gridPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 10, 20, 10)
            };

            // Grid pentru închirieri
            gridInchirieri = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                RowHeadersVisible = false,
                AllowUserToResizeRows = false,
                AutoGenerateColumns = true,
                BorderStyle = BorderStyle.None,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    BackColor = Color.FromArgb(41, 128, 185),
                    ForeColor = Color.White,
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 9F),
                    SelectionBackColor = Color.FromArgb(52, 152, 219),
                    SelectionForeColor = Color.White
                }
            };

            gridPanel.Controls.Add(gridInchirieri);

            // Panel pentru butonul înapoi
            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 50
            };

            // Buton înapoi
            btnBack = new Button
            {
                Text = "Înapoi",
                Width = 120,
                Height = 35,
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.None
            };
            btnBack.Click += (s, e) => this.Close();

            // Center the back button
            TableLayoutPanel backButtonLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Padding = new Padding(0)
            };
            backButtonLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            backButtonLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            backButtonLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            backButtonLayout.Controls.Add(btnBack, 1, 0);
            bottomPanel.Controls.Add(backButtonLayout);

            // Add all panels to main layout
            mainLayout.Controls.Add(filterPanel, 0, 0);
            mainLayout.Controls.Add(gridPanel, 0, 1);
            mainLayout.Controls.Add(bottomPanel, 0, 2);

            this.Controls.Add(mainLayout);
        }

        private void IncarcaDate()
        {
            try
            {
                var inchirieri = adminInchirieri.GetInchirieri();
                var masini = adminMasini.GetMasini();
                var clienti = adminClienti.GetClienti();

                if (inchirieri == null || !inchirieri.Any())
                {
                    MessageBox.Show("Nu există închirieri în baza de date.", "Informație", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (masini == null || !masini.Any() || clienti == null || !clienti.Any())
                {
                    return;
                }

                var rezultate = from i in inchirieri
                              join m in masini on i.IdMasina equals m.IdMasina
                              join c in clienti on i.IdClient equals c.IdClient
                              select new
                              {
                                  Client = $"{c.nume}",
                                  Masina = $"{m.Marca} {m.Model}",
                                  DataStart = i.DataStart.ToString("dd.MM.yyyy"),
                                  DataSfarsit = i.DataEnd.ToString("dd.MM.yyyy"),
                                  DataReturnare = i.DataReturnare.HasValue ? i.DataReturnare.Value.ToString("dd.MM.yyyy") : "-",
                                  PretTotal = $"{i.PretTotal:F2} lei",
                                  Status = i.DataReturnare.HasValue ? 
                                          (i.DataReturnare.Value <= i.DataEnd ? "Returnată la timp" : "Returnată cu întârziere") :
                                          (DateTime.Now > i.DataEnd ? "Încheiată" : "În derulare")
                              };

                var listaRezultate = rezultate.ToList();

                if (!listaRezultate.Any())
                {
                    MessageBox.Show("Nu s-au găsit închirieri care să corespundă criteriilor.", "Informație", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                gridInchirieri.DataSource = null;
                gridInchirieri.DataSource = listaRezultate;
                gridInchirieri.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcarea datelor: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCauta_Click(object sender, EventArgs e)
        {
            string cautare = txtCautare.Text.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(cautare))
            {
                IncarcaDate();
                return;
            }

            var inchirieri = adminInchirieri.GetInchirieri();
            var masini = adminMasini.GetMasini();
            var clienti = adminClienti.GetClienti();

            var rezultate = from i in inchirieri
                          join m in masini on i.IdMasina equals m.IdMasina
                          join c in clienti on i.IdClient equals c.IdClient
                          where (cmbFiltru.SelectedItem.ToString() == "Client" && 
                                 (c.nume.ToLower().Contains(cautare)) ||
                                (cmbFiltru.SelectedItem.ToString() == "Mașină" && 
                                 (m.Marca.ToString().ToLower().Contains(cautare) || m.Model.ToLower().Contains(cautare))) ||
                                (cmbFiltru.SelectedItem.ToString() == "Data început" && 
                                 i.DataStart.ToString("dd.MM.yyyy").Contains(cautare)) ||
                                (cmbFiltru.SelectedItem.ToString() == "Data sfârșit" && 
                                 i.DataEnd.ToString("dd.MM.yyyy").Contains(cautare)))
                          select new
                          {
                              Client = $"{c.nume}",
                              Masina = $"{m.Marca} {m.Model}",
                              DataStart = i.DataStart.ToString("dd.MM.yyyy"),
                              DataSfarsit = i.DataEnd.ToString("dd.MM.yyyy"),
                              DataReturnare = i.DataReturnare.HasValue ? i.DataReturnare.Value.ToString("dd.MM.yyyy") : "-",
                              PretTotal = $"{i.PretTotal:F2} lei",
                              Status = i.DataReturnare.HasValue ? 
                                      (i.DataReturnare.Value <= i.DataEnd ? "Returnată la timp" : "Returnată cu întârziere") :
                                      (DateTime.Now > i.DataEnd ? "Încheiată" : "În derulare")
                          };

            gridInchirieri.DataSource = rezultate.ToList();
        }
    }
} 