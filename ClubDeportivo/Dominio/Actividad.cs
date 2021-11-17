using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Actividad
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int EdadMin { get; set; }
        public int EdadMax { get; set; }
        public int Duracion { get; set; }
        public int CuposDisponibles { get; set; }
    }
}
