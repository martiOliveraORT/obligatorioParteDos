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
    public class RepoRegistroActividad : IRepositorio<RegistroActividad>
    {
        public bool Alta(RegistroActividad obj)
        {
            // Iniciamos la conexion con la BD
            Conexion manejadorConexion = new Conexion();
            SqlConnection cn = manejadorConexion.CrearConexion();

            bool resultado;

            // Seteamos la Query para la BD
            SqlCommand cmd = new SqlCommand
            {
                CommandText = @"INSERT INTO registroActividad (socio, actividad, fecha, hora) VALUES (@ced, @nomAct, @fecha, @hr)"
            };
            // Definimos que varibales corresponde a que dato de la query
            cmd.Parameters.AddWithValue("@ced", obj.Socio);
            cmd.Parameters.AddWithValue("@nomAct", obj.Nombre);
            cmd.Parameters.AddWithValue("@fecha", obj.Fecha);
            cmd.Parameters.AddWithValue("@hr", obj.Hora);

            cmd.Connection = cn;// SETEAR!!

            // Intentamos ejecutar la Query (Try)
            // Si el resultado de las filas es 1(Se modifico), se realizo correctamente
            // Si falla capturamos el error en el cathch y mostramos el mensajes (ex.message)
            try
            {
                manejadorConexion.AbrirConexion(cn);
                int rows = cmd.ExecuteNonQuery();

                if (rows == 1)
                {
                    resultado = true;
                }
                else
                {
                    resultado = false;
                }
                return resultado;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                // Finalmente si o si cerramos la conexion
                manejadorConexion.CerrarConexion(cn);
            }
        }

        public bool Baja(int id)
        {
            throw new NotImplementedException();
        }

        public bool Modificacion(RegistroActividad obj)
        {
            throw new NotImplementedException();
        }

        public List<RegistroActividad> TraerTodo()
        {
            // Iniciamos la conexion con la BD
            Conexion manejadorConexion = new Conexion();
            SqlConnection cn = manejadorConexion.CrearConexion();

            List<RegistroActividad> ingresos = new List<RegistroActividad>();

            // Seteamos la Query para la BD
            SqlCommand cmd = new SqlCommand
            {
                // Los traemos asi para tener la mas reciente arriba
                CommandText = @"SELECT * FROM RegistroActividad ORDER BY fecha DESC"
            };
            cmd.Connection = cn;// SETEAR!!

            // Intentamos ejecutar la Query (Try)
            // Leemos el reusltado de la query y guardamos los datos en sus respectivas posiciones
            // Si falla capturamos el error en el cathch y mostramos el mensajes (ex.message)
            try
            {
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader filas = cmd.ExecuteReader();
                while (filas.Read())
                {

                    ingresos.Add(new RegistroActividad
                    {
                        Socio = (int)filas["socio"],
                        Nombre = (string)filas["Actividad"],
                        Fecha = (DateTime)filas["fecha"],
                        Hora = (int)filas["hora"],
                    });
                }
                return ingresos;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ingresos;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }

        public RegistroActividad BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        //  Funcionalidad que susplanta al BuscarPorId
        public RegistroActividad BusquedaEspecifica(int socio, string act, string fecha)
        {
            // Iniciamos la conexion con la BD
            Conexion manejadorConexion = new Conexion();
            SqlConnection cn = manejadorConexion.CrearConexion();
            RegistroActividad registro = null;

            // Seteamos la Query para la BD
            SqlCommand cmd = new SqlCommand
            {
                CommandText = @"SELECT * FROM RegistroActividad WHERE socio = @socio AND actividad = @act AND fecha = @fecha"
            };
            // Definimos que varibales corresponde a que dato de la query
            cmd.Parameters.AddWithValue("@socio", socio);
            cmd.Parameters.AddWithValue("@act", act);
            cmd.Parameters.AddWithValue("@fecha", fecha);
            cmd.Connection = cn;// Setear

            // Intentamos ejecutar la Query (Try)
            // Leemos el reusltado de la query y guardamos los datos en sus respectivas posiciones
            // Si falla capturamos el error en el cathch y mostramos el mensajes (ex.message)
            try
            {
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    registro = new RegistroActividad
                    {
                        Socio = (int)reader["socio"],
                        Nombre = (string)reader["actividad"],
                        Fecha = (DateTime)reader["fecha"]
                    };
                }
                return registro;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return registro;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }

        //Funcion que devuelve cuantos cupos disponibles hay en una actividad
        public int CuposDisponibles(String act, string fecha, int hora)
        {
            // Iniciamos la conexion con la BD
            Conexion manejadorConexion = new Conexion();
            SqlConnection cn = manejadorConexion.CrearConexion();
            // Seteo -1 como numero error si no se pudo consultar
            int registro = -1;

            // Seteamos la Query para la BD
            SqlCommand cmd = new SqlCommand
            {
                CommandText = @"SELECT COUNT(actividad) AS cantidad FROM registroActividad WHERE actividad = @act AND fecha = @fecha AND hora = @hr"

            };
            // Definimos que varibales corresponde a que dato de la query
            cmd.Parameters.AddWithValue("@act", act);
            cmd.Parameters.AddWithValue("@fecha", fecha);
            cmd.Parameters.AddWithValue("@hr", hora);
            cmd.Connection = cn;// Setear

            // Intentamos ejecutar la Query (Try)
            // Leemos el reusltado de la query y guardamos los datos en sus respectivas posiciones
            // Si falla capturamos el error en el cathch y mostramos el mensajes (ex.message)
            try
            {
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    registro = (int)reader["cantidad"];
                }
                return registro;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return registro;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }

        public List<RegistroActividad> ingresoSocioPorFecha(int ci, DateTime fecha)
        {
            // Iniciamos la conexion con la BD
            Conexion manejadorConexion = new Conexion();
            SqlConnection cn = manejadorConexion.CrearConexion();

            List<RegistroActividad> ingresos = new List<RegistroActividad>();

            // Seteamos la Query para la BD
            SqlCommand cmd = new SqlCommand
            {
                // Los traemos asi para tener la mas reciente arriba
                CommandText = @"SELECT * FROM registroActividad Where socio = @socio AND fecha = @fecha"
            };
            cmd.Parameters.AddWithValue("@socio", ci);
            cmd.Parameters.AddWithValue("@fecha", fecha);
            cmd.Connection = cn;// SETEAR!!

            // Intentamos ejecutar la Query (Try)
            // Leemos el reusltado de la query y guardamos los datos en sus respectivas posiciones
            // Si falla capturamos el error en el cathch y mostramos el mensajes (ex.message)
            try
            {
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader filas = cmd.ExecuteReader();
                while (filas.Read())
                {

                    ingresos.Add(new RegistroActividad
                    {
                        Socio = (int)filas["socio"],
                        Nombre = (string)filas["Actividad"],
                        Fecha = (DateTime)filas["fecha"],
                        Hora = (int)filas["hora"],
                    });
                }
                return ingresos;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ingresos = null;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }
    }
}
