using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{

    [Table("Mensualidades")]
    public abstract class Mensualidad
    {        
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage ="El costo es un campo requerido")]
        public decimal Costo { get; set; }
        [Required]
        [Column(TypeName = "Date")]
        public DateTime Fecha { get; set; }
        [ForeignKey("Socio")]
        [Required]
        [Column("Socio")]
        public int CiSocio { get; set; }                
        public Socio Socio { get; set; }
        [Required]
        [Column(TypeName = "Date")]
        public DateTime Vencimiento { get; set; }
        public decimal Descuento { get; set; }        
        public abstract string Tipo { get; set; }
    }
}
