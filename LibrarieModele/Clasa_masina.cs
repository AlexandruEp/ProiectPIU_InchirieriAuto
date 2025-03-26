using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarieModele
{
   public class Masina
    {
 
           public int IdMasina { get; set; }
           public Model_masina model { get; set; }
           public Tip_combustibil combustibil { get; set; }
           public int an_fabricatie { get; set; }
           public Culoare_masina culoare { get; set; }

            private const char SEPARATOR_PRINCIPAL_FISIER = ';';
            private const int ID = 0;
            private const int MODEL = 1;
            private const int COMBUSTIBIL = 2;
            private const int AN_FABRICATIE = 3;
            private const int CULOARE = 4;

        public Masina()
        {
            IdMasina = 0;
            model = Model_masina.None;
            combustibil = Tip_combustibil.None;
            an_fabricatie = 0;
            culoare = Culoare_masina.None;
        }

        public Masina(Model_masina model, Tip_combustibil tip_combustibil, int an_fabricatie, Culoare_masina culoare)
        {
            this.model = model;
            this.combustibil = tip_combustibil;
            this.an_fabricatie = an_fabricatie;
            this.culoare = culoare;
        }
        public Masina(string liniefisier)
        {
            string[] dateFisier = liniefisier.Split(SEPARATOR_PRINCIPAL_FISIER);
            this.IdMasina = Convert.ToInt32(dateFisier[ID]);
            this.model = (Model_masina)Enum.Parse(typeof(Model_masina), dateFisier[MODEL]);
            this.combustibil = (Tip_combustibil)Enum.Parse(typeof(Tip_combustibil), dateFisier[COMBUSTIBIL]);
            this.an_fabricatie = Convert.ToInt32(dateFisier[AN_FABRICATIE]);
            this.culoare = (Culoare_masina)Enum.Parse(typeof(Culoare_masina), dateFisier[CULOARE]);
        }

            public string Info()
            {
                return $"ID: {IdMasina}\n"+
                    $"Model: {model}\n" +
                    $"Motorizare: {combustibil}\n" +
                    $"An fabricatie: {an_fabricatie}\n" +
                    $"Culoare: {culoare}";
            }
        public string ConversieLaSir()
        {
            string masinaPentruFisier = string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}",
                SEPARATOR_PRINCIPAL_FISIER, IdMasina, model, combustibil, an_fabricatie, culoare);
            return masinaPentruFisier;
        }
    }
}

