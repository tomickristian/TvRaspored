using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TvRaspored.Selection
{
    public class EmisijeIzbornikSelection
    {
        public int EmisijaId { get; set; }
        public string Naziv { get; set; }
        public int ZanrId { get; set; }
        public DateTime DatumVrijemePocetka { get; set; }
        public string ZanrNaziv { get; set; }
        public int TvpostajaId { get; set; }
    }
}
