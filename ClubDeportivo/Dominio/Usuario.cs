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
    [Table("Usuarios")]
    public class Usuario
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
