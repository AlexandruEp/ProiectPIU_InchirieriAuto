using System;
using System.Configuration;
using LibrarieModele;
using NivelStocareDate;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace InchirieriAuto
{
    public class Program
    {
        static void Main()
        {
            string numeFisierClienti = ConfigurationManager.AppSettings["NumeFisierClienti"];
            string numeFisierMasini = ConfigurationManager.AppSettings["NumeFisierMasini"];
            string locatieFisierSolutie = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string caleCompletaFisierClienti = Path.Combine(locatieFisierSolutie,numeFisierClienti);
            string caleCompletaFisierMasini = Path.Combine(locatieFisierSolutie, numeFisierMasini);

            AdministrareClienti_FisierText adminClienti = new AdministrareClienti_FisierText(caleCompletaFisierClienti);
            AdministrareMasini_FisierText adminMasini = new AdministrareMasini_FisierText(caleCompletaFisierMasini);

            Client clientNou = new Client();
            Masina masinaNoua = new Masina();
           
            List<Client> Clienti = adminClienti.GetClienti();
            int nrClienti = Clienti.Count;
            string opt;

            do
            {
                Console.WriteLine("\nMENIU: \n");
                Console.WriteLine("C. Citire masina de la tastatura");
                Console.WriteLine("A. Afisare masina");
                Console.WriteLine("S. Salvare masina");
                Console.WriteLine("W. Afisare masini salvate in fisier");
                Console.WriteLine("E. Afisare date masina dupa model");

                Console.WriteLine("D. Citire client de la tastatura");
                Console.WriteLine("I. Afisare client");
                Console.WriteLine("Y. Afisare date client dupa nume");
                Console.WriteLine("P. Salvare client");
                Console.WriteLine("R. Afisare clienti salvati");
                
                Console.WriteLine("X. Inchidere program");
                Console.WriteLine();

                Console.WriteLine("Alegeti o optiune: ");
                opt = Console.ReadLine();
                Console.Clear();

                switch (opt.ToUpper())
                {
                    case "C":
                        masinaNoua = CitireMasinaTastatura(adminMasini);
                        Console.WriteLine();
                        break;

                    case "A":
                        Console.WriteLine(AfisareMasina(masinaNoua));
                        break;
                    case "S":
                        adminMasini.AddMasina(masinaNoua);
                        Console.WriteLine("Masina salvata cu succes");
                        break;
                    case "W":
                        AfisareMasiniFisier(adminMasini);
                        break;
                    case "E":
                        IdentificareDupaModel(adminMasini);
                        break;
                    case "D":
                        clientNou = CitireClientT(adminClienti);
                       
                        Console.WriteLine();
                        break;

                    case "I":
                        AfisareClient(clientNou);
                        break;

                    case "Y":
                        IdentificareDupaNume(adminClienti);
                        break;

                    case "P":
                        adminClienti.AddClient(clientNou);
                        Console.WriteLine("Client salvat");
                        break;

                    case "R":

                        List<Client> clienti = adminClienti.GetClienti();
                        AfisareClienti(clienti);
                        break;

                    case "X":
                        return;

                    default:
                        Console.WriteLine("Optiune inexistenta");
                        break;
                }
            
            } while(true);
        

        }

        public static Masina CitireMasinaTastatura(AdministrareMasini_FisierText adminMasini)
        {
            Console.WriteLine("Alegeti modelul: ");
            foreach (var model in Enum.GetValues(typeof(Model_masina)))
            {
                Console.WriteLine($"{(int)model}- {model}");
            }
            Model_masina modelMasina;

            while (!Enum.TryParse(Console.ReadLine(), out modelMasina) || !Enum.IsDefined(typeof(Model_masina), modelMasina))
            {
                Console.WriteLine("Valoare invalida! Alegeti din lista:");
            }

            Console.WriteLine("Alegeti tipul combustibilului: ");
            foreach (var combustibil in Enum.GetValues(typeof(Tip_combustibil)))
            {
                Console.WriteLine($"{(int)combustibil}- {combustibil}");
            }
            Tip_combustibil tip_Combustibil;
            while (!Enum.TryParse(Console.ReadLine(), out tip_Combustibil) || !Enum.IsDefined(typeof(Tip_combustibil), tip_Combustibil))
            {
                Console.WriteLine("Valoare invalida! Alegeti din lista:");
            }

            Console.WriteLine("Introduceti anul fabricatiei: ");
            int anFabricatie;
            while (!int.TryParse(Console.ReadLine(), out anFabricatie))
            {
                Console.WriteLine("Anul trebuie sa fie un numar! Introduceti din nou: ");
            }

            Console.WriteLine("Alegeti culoarea: ");
            foreach (var culoare in Enum.GetValues(typeof(Culoare_masina)))
            {
                Console.WriteLine($"{(int)culoare}- {culoare}");
            }
            Culoare_masina culoareMasina;
            while (!Enum.TryParse(Console.ReadLine(), out culoareMasina) || !Enum.IsDefined(typeof(Culoare_masina), culoareMasina))
            {
                Console.WriteLine("Valoare invalida! Alegeti din lista:");
            }

            // Generate a new ID for the car
            List<Masina> masiniExistente = adminMasini.GetMasini();
            int idNou = masiniExistente.Count > 0
                ? masiniExistente.Max(m => m.IdMasina) + 1
                : 1;

            return new Masina()
            {
                IdMasina = idNou,
                model = modelMasina,
                combustibil = tip_Combustibil,
                an_fabricatie = anFabricatie,
                culoare = culoareMasina
            };
        }

        public static string AfisareMasina(Masina masina)
        {
            string informatii_masina = string.Format("{0}. Masina {1}\n" +
                "Motorizare: {2}\n" +
                "Fabricata in {3}\n" +
                "De culoare {4}",
                masina.IdMasina,
                masina.model.ToString(),
                masina.combustibil.ToString(),
                masina.an_fabricatie,
                masina.culoare.ToString());

            return informatii_masina;
        }

        public static void AfisareMasiniFisier(AdministrareMasini_FisierText administrareMasini_FisierText)
        {
            List<Masina> masini = administrareMasini_FisierText.GetMasini();
            int nrMasini = masini.Count; // Added this line to define nrMasini
            if (nrMasini == 0)
            {
                Console.WriteLine("Nu sunt masini salvate");
            }
            else
            {
                Console.WriteLine("\nMasinile salvate sunt: ");
                for (int i = 0; i < nrMasini; i++)
                {
                    string infoMasina = AfisareMasina(masini[i]);
                    Console.WriteLine(infoMasina);
                }
            }
        }
        public static void IdentificareDupaModel(AdministrareMasini_FisierText adminMasini)
        {
            Console.WriteLine("Introduceti modelul");
            string numeM = Console.ReadLine();
            Masina masinaGasitNume = adminMasini.CautareDupaModel(numeM);
            if (masinaGasitNume != null)
            {
                Console.WriteLine($"Masina: {masinaGasitNume.model},motorizare: {masinaGasitNume.combustibil}," +
                    $"fabricata in {masinaGasitNume.an_fabricatie}, de culoare {masinaGasitNume.culoare}");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Modelul introdus nu corespunde");
            }
        }
        public static Client CitireClientT(AdministrareClienti_FisierText adminClienti)
        {
            Console.WriteLine("Introduceti numele: ");
            string nume = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("Introduceti email-ul: ");
            string email = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("Introduceti numarul de telefon: ");
            string telefon = Console.ReadLine();

            while (telefon.Length != 10)
            {
                Console.WriteLine("Numar de telefon invalid. Reintroduceti: ");
                telefon = Console.ReadLine();
            }
            Console.WriteLine("Introduceti CNP-ul: ");
            string CNP = Console.ReadLine();

            while (CNP.Length != 13)
            {
                Console.WriteLine("CNP invalid. Reintroduceti: ");
                CNP = Console.ReadLine();
            }
            Console.WriteLine();

            // 2. Obții lista de clienți existenți
            List<Client> clientiExistenti = adminClienti.GetClienti();

            // 3. Calculezi următorul ID disponibil
            int idNou = clientiExistenti.Count > 0
                ? clientiExistenti.Max(c => c.IdClient) + 1
                : 1;

            // 4. Creezi un nou client cu ID-ul calculat
            Client clientNou = new Client
            {
                IdClient = idNou,
                nume = nume,
                email = email,
                CNP = CNP,
                telefon = telefon
            };

            // 5. Returnezi clientul complet
            return clientNou;
        }

        public static void AfisareClient(Client client)
        {
            Console.WriteLine(client.Info());
        }
        public static void AfisareClienti(List<Client> clienti)
        {
            foreach (var client in clienti)
            {
                if (client != null)
                {
                    AfisareClient(client);
                    Console.WriteLine();
                }
            }
        }
        public static void IdentificareDupaNume(AdministrareClienti_FisierText adminClient)
        {
            Console.WriteLine("Introduceti numele");
            string numeC = Console.ReadLine();
            Client clientGasitNume = adminClient.CautareDupaNume(numeC);
            if (clientGasitNume != null)
            {
                Console.WriteLine($"Clientul {clientGasitNume.nume} are numarul de telefon {clientGasitNume.telefon}");
            }
            else
            {
                Console.WriteLine("Numele introdus nu corespunde");
            }
        }
    
    }
}
