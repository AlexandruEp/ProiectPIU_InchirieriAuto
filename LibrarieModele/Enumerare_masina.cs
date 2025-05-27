using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarieModele
{

    public enum MarcaMasina
    {
        None= 0,
        BMW = 1,
        Audi = 2,
        Mercedes = 3,
        Ford = 4,
        Opel = 5
    };

    public enum Tip_combustibil
    {
        None = 0,
        Diesel = 1,
        Benzina = 2,
        Electric = 3,
        Hibrid = 4
    };


    public enum Culoare_masina
    {
        None = 0,
        Alba = 1,
        Neagra = 2,
        Rosie = 3,
        Albastra = 4,
        Gri = 5
    };

    public enum TipTransmisie
    {
        None = 0,
        Manuala = 1,
        Automata = 2
    };
}
