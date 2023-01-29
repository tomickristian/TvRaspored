using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TvRaspored.Models;

namespace TvRaspored.ViewModels
{
    public class TvpostajeDodajUrediViewModel
    {
        public IEnumerable<Korisnik> Moderatori { get; set; }

        [Required(ErrorMessage = "Upišite naziv.")]
        [StringLength(50, ErrorMessage = "Naziv može sadržavati do 50 simbola.")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Odaberite moderatora.")]
        public int ModeratorId { get; set; }
        public int TvpostajaId { get; set; }
    }
}
