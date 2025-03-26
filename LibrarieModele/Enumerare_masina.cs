using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarieModele
{

    public enum Model_masina
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
        alba = 1,
        neagra = 2,
        rosie = 3,
        albastra = 4,
        gri = 5
    };
}
