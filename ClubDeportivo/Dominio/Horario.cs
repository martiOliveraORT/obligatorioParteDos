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
    [Table("Horarios")]
    public class Horario
    {
        [Key]
        public string Actividad { get; set; } //Agregar FK con actividad o capaz no es necesario 
        [Key]
        public string Dia { get; set; }
        [Key]
        public int Hora { get; set; }
    }
}
