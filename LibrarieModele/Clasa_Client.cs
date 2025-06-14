﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LibrarieModele
{
    //Clasa client
   public class Client
    {
        private const char SEPARATOR_AFISARE = ';';

        
        private const int ID = 0;
        private const int NUME = 1;
        private const int EMAIL = 2;
        private const int TELEFON = 3;
        private const int CNP_ = 4;
        private const int PAROLA = 5;
        public int IdClient { get; set; }
        public string nume { get; set; }
        public string email { get; set; }
        public string CNP { get; set; }
        public string telefon { get; set; }
        public string parola { get; set; }
        

        public Client()
        {
            nume = email = CNP = telefon = parola = string.Empty;
        }

        public Client(int _idclient, string _nume, string _email, string _CNP, string _telefon, string _parola)
        {
            IdClient = _idclient;
            nume = _nume;
            email = _email;
            CNP = _CNP;
            telefon = _telefon;
            parola = _parola;
        }
        public Client(string linieFisier)
        {
               string[] dateFisier = linieFisier.Split(SEPARATOR_AFISARE);
               this.IdClient = Convert.ToInt32(dateFisier[ID]);
               this.nume = dateFisier[NUME];
               this.email = dateFisier[EMAIL];
               this.telefon = dateFisier[TELEFON];
               this.CNP = dateFisier[CNP_];
               this.parola = dateFisier.Length > PAROLA ? dateFisier[PAROLA] : "parola123"; // parola default pentru compatibilitate
        
        }
        public string Info()
        {
            return $"CLientul cu id-ul: {IdClient}\n" +
                $"Nume: {nume}\n" +
                $"Email: {email}\n" +
                $"Numar telefon: {telefon}\n"+
                $"CNP:{CNP}\n";

        }
        public string ConversieLaSir()
        {
            string obiectClientPentruFisier = string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}",
                SEPARATOR_AFISARE,
                IdClient.ToString(),
                (nume ?? "NECUNOSCUT"),
                (email ?? "NECUNOSCUT"),
                (telefon ?? "NECUNOSCUT"),
                (CNP ?? "NECUNOSCUT"),
                (parola ?? "parola123"));
            return obiectClientPentruFisier;
        }
        public void SetID(int id)
        {
            this.IdClient = id;
        }
    }
}
