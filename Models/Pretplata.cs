using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvRaspored.Models
{
    [Table("pretplata")]
    public partial class Pretplata
    {
        [Key]
        [Column("pretplata_id")]
        public int PretplataId { get; set; }
        [Column("korisnik_id")]
        public int KorisnikId { get; set; }
        [Column("emisija_id")]
        public int EmisijaId { get; set; }
        [Column("status")]
        public int Status { get; set; }

        [ForeignKey(nameof(EmisijaId))]
        [InverseProperty("Pretplata")]
        public virtual Emisija Emisija { get; set; }
        [ForeignKey(nameof(KorisnikId))]
        [InverseProperty("Pretplata")]
        public virtual Korisnik Korisnik { get; set; }
    }
}
