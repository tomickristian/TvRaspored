using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvRaspored.Models
{
    [Table("korisnik")]
    public partial class Korisnik
    {
        public Korisnik()
        {
            Pretplata = new HashSet<Pretplata>();
            Tvpostaja = new HashSet<Tvpostaja>();
        }

        [Key]
        [Column("korisnik_id")]
        public int KorisnikId { get; set; }
        [Column("tip_id")]
        public int TipId { get; set; }
        [Required]
        [Column("korisnicko_ime")]
        [StringLength(50)]
        public string KorisnickoIme { get; set; }
        [Column("lozinka")]
        [StringLength(256)]
        public string Lozinka { get; set; }
        [Required]
        [Column("ime")]
        [StringLength(50)]
        public string Ime { get; set; }
        [Required]
        [Column("prezime")]
        [StringLength(50)]
        public string Prezime { get; set; }
        [Column("email")]
        [StringLength(50)]
        public string Email { get; set; }
        [Column("slika")]
        [StringLength(200)]
        public string Slika { get; set; }
        [Column("datum_prijave", TypeName = "datetime")]
        public DateTime? DatumPrijave { get; set; }

        [ForeignKey(nameof(TipId))]
        [InverseProperty(nameof(TipKorisnika.Korisnik))]
        public virtual TipKorisnika Tip { get; set; }
        [InverseProperty("Korisnik")]
        public virtual ICollection<Pretplata> Pretplata { get; set; }
        [InverseProperty("Moderator")]
        public virtual ICollection<Tvpostaja> Tvpostaja { get; set; }
    }
}
