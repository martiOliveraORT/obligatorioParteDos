using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Socio
    {
        public int Cedula { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNac { get; set; }
        public DateTime FechaIngreso { get; set; }
        public bool Estado { get; set; }
    }
}
