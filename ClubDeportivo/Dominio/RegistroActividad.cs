using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class RegistroActividad
    {
        public int Socio { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public int Hora { get; set; }
    }
}
