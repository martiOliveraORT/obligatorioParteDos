using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    [Table("Mensualidades")]
    public class Cuponera : Mensualidad
    {
        public int IngresosDisponibles { get; set; }     

        private string tipo = "c";

        public override string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }
    }
}
