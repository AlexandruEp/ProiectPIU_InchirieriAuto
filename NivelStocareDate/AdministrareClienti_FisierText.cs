using LibrarieModele;
using System;
using System.Collections.Generic;
using System.IO;

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

        public List<Client> GetClienti()
        {
            var clienti = new List<Client>();
            using (StreamReader streamReader = new StreamReader(numeFisier))
            {
                string linieFisier;


                while ((linieFisier = streamReader.ReadLine()) != null)
                {
                    linieFisier = linieFisier.Trim();
                    if (string.IsNullOrWhiteSpace(linieFisier))
                    {
                        continue;
                    }
                    string[] dateClient = linieFisier.Split(';');
                    if (dateClient.Length >= 5)
                    {
                        // Updated line in GetClienti method
                        int idClient = int.Parse(dateClient[0]); // Convert the string to an integer
                        string nume = dateClient[1];
                        string email = dateClient[2];
                        string telefon = dateClient[3];
                        string CNP = dateClient[4];
                        string parola = dateClient.Length > 5 ? dateClient[5] : "parola123"; // parola default pentru compatibilitate

                        clienti.Add(new Client(idClient, nume, email, CNP, telefon, parola));
                    }
                }
            }
            return clienti;
        }
        public Client CautareDupaNume(string nume)
        {
            List<Client> clienti = GetClienti();
            foreach (var client in clienti)
            {
                if (!string.IsNullOrEmpty(client.nume) && client.nume.Equals(nume, StringComparison.OrdinalIgnoreCase))
                {
                    return client;
                }
            }
            return null;
        }
        public void SalveazaClienti(List<Client> listaClienti)
        {
            using (StreamWriter writer = new StreamWriter(numeFisier, false)) // false = rescrie complet fișierul
            {
                foreach (var client in listaClienti)
                {
                    writer.WriteLine(client.ConversieLaSir());
                }
            }
        }

    }
}

