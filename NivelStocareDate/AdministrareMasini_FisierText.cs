using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareMasini_FisierText
    {
        private const int NR_MAX_MASINI = 50;
        private string numeFisier;

        public AdministrareMasini_FisierText(string numeFisier)
        {
            this.numeFisier = numeFisier;
            // se incearca deschiderea fisierului in modul OpenOrCreate
            Stream streamFisierText = File.Open(numeFisier, FileMode.OpenOrCreate);
            streamFisierText.Close();
        }
        public void AddMasina(Masina masina)
        {
            int idNou = GetUltimulID() + 1;
            masina.IdMasina = idNou;

            using (StreamWriter streamWriter = new StreamWriter(numeFisier, true))
            {
                streamWriter.WriteLine(masina.ConversieLaSir());
            }
        }
        public List<Masina> GetMasini()
        {
            List<Masina> masini = new List<Masina>();

            using (StreamReader streamReader = new StreamReader(numeFisier))
            {
                string linieFisier;

                while ((linieFisier = streamReader.ReadLine()) != null)
                {
                    linieFisier = linieFisier.Trim();
                    if (string.IsNullOrWhiteSpace(linieFisier)) continue;

                    string[] dateMasina = linieFisier.Split(';');
                    if (dateMasina.Length >= 5)
                    {
                        try
                        {
                            int id = Convert.ToInt32(dateMasina[0]);
                            Model_masina model = (Model_masina)Enum.Parse(typeof(Model_masina), dateMasina[1]);
                            Tip_combustibil combustibil = (Tip_combustibil)Enum.Parse(typeof(Tip_combustibil), dateMasina[2]);
                            int an_fabricatie = Convert.ToInt32(dateMasina[3]);
                            Culoare_masina culoare = (Culoare_masina)Enum.Parse(typeof(Culoare_masina), dateMasina[4]);

                            Masina masina = new Masina(id, model, combustibil, an_fabricatie, culoare);
                            masina.IdMasina = id;

                            masini.Add(masina);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Eroare la parsarea liniei: {linieFisier}\n{ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Linie invalidă: {linieFisier}");
                    }
                }
            }

            return masini;
        }
        public int GetUltimulID()
        {
            List<Masina> masini = GetMasini();
            return masini.Count > 0 ? masini.Max(m => m.IdMasina) : 0;
        }
        public Masina CautareDupaModel(string nume)
        {
            List<Masina> masini = GetMasini();

            foreach (var masina in masini)
            {
                if (masina.model.ToString().Equals(nume, StringComparison.OrdinalIgnoreCase))
                {
                    return masina;
                }
            }
            return null;
        }





    }
}