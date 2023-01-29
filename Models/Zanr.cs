using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvRaspored.Models
{
    [Table("zanr")]
    public partial class Zanr
    {
        public Zanr()
        {
            Emisija = new HashSet<Emisija>();
        }

        [Key]
        [Column("zanr_id")]
        public int ZanrId { get; set; }
        [Required]
        [Column("naziv")]
        [StringLength(50)]
        public string Naziv { get; set; }

        [InverseProperty("Zanr")]
        public virtual ICollection<Emisija> Emisija { get; set; }
    }
}
