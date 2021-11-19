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
        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<Mensualidad> Mensualidades { get; set; }
        public DbSet<Horario> Horarios { get; set; }

        //Cuando creo una instancia de dbContext, debo pasarle la conection string 
        public RepoContext(string con) : base(con) { }

        //A la tabla Horarios, seteamos la key compuesta
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Horario>().HasKey(h=> new{h.Actividad, h.Dia, h.Hora});
        }

    }
}
