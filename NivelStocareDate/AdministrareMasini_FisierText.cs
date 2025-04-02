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
            int idNou = GetUltimulID()+1;
            masina.IdMasina = idNou;
            // Deschidem fișierul în modul 'append' pentru a adăuga masini noi
            using (StreamWriter streamWriterFisierText = new StreamWriter(numeFisier, true))
            {
                streamWriterFisierText.WriteLine(masina.ConversieLaSir());
            }
        }
        public Masina[] GetMasini(out int nrMasini)
        {
            Masina[] masini = new Masina[NR_MAX_MASINI];
            using (StreamReader streamReader = new StreamReader(numeFisier))
            {
                string linieFisier;
                nrMasini = 0;
                int lastid = GetUltimulID();
                while ((linieFisier = streamReader.ReadLine()) != null)
                {
                    string[] dateMasina = linieFisier.Split(';');
                    if (dateMasina.Length >= 5)
                    {
                        Model_masina model = (Model_masina)Enum.Parse(typeof(Model_masina), dateMasina[1]);
                        Tip_combustibil combustibil = (Tip_combustibil)Enum.Parse(typeof(Tip_combustibil), dateMasina[2]);
                        int an_fabricatie = Convert.ToInt32(dateMasina[3]);
                        Culoare_masina culoare = (Culoare_masina)Enum.Parse(typeof(Culoare_masina), dateMasina[4]);

                        masini[nrMasini++] = new Masina(model, combustibil, an_fabricatie, culoare)
                        {
                            IdMasina = ++lastid
                        };
                    }
                    else
                    {
                        // Handle the case where the line is not properly formatted
                        Console.WriteLine($"Invalid line format: {linieFisier}");
                    }
                }
            }

            return masini;
        }
        public Masina CautareDupaModel(string nume)
        {
            Masina[] masini = GetMasini(out int nrMasini);

            for (int i = 0; i < nrMasini; i++)
            {
                if (masini[i].model.ToString().Equals(nume))
                {

                    return masini[i];
                }
            }
            return null;
        }

        public int GetUltimulID()
        {
            int lastID = 0;
            using (StreamReader streamReader = new StreamReader(numeFisier))
            {
                string linieFisier;
                while ((linieFisier = streamReader.ReadLine()) != null)
                {
                    string[] dateMasina = linieFisier.Split(';');
                    if (dateMasina.Length > 0 && int.TryParse(dateMasina[0], out int id))
                    {
                        lastID = id;
                    }
                }
            }
            return lastID;
        }



    }
}
