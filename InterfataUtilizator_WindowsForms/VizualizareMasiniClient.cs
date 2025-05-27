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
    public partial class VizualizareMasiniClient : Form
    {
        private FlowLayoutPanel panelMasini;
        private AdministrareMasini_FisierText adminMasini;
        private List<Masina> listaMasini;
        private TextBox txtCautare;
        private ComboBox cmbCriteriu;
        private Button btnCauta, btnReset, btnBack;
        private Client clientCurent;
        private CheckBox chkShowAll;

        public VizualizareMasiniClient(Client client)
        {
            clientCurent = client;

            // Calculăm dimensiunea optimă pentru 3 carduri
            int cardWidth = 300; // Lățimea unui card
            int cardMargin = 10; // Marginea unui card
            int searchPanelHeight = 50; // Înălțimea panoului de căutare
            int buttonPanelHeight = 50; // Înălțimea panoului cu butonul înapoi
            int windowPadding = 40; // Padding pentru fereastră

            // Calculăm lățimea totală necesară pentru 3 carduri + margini
            int optimalWidth = (cardWidth * 3) + (cardMargin * 6) + windowPadding;
            
            // Înălțimea optimă pentru un rând de carduri + controalele adiționale
            int optimalHeight = searchPanelHeight + 450 + buttonPanelHeight + windowPadding;

            this.Text = "Lista Mașini";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Normal;
            this.MinimumSize = new Size(optimalWidth, optimalHeight);
            this.Size = new Size(optimalWidth, optimalHeight);
            this.Font = new Font("Segoe UI", 10F);
            this.BackColor = ColorTranslator.FromHtml("#e3f2fd");

            string numeFisierMasini = ConfigurationManager.AppSettings["NumeFisierMasini"];
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string caleCompletaFisierMasini = Path.Combine(locatieFisierSolutie, numeFisierMasini);
            adminMasini = new AdministrareMasini_FisierText(caleCompletaFisierMasini);
            listaMasini = adminMasini.GetMasini();

            ConfigureazaComponente();

            AfiseazaToateMasinile(listaMasini.Where(m => m.Disponibil).ToList());
        }

        private void ConfigureazaComponente()
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
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Cards panel
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // Back button

            // Panel pentru controalele de căutare
            Panel searchPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            // TableLayout pentru controalele de căutare
            TableLayoutPanel searchControls = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 5,
                Padding = new Padding(10, 5, 10, 5)
            };

            // Set column styles for search controls
            searchControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20)); // ComboBox
            searchControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40)); // TextBox
            searchControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13)); // Search button
            searchControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13)); // Reset button
            searchControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14)); // Show all checkbox

            cmbCriteriu = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F)
            };
            cmbCriteriu.Items.AddRange(new string[] { "Marcă", "Model", "Combustibil", "Transmisie", "Culoare", "An", "Pret" });
            cmbCriteriu.SelectedIndex = 0;

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
                cmbCriteriu.SelectedIndex = 0;
                chkShowAll.Checked = false;
                listaMasini = adminMasini.GetMasini();
                AfiseazaToateMasinile(listaMasini.Where(m => m.Disponibil).ToList());
            };

            chkShowAll = new CheckBox
            {
                Text = "Arată toate",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F),
                TextAlign = ContentAlignment.MiddleCenter
            };
            chkShowAll.CheckedChanged += (s, e) =>
            {
                AfiseazaToateMasinile(chkShowAll.Checked ? listaMasini : listaMasini.Where(m => m.Disponibil).ToList());
            };

            searchControls.Controls.Add(cmbCriteriu, 0, 0);
            searchControls.Controls.Add(txtCautare, 1, 0);
            searchControls.Controls.Add(btnCauta, 2, 0);
            searchControls.Controls.Add(btnReset, 3, 0);
            searchControls.Controls.Add(chkShowAll, 4, 0);

            searchPanel.Controls.Add(searchControls);

            // Panel pentru cardurile de mașini
            panelMasini = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(20, 10, 20, 10)
            };

            // Panel pentru butonul înapoi
            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            // Center the back button
            TableLayoutPanel backButtonLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1
            };
            backButtonLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            backButtonLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            backButtonLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

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

            backButtonLayout.Controls.Add(btnBack, 1, 0);
            bottomPanel.Controls.Add(backButtonLayout);

            // Add all panels to main layout
            mainLayout.Controls.Add(searchPanel, 0, 0);
            mainLayout.Controls.Add(panelMasini, 0, 1);
            mainLayout.Controls.Add(bottomPanel, 0, 2);

            this.Controls.Add(mainLayout);

            // Add resize handler for cards
            this.Resize += (s, e) => RecalculateCardSizes();
        }

        private void RecalculateCardSizes()
        {
            const int CARDS_PER_ROW = 3;
            const int CARD_MARGIN = 10;
            const int MIN_CARD_WIDTH = 280;

            // Calculăm lățimea disponibilă pentru carduri
            int availableWidth = panelMasini.ClientSize.Width - panelMasini.Padding.Left - panelMasini.Padding.Right;
            
            // Calculăm lățimea unui card (inclusiv marginile)
            int cardWidth = Math.Max(MIN_CARD_WIDTH, (availableWidth - (CARD_MARGIN * 2 * (CARDS_PER_ROW - 1))) / CARDS_PER_ROW);
            
            // Calculăm padding-ul lateral pentru centrare
            int totalCardsWidth = (cardWidth * CARDS_PER_ROW) + (CARD_MARGIN * 2 * (CARDS_PER_ROW - 1));
            int sidePadding = Math.Max(20, (availableWidth - totalCardsWidth) / 2);
            
            panelMasini.Padding = new Padding(sidePadding, 20, sidePadding, 20);

            foreach (Control control in panelMasini.Controls)
            {
                if (control is Panel card)
                {
                    card.Width = cardWidth;
                    card.Margin = new Padding(CARD_MARGIN);

                    foreach (Control childControl in card.Controls)
                    {
                        if (childControl is PictureBox pic)
                        {
                            pic.Width = cardWidth - 20;
                        }
                        else if (childControl is Label lbl)
                        {
                            lbl.Width = cardWidth - 20;
                        }
                        else if (childControl is Button btn)
                        {
                            btn.Width = cardWidth - 40;
                            btn.Left = 20;
                        }
                    }
                }
            }
        }

        private void AfiseazaToateMasinile(List<Masina> masini)
        {
            panelMasini.Controls.Clear();
            if (!masini.Any())
            {
                panelMasini.Controls.Add(new Label
                {
                    Text = "Nu s-au găsit mașini.",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 12F),
                    ForeColor = Color.Gray,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                });
                return;
            }

            // Dimensiuni
            int nrColoane = 3;
            int cardWidth = 300;
            int cardHeight = 350; // Redus înălțimea cardului

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = nrColoane,
                RowCount = (int)Math.Ceiling(masini.Count / (double)nrColoane),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };

            for (int i = 0; i < nrColoane; i++)
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / nrColoane));

            int row = 0, col = 0;
            foreach (var m in masini)
            {
                Panel card = new Panel
                {
                    Size = new Size(cardWidth, cardHeight),
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(10)
                };

                PictureBox img = new PictureBox
                {
                    Size = new Size(260, 150),
                    Image = File.Exists(m.ImagePath) ? Image.FromFile(m.ImagePath) : null,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Location = new Point(20, 10)
                };

                Label lbl = new Label
                {
                    Text = $"{m.Marca} {m.Model}\n{m.Combustibil}, {m.Transmisie}\n{m.Culoare}, {m.AnFabricatie}\n{m.NrUsi} uși, {m.Pret} lei/zi",
                    Location = new Point(20, 170),
                    Size = new Size(260, 80), // Redus înălțimea label-ului
                    Font = new Font("Segoe UI", 9),
                    ForeColor = m.Disponibil ? Color.Black : Color.Gray
                };

                Button btn = new Button
                {
                    Text = "Închiriază",
                    Enabled = m.Disponibil,
                    Size = new Size(200, 35), // Redus lățimea butonului pentru centrare mai bună
                    Location = new Point((cardWidth - 200) / 2, 260), // Centrat butonul și ajustat poziția vertical
                    BackColor = m.Disponibil ? Color.MediumSeaGreen : Color.Gray,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };

                btn.Click += (s, e) =>
                {
                    var f = new FormInchiriereMasina(m, clientCurent);
                    f.FormClosed += (s2, e2) =>
                    {
                        listaMasini = adminMasini.GetMasini();
                        AfiseazaToateMasinile(chkShowAll.Checked ? listaMasini : listaMasini.Where(x => x.Disponibil).ToList());
                    };
                    f.ShowDialog();
                };

                card.Controls.Add(img);
                card.Controls.Add(lbl);
                card.Controls.Add(btn);

                layout.Controls.Add(card, col, row);
                col++;
                if (col >= nrColoane)
                {
                    col = 0;
                    row++;
                }
            }

            panelMasini.Controls.Add(layout);
        }


        private void BtnCauta_Click(object sender, EventArgs e)
        {
            string valoare = txtCautare.Text.Trim().ToLower();
            string criteriu = cmbCriteriu.SelectedItem.ToString();

            if (string.IsNullOrWhiteSpace(valoare))
            {
                listaMasini = adminMasini.GetMasini();
                AfiseazaToateMasinile(chkShowAll.Checked ? listaMasini : listaMasini.Where(m => m.Disponibil).ToList());
                return;
            }

            var toateMasinile = adminMasini.GetMasini();
            var filtrate = toateMasinile.Where(m =>
            {
                switch (criteriu)
                {
                    case "Marcă":
                        return m.Marca.ToString().ToLower().Contains(valoare);
                    case "Model":
                        return m.Model.ToLower().Contains(valoare);
                    case "Combustibil":
                        return m.Combustibil.ToString().ToLower().Contains(valoare);
                    case "Transmisie":
                        return m.Transmisie.ToString().ToLower().Contains(valoare);
                    case "Culoare":
                        return m.Culoare.ToString().ToLower().Contains(valoare);
                    case "An":
                        int anCautat;
                        if (int.TryParse(valoare, out anCautat))
                            return m.AnFabricatie == anCautat;
                        return m.AnFabricatie.ToString().Contains(valoare);
                    case "Pret":
                        if (double.TryParse(valoare, out double pretCautat))
                            return m.Pret <= pretCautat;
                        return false;
                    default:
                        return false;
                }
            }).ToList();

            if (!chkShowAll.Checked)
                filtrate = filtrate.Where(m => m.Disponibil).ToList();

            if (!filtrate.Any())
            {
                MessageBox.Show("Nu s-au găsit mașini care să corespundă criteriilor de căutare.", "Informație", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            listaMasini = filtrate;
            AfiseazaToateMasinile(filtrate);
        }
    }
}
