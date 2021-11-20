using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    [Table("Generalidades")]
    public class Generalidades
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public decimal PrecioUnitario { get; set; }
        [Required]
        public decimal ValorCuota { get; set; }
        [Required]
        public decimal DescuentoAntig { get; set; }
        [Required]
        public decimal DescuentoCantAct { get; set; }
        [Required]
        public int Antiguedad { get; set; }
        [Required]
        public int CantActividades { get; set; }

    }
}
