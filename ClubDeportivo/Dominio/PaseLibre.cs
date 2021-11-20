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
    public class PaseLibre : Mensualidad
    {

        private string tipo = "l";

        public override string Tipo

        {
            get { return tipo; }
            set { tipo = value; }
        }  
    }
}
