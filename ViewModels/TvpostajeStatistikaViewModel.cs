using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TvRaspored.Models;
using TvRaspored.Selection;

namespace TvRaspored.ViewModels
{
    public class TvpostajeStatistikaViewModel
    {
        public List<TvPostajeBrojPretplataSelection> Tvpostaje { get; set; }
        public List<Korisnik> Korisnici { get; set; }
    }
}
