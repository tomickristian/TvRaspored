using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvRaspored.Models
{
    [Table("tip_korisnika")]
    public partial class TipKorisnika
    {
        public TipKorisnika()
        {
            Korisnik = new HashSet<Korisnik>();
        }

        [Key]
        [Column("tip_id")]
        public int TipId { get; set; }
        [Required]
        [Column("naziv")]
        [StringLength(50)]
        public string Naziv { get; set; }

        [InverseProperty("Tip")]
        public virtual ICollection<Korisnik> Korisnik { get; set; }
    }
}
