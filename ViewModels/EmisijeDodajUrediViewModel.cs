using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TvRaspored.Models;

namespace TvRaspored.ViewModels
{
    public class EmisijeDodajUrediViewModel
    {
        public IEnumerable<Tvpostaja> tvPostaje { get; set; }
        public IEnumerable<Zanr> zanrovi { get; set; }

        public int EmisijaId { get; set; }
        public DateTime DatumVrijemeZavrsetka { get; set; }

        [Required(ErrorMessage = "Odaberite tv postaju.")]
        [Remote(action: "VrijemeTrajanja", controller: "Emisije", AdditionalFields = "EmisijaId,DatumVrijemePocetka,vrijemeTrajanjaSati,vrijemeTrajanjaMinute,TvpostajaId")]
        public int TvpostajaId { get; set; }

        [Required(ErrorMessage = "Odaberite žanr.")]
        public int ZanrId { get; set; }

        [Required(ErrorMessage = "Unesite naziv.")]
        [StringLength(50, ErrorMessage = "Naziv emisije može sadržavati do 50 simbola.")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Unesite opis.")]
        public string Opis { get; set; }

        [Required(ErrorMessage = "Unesite vrijeme poćetka.")]
        [Remote(action: "VrijemePocetkaZauzeto", controller: "Emisije", AdditionalFields = "EmisijaId,DatumVrijemePocetka,TvpostajaId")]
        public DateTime DatumVrijemePocetka { get; set; }

        [Required(ErrorMessage = "Unesite sate.")]
        public int? vrijemeTrajanjaSati { get; set; }

        [Required(ErrorMessage = "Unesite minute.")]
        [Remote(action: "VrijemeTrajanja", controller: "Emisije", AdditionalFields = "EmisijaId,DatumVrijemePocetka,vrijemeTrajanjaSati,vrijemeTrajanjaMinute,TvpostajaId")]
        public int? vrijemeTrajanjaMinute { get; set; }

    }
}
