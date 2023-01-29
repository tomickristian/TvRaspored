using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvRaspored.Models
{
    [Table("tvpostaja")]
    public partial class Tvpostaja
    {
        public Tvpostaja()
        {
            Emisija = new HashSet<Emisija>();
        }

        [Key]
        [Column("tvpostaja_id")]
        public int TvpostajaId { get; set; }
        [Column("moderator_id")]
        public int ModeratorId { get; set; }
        [Required]
        [Column("naziv")]
        [StringLength(50)]
        public string Naziv { get; set; }

        [ForeignKey(nameof(ModeratorId))]
        [InverseProperty(nameof(Korisnik.Tvpostaja))]
        public virtual Korisnik Moderator { get; set; }
        [InverseProperty("Tvpostaja")]
        public virtual ICollection<Emisija> Emisija { get; set; }
    }
}
