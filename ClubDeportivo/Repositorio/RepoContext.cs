using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Dominio;

namespace Repositorio
{
    class RepoContext:DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Socio> Socios { get; set; }

        //Cuando creo una instancia de dbContext, debo pasarle la conection string 
        public RepoContext(string con) : base(con) { }
    }
}
