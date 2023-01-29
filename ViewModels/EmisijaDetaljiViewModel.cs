using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TvRaspored.Models;

namespace TvRaspored.ViewModels
{
    public class EmisijaDetaljiViewModel
    {
        public int PretplataId { get; set; }
        public int EmisijaId { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public int Status { get; set; }
        public int korisnik_id { get; set; }
        public int ModeratorId { get; set; }
    }
}
