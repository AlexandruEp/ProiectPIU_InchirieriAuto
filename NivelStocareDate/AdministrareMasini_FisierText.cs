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
        private readonly string numeFisier;

        public AdministrareMasini_FisierText(string numeFisier)
        {
            this.numeFisier = numeFisier;
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

                    try
                    {
                        Masina masina = new Masina(linieFisier);
                        masini.Add(masina);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Eroare la parsarea liniei: {linieFisier}\n{ex.Message}");
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

        public Masina CautareDupaModel(string model)
        {
            return GetMasini().FirstOrDefault(m => m.Model.Equals(model, StringComparison.OrdinalIgnoreCase));
        }

        public void SalveazaMasini(List<Masina> masini)
        {
            using (StreamWriter writer = new StreamWriter(numeFisier, false))
            {
                foreach (var m in masini)
                {
                    writer.WriteLine(m.ConversieLaSir());
                }
            }
        }
    }
}
