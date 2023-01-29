using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TvRaspored.Models;

namespace TvRaspored.Selection
{
    public class KorisniciListaSelection
    {
        public int KorisnikId { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string KorisnickoIme { get; set; }
        public string Slika { get; set; }
        public string TipNaziv { get; set; }
    }
}
