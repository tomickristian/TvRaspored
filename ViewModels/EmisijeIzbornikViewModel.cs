using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TvRaspored.Models;
using TvRaspored.Selection;

namespace TvRaspored.ViewModels
{
    public class EmisijeIzbornikViewModel
    {
        public List<EmisijeIzbornikSelection> emisije { get; set; }
        public int Zanr_id { get; set; }
        public string ZanrNaziv { get; set; }
        public Tvpostaja TvPostaja { get; set; }
        public int KorisnikId { get; set; }
    }
}
