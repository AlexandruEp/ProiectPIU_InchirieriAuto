using System;
using System.IO;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareClienti_FisierText
    {
        private const int NR_MAX_CLIENTI = 50;
        private string numeFisier;

        public AdministrareClienti_FisierText(string numeFisier)
        {
            this.numeFisier = numeFisier;
            // se incearca deschiderea fisierului in modul OpenOrCreate
            Stream streamFisierText = File.Open(numeFisier, FileMode.OpenOrCreate);
            streamFisierText.Close();
        }

        public void AddClient(Client client)
        {
            // Deschidem fișierul în modul 'append' pentru a adăuga clienți noi
            using (StreamWriter streamWriterFisierText = new StreamWriter(numeFisier, true))
            {
                streamWriterFisierText.WriteLine(client.ConversieLaSir());
            }
        }

        public Client[] GetClienti(out int nrClienti)
        {
            Client[] clienti = new Client[NR_MAX_CLIENTI];
            using (StreamReader streamReader = new StreamReader(numeFisier))
            {
                string linieFisier;
                nrClienti = 0;

                while ((linieFisier = streamReader.ReadLine()) != null)
                {
                    string[] dateClient = linieFisier.Split(';');
                    if (dateClient.Length >= 5)
                    {
                       
                        string nume = dateClient[1];
                        string email = dateClient[2];
                        string CNP = dateClient[3];
                        string telefon = dateClient[4];

                        clienti[nrClienti++] = new Client(nume, email, CNP, telefon);
                    }
                    else
                    {
                        // Handle the case where the line does not have enough fields
                        Console.WriteLine($"Invalid line format: {linieFisier}");
                    }
                }
            }
            return clienti;
        }
    }
}

