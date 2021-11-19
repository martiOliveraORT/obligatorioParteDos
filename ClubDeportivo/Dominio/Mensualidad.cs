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
    [Table("Mensualidad")]
    public abstract class Mensualidad
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public decimal Costo { get; set; }
        public DateTime Fecha { get; set; }
        public Socio Socio { get; set; }
        public DateTime Vencimiento { get; set; }
        public decimal Descuento { get; set; }
        [NotMapped]
        public string Tipo { get; set; }
        public abstract string TipoMetodo();
    }
}
