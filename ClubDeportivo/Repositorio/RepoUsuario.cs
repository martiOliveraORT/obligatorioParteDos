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
            //Crear conexion
            Conexion manejadorConexion = new Conexion();
            SqlConnection cn = manejadorConexion.CrearConexion();

            bool resultado = false;

            //Preparar consulta
            SqlCommand cmd = new SqlCommand
            {
                CommandText = @"INSERT INTO Usuarios (email, password) VALUES (@email, @pass)"
            };

            cmd.Parameters.AddWithValue("@email", obj.Email);
            cmd.Parameters.AddWithValue("@pass", obj.Password);
            cmd.Connection = cn;

            try
            {
                manejadorConexion.AbrirConexion(cn);
                int afectadas = cmd.ExecuteNonQuery();

                if (afectadas == 1)
                {
                resultado = true;
                }
                else
                {
                resultado = false;
                }
                return resultado;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }

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

            // Iniciamos la conexion con la BD
            Conexion manejadorConexion = new Conexion();
            SqlConnection cn = manejadorConexion.CrearConexion();

            List<Usuario> users = new List<Usuario>();

            // Seteamos la Query para la BD
            SqlCommand cmd = new SqlCommand
            {

                CommandText = @"SELECT * FROM usuarios "
            };
            cmd.Connection = cn;
            try
            {
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader filas = cmd.ExecuteReader();
                while (filas.Read())
                {
                    // Guardo la info de la tabla que necesito tener
                    users.Add(new Usuario
                    {
                        Email = (string)filas["email"],
                        Password = (string)filas["password"],


                    });
                }
                return users;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return users;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
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

            /*
            public Usuario BuscarPorEmail(string email)
            {
                //Crear conexion
                Conexion manejadorConexion = new Conexion();
                SqlConnection cn = manejadorConexion.CrearConexion();

                Usuario user = null;

                //Preparar consulta
                SqlCommand cmd = new SqlCommand
                {
                    CommandText = @"SELECT * FROM Usuarios WHERE email = @email"
                };

                cmd.Parameters.AddWithValue("@email", email);
                cmd.Connection = cn;

                try
                {
                    manejadorConexion.AbrirConexion(cn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        user = new Usuario
                        {
                            Email = (string)reader["email"],
                            Password = (string)reader["password"]
                        };
                    }
                    return user;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return user;
                }
                finally
                {
                    manejadorConexion.CerrarConexion(cn);
                }
            }*/
        }
}
