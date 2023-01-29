using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TvRaspored.ViewModels
{
    public class KorisniciPrijavaViewModel
    {
        [Required(ErrorMessage = "Upišite vaše korisničko ime.")]
        public string KorisnickoIme { get; set; }

        [Required(ErrorMessage = "Upišite lozinku.")]
        [DataType(DataType.Password)]
        public string Lozinka { get; set; }

        [Display(Name = "Zapamti me")]
        public bool ZapamtiMe { get; set; }
    }
}
