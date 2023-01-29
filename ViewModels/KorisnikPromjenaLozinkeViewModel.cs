using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TvRaspored.ViewModels
{
    public class KorisnikPromjenaLozinkeViewModel
    {
        public string KorisnickoIme { get; set; }

        [Required(ErrorMessage = "Unesite lozinku.")]
        [StringLength(256, ErrorMessage = "Lozinka može sadržavati do 256 simbola.")]
        [DataType(DataType.Password)]
        public string Lozinka { get; set; }

        [Required(ErrorMessage = "Potvrdite lozinku.")]
        [DataType(DataType.Password)]
        [Compare("Lozinka", ErrorMessage = "Lozinke se ne poklapaju.")]
        public string PotvrdiLozinku { get; set; }
    }
}
