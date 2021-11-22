using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Repositorio
{
    public class RepoUsuario : IRepositorio<Usuario>
    {
        string cadena = Conexion.stringConexion;

        public bool Alta(Usuario obj)
        {
            bool ok = false;
            if (obj == null) return ok;
            try
            {
                RepoContext db = new RepoContext(cadena);
                Usuario aux = BuscarPorEmail(obj.Email);
                if (aux == null)
                {
                    db.Usuarios.Add(obj);
                    db.SaveChanges();
                    ok = true;
                }
                db.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.InnerException);
            }
            return ok;
        }
        
        public bool Baja(int id)
        {
            throw new NotImplementedException();
        }

        public bool Modificacion(Usuario obj)
        {
            throw new NotImplementedException();
        }

        public List<Usuario> TraerTodo()
        {
            List<Usuario> users = new List<Usuario>();
            try
            {
                RepoContext db = new RepoContext(cadena);
                users = db.Usuarios.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return users;
        }

        public Usuario BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Usuario BuscarPorEmail(string email)
        {
            Usuario usuario = null;
            if (email == "" || email == null) return usuario;
            try
            {
                RepoContext db = new RepoContext(cadena);
                usuario = db.Usuarios.Find(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return usuario;
        }
    }
}
