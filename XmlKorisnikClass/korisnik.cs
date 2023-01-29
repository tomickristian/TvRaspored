using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TvRaspored.XmlKorisnikClass
{
    [Serializable, XmlRoot("korisnik")]
    public class korisnik
    {
        public int korisnik_id { get; set; }
        public int tip_id { get; set; }
        public string korisnicko_ime { get; set; }
        public string lozinka { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
        public string email { get; set; }
        public string slika { get; set; }
        public DateTime? datum_prijave { get; set; }
    }
}
