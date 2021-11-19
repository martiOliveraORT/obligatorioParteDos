using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ActividadHorario
    {
        public string Nombre { get; set; }
        public int Id { get; set; }
        public int EdadMin { get; set; }
        public int EdadMax { get; set; }
        public int Duracion { get; set; }
        public int CuposDisponibles { get; set; }
        public string Dia { get; set; }
        public int Hora { get; set; }
    }
}
