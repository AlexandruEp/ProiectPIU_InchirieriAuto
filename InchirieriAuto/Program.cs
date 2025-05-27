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
            string caleCompletaFisierClienti = Path.Combine(locatieFisierSolutie, numeFisierClienti);
            string caleCompletaFisierMasini = Path.Combine(locatieFisierSolutie, numeFisierMasini);

            AdministrareClienti_FisierText adminClienti = new AdministrareClienti_FisierText(caleCompletaFisierClienti);
            AdministrareMasini_FisierText adminMasini = new AdministrareMasini_FisierText(caleCompletaFisierMasini);

            Client clientNou = new Client();
            Masina masinaNoua = new Masina();

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

                Console.Write("Alegeti o optiune: ");
                opt = Console.ReadLine();
                Console.Clear();

                switch (opt.ToUpper())
                {
                    case "C":
                        // Remove the incorrect method call with an argument
                        masinaNoua = CitireMasinaTastatura();
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

            } while (true);
        }

        public static Masina CitireMasinaTastatura()
        {
            Console.WriteLine("Alegeti marca:");
            foreach (var marca in Enum.GetValues(typeof(MarcaMasina)))
                Console.WriteLine($"{(int)marca}- {marca}");
            MarcaMasina marcaSelectata = (MarcaMasina)Enum.Parse(typeof(MarcaMasina), Console.ReadLine());

            Console.Write("Model: ");
            string model = Console.ReadLine();

            Console.WriteLine("Alegeti combustibilul:");
            foreach (var combustibil in Enum.GetValues(typeof(Tip_combustibil)))
                Console.WriteLine($"{(int)combustibil}- {combustibil}");
            Tip_combustibil tipCombustibil = (Tip_combustibil)Enum.Parse(typeof(Tip_combustibil), Console.ReadLine());

            Console.WriteLine("Alegeti transmisia:");
            foreach (var tr in Enum.GetValues(typeof(TipTransmisie)))
                Console.WriteLine($"{(int)tr}- {tr}");
            TipTransmisie transmisie = (TipTransmisie)Enum.Parse(typeof(TipTransmisie), Console.ReadLine());

            Console.Write("An fabricatie: ");
            int an = int.Parse(Console.ReadLine());

            Console.WriteLine("Alegeti culoarea:");
            foreach (var c in Enum.GetValues(typeof(Culoare_masina)))
                Console.WriteLine($"{(int)c}- {c}");
            Culoare_masina culoare = (Culoare_masina)Enum.Parse(typeof(Culoare_masina), Console.ReadLine());

            Console.Write("Numar usi: ");
            int usi = int.Parse(Console.ReadLine());

            Console.Write("Pret pe zi: ");
            double pret = double.Parse(Console.ReadLine());

            return new Masina
            {
                Marca = marcaSelectata,
                Model = model,
                Combustibil = tipCombustibil,
                Transmisie = transmisie,
                AnFabricatie = an,
                Culoare = culoare,
                NrUsi = usi,
                Pret = pret
            };
        }

        public static string AfisareMasina(Masina m)
        {
            return $"ID: {m.IdMasina}\nMarca: {m.Marca}\nModel: {m.Model}\nCombustibil: {m.Combustibil}\nTransmisie: {m.Transmisie}\nAn: {m.AnFabricatie}\nCuloare: {m.Culoare}\nUsi: {m.NrUsi}\nPret: {m.Pret} lei/zi";
        }

        public static void AfisareMasiniFisier(AdministrareMasini_FisierText admin)
        {
            List<Masina> masini = admin.GetMasini();
            if (masini.Count == 0)
                Console.WriteLine("Nu sunt masini salvate.");
            else
                masini.ForEach(m => Console.WriteLine(AfisareMasina(m) + "\n"));
        }

        public static void IdentificareDupaModel(AdministrareMasini_FisierText adminMasini)
        {
            Console.WriteLine("Introduceti modelul");
            string model = Console.ReadLine();
            Masina masina = adminMasini.CautareDupaModel(model);
            if (masina != null)
                Console.WriteLine(AfisareMasina(masina));
            else
                Console.WriteLine("Modelul nu a fost gasit.");
        }

        public static Client CitireClientT(AdministrareClienti_FisierText adminClienti)
        {
            Console.Write("Nume: ");
            string nume = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Telefon: ");
            string telefon = Console.ReadLine();
            Console.Write("CNP: ");
            string cnp = Console.ReadLine();
            string parola = "parola123"; // Parola default, poate fi schimbata ulterior

            int idNou = adminClienti.GetClienti().Count + 1;
            return new Client(idNou, nume, email, cnp, telefon, parola);
        }

        public static void AfisareClient(Client client)
        {
            Console.WriteLine(client.Info());
        }

        public static void AfisareClienti(List<Client> clienti)
        {
            clienti.ForEach(c => Console.WriteLine(c.Info() + "\n"));
        }

        public static void IdentificareDupaNume(AdministrareClienti_FisierText adminClient)
        {
            Console.Write("Introduceti numele: ");
            string nume = Console.ReadLine();
            Client client = adminClient.CautareDupaNume(nume);
            if (client != null)
                Console.WriteLine(client.Info());
            else
                Console.WriteLine("Clientul nu a fost gasit.");
        }
    }
}
