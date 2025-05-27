using System;

namespace LibrarieModele
{
    public class Masina
    {
        public int IdMasina { get; set; }
        public MarcaMasina Marca { get; set; }
        public string Model { get; set; }
        public Tip_combustibil Combustibil { get; set; }
        public TipTransmisie Transmisie { get; set; }
        public int AnFabricatie { get; set; }
        public Culoare_masina Culoare { get; set; }
        public string ImagePath { get; set; }
        public int NrUsi { get; set; }
        public double Pret { get; set; }
        public bool Disponibil { get; set; } = true; // Adăugat pentru a marca disponibilitatea mașinii

        private const char SEPARATOR_PRINCIPAL_FISIER = ';';

        // Indici pentru parsare din fișier
        private const int ID = 0;
        private const int MARCA = 1;
        private const int MODEL = 2;
        private const int COMBUSTIBIL = 3;
        private const int TRANSMISIE = 4;
        private const int AN = 5;
        private const int CULOARE = 6;
        private const int IMAGINE = 7;
        private const int NR_USI = 8;
        private const int PRET = 9;

        public Masina()
        {
            Marca = MarcaMasina.None;
            Model = string.Empty;
            Combustibil = Tip_combustibil.None;
            Transmisie = TipTransmisie.Manuala;
            AnFabricatie = 0;
            Culoare = Culoare_masina.None;
            ImagePath = string.Empty;
            NrUsi = 0;
            Pret = 0.0;
        }

        public Masina(int id, MarcaMasina marca, string model, Tip_combustibil combustibil, TipTransmisie transmisie,
                      int an, Culoare_masina culoare, string imagePath, int nrUsi, double pret)
        {
            IdMasina = id;
            Marca = marca;
            Model = model;
            Combustibil = combustibil;
            Transmisie = transmisie;
            AnFabricatie = an;
            Culoare = culoare;
            ImagePath = imagePath;
            NrUsi = nrUsi;
            Pret = pret;
        }

        public Masina(string linieFisier)
        {
            string[] date = linieFisier.Split(SEPARATOR_PRINCIPAL_FISIER);

            IdMasina = int.Parse(date[ID]);
            Marca = (MarcaMasina)Enum.Parse(typeof(MarcaMasina), date[MARCA]);
            Model = date[MODEL];
            Combustibil = (Tip_combustibil)Enum.Parse(typeof(Tip_combustibil), date[COMBUSTIBIL]);
            Transmisie = (TipTransmisie)Enum.Parse(typeof(TipTransmisie), date[TRANSMISIE]);
            AnFabricatie = int.Parse(date[AN]);
            Culoare = (Culoare_masina)Enum.Parse(typeof(Culoare_masina), date[CULOARE]);
            ImagePath = date[IMAGINE];
            NrUsi = int.Parse(date[NR_USI]);
            Pret = double.Parse(date[PRET]);
            if (date.Length > 10)
            {
                Disponibil = bool.Parse(date[10]);
            }
            else
            {
                Disponibil = true; // Dacă nu este specificat, presupunem că mașina este disponibilă
            }
        }

        public string ConversieLaSir()
        {
            return string.Join(SEPARATOR_PRINCIPAL_FISIER.ToString(), new string[]
            {
                IdMasina.ToString(),
                Marca.ToString(),
                Model,
                Combustibil.ToString(),
                Transmisie.ToString(),
                AnFabricatie.ToString(),
                Culoare.ToString(),
                ImagePath,
                NrUsi.ToString(),
                Pret.ToString("F2"),
                Disponibil.ToString()
            });
        }

        public string Info()
        {
            return $"ID: {IdMasina}\n" +
                   $"Marca: {Marca}\n" +
                   $"Model: {Model}\n" +
                   $"Combustibil: {Combustibil}\n" +
                   $"Transmisie: {Transmisie}\n" +
                   $"An: {AnFabricatie}\n" +
                   $"Culoare: {Culoare}\n" +
                   $"Uși: {NrUsi}\n" +
                   $"Preț/zi: {Pret:F2} lei";
        }

        public void SetID(int id)
        {
            IdMasina = id;
        }
    }
}
