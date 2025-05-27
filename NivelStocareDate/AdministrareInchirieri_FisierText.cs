using LibrarieModele;
using System;
using System.Collections.Generic;
using System.IO;

namespace NivelStocareDate
{
    public class AdministrareInchirieri_FisierText
    {
        private readonly string numeFisier;

        public AdministrareInchirieri_FisierText(string numeFisier)
        {
            this.numeFisier = numeFisier;
            if (!File.Exists(numeFisier))
                File.Create(numeFisier).Close();
        }

        public void AdaugaInchiriere(Inchiriere inchiriere)
        {
            using (StreamWriter sw = new StreamWriter(numeFisier, true))
            {
                sw.WriteLine(inchiriere.ConversieLaSir());
            }
        }

        public List<Inchiriere> GetInchirieri()
        {
            List<Inchiriere> lista = new List<Inchiriere>();
            using (StreamReader sr = new StreamReader(numeFisier))
            {
                string linie;
                while ((linie = sr.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(linie))
                    {
                        lista.Add(new Inchiriere(linie));
                    }
                }
            }
            return lista;
        }

        public void AddInchiriere(List<Inchiriere> inchirieri)
        {
            using (StreamWriter sw = new StreamWriter(numeFisier, false))
            {
                foreach (var i in inchirieri)
                {
                    sw.WriteLine(i.ConversieLaSir());
                }
            }
        }
    }
}
