using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TvRaspored.ViewModels
{
    public class XmlViewModel
    {
        [Required(ErrorMessage = "Odaberite .xml datoteku.")]
        public IFormFile XmlFile { get; set; }
    }
}
