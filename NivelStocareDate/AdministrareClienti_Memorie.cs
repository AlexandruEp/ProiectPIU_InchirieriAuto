using System;
using System.IO;
using LibrarieModele;

namespace NivelStocareDateNou
{
    class AdministrareClienti_Memorie
    {
        private const int NR_MAX_CLIENTI = 50;

        private Client[] clienti;
        private int nrClienti;

        public AdministrareClienti_Memorie()
        {
            clienti = new Client[NR_MAX_CLIENTI];
            nrClienti = 0;
        }

        // Adaugă un client în memorie
        public void AddClient(Client client)
        {
            if (nrClienti < NR_MAX_CLIENTI)
            {
                clienti[nrClienti] = client;
                nrClienti++;
            }
            else
            {
                Console.WriteLine("Numărul maxim de clienți a fost atins!");
            }
        }

        // Obține lista de clienți din memorie
        public Client[] GetClienti(out int nrClienti)
        {
            nrClienti = this.nrClienti;
            return clienti;
        }
    }
}

