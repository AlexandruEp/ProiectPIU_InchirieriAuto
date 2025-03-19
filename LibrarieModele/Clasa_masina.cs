using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarieModele
{
   public class Masina
    {
 
           public string model { get; set; }
           public string tip_combustibil { get; set; }
           public int an_fabricatie { get; set; }

            public Masina()
            {
                model = string.Empty;
                tip_combustibil = string.Empty;
                an_fabricatie = 0;
            }

            public Masina(string _model, string _tip_combustibil, int _an_fabricatie)
            {
                model = _model;
                tip_combustibil = _tip_combustibil;
                an_fabricatie = _an_fabricatie;
            }

            public string Info()
            {
                return $"Model: {model}\n" +
                    $"Motorizare: {tip_combustibil}\n" +
                    $"An fabricatie: {an_fabricatie}";

            }
    }
}

