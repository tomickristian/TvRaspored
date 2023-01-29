using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TvRaspored.Models;

namespace TvRaspored.ViewModels
{
    public class KorisniciUrediViewModel
    {
        public IEnumerable<TipKorisnika> Tipovi { get; set; }
        public int TipId { get; set; }
        public int KorisnikId { get; set; }
        public DateTime? DatumPrijave { get; set; }
        public string PutanjaSlike { get; set; }
        public string Lozinka { get; set; }

        [Required(ErrorMessage = "Unesite korisničko ime.")]
        [StringLength(50, ErrorMessage = "Korisničko ime može sadržavati do 50 simbola.")]
        [Remote(action: "UrediKorisnickoImeZauzeto", controller:"Korisnici", AdditionalFields = "KorisnickoIme,KorisnikId")]
        public string KorisnickoIme { get; set; }

        [Required(ErrorMessage = "Unesite ime.")]
        [StringLength(50, ErrorMessage = "Ime može sadržavati do 50 simbola.")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Unesite prezime.")]
        [StringLength(50, ErrorMessage = "Prezime može sadržavati do 50 simbola.")]
        public string Prezime { get; set; }

        [StringLength(50, ErrorMessage = "E-mail adresa može sadržavati do 50 simbola.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Unesite važeću e-mail adresu")]
        public string Email { get; set; }
        public IFormFile Slika { get; set; }
    }
}
