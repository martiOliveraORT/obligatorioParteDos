using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public abstract class Mensualidad
    {
        public int Id { get; set; }
        public decimal Costo { get; set; }
        public DateTime Fecha { get; set; }
        public Socio Socio { get; set; }
        public DateTime Vencimiento { get; set; }
        public abstract string Tipo();
    }
}
