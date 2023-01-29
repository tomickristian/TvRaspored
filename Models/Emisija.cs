using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvRaspored.Models
{
    [Table("emisija")]
    public partial class Emisija
    {
        public Emisija()
        {
            Pretplata = new HashSet<Pretplata>();
        }

        [Key]
        [Column("emisija_id")]
        public int EmisijaId { get; set; }
        [Column("tvpostaja_id")]
        public int TvpostajaId { get; set; }
        [Column("zanr_id")]
        public int ZanrId { get; set; }
        [Required]
        [Column("naziv")]
        [StringLength(50)]
        public string Naziv { get; set; }
        [Required]
        [Column("opis")]
        public string Opis { get; set; }
        [Column("datum_vrijeme_pocetka", TypeName = "datetime2(0)")]
        public DateTime DatumVrijemePocetka { get; set; }
        [Column("datum_vrijeme_zavrsetka", TypeName = "datetime2(0)")]
        public DateTime DatumVrijemeZavrsetka { get; set; }

        [ForeignKey(nameof(TvpostajaId))]
        [InverseProperty("Emisija")]
        public virtual Tvpostaja Tvpostaja { get; set; }
        [ForeignKey(nameof(ZanrId))]
        [InverseProperty("Emisija")]
        public virtual Zanr Zanr { get; set; }
        [InverseProperty("Emisija")]
        public virtual ICollection<Pretplata> Pretplata { get; set; }
    }
}
