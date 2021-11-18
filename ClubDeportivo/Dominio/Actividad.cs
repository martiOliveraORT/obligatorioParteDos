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
    [Table("Actividades")]
    public class Actividad
    {
        [Key]
        public string Nombre { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EdadMin { get; set; }
        public int EdadMax { get; set; }
        public int Duracion { get; set; }
        public int CuposDisponibles { get; set; }
    }
}
