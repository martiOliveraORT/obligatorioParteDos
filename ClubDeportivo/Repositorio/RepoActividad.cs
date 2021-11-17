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
    public class RepoActividad : IRepositorio<Actividad>
    {
        public bool Alta(Actividad obj)
        {
            bool success = false;
            // Iniciamos la conexion con la BD
            Conexion manejadorConexion = new Conexion();
            SqlConnection cn = manejadorConexion.CrearConexion();

            // Seteamos la Query para la BD
            SqlCommand cmd = new SqlCommand
            {
                CommandText = @"INSERT INTO Actividades (nombre, edadMin, edadMax, duracion, cupos) VALUES (@nom, @eMin, @eMax, @dur, @cupos)"
            };
            // Definimos que varibales corresponde a que dato de la query
            cmd.Parameters.AddWithValue("@nom", obj.Nombre);
            cmd.Parameters.AddWithValue("@eMin", obj.EdadMin);
            cmd.Parameters.AddWithValue("@eMax", obj.EdadMax);
            cmd.Parameters.AddWithValue("@dur", obj.Duracion);
            cmd.Parameters.AddWithValue("@cupos", obj.CuposDisponibles);
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
                    success = true;
                }
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            // Finalmente si o si cerramos la conexion
            finally
            {

                manejadorConexion.CerrarConexion(cn);
            }
        }

        public bool Baja(int id)
        {
            bool success = false;
            // Iniciamos la conexion con la BD
            Conexion manejadorConexion = new Conexion();
            SqlConnection cn = manejadorConexion.CrearConexion();

            // Seteamos la Query para la BD
            SqlCommand cmd = new SqlCommand
            {
                CommandText = @"DELETE FROM Actividades WHERE id = @id"
            };
            // Definimos que varibales corresponde a que dato de la query
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Connection = cn; // SETEAR!!


            // Intentamos ejecutar la Query (Try)
            // Si el resultado de las filas es 1(Se modifico), se realizo correctamente
            // Si falla capturamos el error en el cathch y mostramos el mensajes (ex.message)
            try
            {
                manejadorConexion.AbrirConexion(cn);
                int rows = cmd.ExecuteNonQuery();

                if (rows == 1)
                {
                    success = true;
                }
                return success;
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

        public bool Modificacion(Actividad obj)
        {
            bool success = false;
            // Iniciamos la conexion con la BD
            Conexion manejadorConexion = new Conexion();
            SqlConnection cn = manejadorConexion.CrearConexion();

            // Seteamos la Query para la BD
            SqlCommand cmd = new SqlCommand
            {
                CommandText = @"UPDATE Actividades SET nombre = @nom,edadMin = @eMin, edadMax = @eMax, duracion = @dur, cupos = @cupos WHERE id = @id"
            };
            //Definimos que varibales corresponde a que dato de la query
            cmd.Parameters.AddWithValue("@nom", obj.Nombre);
            cmd.Parameters.AddWithValue("@eMin", obj.EdadMin);
            cmd.Parameters.AddWithValue("@eMax", obj.EdadMax);
            cmd.Parameters.AddWithValue("@dur", obj.Duracion);
            cmd.Parameters.AddWithValue("@cupos", obj.CuposDisponibles);
            cmd.Connection = cn;

            // Intentamos ejecutar la Query (Try)
            // Si el resultado de las filas es 1(Se modifico), se realizo correctamente
            // Si falla capturamos el error en el cathch y mostramos el mensajes (ex.message)
            try
            {
                manejadorConexion.AbrirConexion(cn);
                int rows = cmd.ExecuteNonQuery();

                if (rows == 1)
                {
                    success = true;
                }
                return success;
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

        public List<Actividad> TraerTodo()
        {
            // Iniciamos la conexion con la BD
            Conexion manejadorConexion = new Conexion();
            SqlConnection cn = manejadorConexion.CrearConexion();

            List<Actividad> acts = new List<Actividad>();

            // Seteamos la Query para la BD
            SqlCommand cmd = new SqlCommand
            {

                CommandText = @"SELECT * FROM Actividades "
            };
            cmd.Connection = cn;
            try
            {
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader filas = cmd.ExecuteReader();
                while (filas.Read())
                {
                    // Guardo la info de la tabla que necesito tener
                    acts.Add(new Actividad
                    {
                        Id = (int)filas["id"],
                        Nombre = (string)filas["Nombre"],
                        EdadMin = (int)filas["edadMin"],
                        EdadMax = (int)filas["edadMax"],
                        CuposDisponibles = (int)filas["cupos"],

                    });
                }
                return acts;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return acts;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }

        public Actividad BuscarPorId(int id)
        {
            //INICIO LA CONEXION CON LA BD
            Conexion manejadorConexion = new Conexion();
            SqlConnection cn = manejadorConexion.CrearConexion();
            Actividad act = null;

            //CREAMOS LA QUERY A EJECUTAR LUEGO
            SqlCommand cmd = new SqlCommand
            {
                CommandText = @"SELECT * FROM Actividades WHERE id = @act "
            };
            cmd.Parameters.AddWithValue("@act", id);



            try
            {
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    act = new Actividad
                    {
                        Id = (int)reader["id"],
                        Nombre = (string)reader["Nombre"],
                        EdadMin = (int)reader["edadMin"],
                        EdadMax = (int)reader["edadMax"],
                        CuposDisponibles = (int)reader["cupos"],
                    };
                }
                return act;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return act;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }

        public Actividad BusarPorNombre(string n)
        {
            // Iniciamos la conexion con la BD
            Conexion manejadorConexion = new Conexion();
            SqlConnection cn = manejadorConexion.CrearConexion();
            // Paso el parametro a MIN para mantener el criterio de busqeda siempre en MIN
            Actividad act = null;


            // Seteamos la Query para la BD
            SqlCommand cmd = new SqlCommand
            {
                CommandText = @"SELECT * FROM Actividades WHERE nombre = @act "
            };
            //Definimos que varibales corresponde a que dato de la query
            cmd.Parameters.AddWithValue("@act", n);
            cmd.Connection = cn;// SETEAR!!

            // Intentamos ejecutar la Query (Try)
            // Si el resultado de las filas es 1(Se modifico), se realizo correctamente
            // Si falla capturamos el error en el cathch y mostramos el mensajes (ex.message)
            try
            {
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    act = new Actividad
                    {
                        Id = (int)reader["id"],
                        Nombre = (string)reader["Nombre"],
                        EdadMin = (int)reader["edadMin"],
                        EdadMax = (int)reader["edadMax"],
                        CuposDisponibles = (int)reader["cupos"],
                    };
                }
                return act;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return act;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }



        // SECCION CORRESPONDEA HORARIOS
        #region Horarios
        public List<Horario> BuscarHorarios(string dia, int hora)
        {
            // Iniciamos la conexion con la BD
            Conexion manejadorConexion = new Conexion();
            SqlConnection cn = manejadorConexion.CrearConexion();
            List<Horario> horas = new List<Horario>();


            // Seteamos la Query para la BD
            SqlCommand cmd = new SqlCommand
            {
                CommandText = @"SELECT * FROM Horarios WHERE hora >= @hora AND dia = @dia ORDER BY hora ASC"
            };

            // Setear el parametro CONNECTION con el valor del cn
            // Esto es IMPORTANTE!!!! si no, no se conecta
            cmd.Parameters.AddWithValue("@dia", dia);
            cmd.Parameters.AddWithValue("@hora", hora);
            cmd.Connection = cn; // SETEAR !!!

            // Intentamos ejecutar la Query (Try)
            // Si el resultado de las filas es 1(Se modifico), se realizo correctamente
            // Si falla capturamos el error en el cathch y mostramos el mensajes (ex.message)

            try
            {
                if (manejadorConexion.AbrirConexion(cn))
                {
                    SqlDataReader filas = cmd.ExecuteReader();
                    while (filas.Read())
                    {

                        horas.Add(new Horario
                        {
                            Actividad = (string)filas["actividad"],
                            Dia = (string)filas["dia"],
                            Hora = (int)filas["hora"],

                        });
                    }
                }
                return horas;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return horas;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }


        public List<Horario> TraerTodosHorarios()
        {
            // Iniciamos la conexion con la BD
            Conexion manejadorConexion = new Conexion();
            SqlConnection cn = manejadorConexion.CrearConexion();
            List<Horario> horas = new List<Horario>();


            // Seteamos la Query para la BD
            SqlCommand cmd = new SqlCommand
            {
                CommandText = @"SELECT * FROM Horarios"
            };

            // Setear el parametro CONNECTION con el valor del cn
            // Esto es IMPORTANTE!!!! si no, no se conecta
            cmd.Connection = cn; // SETEAR !!!

            // Intentamos ejecutar la Query (Try)
            // Si el resultado de las filas es 1(Se modifico), se realizo correctamente
            // Si falla capturamos el error en el cathch y mostramos el mensajes (ex.message)

            try
            {
                if (manejadorConexion.AbrirConexion(cn))
                {
                    SqlDataReader filas = cmd.ExecuteReader();
                    while (filas.Read())
                    {

                        horas.Add(new Horario
                        {
                            Actividad = (string)filas["actividad"],
                            Dia = (string)filas["dia"],
                            Hora = (int)filas["hora"],

                        });
                    }
                }
                return horas;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return horas;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }
        #endregion
    }
}
