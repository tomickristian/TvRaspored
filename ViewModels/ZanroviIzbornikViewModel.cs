using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TvRaspored.Models;

namespace TvRaspored.ViewModels
{
    public class ZanroviIzbornikViewModel
    {
        public string ModeratorIme { get; set; }
        public int TvPostajaId { get; set; }
        public string TvPostajaNaziv { get; set; }
        public IEnumerable<Emisija> Emisije { get; set; }
        public IEnumerable<Zanr> Zanrovi { get; set; }

        [Required(ErrorMessage = "Upišite naziv.")]
        [StringLength(50, ErrorMessage = "Naziv može sadržavati do 50 simbola.")]
        public string Naziv { get; set; }
    }
}
