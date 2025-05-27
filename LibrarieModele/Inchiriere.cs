using System;

namespace LibrarieModele
{
    public class Inchiriere
    {
        public int IdClient { get; set; }
        public int IdMasina { get; set; }
        public DateTime DataStart { get; set; }
        public DateTime DataEnd { get; set; }
        public DateTime? DataReturnare { get; set; }
        public double PretTotal { get; set; }

        private const char SEPARATOR = ';';

        public Inchiriere() { }

        public Inchiriere(string linie)
        {
            var valori = linie.Split(SEPARATOR);
            IdClient = int.Parse(valori[0]);
            IdMasina = int.Parse(valori[1]);
            DataStart = DateTime.Parse(valori[2]);
            DataEnd = DateTime.Parse(valori[3]);
            PretTotal = double.Parse(valori[4]);
            if (valori.Length > 5 && !string.IsNullOrEmpty(valori[5]))
            {
                DataReturnare = DateTime.Parse(valori[5]);
            }
        }

        public string ConversieLaSir()
        {
            string dataReturnareStr = DataReturnare.HasValue ? DataReturnare.Value.ToString("yyyy-MM-dd") : "";
            return $"{IdClient}{SEPARATOR}{IdMasina}{SEPARATOR}{DataStart:yyyy-MM-dd}{SEPARATOR}{DataEnd:yyyy-MM-dd}{SEPARATOR}{PretTotal}{SEPARATOR}{dataReturnareStr}";
        }
    }
}
