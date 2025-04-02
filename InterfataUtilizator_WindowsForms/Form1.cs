using LibrarieModele;
using NivelStocareDate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;


namespace InterfataUtilizator_WindowsForms
{
    public partial class Form1 : Form
    {
        private Label lblClientInfo;
        public Form1()
        {
            InitializeComponent();
            lblClientInfo = new Label();
            lblClientInfo.Name = "lblClientInfo";
            lblClientInfo.AutoSize = true;
            lblClientInfo.Location = new Point(30, 30);
            lblClientInfo.Font = new Font("Arial", 10);
            this.Controls.Add(lblClientInfo);
            AfiseazaUltimulClient();
        }

        private void AfiseazaUltimulClient()
        {
            string numeFisierClienti = ConfigurationManager.AppSettings["NumeFisierClienti"];
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string caleCompletaFisierClienti = Path.Combine(locatieFisierSolutie, numeFisierClienti);

            AdministrareClienti_FisierText adminClienti = new AdministrareClienti_FisierText(caleCompletaFisierClienti);
            int nrClienti = 0;
            Client[] clienti = adminClienti.GetClienti(out nrClienti);

            if (nrClienti > 0)
            {
                Client ultimulClient = clienti[nrClienti - 1];

                lblClientInfo.Text = $@"Nume: {ultimulClient.nume}
                                    Email: {ultimulClient.email}
                                    Telefon: {ultimulClient.telefon}
                                    CNP: {ultimulClient.CNP}";
            }
            else
            {
                lblClientInfo.Text = "Nu există clienți înregistrați.";
            }
        }

    }

}
