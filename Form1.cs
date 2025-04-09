namespace InterfataUtilizator_WindowsForms
{
    public partial class Form1 : Form
    {
        private Label lblClientInfo; // Add this line to declare the label

        public Form1()
        {
            InitializeComponent();
            lblClientInfo = new Label(); // Initialize the label
            lblClientInfo.Location = new Point(10, 10); // Set the location of the label
            lblClientInfo.Size = new Size(300, 100); // Set the size of the label
            this.Controls.Add(lblClientInfo); // Add the label to the form
            AfiseazaUltimulClient();
        }

        private void AfiseazaUltimulClient()
        {
            string numeFisierClienti = ConfigurationManager.AppSettings["NumeFisierClienti"];
            string locatieFisierSolutie = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string caleCompletaFisierClienti = Path.Combine(locatieFisierSolutie, numeFisierClienti);

            AdministrareClienti_FisierText adminClienti = new AdministrareClienti_FisierText(caleCompletaFisierClienti);

            int nrClienti;
            Client[] clienti = adminClienti.GetClienti(out nrClienti);

            if (nrClienti > 0)
            {
                Client ultimulClient = clienti[nrClienti - 1];

                // Asigurã-te cã ai un Label pe Form cu numele "lblClientInfo"
                lblClientInfo.Text = $"Nume: {ultimulClient.nume}\n" +
                                     $"Email: {ultimulClient.email}\n" +
                                     $"Telefon: {ultimulClient.telefon}\n" +
                                     $"CNP: {ultimulClient.CNP}";
            }
            else
            {
                lblClientInfo.Text = "Nu existã clien?i înregistra?i.";
            }
        }
    }
}
