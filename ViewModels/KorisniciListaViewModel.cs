using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TvRaspored.Models;
using TvRaspored.Selection;

namespace TvRaspored.ViewModels
{
    public class KorisniciListaViewModel
    {
        public List<KorisniciListaSelection> korisnici { get; set; }
        public IEnumerable<TipKorisnika> tipoviKorisnika { get; set; }
    }
}
