using LibrarieModele; // Ensure this namespace is included

namespace InchirieriAuto
{
    public class Program
    {
        // Other code...

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
    }
}
