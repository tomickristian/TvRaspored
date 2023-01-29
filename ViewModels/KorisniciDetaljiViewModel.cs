using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TvRaspored.Models;

namespace TvRaspored.ViewModels
{
    public class KorisniciDetaljiViewModel
    {
        public string Tip_korisnika { get; set; }
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string Slika { get; set; }
        public DateTime? DatumPrijave { get; set; }

    }
}
