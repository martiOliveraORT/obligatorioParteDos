using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Cuponera : Mensualidad
    {
        public int IngresosDisponibles { get; set; }
        public decimal Descuento { get; set; }

        public override string Tipo()
        {
            return "c";
        }
    }
}
