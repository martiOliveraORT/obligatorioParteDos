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
    [Table("RegistroActividades")]
    public class RegistroActividad
    {
        public int Socio { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public int Hora { get; set; }
    }
}
