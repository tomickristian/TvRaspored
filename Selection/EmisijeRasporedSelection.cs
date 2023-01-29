using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TvRaspored.Selection
{
    public class EmisijeRasporedSelection
    {
        public int EmisijaId { get; set; }
        public string Naziv { get; set; }
        public DateTime DatumVrijemePocetka { get; set; }
        public int KorisnikId { get; set; }
        public int Status { get; set; }
        public string ZanrNaziv { get; set; }

    }
}
